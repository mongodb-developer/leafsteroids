using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class PlayerResponse
{
    [JsonProperty("_id")] public string? Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("team")] public string? Team { get; set; }
    [JsonProperty("email")] public string? Email { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }

    public PlayerResponse(Player player)
    {
        Id = player.Id.ToString();
        Name = player.Name;
        Email = player.Email;
        Team = player.Team;
        Location = player.Location;
    }

    public PlayerResponse(PlayerUnique player)
    {
        Id = player.Id.ToString();
        Name = player.Name;
        Location = player.Location;
    }
}