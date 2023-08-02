using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Entities;
using RestService.Entities.Atlas;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly MongoClient _mongoClient;

        public EventsController(ILogger<EventsController> logger)
        {
            _logger = logger;
            DotEnv.Load();
            var envVars = DotEnv.Read();
            var connectionString = envVars[Constants.ConnectionString];
            _mongoClient = new MongoClient(connectionString);
        }

        [HttpGet(Name = "GetEvents")]
        public async Task<EventResponse[]> GetEvents()
        {
            _logger.LogDebug($"Route {nameof(GetEvents)} called.");
            var database = _mongoClient.GetDatabase(Constants.DatabaseName);
            var eventsCollection = database.GetCollection<EventAtlas>(Constants.EventsCollectionName);
            var eventsAtlas = await eventsCollection.FindAsync(new BsonDocument());
            var eventsResponse = eventsAtlas.ToList().Select(eventAtlas => new EventResponse(eventAtlas)).ToArray();
            return eventsResponse;
        }
    }
}