using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RestService.Dtos.RequestObjects;
using RestService.Entities;
using RestService.Exceptions;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class RecordingsController : BaseController
{
    private readonly IMongoCollection<Recording> _recordingsCollection;
    private readonly IMongoCollection<Event> _eventsCollection;
    private readonly IMongoCollection<Player> _playersCollection;

    public RecordingsController(ILogger<RecordingsController> logger) : base(logger)
    {
        _recordingsCollection = Database!.GetCollection<Recording>(Constants.RecordingsCollectionName);
        _eventsCollection = Database!.GetCollection<Event>(Constants.EventsCollectionName);
        /*  
         *  Use players_unique collection to query by Name
         *  This avoids a collscan against the players globally sharded collection
         *  whose shard key is {location:1, Name:1}
        */
        _playersCollection = Database!.GetCollection<Player>(Constants.PlayersUniqueCollectionName);
    }

    [HttpPost(Name = "PostRecording")]
    public async Task<IActionResult> PostRecording([FromBody] RecordingRequest recordingRequest)
    {
        Logger.LogDebug($"Route {nameof(PostRecording)} called.");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newRecording = new Recording(recordingRequest);
        try
        {
            await AddLocation(newRecording);
        }
        catch (EventNotFoundException)
        {
            return BadRequest(new { Message = $"The event with id '{newRecording.Event.Id}' does not exist." });
        }
        catch (MultipleEventsFoundException)
        {
            return BadRequest(new { Message = $"The event with id '{newRecording.Event.Id}' exists multiple times." });
        }

        try
        {
            await AddPlayer(newRecording);
        }
        catch (PlayerNotFoundException)
        {
            return BadRequest(new { Message = $"The player '{newRecording.Player.Name}' does not exist." });
        }
        catch (MultiplePlayersFoundException)
        {
            return BadRequest(new
                { Message = $"The player '{newRecording.Player.Name}' exists multiple times." });
        }

        await _recordingsCollection.InsertOneAsync(newRecording);

        return Ok(new { Message = "Recording created successfully." });
    }

    private async Task AddLocation(Recording recording)
    {
        var eventId = recording.Event.Id;
        var eventFilter = Builders<Event>.Filter.Eq("_id", eventId);
        var events = await _eventsCollection.Find(eventFilter).ToListAsync();
        switch (events.Count)
        {
            case 1:
            {
                var eventLocation = events[0].Location;
                recording.Location = eventLocation;
                break;
            }
            case 0:
                throw new EventNotFoundException();
            default:
                throw new MultipleEventsFoundException();
        }
    }

    private async Task AddPlayer(Recording recording)
    {
        var playerName = recording.Player.Name;
        var playerFilter = Builders<Player>.Filter.Eq("Name", playerName);
        var players = await _playersCollection.Find(playerFilter).ToListAsync();
        switch (players.Count)
        {
            case 1:
            {
                var playerId = players[0].Id;
                recording.Player.Id = playerId;
                break;
            }
            case 0:
                throw new PlayerNotFoundException();
            default:
                throw new MultiplePlayersFoundException();
        }
    }
}