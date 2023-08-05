using RestService.Entities.Atlas;

namespace RestService.Entities.ResponseObjects;

public class RecordingRequest
{
    public SessionStatisticsPlain SessionStatisticsPlain { get; set; }
    public string PlayerName { get; set; }
    public List<Snapshot> Snapshots { get; set; }
    public string EventId { get; set; }
}