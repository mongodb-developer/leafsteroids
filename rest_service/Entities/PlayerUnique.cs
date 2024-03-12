using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class PlayerUnique
{
    [BsonId]
    public string Name { get; set; }

    [BsonElement("location")]
    public string? Location { get; set; }
}