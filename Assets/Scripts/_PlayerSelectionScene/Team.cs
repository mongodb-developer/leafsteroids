using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _PlayerSelectionScene
{
    public class Player
    {
        [BsonId] public ObjectId Id { get; set; }
        [BsonElement("nickname")] public string Nickname { get; set; }
        [BsonElement("team")] public string Team { get; set; }
        [BsonElement("slogan")] public string Slogan { get; set; }
    }
}