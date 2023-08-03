using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Entities.Atlas;
using RestService.Entities.ResponseObjects;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigsController : BaseController
    {
        private readonly IMongoCollection<ConfigAtlas> _configsCollection;

        public ConfigsController(ILogger<ConfigsController> logger) : base(logger)
        {
            _configsCollection = Database!.GetCollection<ConfigAtlas>(Constants.ConfigsCollectionName);
        }

        [HttpGet(Name = "GetConfigs")]
        public async Task<ConfigResponse> GetConfigs()
        {
            Logger.LogDebug($"Route {nameof(GetConfigs)} called.");

            var configsAtlas = await _configsCollection.FindAsync(new BsonDocument());
            var configsResponse = configsAtlas.ToList().Select(configAtlas => new ConfigResponse(configAtlas)).First();

            return configsResponse;
        }
    }
}