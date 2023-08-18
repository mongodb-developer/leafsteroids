using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Recording
{
    [BsonElement("location")] public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime? DateTime { get; set; }
    public RecordingPlayer Player { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
    public RecordingEvent Event { get; set; }
}