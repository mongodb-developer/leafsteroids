using System.Diagnostics.CodeAnalysis;
using RestService.Entities;

#pragma warning disable CS8618

namespace RestService.Dtos.RequestObjects;

[SuppressMessage("ReSharper", "CollectionNeverUpdated.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class RecordingRequest
{
    public SessionStatisticsPlain SessionStatisticsPlain { get; set; }
    public string PlayerName { get; set; }
    public List<SnapshotRequest> Snapshots { get; set; }
    public string EventId { get; set; }
}