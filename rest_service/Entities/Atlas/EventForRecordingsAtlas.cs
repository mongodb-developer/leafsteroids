using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities.Atlas;

public class EventForRecordingsAtlas
{
    [BsonElement("_id")] public string? Id { get; set; }
}