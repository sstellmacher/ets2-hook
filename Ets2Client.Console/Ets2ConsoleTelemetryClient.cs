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
        private readonly IConfiguration config;

        public Ets2ConsoleTelemetryClient(IConfiguration config, IEts2TelemetryProvider provider, ILogger<Ets2ConsoleTelemetryClient> logger)
        {
            this.config = config;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task SendData(Ets2Telemetry data)
        {
            var apiConfig = config.GetSection("Api");
            var request = WebRequest.Create(apiConfig.GetValue<string>("URL"));
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + apiConfig.GetValue<string>("Key");

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

                    await Task.Delay(2000); // todo
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"{ex.Message}");
                }
            }
        }
    }
}
