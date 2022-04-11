using Ets2Client.Telemetry.Data;
using System.Threading.Tasks;

namespace Ets2Client.Telemetry
{
    public interface IEts2TelemetryClient
    {
        Task SendData(Ets2Telemetry data);
    }
}
