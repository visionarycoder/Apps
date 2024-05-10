using Microsoft.Extensions.Logging;

using Swag.Access.Data.Interface;
using Swag.Engine.Calculator.Interface;
using Swag.Engine.Calculator.Service.Helpers;

namespace Swag.Engine.Calculator.Service;

public class CalculatorEngine(ILogger<CalculatorEngine> logger, IDataAccess dataAccess) : ICalculatorEngine
{

    private readonly SwagCalculator swagCalculator = new();

    public async Task<decimal> CalculateEstimate(int optimistic, int mostLikely, int pessimistic)
    {
        logger.LogDebug($"{nameof(CalculatorEngine)}.{nameof(CalculateEstimate)}() called.");
        logger.LogDebug("Calculating estimate for optimistic: {optimistic}, most likely: {mostLikely}, pessimistic: {pessimistic}", optimistic, mostLikely, pessimistic);
        var calculated = await swagCalculator.Calculate(optimistic, mostLikely, pessimistic);
        logger.LogDebug($"Calculated estimate: {calculated}");
        return calculated;
    }

}