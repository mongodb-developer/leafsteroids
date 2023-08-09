using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace RestService.Entities;

public class RecordingEvent : Event
{
    // This class is empty intentionally.
    // Its purpose is to override the serialization to store a subset of the parent class
    // Thus implementing the Extended Reference Schema Design Pattern:
    // https://www.mongodb.com/blog/post/building-with-patterns-the-extended-reference-pattern 
}

public class EventForRecordingSerializer : SerializerBase<RecordingEvent>
{
    public override RecordingEvent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return BsonSerializer.Deserialize<RecordingEvent>(context.Reader);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RecordingEvent value)
    {
        var document = new BsonDocument
            {
                { "_id", value.Id }
            };

        BsonDocumentSerializer.Instance.Serialize(context, document);
    }
}