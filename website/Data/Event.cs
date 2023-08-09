using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

#pragma warning disable CS8618

namespace website.Data;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Event
{
    [JsonProperty("id")] public string Id { get; set; }
    public string? name { get; set; }
    public string? location { get; set; }
}