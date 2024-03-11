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
            Id = ObjectId.GenerateNewId(),
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

        // Calculate vectors
        try
        {
            newRecording.SpeedVector = CalculateSpeedVector(newRecording.Snapshots);
            newRecording.AccelVector = CalculateAcceleration(newRecording.SpeedVector);
        } catch (Exception) {
            // Favor persisting Recording over setting vectors
        }

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

    private static double[] CalculateSpeedVector(List<Snapshot> snapshots)
    {
        long vectorSize = snapshots.Count - 1;
        if (vectorSize != 589) // 590 represents movements in 60s
            return Array.Empty<double>();

        double[] speed = new double[vectorSize];
        for (int i = 0; i < vectorSize; i++)
        {
            double speedX = snapshots[i + 1].Position.X - snapshots[i].Position.X;
            //double Y = snapshots[i + 1].Position.X - snapshots[i].Position.X;
            double speedY = snapshots[i + 1].Position.Z - snapshots[i].Position.Z;
            speed[i] = Math.Sqrt(Math.Pow(speedX, 2) + Math.Pow(speedY, 2));
        }

        return speed;
    }

    private static double[] CalculateAcceleration(double[] speedVector)
    {
        long vectorSize = speedVector.Length - 1;
        if (speedVector.Length != 589) // speed vector should be 1 less 60s run
            return Array.Empty<double>();

        double dt = 1; // Assuming a constant time step of 1 unit.
        double[] accelVector = new double[vectorSize];

        for (int i = 0; i < speedVector.Length - 1; i++)
            accelVector[i] = (speedVector[i + 1] - speedVector[i]) / dt;

        return accelVector;
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

        // Return this player's top recording + top similar
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

        // Return this player's top recording + top similar
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