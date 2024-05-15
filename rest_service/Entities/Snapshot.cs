using MongoDB.Bson.Serialization.Attributes;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class Snapshot
{
    public Position Position { get; set; }

    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
}