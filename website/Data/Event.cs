using Newtonsoft.Json;

namespace website.Data;

public class Event
{
    [JsonProperty("id")] public string? Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }
}