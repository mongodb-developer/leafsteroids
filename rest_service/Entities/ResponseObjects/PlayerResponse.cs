using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities.Atlas;

namespace RestService.Entities.ResponseObjects
{
    public class PlayerResponse
    {
        [BsonElement("_id")] public string? Id { get; set; }
        [BsonElement("name")] public string? Name { get; set; }

        public PlayerResponse(PlayerAtlas playerAtlas)
        {
            Id = playerAtlas.Id.ToString();
            Name = playerAtlas.Nickname;
        }
    }
}