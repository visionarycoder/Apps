using Microsoft.Extensions.Logging;
using Swag.Access.Data.Interface;
using Swag.Engine.Calculator.Interface;
using Swag.Manager.Content.Interface;
using Swag.Manager.Content.Interface.Models;
using Swag.Manager.Content.Service.Helpers;

namespace Swag.Manager.Content.Service;

public class ContentManager(ILogger<ContentManager> logger, ICalculatorEngine calculatorEngine, IDataAccess dataAccess) : IContentManager
{
        
    public async Task<Estimate> CalculateEstimate(int optimistic, int mostLikely, int pessimistic)
    {
    
        logger.LogDebug($"{nameof(ContentManager)}.{nameof(CalculateEstimate)}() called.");
        logger.LogDebug("Calculating estimate for optimistic: {optimistic}, most likely: {mostLikely}, pessimistic: {pessimistic}", optimistic, mostLikely, pessimistic);
        var existing = await dataAccess.GetEstimate(optimistic, mostLikely, pessimistic);
        if (existing.EntryFound)
        {
            logger.LogDebug("Estimate found in database");
            return existing.Estimate.Convert();
        }

        var calculated = await calculatorEngine.CalculateEstimate(optimistic, mostLikely, pessimistic);
        var estimate = new Estimate
        {
            Optimistic = optimistic,
            MostLikely = mostLikely,
            Pessimistic = pessimistic,
            Calculated = calculated
        };
        return estimate;

    }

}