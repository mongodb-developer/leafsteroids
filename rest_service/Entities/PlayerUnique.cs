using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;


namespace RestService.Entities
{
    public class PlayerUnique
    {
        [BsonId]
        [JsonProperty("_id")]
        public PlayerUniqueId Id { get; set; }

        [JsonProperty("nickname")]
        [BsonIgnore]
        public string? Name
        {
            get { return Id.Name; }
            set { Id.Name = value; }
        }

        [JsonProperty("location")]
        [BsonIgnore]
        public string? Location {
            get { return Id.Location; }
            set { Id.Location = value; }
        }

        public PlayerUnique() { Id = new PlayerUniqueId(); }

        public PlayerUnique(string nickname, string location) : this()
        {
            Name = nickname;
            Location = location;
        }
    }

    public class PlayerUniqueId
    {
        [BsonElement("Nickname")]
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("location")]
        [BsonElement("location")]
        public string? Location { get; set; }
    }
}