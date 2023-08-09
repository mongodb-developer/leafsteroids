using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects
{
    public class EventResponse
    {
        [BsonElement("_id")] public string? Id { get; set; }
        [BsonElement("name")] public string? Name { get; set; }

        public EventResponse(Event evt)
        {
            Id = evt.Id;
            Name = evt.Name;
        }
    }
}