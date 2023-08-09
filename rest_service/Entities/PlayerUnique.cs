using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using RestService.Dtos.RequestObjects;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class PlayerUnique
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    public string? Nickname { get; set; }

    [JsonProperty("location")]
    [BsonElement("location")]
    public string? Location { get; set; }

    public PlayerUnique(PlayerRequest request)
    {
        Id = ObjectId.GenerateNewId();
        Nickname = request.Nickname;
        Location = request.Location;
    }
}