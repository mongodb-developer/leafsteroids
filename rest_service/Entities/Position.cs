using System.Diagnostics.CodeAnalysis;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Position
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}