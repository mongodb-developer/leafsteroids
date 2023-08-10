using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using RestService.Dtos.RequestObjects;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Player
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    [BsonElement("Nickname")] public string? Name { get; set; }
    [BsonElement("TeamName")] public string? Team { get; set; }
    [BsonElement("Email")] public string? Email { get; set; }

    [JsonProperty("location")]
    [BsonElement("location")]
    public string? Location { get; set; }

    public Player(PlayerRequest request)
    {
        Id = ObjectId.GenerateNewId();
        Name = request.Name;
        Team = request.Team;
        Email = request.Email;
        Location = request.Location;
    }

    protected Player()
    {
    }
}