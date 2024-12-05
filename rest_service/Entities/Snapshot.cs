using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using System.Text.Json;



namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class Snapshot
{
    public Position Position { get; set; }
    [BsonIgnore]
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
}
