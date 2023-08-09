using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using RestService.Dtos.RequestObjects;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class Player
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    public string? Nickname { get; set; }
    public string? TeamName { get; set; }
    public string? Email { get; set; }

    [JsonProperty("location")]
    [BsonElement("location")]
    public string? Location { get; set; }

    public Player(PlayerRequest request)
    {
        Id = ObjectId.GenerateNewId();
        Nickname = request.Nickname;
        TeamName = request.TeamName;
        Email = request.Email;
        Location = request.Location;
    }

    protected Player()
    {
        throw new NotImplementedException();
    }
}