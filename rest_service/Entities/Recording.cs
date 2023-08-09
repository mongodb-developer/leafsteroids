using MongoDB.Bson.Serialization.Attributes;
using RestService.Dtos.ResponseObjects;

namespace RestService.Entities;

public class Recording
{
    [BsonElement("location")] public string? Location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime? DateTime { get; set; }
    public RecordingPlayer Player { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
    public RecordingEvent Event { get; set; }

    public Recording(RecordingRequest recordingRequest)
    {
        SessionStatisticsPlain = recordingRequest.SessionStatisticsPlain;

        DateTime = System.DateTime.UtcNow;

        Player = new RecordingPlayer { Nickname = recordingRequest.PlayerName };

        Event = new RecordingEvent { Id = recordingRequest.EventId };

        Snapshots = recordingRequest.Snapshots.Select(dto => new Snapshot
        {
            Position = new Position
            {
                X = dto.Position.X,
                Y = dto.Position.Y,
                Z = dto.Position.Z
            }
        }).ToList();
    }
}