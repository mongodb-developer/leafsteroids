using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class Player
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    [BsonElement("Nickname")] public string? Name { get; set; }
    [BsonElement("TeamName")] public string? Team { get; set; }
    [BsonElement("Email")] public string? Email { get; set; }
    [BsonElement("location")]
    public string? Location { get; set; }
}