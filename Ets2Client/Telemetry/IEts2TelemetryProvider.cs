using Ets2Client.Telemetry.Data;

namespace Ets2Client.Telemetry
{
    public interface IEts2TelemetryProvider
    {
        bool Connect(string mapName);
        Ets2Telemetry GetTelemetry();
    }
}
