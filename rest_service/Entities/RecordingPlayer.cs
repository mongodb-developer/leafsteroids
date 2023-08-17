using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace RestService.Entities;

public class RecordingPlayer : Player
{
    // This class is empty intentionally.
    // Its purpose is to override the serialization to store a subset of the parent class
    // Thus implementing the Extended Reference Schema Design Pattern:
    // https://www.mongodb.com/blog/post/building-with-patterns-the-extended-reference-pattern 
}

public class PlayerForRecordingsSerializer : SerializerBase<RecordingPlayer>
{
    public override RecordingPlayer Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return BsonSerializer.Deserialize<RecordingPlayer>(context.Reader);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RecordingPlayer value)
    {
        var document = new BsonDocument
            {
                { "Nickname", value.Name },
                { "location", value.Location }
            };
        
        BsonDocumentSerializer.Instance.Serialize(context, document);
    }
}