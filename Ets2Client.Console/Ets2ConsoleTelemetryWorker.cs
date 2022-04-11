using Ets2Client.Telemetry;
using Ets2Client.Telemetry.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ets2Client.Console
{
    class Ets2ConsoleTelemetryWorker : BackgroundService
    {
        private readonly ILogger<Ets2ConsoleTelemetryWorker> logger;
        private readonly IEts2TelemetryProvider provider;
        private readonly IEts2TelemetryApiClient client;

        private readonly int workerInterval;
        private readonly string memoryMapName;

        public Ets2ConsoleTelemetryWorker(IConfiguration config, IEts2TelemetryProvider provider, IEts2TelemetryApiClient client,
            ILogger<Ets2ConsoleTelemetryWorker> logger)
        {
            this.provider = provider;
            this.logger = logger;
            this.client = client;
            this.client.ApiUrl = config.GetSection("API").GetValue<string>("URL");
            this.client.ApiKey = config.GetSection("API").GetValue<string>("Key");

            workerInterval = config.GetSection("Worker").GetValue<int>("Interval");
            memoryMapName = config.GetSection("TelemetrySdk").GetValue<string>("MemoryMapName");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Connecting to memory...");
            while (!provider.Connect(memoryMapName))
            {
                await Task.Delay(workerInterval, stoppingToken);
            }

            logger.LogInformation("Memory connected.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("Reading Telemetry Data.");

                    var data = provider.GetTelemetry();
                    
                    logger.LogInformation("Sending Telemetry Data.");

                    await client.SendData(data);

                    logger.LogInformation("Telemetry Data Send.");

                    await Task.Delay(workerInterval, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{ex.Message}");
                }
            }
        }
    }
}
