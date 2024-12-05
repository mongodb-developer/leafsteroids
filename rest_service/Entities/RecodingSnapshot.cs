using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using System.Text.Json;
namespace RestService.Entities;
[BsonIgnoreExtraElements]
public class RecordingSnapshot : Snapshot
{
    // This class is empty intentionally.
    // Its purpose is to override the serialization to store a subset of the parent class
    // Thus implementing the Extended Reference Schema Design Pattern:
    // https://www.mongodb.com/blog/post/building-with-patterns-the-extended-reference-pattern
}
public class SnapshotSerializer : SerializerBase<RecordingSnapshot>, IBsonDocumentSerializer
{

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RecordingSnapshot value)
    {
        string jsonString = JsonSerializer.Serialize(value.Position);
        var document = new BsonDocument
            {
                { "Position", jsonString },
                { "SessionStatisticsPlain",null }
            };

        BsonDocumentSerializer.Instance.Serialize(context, document);
    }

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo? serializationInfo)
    {
        switch (memberName)
        {
            case "Position":
                serializationInfo = new BsonSerializationInfo("Position", ObjectIdSerializer.Instance, typeof(Position));
                return true;
            default:
                serializationInfo = null;
                return false;
        }
    }
}