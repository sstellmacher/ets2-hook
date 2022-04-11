using Ets2SdkClient;
using System.Threading.Tasks;

namespace Ets2Client.Telemetry
{
    public interface IEts2TelemetryProvider
    {
        Task<Ets2Telemetry> GetTelemetryInfoAsync();
    }
}
