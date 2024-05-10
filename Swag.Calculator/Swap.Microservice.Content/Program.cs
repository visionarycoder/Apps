using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Swag.Access.Data.Interface;
using Swag.Access.Data.Service;
using Swag.Client.ConsoleApp.Interface;
using Swag.Client.ConsoleApp.Service;
using Swag.Engine.Calculator.Interface;
using Swag.Engine.Calculator.Service;
using Swag.Manager.Content.Interface;
using Swag.Manager.Content.Service;
using Swag.Resource.Data.SwagDb;

var host = new HostBuilder()
    .ConfigureDefaults(args)
    .ConfigureHostConfiguration(builder =>
    {
        // ToDo: Add Host Configuration
    })
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("AppSettings.json", optional: true);
        builder.AddJsonFile($"AppSettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
    })
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
        builder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "[HH:mm:ss] ";
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.UseUtcTimestamp = false;
        });
    })
    .ConfigureServices((context, services) =>
    {
        var options = new DbContextOptionsBuilder<SwagContext>()
            .UseSqlite(context.Configuration.GetConnectionString("SwagDb"))
            .EnableDetailedErrors(true)
            .EnableServiceProviderCaching(false)
            .EnableSensitiveDataLogging(true)
            .Options;
        services.AddSingleton(options);
        services.AddHealthChecks();
        services.AddScoped<IConsoleClient, ConsoleClient>();
        services.AddScoped<IDataAccess, DataAccess>();
        services.AddScoped<ICalculatorEngine, CalculatorEngine>();
        services.AddScoped<IContentManager, ContentManager>();

    })
    .Build();

var client = host.Services.GetRequiredService<IConsoleClient>();
client.Run();