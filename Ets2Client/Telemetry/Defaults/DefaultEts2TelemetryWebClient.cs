using Ets2Client.Telemetry.Web;
using Ets2SdkClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Ets2Client.Telemetry.Defaults
{
    public class DefaultEts2TelemetryWebClient : IEts2TelemetryApiClient
    {
        private readonly ILogger<DefaultEts2TelemetryWebClient> logger;

        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
        public WebResponse LastResponse { get; private set; }

        public DefaultEts2TelemetryWebClient(ILogger<DefaultEts2TelemetryWebClient> logger)
        {
            this.logger = logger;
        }

        public async Task SendData(Ets2Telemetry data)
        {
            var request = WebRequest.Create(ApiUrl);
            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + ApiKey;

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
    }
}
