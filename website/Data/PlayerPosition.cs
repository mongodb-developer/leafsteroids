using System.Diagnostics.CodeAnalysis;

namespace website.Data;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class PlayerPosition
{
    public double? X { get; set; }
    public double? Y { get; set; }
    public double? Z { get; set; }
}