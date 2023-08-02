using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities.Atlas;

public class EventAtlas
{
    [BsonElement("_id")] public string? Id { get; set; }
    [BsonElement("name")] public string? Name { get; set; }
    [BsonElement("location")] public string? Location { get; set; }
}