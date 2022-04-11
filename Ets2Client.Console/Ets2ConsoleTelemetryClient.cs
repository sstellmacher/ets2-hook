using Ets2Client.Telemetry;
using Ets2SdkClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Ets2Client.Console
{
    class Ets2ConsoleTelemetryClient : BackgroundService, IEts2TelemetryClient
    {
        private readonly ILogger<Ets2ConsoleTelemetryClient> logger;
        private readonly IEts2TelemetryProvider provider;

        private readonly int workerInterval;
        private readonly string apiKey;
        private readonly string apiUrl;

        public Ets2ConsoleTelemetryClient(IConfiguration config, IEts2TelemetryProvider provider, ILogger<Ets2ConsoleTelemetryClient> logger)
        {
            this.provider = provider;
            this.logger = logger;

            apiUrl = config.GetSection("API").GetValue<string>("URL");
            apiKey = config.GetSection("API").GetValue<string>("Key");
            workerInterval = config.GetSection("Worker").GetValue<int>("Interval");
        }

        public async Task SendData(Ets2Telemetry data)
        {
            var request = WebRequest.Create(apiUrl);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + apiKey;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(data);

                streamWriter.Write(json);
            }

            var response = await request.GetResponseAsync();
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                logger.LogInformation("Response: {0}", responseText);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("Reading Telemetry Data.");

                    var data = await provider.GetTelemetryInfoAsync();
                    
                    logger.LogInformation("Sending Telemetry Data.");

                    await SendData(data);

                    logger.LogInformation("Telemetry Data Send.");

                    await Task.Delay(workerInterval);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{ex.Message}");
                }
            }
        }
    }
}
