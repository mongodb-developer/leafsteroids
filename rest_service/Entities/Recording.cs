using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class Recording
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    [BsonElement("location")]
    public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime? DateTime { get; set; }
    public RecordingPlayer Player { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
    public RecordingEvent Event { get; set; }
    [BsonElement("speed_vector")]
    public double[]? SpeedVector { get; set; }
    [BsonElement("accel_vector")]
    public double[]? AccelVector { get; set; }
    [BsonElement("stats_vector")]
    public double[]? StatsVector { get; set; }
    [BsonElement("similarity_vector")]
    public double[]? SimilarityVector { get; set; }
}