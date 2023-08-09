using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

#pragma warning disable CS8618

namespace website.Data;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Player
{
    [JsonProperty("nickname")] public string Nickname { get; set; }
    [JsonProperty("teamname")] public string? TeamName { get; set; }
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }
}