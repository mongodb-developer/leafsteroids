using System.Diagnostics.CodeAnalysis;

namespace website.Data;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class Snapshot
{
    public PlayerPosition? PlayerPosition { get; set; }
}