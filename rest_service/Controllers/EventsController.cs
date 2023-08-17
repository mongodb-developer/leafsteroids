using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Dtos.RequestObjects;
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
    public async Task<List<EventResponse>> GetEvents([FromQuery] EventRequest eventRequest)
    {
        Logger.LogDebug($"Route {nameof(GetEvents)} called.");

        FilterDefinition<Event> filter = Builders<Event>.Filter.Empty;
        if (!string.IsNullOrEmpty(eventRequest.Id))
            filter &= Builders<Event>.Filter.Eq("Id", eventRequest.Id);
        if (!string.IsNullOrEmpty(eventRequest.Name))
            filter &= Builders<Event>.Filter.Eq("Name", eventRequest.Name);
        if (!string.IsNullOrEmpty(eventRequest.Location))
            filter &= Builders<Event>.Filter.Eq("Location", eventRequest.Location);

        var events = await _eventsCollection.FindAsync(filter);
        var eventsResponse = events.ToList().Select(e => new EventResponse(e)).ToList();

        return eventsResponse;
    }
}