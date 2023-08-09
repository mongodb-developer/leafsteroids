using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace website.Data;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class PlayerUnique
{
    public string? Nickname { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }
}