using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Dtos.RequestObjects;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : BaseController
{
    private readonly IMongoCollection<Player> _playersCollection;
    private readonly IMongoCollection<PlayerUnique> _playersUniqueCollection;

    public PlayersController(ILogger<PlayersController> logger) : base(logger)
    {
        _playersCollection = Database!.GetCollection<Player>(Constants.PlayersCollectionName);
        _playersUniqueCollection = Database!.GetCollection<PlayerUnique>(Constants.PlayersUniqueCollectionName);
    }

    [HttpGet("All", Name = "GetPlayers")]
    public async Task<List<PlayerResponse>> GetPlayers()
    {
        Logger.LogDebug($"Route {nameof(GetPlayers)} called.");

        var players = await _playersCollection.FindAsync(new BsonDocument());
        var playersResponse =
            players.ToList().Select(player => new PlayerResponse(player)).ToList();

        return playersResponse;
    }

    [HttpGet("Unique", Name = "GetPlayersUnique")]
    public async Task<List<PlayerUniqueResponse>> GetPlayersUnique()
    {
        Logger.LogDebug($"Route {nameof(GetPlayersUnique)} called.");

        var playersUnique = await _playersUniqueCollection.FindAsync(new BsonDocument());
        var playersUniqueList = playersUnique.ToList();
        var playersResponse = playersUniqueList.Select(player => new PlayerUniqueResponse(player)).ToList();

        return playersResponse;
    }

    [HttpPost(Name = "CreatePlayer")]
    public async Task<ActionResult<PlayerResponse>> CreatePlayer(PlayerRequest playerRequest)
    {
        Logger.LogDebug($"Route {nameof(CreatePlayer)} called.");

        var player = new Player(playerRequest);
        await _playersCollection.InsertOneAsync(player);

        var playerUnique = new PlayerUnique(player);
        await _playersUniqueCollection.InsertOneAsync(playerUnique);

        return CreatedAtRoute("GetPlayers", null, null);
    }
}