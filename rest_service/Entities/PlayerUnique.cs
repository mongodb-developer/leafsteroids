using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace RestService.Entities;

public class PlayerUnique
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    [BsonElement("Nickname")] public string? Name { get; set; }

    [JsonProperty("location")]
    [BsonElement("location")]
    public string? Location { get; set; }

    public PlayerUnique(Player player)
    {
        Id = player.Id;
        Name = player.Name;
        Location = player.Location;
    }

    protected PlayerUnique()
    {
    }
}