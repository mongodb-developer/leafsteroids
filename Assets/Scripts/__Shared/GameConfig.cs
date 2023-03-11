using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace __Shared
{
    public class GameConfig
    {
        public float BulletDamage;
        public float BulletLifespan;
        public float BulletSpeed;
        [BsonId] public ObjectId Id;
        public float PlayerMoveSpeed;
        public float PlayerRotateSpeed;
        public float RoundDuration;
    }
}