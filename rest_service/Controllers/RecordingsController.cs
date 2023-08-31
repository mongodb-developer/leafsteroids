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
    private readonly IMongoCollection<PlayerUnique> _playersUniqueCollection;

    public RecordingsController(ILogger<RecordingsController> logger) : base(logger)
    {
        /*
         *  Use players_unique collection to query by Name
         *  This avoids a collscan against the globally sharded collection "players"
         *  whose shard key is {location:1, Nickname:1}
        */
        _playersUniqueCollection = Database!.GetCollection<PlayerUnique>(Constants.PlayersUniqueCollectionName);
        _recordingsCollection = Database!.GetCollection<Recording>(Constants.RecordingsCollectionName);
        _eventsCollection = Database!.GetCollection<Event>(Constants.EventsCollectionName);
    }

    [HttpPost(Name = "PostRecording")]
    public async Task<IActionResult> PostRecording([FromBody] RecordingRequest recordingRequest)
    {
        Logger.LogDebug($"Route {nameof(PostRecording)} called.");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newRecording = new Recording()
        {
            SessionStatisticsPlain = recordingRequest.SessionStatisticsPlain,
            DateTime = DateTime.UtcNow,
            Player = new RecordingPlayer { Name = recordingRequest.PlayerName },
            Event = new RecordingEvent { Id = recordingRequest.EventId },
            Snapshots = recordingRequest.Snapshots.Select(dto => new Snapshot
            {
                Position = new Position
                {
                    X = dto.Position.X,
                    Y = dto.Position.Y,
                    Z = dto.Position.Z
                }
            }).ToList()
        };

        try
        {
            await AddLocation(newRecording);
        }
        catch (EventNotFoundException)
        {
            var message = $"The event with id '{newRecording.Event.Id}' does not exist.";
            Logger.LogError(message);
            return BadRequest(new { Message = message });
        }
        catch (MultipleEventsFoundException)
        {
            var message = $"The event with id '{newRecording.Event.Id}' exists multiple times.";
            Logger.LogError(message);
            return BadRequest(new { Message = message });
        }

        try
        {
            await AddPlayer(newRecording);
        }
        catch (PlayerNotFoundException)
        {
            var message = $"The player '{newRecording.Player.Name}' does not exist.";
            Logger.LogError(message);
            return BadRequest(new { Message = message });
        }
        catch (MultiplePlayersFoundException)
        {
            var message = $"The player '{newRecording.Player.Name}' exists multiple times.";
            Logger.LogError(message);
            return BadRequest(new { Message = message });
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
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
        var playerFilter = Builders<PlayerUnique>.Filter.Eq(x => x.Name, playerName);
        var players = await _playersUniqueCollection.Find(playerFilter).ToListAsync();
        switch (players.Count)
        {
            case 1:
            {
                recording.Player.Name = players[0].Name;
                recording.Player.Location = players[0].Location;
                break;
            }
            case 0:
                throw new PlayerNotFoundException();
            default:
                throw new MultiplePlayersFoundException();
        }
    }
}