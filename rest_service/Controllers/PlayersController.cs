using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Entities.Atlas;
using RestService.Entities.ResponseObjects;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly MongoClient _mongoClient;

        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
            DotEnv.Load();
            var envVars = DotEnv.Read();
            var connectionString = envVars[Constants.ConnectionString];
            _mongoClient = new MongoClient(connectionString);
        }

        [HttpGet(Name = "GetPlayers")]
        public async Task<PlayerResponse[]> GetPlayers()
        {
            _logger.LogDebug($"Route {nameof(GetPlayers)} called.");
            var database = _mongoClient.GetDatabase(Constants.DatabaseName);
            var playersCollection = database.GetCollection<PlayerAtlas>(Constants.PlayersCollectionName);
            var playersAtlas = await playersCollection.FindAsync(new BsonDocument());
            var playersResponse =
                playersAtlas.ToList().Select(playerAtlas => new PlayerResponse(playerAtlas)).ToArray();
            return playersResponse;
        }
    }
}