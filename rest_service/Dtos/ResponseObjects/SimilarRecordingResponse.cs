using Newtonsoft.Json;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

public class SimilarRecordingResponse
{
    [JsonProperty("_id")]
    public string? Id { get; set; }
    [JsonProperty("sessionStatisticsPlain")]
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }

    public SimilarRecordingResponse(Recording recording)
    {
        Id = recording.Id.ToString();
        SessionStatisticsPlain = recording.SessionStatisticsPlain;
        Name = recording.Player.Name;
    }
}

