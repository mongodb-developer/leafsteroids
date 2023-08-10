using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class PlayerUniqueResponse
{
    [JsonProperty("_id")] public string? Id { get; set; }
    [JsonProperty("name")] public string? Name { get; set; }
    [JsonProperty("location")] public string? Location { get; set; }

    public PlayerUniqueResponse(Player player)
    {
        Id = player.Id.ToString();
        Name = player.Name;
        Location = player.Location;
    }

    public PlayerUniqueResponse(PlayerUnique player)
    {
        Id = player.Id.ToString();
        Name = player.Name;
        Location = player.Location;
    }
}