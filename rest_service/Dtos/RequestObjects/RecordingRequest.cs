using System.Diagnostics.CodeAnalysis;
using RestService.Entities;

namespace RestService.Dtos.RequestObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
public class RecordingRequest
{
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public string? PlayerName { get; set; }
    public List<SnapshotRequest>? Snapshots { get; set; }
    public string? EventId { get; set; }
}