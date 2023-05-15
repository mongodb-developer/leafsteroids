using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gdc_2023_demo_registration.Data;

public class PlayerUnique
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string? Nickname { get; set; }
    public string? location { get; set; }
}