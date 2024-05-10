using Microsoft.Extensions.Logging;
using Swag.Client.ConsoleApp.Interface;
using Swag.Client.ConsoleApp.Interface.Models;
using Swag.Client.ConsoleApp.Service.Helpers;
using Swag.Framework.Helpers;
using Swag.Manager.Content.Interface;

namespace Swag.Client.ConsoleApp.Service;

public class ConsoleClient(ILogger<ConsoleClient> logger, IContentManager contentManager) : IConsoleClient
{

    public async Task<int> Run()
    {

        logger.LogInformation("Starting Console Client");
        try
        {

            var estimates = new List<Estimate>();
            ConsoleHelper.Title = "Swag Helper";
            ConsoleHelper.Description = "Estimate Calculator";
            ConsoleHelper.ShowHeader();

            var stop = false;
            do
            {
                ConsoleHelper.ShowUpdate("Enter Estimates Values: ");
                Console.Write("Optimistic  : ");
                var optimistic = ConsoleHelper.GetIntegerInput();

                Console.Write("MostLikely  : ");
                var mostLikely = ConsoleHelper.GetIntegerInput();

                Console.Write("Pessimistic : ");
                var pessimistic = ConsoleHelper.GetIntegerInput();

                var estimate = await contentManager.CalculateEstimate(optimistic, mostLikely, pessimistic);
                estimates.Add(estimate.Convert());

                Console.Write($"Calculated : {estimate.Calculated}");
                bool invalidInput;
                do
                {
                    ConsoleHelper.ShowUpdate("Do you want to calculate another estimate? (Y/N): ");
                    (var exit, invalidInput) = ConsoleHelper.GetYesNoInput();
                    if (invalidInput)
                    {
                        stop = exit;
                    }
                } while (! invalidInput);
            } while (stop);

            ConsoleHelper.ShowAsTable(estimates);
            ConsoleHelper.ShowFooter();
            ConsoleHelper.ShowExit();
            logger.LogInformation("Console Client Finished");
            return await Task.FromResult(0);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in Console Client");
            return await Task.FromResult(1);
        }

    }

}