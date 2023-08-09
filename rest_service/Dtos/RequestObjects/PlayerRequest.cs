using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

#pragma warning disable CS8618

namespace RestService.Dtos.RequestObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class PlayerRequest
{
    [JsonProperty("nickname")] public string Nickname { get; set; }
    [JsonProperty("teamname")] public string? TeamName { get; set; }
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }
}