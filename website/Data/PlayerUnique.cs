using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace website.Data;

public class PlayerUnique
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string? Nickname { get; set; }
    public string? location { get; set; }
}