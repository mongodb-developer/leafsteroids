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

    [HttpGet(Name = "GetPlayers")]
    public async Task<List<PlayerResponse>> GetPlayers([FromQuery] PlayerRequest playerRequest)
    {
        Logger.LogDebug($"Route {nameof(GetPlayers)} called.");

        FilterDefinition<Player> filter = Builders<Player>.Filter.Empty;
        if (!string.IsNullOrEmpty(playerRequest.Name))
            filter &= Builders<Player>.Filter.Eq("Name", playerRequest.Name);
        if (!string.IsNullOrEmpty(playerRequest.Location))
            filter &= Builders<Player>.Filter.Eq("Location", playerRequest.Location);
        if (!string.IsNullOrEmpty(playerRequest.Team))
            filter &= Builders<Player>.Filter.Eq("Team", playerRequest.Team);
        if (!string.IsNullOrEmpty(playerRequest.Id))
            filter &= Builders<Player>.Filter.Eq("Id", playerRequest.Id);

        // If Name but no Location, then get Location from players_unique
        if (!string.IsNullOrEmpty(playerRequest.Name) && string.IsNullOrEmpty(playerRequest.Location))
        {
            var playerUnique = _playersUniqueCollection
                .Find(Builders<PlayerUnique>
                    .Filter.Eq(x => x.Name, playerRequest.Name))
                .First<PlayerUnique>();

            if (playerUnique != null)
                filter &= Builders<Player>.Filter.Eq("Location", playerUnique.Location);
        }

        var players = await _playersCollection.FindAsync(filter);

        var playersResponse =
            players.ToList().Select(player => new PlayerResponse(player)).ToList();

        return playersResponse;
    }

    [HttpPost(Name = "CreatePlayer")]
    public async Task<ActionResult<PlayerResponse>> CreatePlayer(PlayerRequest playerRequest)
    {
        Logger.LogDebug($"Route {nameof(CreatePlayer)} called.");

        // ACID Transaction: player + player_unique
        using (var session = await Client.StartSessionAsync())
        {
            session.StartTransaction();

            try
            {
                var player = new Player()
                {
                    Id = ObjectId.GenerateNewId(),
                    Name = playerRequest.Name,
                    Team = playerRequest.Team,
                    Email = playerRequest.Email,
                    Location = playerRequest.Location
                };
                await _playersCollection.InsertOneAsync(session, player);

                var playerUnique = new PlayerUnique
                {
                    Id = player.Id,
                    Name = player.Name,
                    Location = player.Location
                };
                await _playersUniqueCollection.InsertOneAsync(session, playerUnique);

                await session.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                await session.AbortTransactionAsync();

                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        return CreatedAtRoute("GetPlayers", null, null);
    }
}