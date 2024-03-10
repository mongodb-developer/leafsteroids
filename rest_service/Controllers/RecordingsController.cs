using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RestService.Dtos.RequestObjects;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;
using RestService.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

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

    [HttpGet("similarBySpeed", Name = "GetSimilarBySpeed")]
    public async Task<List<SimilarRecordingResponse>> SimilarBySpeed([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.SpeedVector,
                topRecording.SpeedVector,
                3,
                new VectorSearchOptions<Recording>()
                {
                    IndexName = "vector_index",
                    NumberOfCandidates = 1000,
                    Filter = Builders<Recording>.Filter
                        .Where(r => !r.Player.Name.Equals(playerRequest.Name))
                })
            .ToList();

        List<SimilarRecordingResponse> response = new()
        {
            new SimilarRecordingResponse(topRecording)
        };
        response.AddRange(
            similarRecordings
            .Select(r => new SimilarRecordingResponse(r))
            .ToList());

        return response;
    }

    
    [HttpGet("similarByAcceleration", Name = "GetSimilarByAcceleration")]
    public async Task<List<SimilarRecordingResponse>> SimilarByAcceleration([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.AccelVector,
                topRecording.AccelVector,
                3,
                new VectorSearchOptions<Recording>()
                {
                    IndexName = "vector_index",
                    NumberOfCandidates = 1000,
                    Filter = Builders<Recording>.Filter
                        .Where(r => !r.Player.Name.Equals(playerRequest.Name))
                })
            .ToList();

        List<SimilarRecordingResponse> response = new()
        {
            new SimilarRecordingResponse(topRecording)
        };
        response.AddRange(
            similarRecordings
            .Select(r => new SimilarRecordingResponse(r))
            .ToList());

        return response;
    }
    
}