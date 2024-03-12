using Newtonsoft.Json;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

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
}