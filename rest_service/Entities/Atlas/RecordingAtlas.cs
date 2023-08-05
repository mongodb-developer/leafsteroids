using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities.ResponseObjects;

namespace RestService.Entities.Atlas;

public class RecordingAtlas
{
    // [BsonElement("_id")] public ObjectId? Id { get; set; }
    [BsonElement("location")] public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime? DateTime { get; set; }
    public string? PlayerName { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
    public string? EventId { get; set; }

    public RecordingAtlas(RecordingRequest recordingRequest)
    {
        SessionStatisticsPlain = recordingRequest.SessionStatisticsPlain;
        DateTime = System.DateTime.UtcNow;
        PlayerName = recordingRequest.PlayerName;
        Snapshots = recordingRequest.Snapshots;
        EventId = recordingRequest.EventId;
    }
}