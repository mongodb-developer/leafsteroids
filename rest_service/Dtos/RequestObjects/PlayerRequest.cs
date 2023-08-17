using Newtonsoft.Json;

namespace RestService.Dtos.RequestObjects;

public class PlayerRequest
{
    [JsonProperty("id")] public string? Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("team")] public string? Team { get; set; }
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }
}