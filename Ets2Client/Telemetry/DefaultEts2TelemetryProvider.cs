using Ets2SdkClient;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Ets2Client.Telemetry
{
    public class DefaultEts2TelemetryProvider : IEts2TelemetryProvider
    {
        private readonly Ets2SdkTelemetry telemetry = new Ets2SdkTelemetry();
        private readonly ILogger<DefaultEts2TelemetryProvider> logger;

        private Ets2Telemetry telemetryData;
        private bool newTimestamp = false;

        public DefaultEts2TelemetryProvider(ILogger<DefaultEts2TelemetryProvider> logger)
        {
            this.logger = logger;
            telemetry.Data += Telemetry_Data;
        }

        private void Telemetry_Data(Ets2Telemetry telemetryData, bool newTimestamp)
        {
            this.telemetryData = telemetryData;
            this.newTimestamp = newTimestamp;
        }

        public async Task<Ets2Telemetry> GetTelemetryInfoAsync()
        {
            if (telemetry.Error != null)
                throw telemetry.Error;

            logger.LogInformation("Waiting for new data...");
            while (!newTimestamp)
            {
                await Task.Yield();
            }

            newTimestamp = false;
            logger.LogInformation("Data received.");

            return telemetryData;
        }
    }
}
