using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : BaseController
{
    private readonly IMongoCollection<Player> _playersCollection;

    public PlayersController(ILogger<PlayersController> logger) : base(logger)
    {
        _playersCollection = Database!.GetCollection<Player>(Constants.PlayersCollectionName);
    }

    [HttpGet(Name = "GetPlayers")]
    public async Task<PlayerResponse[]> GetPlayers()
    {
        Logger.LogDebug($"Route {nameof(GetPlayers)} called.");

        var players = await _playersCollection.FindAsync(new BsonDocument());
        var playersResponse =
            players.ToList().Select(player => new PlayerResponse(player)).ToArray();

        return playersResponse;
    }
}