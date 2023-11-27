using Newtonsoft.Json;

namespace _00_Shared
{
    public class LocalConfig
    {
        [JsonProperty("REST_SERVICE_IP")] public string RestServiceIp { get; set; }
        [JsonProperty("REST_SERVICE_PORT")] public string RestServicePort { get; set; }
        [JsonProperty("EVENT_ID")] public string EventId { get; set; }
        [JsonProperty("WEBSITE_URL")] public string WebsiteURL { get; set; }
    }
}