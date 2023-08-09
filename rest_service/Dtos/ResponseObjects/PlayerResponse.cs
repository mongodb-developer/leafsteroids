using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects
{
    public class PlayerResponse
    {
        [BsonElement("_id")] public string? Id { get; set; }
        [BsonElement("name")] public string? Name { get; set; }

        public PlayerResponse(Player player)
        {
            Id = player.Id.ToString();
            Name = player.Nickname;
        }
    }
}