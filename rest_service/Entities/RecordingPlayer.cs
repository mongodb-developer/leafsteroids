using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace RestService.Entities;

[BsonIgnoreExtraElements]
public class RecordingPlayer : Player
{
    // This class is empty intentionally.
    // Its purpose is to override the serialization to store a subset of the parent class
    // Thus implementing the Extended Reference Schema Design Pattern:
    // https://www.mongodb.com/blog/post/building-with-patterns-the-extended-reference-pattern
}

public class RecordingPlayerSerializer : SerializerBase<RecordingPlayer>, IBsonDocumentSerializer
{
    public override RecordingPlayer Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var deserialized = BsonDocumentSerializer.Instance.Deserialize(context);
        return new RecordingPlayer()
        {
            Name = deserialized.GetValue("Nickname").ToString(),
            Location = deserialized.GetValue("location").ToString()
        };
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

    public bool TryGetMemberSerializationInfo(string memberName, out BsonSerializationInfo? serializationInfo)
    {
        switch (memberName)
        {
            case "Id":
                serializationInfo = new BsonSerializationInfo("_id", new ObjectIdSerializer(), typeof(ObjectId));
                return true;
            case "Name":
                serializationInfo = new BsonSerializationInfo("Nickname", new StringSerializer(), typeof(string));
                return true;
            case "Team":
                serializationInfo = new BsonSerializationInfo("TeamName", new StringSerializer(), typeof(string));
                return true;
            case "Email":
                serializationInfo = new BsonSerializationInfo("Email", new StringSerializer(), typeof(string));
                return true;
            case "Location":
                serializationInfo = new BsonSerializationInfo("location", new StringSerializer(), typeof(string));
                return true;
            default:
                serializationInfo = null;
                return false;
        }
    }
}