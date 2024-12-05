using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RestService.Dtos.RequestObjects;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;
using RestService.Exceptions;
using MongoDB.Bson;

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
            Snapshots = recordingRequest.Snapshots.Select(dto => new RecordingSnapshot
            {
                Position = new Position
                {
                    X = dto.Position.X,
                    Z = dto.Position.Z
                },
                SessionStatisticsPlain = new SessionStatisticsPlain
                {
                    Score = dto.SessionStatisticsPlain.Score,
                    DamageDone = dto.SessionStatisticsPlain.DamageDone,
                    BulletsFired = dto.SessionStatisticsPlain.BulletsFired,
                    PelletsDestroyedLarge = dto.SessionStatisticsPlain.PelletsDestroyedLarge,
                    PelletsDestroyedMedium = dto.SessionStatisticsPlain.PelletsDestroyedMedium,
                    PelletsDestroyedSmall = dto.SessionStatisticsPlain.PelletsDestroyedSmall,
                    PowerUpBulletDamageCollected = dto.SessionStatisticsPlain.PowerUpBulletDamageCollected,
                    PowerUpBulletSpeedCollected = dto.SessionStatisticsPlain.PowerUpBulletSpeedCollected,
                    PowerUpPlayerSpeedCollected = dto.SessionStatisticsPlain.PowerUpPlayerSpeedCollected
                }
            }).ToList()
        };

        // Calculate vectors
        try
        {
            newRecording.StatsVector = CalculateStatsVector(newRecording.SessionStatisticsPlain);
            newRecording.SpeedVector = CalculateSpeedVector(newRecording.Snapshots);
            newRecording.AccelVector = CalculateAcceleration(newRecording.SpeedVector);
            newRecording.ScoreVector = CalculateScoreVector(newRecording.Snapshots);
            newRecording.ScoreCumulativeVector = CalculateScoreCumulativeVector(newRecording.Snapshots);
            double [][] ratios = CalculateRatiosVector(newRecording.Snapshots);
            newRecording.AverageScorePerBulletVector = ratios[0];
            newRecording.AverageDamagePerBulletVector = ratios[1];
            newRecording.AveragePelletsPerBulletVector = ratios[2];
            newRecording.RatiosVector = ratios[3];

            newRecording.SimilarityVector = CalculateSimilarityVector(
                new List<double[]>() {
                    newRecording.SpeedVector,
                    newRecording.StatsVector,
                });
        }
        catch (Exception)
        {
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
        }

        await _recordingsCollection.InsertOneAsync(newRecording);

        return Ok(new { Message = "Recording created successfully." });
    }

    [HttpGet(Name = "GetRecording")]
    public async Task<string> GetRecording([FromQuery] string id)
    {
        Recording recording = _recordingsCollection
            .Find(r => r.Id.ToString() == id)
            .Limit(1).ToList().First();

        return recording.ToJson();
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

    private static double[] CalculateStatsVector(SessionStatisticsPlain ssp)
    {
        double[] stats = new double[9];

        stats[0] = Convert.ToDouble(ssp.Score);
        stats[1] = Convert.ToDouble(ssp.DamageDone);
        stats[2] = Convert.ToDouble(ssp.BulletsFired);
        stats[3] = Convert.ToDouble(ssp.PelletsDestroyedLarge);
        stats[4] = Convert.ToDouble(ssp.PelletsDestroyedMedium);
        stats[5] = Convert.ToDouble(ssp.PelletsDestroyedSmall);
        stats[6] = Convert.ToDouble(ssp.PowerUpBulletDamageCollected);
        stats[7] = Convert.ToDouble(ssp.PowerUpBulletSpeedCollected);
        stats[8] = Convert.ToDouble(ssp.PowerUpPlayerSpeedCollected);

        return stats;
    }

    private static double[] CalculateSpeedVector(List<RecordingSnapshot> snapshots)
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

    private static double[] CalculateScoreVector(List<RecordingSnapshot> snapshots)
    {
        double[] score = new double[60]; 
        long vectorSize = snapshots.Count - 1;
        uint index = 0;
        for (int i = 0; i < vectorSize; i=i+10)
        {
            // Calculate score for each 10s interval
            score[index] = snapshots[i+9].SessionStatisticsPlain.Score-snapshots[i].SessionStatisticsPlain.Score;
            index++;
        }
        return score;
    }

    private static double[] CalculateScoreCumulativeVector(List<RecordingSnapshot> snapshots)
    {
        double[] score = new double[60]; 
        long vectorSize = snapshots.Count - 1;
        uint index = 0;
        for (int i = 0; i < vectorSize; i=i+10)
        {
            // Calculate score for each 10s interval
            if(index == 0 ){
                score[index] = snapshots[i+9].SessionStatisticsPlain.Score-snapshots[i].SessionStatisticsPlain.Score;
            
            }
            else{
                score[index] = snapshots[i+9].SessionStatisticsPlain.Score-snapshots[i].SessionStatisticsPlain.Score + score[index-1];
            }
            index++;
        }
        return score;
    }

    private static double[][] CalculateRatiosVector(List<RecordingSnapshot> snapshots)
    {
        long vectorSize = snapshots.Count - 1;

        double[] scorePerBullet=new double[vectorSize+1];
        double[] damagePerBullet=new double[vectorSize+1];
        double[] pelletsPerBullet=new double[vectorSize+1];
        double[] averageScorePerBullet = new double[60];
        double[] averageDamagePerBullet = new double[60];
        double[] averagePelletsPerBullet = new double[60];
        double[] ratiosVector = new double[60];
        uint index = 0;
        for (int i = 0; i <= vectorSize; i++)
        {
            // Calculate ratios 
            if (snapshots[i].SessionStatisticsPlain.BulletsFired > 0)
            {
                scorePerBullet[i] = (double)snapshots[i].SessionStatisticsPlain.Score/snapshots[i].SessionStatisticsPlain.BulletsFired;
                damagePerBullet[i] = (double)snapshots[i].SessionStatisticsPlain.DamageDone/snapshots[i].SessionStatisticsPlain.BulletsFired;
                pelletsPerBullet[i] = (double)(snapshots[i].SessionStatisticsPlain.PelletsDestroyedLarge + snapshots[i].SessionStatisticsPlain.PelletsDestroyedMedium + snapshots[i].SessionStatisticsPlain.PelletsDestroyedSmall) / snapshots[i].SessionStatisticsPlain.BulletsFired;
            }else
            {
                scorePerBullet[i] = 0;
                damagePerBullet[i] = 0;
                pelletsPerBullet[i] = 0;
            }
        }
        //average of the ratios for each second  
        for (int i = 0; i < vectorSize; i=i+10)
        {
            averageScorePerBullet[index] = (double)(scorePerBullet[i]+scorePerBullet[i+1]+scorePerBullet[i+2]+scorePerBullet[i+3]+scorePerBullet[i+4]+scorePerBullet[i+5]+scorePerBullet[i+6]+scorePerBullet[i+7]+scorePerBullet[i+8]+scorePerBullet[i+9])/10;
            averageDamagePerBullet[index] = (double)(damagePerBullet[i]+damagePerBullet[i+1]+damagePerBullet[i+2]+damagePerBullet[i+3]+damagePerBullet[i+4]+damagePerBullet[i+5]+damagePerBullet[i+6]+damagePerBullet[i+7]+damagePerBullet[i+8]+damagePerBullet[i+9])/10;
            averagePelletsPerBullet[index] = (double)(pelletsPerBullet[i]+pelletsPerBullet[i+1]+pelletsPerBullet[i+2]+pelletsPerBullet[i+3]+pelletsPerBullet[i+4]+pelletsPerBullet[i+5]+pelletsPerBullet[i+6]+pelletsPerBullet[i+7]+pelletsPerBullet[i+8]+pelletsPerBullet[i+9])/10;
            ratiosVector[index] =  (double)(averageScorePerBullet[index] + averageDamagePerBullet[index] + averagePelletsPerBullet[index])/3;
            index++;
        }
        
        double [][] vectors = new double[5][];
        vectors[0] = averageScorePerBullet;
        vectors[1] = averageDamagePerBullet;
        vectors[2] = averagePelletsPerBullet;
        vectors[3] = ratiosVector;
        return vectors;
    }

    private static double[] CalculateSimilarityVector(List<double[]> vectors)
    {
        double[] similar = Array.Empty<double>();
        foreach (double[] vector in vectors)
            similar = similar.Concat(vector).ToArray();
        return similar;
    }

    [HttpGet("similar", Name = "GetSimilar")]
    public async Task<List<SimilarRecordingResponse>> Similar([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.SimilarityVector,
                topRecording.SimilarityVector,
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

    [HttpGet("similarByStats", Name = "GetSimilarByStats")]
    public async Task<List<SimilarRecordingResponse>> SimilarByStats([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.StatsVector,
                topRecording.StatsVector,
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
    [HttpGet("similarByScoreProgress", Name = "GetSimilarByScoreProgress")]
    public async Task<List<SimilarRecordingResponse>> SimilarByScoreProgress([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.ScoreCumulativeVector,
                topRecording.ScoreCumulativeVector,
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

    [HttpGet("similarByGameStyle", Name = "GetSimilarByGameStyle")]
    public async Task<List<SimilarRecordingResponse>> SimilarByGameStyle([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.RatiosVector,
                topRecording.RatiosVector,
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

    [HttpGet("similarByAverageScorePerBullet", Name = "GetSimilarByAverageScorePerBullet")]
    public async Task<List<SimilarRecordingResponse>> SimilarByAverageScorePerBullet([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.AverageScorePerBulletVector,
                topRecording.AverageScorePerBulletVector,
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
    [HttpGet("similarByAverageDamagePerBullet", Name = "GetSimilarByAverageDamagePerBullet")]
    public async Task<List<SimilarRecordingResponse>> SimilarByAverageDamagePerBullet([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.AverageDamagePerBulletVector,
                topRecording.AverageDamagePerBulletVector,
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
    [HttpGet("similarByAveragePelletsPerBullet", Name = "GetSimilarByAveragePelletsPerBullet")]
    public async Task<List<SimilarRecordingResponse>> SimilarByAveragePelletsPerBullet([FromQuery] PlayerRequest playerRequest)
    {
        // Get the highest scoring run for this player
        Recording topRecording = _recordingsCollection
            .Find(r => r.Player.Name.Equals(playerRequest.Name))
            .SortByDescending(r => r.SessionStatisticsPlain.Score)
            .Limit(1).ToList().First();

        // Now get similar recordings
        List<Recording> similarRecordings = _recordingsCollection.Aggregate()
            .VectorSearch(
                r => r.AveragePelletsPerBulletVector,
                topRecording.AveragePelletsPerBulletVector,
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