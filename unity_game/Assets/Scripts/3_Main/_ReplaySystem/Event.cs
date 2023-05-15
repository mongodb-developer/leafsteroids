using Newtonsoft.Json;

namespace _3_Main._ReplaySystem
{
    public class Event
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
    }
}