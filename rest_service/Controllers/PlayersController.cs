using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Entities.Atlas;
using RestService.Entities.ResponseObjects;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : BaseController
    {
        private readonly IMongoCollection<PlayerAtlas> _playersCollection;

        public PlayersController(ILogger<PlayersController> logger) : base(logger)
        {
            _playersCollection = Database!.GetCollection<PlayerAtlas>(Constants.PlayersCollectionName);
        }

        [HttpGet(Name = "GetPlayers")]
        public async Task<PlayerResponse[]> GetPlayers()
        {
            Logger.LogDebug($"Route {nameof(GetPlayers)} called.");

            var playersAtlas = await _playersCollection.FindAsync(new BsonDocument());
            var playersResponse =
                playersAtlas.ToList().Select(playerAtlas => new PlayerResponse(playerAtlas)).ToArray();

            return playersResponse;
        }
    }
}