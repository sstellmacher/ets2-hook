namespace Ets2Client.Telemetry.Web
{
    public interface IEts2TelemetryApiClient : IEts2TelemetryClient
    {
        string ApiUrl { get; set; }
        string ApiKey { get; set; }
    }
}
