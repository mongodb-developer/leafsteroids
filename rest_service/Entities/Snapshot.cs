using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618

namespace RestService.Entities;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Snapshot
{
    public Position Position { get; set; }
}