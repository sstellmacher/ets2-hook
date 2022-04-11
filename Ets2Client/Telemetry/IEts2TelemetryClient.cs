using Ets2SdkClient;
using System.Threading.Tasks;

namespace Ets2Client.Telemetry
{
    public interface IEts2TelemetryClient
    {
        Task SendData(Ets2Telemetry data);
    }
}
