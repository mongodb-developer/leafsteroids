using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Player
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    public string? Nickname { get; set; }
    public string? TeamName { get; set; }
    public string? Email { get; set; }

    [JsonProperty("location")]
    [BsonElement("location")]
    public string? Location { get; set; }
}