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
        Email = MaskEmail(player.Email ?? string.Empty);
        Team = player.Team;
        Location = player.Location;
    }

    public string MaskEmail(string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            return email;

        string[] emailArr = email.Split('@');
        string domainExt = Path.GetExtension(email);

        string maskedEmail = string.Format("{0}****{1}@{2}****{3}{4}",
            emailArr[0][0],
            emailArr[0].Substring(emailArr[0].Length - 1),
            emailArr[1][0],
            emailArr[1].Substring(emailArr[1].Length - domainExt.Length - 1, 1),
            domainExt
            );

        return maskedEmail;
    }
}