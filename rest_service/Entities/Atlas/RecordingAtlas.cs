using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities.ResponseObjects;

namespace RestService.Entities.Atlas;

public class RecordingAtlas
{
    [BsonElement("location")] public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime? DateTime { get; set; }
    public PlayerForRecordingsAtlas Player { get; set; }
    public List<SnapshotRequest>? Snapshots { get; set; }
    public EventForRecordingsAtlas Event { get; set; }

    public RecordingAtlas(RecordingRequest recordingRequest)
    {
        SessionStatisticsPlain = recordingRequest.SessionStatisticsPlain;
        DateTime = System.DateTime.UtcNow;
        Player = new PlayerForRecordingsAtlas { Nickname = recordingRequest.PlayerName };
        Snapshots = recordingRequest.Snapshots;
        Event = new EventForRecordingsAtlas { Id = recordingRequest.EventId };
    }
}