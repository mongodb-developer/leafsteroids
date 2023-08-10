using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : BaseController
{
    private readonly IMongoCollection<Event> _eventsCollection;

    public EventsController(ILogger<EventsController> logger) : base(logger)
    {
        _eventsCollection = Database!.GetCollection<Event>(Constants.EventsCollectionName);
    }

    [HttpGet(Name = "GetEvents")]
    public async Task<List<EventResponse>> GetEvents()
    {
        Logger.LogDebug($"Route {nameof(GetEvents)} called.");

        var events = await _eventsCollection.FindAsync(new BsonDocument());
        var eventsResponse = events.ToList().Select(e => new EventResponse(e)).ToList();

        return eventsResponse;
    }
}