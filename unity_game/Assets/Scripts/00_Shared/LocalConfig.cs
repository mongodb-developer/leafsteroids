using Newtonsoft.Json;

namespace _00_Shared
{
    public class LocalConfig
    {
        [JsonProperty("GAME_SERVER_IP")] public string GameServerIp { get; set; }
        [JsonProperty("GAME_SERVER_PORT")] public string GameServerPort { get; set; }
    }
}