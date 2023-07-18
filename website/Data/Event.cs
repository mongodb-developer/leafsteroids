using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace website.Data;

public class Event
{
    [BsonId]
    public string Id { get; set; }
    public string? name { get; set; }
    public string? location { get; set; }

}