using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

public class Config
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    public float RoundDuration { get; set; }
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float PlayerMoveSpeed { get; set; }
    public float PlayerRotateSpeed { get; set; }
    public float BulletLifespan { get; set; }
    [BsonElement("PelletHeatlhSmall")] public float PelletHealthSmall { get; set; }
    [BsonElement("PelletHeatlhMedium")] public float PelletHealthMedium { get; set; }
    [BsonElement("PelletHeatlhLarge")] public float PelletHealthLarge { get; set; }
}