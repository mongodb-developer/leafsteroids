using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Entities;
using RestService.Entities.Atlas;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : BaseController
    {
        private readonly IMongoCollection<EventAtlas> _eventsCollection;


        public EventsController(ILogger<EventsController> logger) : base(logger)
        {
            _eventsCollection = Database!.GetCollection<EventAtlas>(Constants.EventsCollectionName);
        }

        [HttpGet(Name = "GetEvents")]
        public async Task<EventResponse[]> GetEvents()
        {
            Logger.LogDebug($"Route {nameof(GetEvents)} called.");

            var eventsAtlas = await _eventsCollection.FindAsync(new BsonDocument());
            var eventsResponse = eventsAtlas.ToList().Select(eventAtlas => new EventResponse(eventAtlas)).ToArray();

            return eventsResponse;
        }
    }
}