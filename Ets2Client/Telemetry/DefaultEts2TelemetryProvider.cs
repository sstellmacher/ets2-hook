using Ets2SdkClient;
using Microsoft.Extensions.Logging;
using System;

namespace Ets2Client.Telemetry
{
    public class DefaultEts2TelemetryProvider : IEts2TelemetryProvider
    {
        private readonly SharedMemory sharedMemory = new SharedMemory();
        private readonly ILogger<DefaultEts2TelemetryProvider> logger;

        public DefaultEts2TelemetryProvider(ILogger<DefaultEts2TelemetryProvider> logger)
        {
            this.logger = logger;
        }

        public bool Connect(string mapName)
        {
            return sharedMemory.Connect(mapName);
        }

        public Ets2Telemetry GetTelemetry()
        {
            if (!sharedMemory.Connected)
                throw new InvalidOperationException(nameof(SharedMemory) + " not connected.");

            var rawData = sharedMemory.ReadRawData();
            var etsRaw = sharedMemory.ToObject<Ets2TelemetryData>(rawData);

            return ConvertToTelemetry(etsRaw);
        }

        private Ets2Telemetry ConvertToTelemetry(Ets2TelemetryData etsRaw)
        {
            // todo
            throw new NotImplementedException();
        }
    }
}
