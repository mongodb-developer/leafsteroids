using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson.Serialization.Attributes;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
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