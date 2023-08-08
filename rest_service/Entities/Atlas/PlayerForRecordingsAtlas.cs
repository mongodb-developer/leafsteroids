using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace RestService.Entities.Atlas;

public class PlayerForRecordingsAtlas : PlayerAtlas
{
    // This class is empty intentionally.
    // Its purpose is to override the serialization to store a subset of the parent class
    // Thus implementing the Extended Reference Schema Design Pattern:
    // https://www.mongodb.com/blog/post/building-with-patterns-the-extended-reference-pattern 
}

public class PlayerForRecordingsSerializer : SerializerBase<PlayerForRecordingsAtlas>
{
    public override PlayerForRecordingsAtlas Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return BsonSerializer.Deserialize<PlayerForRecordingsAtlas>(context.Reader);
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, PlayerForRecordingsAtlas value)
    {
        var document = new BsonDocument
            {
                { "_id", value.Id },
                { "Nickname", value.Nickname }
            };
        
        BsonDocumentSerializer.Instance.Serialize(context, document);
    }
}