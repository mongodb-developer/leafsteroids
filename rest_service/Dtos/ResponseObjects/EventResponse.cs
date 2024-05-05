using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

public class EventResponse
{
    [BsonElement("_id")] public string? Id { get; set; }
    [BsonElement("name")] public string? Name { get; set; }
    [BsonElement("location")] public string? Location { get; set; }
    [BsonElement("emailRequired")] public bool EmailRequired { get; set; }

    public EventResponse(Event @event)
    {
        Id = @event.Id;
        Name = @event.Name;
        Location = @event.Location;
        EmailRequired = @event.EmailRequired;
    }
}