using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities.Atlas;

namespace RestService.Entities
{
    public class EventResponse
    {
        [BsonElement("_id")] public string? Id { get; set; }

        [BsonElement("name")] public string? Name { get; set; }

        public EventResponse(EventAtlas eventAtlas)
        {
            Id = eventAtlas.Id;
            Name = eventAtlas.Name;
        }
    }
}