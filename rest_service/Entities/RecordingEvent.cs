using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class RecordingEvent : Event
{
    // This class is empty intentionally.
    // Its purpose is to override the serialization to store a subset of the parent class
    // Thus implementing the Extended Reference Schema Design Pattern:
    // https://www.mongodb.com/blog/post/building-with-patterns-the-extended-reference-pattern 
}

public class RecordingEventSerializer : SerializerBase<RecordingEvent>, IBsonDocumentSerializer
{
    public override RecordingEvent Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var deserialized = BsonDocumentSerializer.Instance.Deserialize(context);
        return new RecordingEvent()
        {
            Id = deserialized.GetValue("_id").ToString()
        };
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, RecordingEvent value)
    {
        var document = new BsonDocument
            {
                { "_id", value.Id }
            };

        BsonDocumentSerializer.Instance.Serialize(context, document);
    }

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo? serializationInfo)
    {
        switch (memberName)
        {
            case "Id":
                serializationInfo = new BsonSerializationInfo("_id", ObjectIdSerializer.Instance, typeof(ObjectId));
                return true;
            case "Name":
                serializationInfo = new BsonSerializationInfo("name", StringSerializer.Instance, typeof(string));
                return true;
            case "Location":
                serializationInfo = new BsonSerializationInfo("location", StringSerializer.Instance, typeof(string));
                return true;
            default:
                serializationInfo = null;
                return false;
        }
    }
}