using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities.Atlas;

public class ConfigAtlas
{
    [BsonElement("_id")] public ObjectId? Id { get; set; }
    public int? RoundDuration { get; set; }
    public int? BulletDamage { get; set; }
    public int? BulletSpeed { get; set; }
    public int? PlayerMoveSpeed { get; set; }
    public int? PlayerRotateSpeed { get; set; }
    public int? BulletLifespan { get; set; }
    public int? PelletHeatlhSmall { get; set; }
    public int? PelletHeatlhLarge { get; set; }
    public int? PelletHeatlhMedium { get; set; }
}