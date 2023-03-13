using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace _00_Shared
{
    public class RegisteredPlayer
    {
        [BsonId] public ObjectId Id;
        public string Nickname;
        public string Slogan;
        public string TeamName;
    }
}