using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;


namespace RestService.Entities
{
    public class PlayerUnique
    {
        [BsonId]
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        [BsonElement("location")]
        public string? Location { get; set; }
    }
}