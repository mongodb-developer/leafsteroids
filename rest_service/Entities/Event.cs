using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class Event
{
    [BsonElement("_id")] public string? Id { get; set; }
    
    [BsonElement("name")] public string? Name { get; set; }
    
    [BsonElement("location")] public string? Location { get; set; }
    
    [BsonIgnoreIfDefault]
    [BsonElement("emailRequired")] public bool EmailRequired { get; set; }
}