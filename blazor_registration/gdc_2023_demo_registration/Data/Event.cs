using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace gdc_2023_demo_registration.Data;

public class Event
{
    [BsonId]
    public string Id { get; set; }
    public string? name { get; set; }
    public string? location { get; set; }
}