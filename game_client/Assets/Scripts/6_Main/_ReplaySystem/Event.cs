using Newtonsoft.Json;

namespace _6_Main._ReplaySystem
{
    public class Event
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
    }
}