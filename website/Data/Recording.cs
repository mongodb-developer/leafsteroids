using Newtonsoft.Json;

namespace website.Data;

public class Recording
{
    [JsonProperty("location")] public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime DateTime { get; set; }
    public Player? Player { get; set; }
    public Event? Event { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
}