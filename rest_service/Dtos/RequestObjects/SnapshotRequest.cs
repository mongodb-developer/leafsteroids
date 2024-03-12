using RestService.Entities;

namespace RestService.Dtos.RequestObjects;

public class SnapshotRequest
{
    public Position? Position { get; set; }
}