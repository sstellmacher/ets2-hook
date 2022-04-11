using Ets2SdkClient;
using System.Threading.Tasks;

namespace Ets2Client.Telemetry
{
    public class DefaultEts2TelemetryProvider : IEts2TelemetryProvider
    {
        // todo

        public async Task<Ets2Telemetry> GetTelemetryInfoAsync()
        {
            await Task.Yield();
            return null;
        }
    }
}
