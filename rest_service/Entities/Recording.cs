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
    public List<RecordingSnapshot>? Snapshots { get; set; }
    public RecordingEvent Event { get; set; }
    [BsonElement("speed_vector")]
    public double[]? SpeedVector { get; set; }
    [BsonElement("accel_vector")]
    public double[]? AccelVector { get; set; }
    [BsonElement("score_vector")]
    public double[]? ScoreVector { get; set; }
    [BsonElement("scoreCumulative_vector")]
    public double[]? ScoreCumulativeVector { get; set; }
    [BsonElement("ratios_vector")]
    public double[]? RatiosVector { get; set; }
    [BsonElement("average_score_per_bullet_vector")]
    public double[]? AverageScorePerBulletVector { get; set; }
    [BsonElement("average_damage_per_bullet_vector")]
    public double[]? AverageDamagePerBulletVector { get; set; }
    [BsonElement("average_pellets_per_bullet_vector")]
    public double[]? AveragePelletsPerBulletVector { get; set; }
    [BsonElement("stats_vector")]
    public double[]? StatsVector { get; set; }
    [BsonElement("similarity_vector")]
    public double[]? SimilarityVector { get; set; }
}