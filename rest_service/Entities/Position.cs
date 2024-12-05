using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class Position
{
    public double X { get; set; }
    public double Z { get; set; }
}