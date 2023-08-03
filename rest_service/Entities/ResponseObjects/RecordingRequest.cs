using RestService.Entities.Atlas;

namespace RestService.Entities.ResponseObjects;

public class RecordingRequest
{
    public SessionStatisticsPlain SessionStatisticsPlain { get; set; }
    public DateTime DateTime { get; set; }
    public string PlayerName { get; set; }
    public PlayerPosition[] Snapshots { get; set; }
    public string EventId { get; set; }
}