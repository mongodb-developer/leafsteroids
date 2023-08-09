using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Event
{
    [BsonElement("_id")] public string? Id { get; set; }
    [BsonElement("name")] public string? Name { get; set; }
    [BsonElement("location")] public string? Location { get; set; }
}