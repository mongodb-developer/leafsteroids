using Newtonsoft.Json;

namespace RestService.Dtos.RequestObjects;

public class EventRequest
{
    [JsonProperty("id")] public string? Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }
    [JsonProperty("emailRequired")] public bool EmailRequired { get; set; }
}