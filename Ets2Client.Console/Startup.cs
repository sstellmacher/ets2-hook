using Ets2Client.Telemetry;
using Ets2Client.Telemetry.Defaults;
using Ets2Client.Telemetry.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Ets2Client.Console
{
    class Startup
    {
        private static Task Main(string[] args) =>
            CreateHostBuilder(args)
            .Build()
            .RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder =>
                builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build())
            .ConfigureServices(services =>
                services
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                    loggingBuilder.AddNLog();
                })
                .AddTransient<IEts2TelemetryProvider, DefaultEts2TelemetryProvider>()
                .AddTransient<IEts2TelemetryApiClient, DefaultEts2TelemetryWebClient>()
                .AddHostedService<Ets2ConsoleTelemetryWorker>());
    }
}
