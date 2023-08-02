using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities.Atlas;

public class PlayerAtlas
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    public string? Nickname { get; set; }
    public string? TeamName { get; set; }
    public string? Email { get; set; }
    [BsonElement("location")] public string? Location { get; set; }
}