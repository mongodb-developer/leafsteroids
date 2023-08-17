using System.Diagnostics.CodeAnalysis;
using RestService.Entities;

namespace RestService.Dtos.RequestObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class SnapshotRequest
{
    public Position? Position { get; set; }
}