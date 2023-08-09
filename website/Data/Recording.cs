using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace website.Data;

public class Recording
{
    public ObjectId Id { get; set; }
    [BsonElement("location")] public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime DateTime { get; set; }
    public Player? Player { get; set; }
    public Event? Event { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
}