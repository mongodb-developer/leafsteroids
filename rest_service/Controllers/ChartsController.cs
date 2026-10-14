using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Dtos.ResponseObjects;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class ChartsController : BaseController
{
    private readonly IMongoCollection<BsonDocument> _recordings;

    public ChartsController(ILogger<ChartsController> logger) : base(logger)
    {
        _recordings = Database!.GetCollection<BsonDocument>(Constants.RecordingsCollectionName);
    }

    [HttpGet("leaderboard")]
    public async Task<ActionResult<List<LeaderboardRow>>> GetLeaderboard(
        [FromQuery] string eventId,
        [FromQuery] string? location)
    {
        if (string.IsNullOrWhiteSpace(eventId))
            return BadRequest("eventId is required.");

        var rows = await Leaderboard(EventMatch(eventId, location, null), 100);
        return rows;
    }

    [HttpGet("event")]
    public async Task<ActionResult<DashboardChartsResponse>> GetEvent(
        [FromQuery] string eventId,
        [FromQuery] string? location)
    {
        if (string.IsNullOrWhiteSpace(eventId))
            return BadRequest("eventId is required.");

        var doc = await Facet(EventMatch(eventId, location, null), EventFacet());
        var response = ReadTotals(doc);
        response.UniquePlayers = UniquePlayerCount(doc);
        response.Leaderboard = ReadLeaderboard(doc);
        response.Activity = ReadActivity(doc);
        response.ActivityValueLabel = "Games";
        response.Heatmap = ReadHeatmap(doc);
        return response;
    }

    [HttpGet("player")]
    public async Task<ActionResult<DashboardChartsResponse>> GetPlayer(
        [FromQuery] string eventId,
        [FromQuery] string? location,
        [FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(eventId) || string.IsNullOrWhiteSpace(name))
            return BadRequest("eventId and name are required.");

        var doc = await Facet(EventMatch(eventId, location, name), PlayerFacet());
        var response = ReadTotals(doc);
        response.Plays = ReadPlays(doc);
        response.Activity = ReadActivity(doc);
        response.ActivityValueLabel = "Score";
        response.Heatmap = ReadHeatmap(doc);
        return response;
    }

    [HttpGet("heatmap")]
    public async Task<ActionResult<List<HeatmapPoint>>> GetHeatmap([FromQuery] string recordingId)
    {
        if (!ObjectId.TryParse(recordingId, out var id))
            return BadRequest("recordingId must be an ObjectId.");

        var pipeline = new[]
        {
            new BsonDocument("$match", new BsonDocument("_id", id)),
            UnwindSnapshots(),
            HeatmapGroup()
        };
        var points = await _recordings.Aggregate<BsonDocument>(pipeline, DiskUse()).ToListAsync();
        return points.Select(ReadHeatmapPoint).ToList();
    }

    private async Task<List<LeaderboardRow>> Leaderboard(BsonDocument match, int limit)
    {
        var pipeline = new[]
        {
            match,
            LeaderboardGroup(),
            new BsonDocument("$sort", new BsonDocument("score", -1)),
            new BsonDocument("$limit", limit)
        };
        var rows = await _recordings.Aggregate<BsonDocument>(pipeline, DiskUse()).ToListAsync();
        return rows.Select(ReadLeaderboardRow).ToList();
    }

    private async Task<BsonDocument> Facet(BsonDocument match, BsonDocument facet)
    {
        var pipeline = new[] { match, facet };
        var cursor = await _recordings.Aggregate<BsonDocument>(pipeline, DiskUse()).FirstOrDefaultAsync();
        return cursor ?? new BsonDocument();
    }

    private static BsonDocument EventMatch(string eventId, string? location, string? player)
    {
        var filter = new BsonDocument("Event._id", eventId);
        if (!string.IsNullOrWhiteSpace(location))
            filter.Add("location", location);
        if (!string.IsNullOrWhiteSpace(player))
            filter.Add("Player.Nickname", player);
        return new BsonDocument("$match", filter);
    }

    private static BsonDocument EventFacet()
    {
        return new BsonDocument("$facet", new BsonDocument
        {
            { "totals", new BsonArray { TotalsGroup(includePlayers: true) } },
            {
                "leaderboard", new BsonArray
                {
                    LeaderboardGroup(),
                    new BsonDocument("$sort", new BsonDocument("score", -1)),
                    new BsonDocument("$limit", 100)
                }
            },
            {
                "activity", new BsonArray
                {
                    DateTimeExists(),
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", DateLabel("%Y-%m-%d %H:00") },
                        { "value", new BsonDocument("$sum", 1) }
                    }),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                }
            },
            { "heatmap", new BsonArray { UnwindSnapshots(), HeatmapGroup() } }
        });
    }

    private static BsonDocument PlayerFacet()
    {
        return new BsonDocument("$facet", new BsonDocument
        {
            { "totals", new BsonArray { TotalsGroup(includePlayers: false) } },
            {
                "plays", new BsonArray
                {
                    DateTimeExists(),
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", DateLabel("%Y-%m-%d %H:%M") },
                        { "score", new BsonDocument("$max", "$SessionStatisticsPlain.Score") }
                    }),
                    new BsonDocument("$sort", new BsonDocument("score", -1))
                }
            },
            {
                "activity", new BsonArray
                {
                    DateTimeExists(),
                    new BsonDocument("$group", new BsonDocument
                    {
                        { "_id", DateLabel("%Y-%m-%d %H:%M") },
                        { "value", new BsonDocument("$max", "$SessionStatisticsPlain.Score") }
                    }),
                    new BsonDocument("$sort", new BsonDocument("_id", 1))
                }
            },
            { "heatmap", new BsonArray { UnwindSnapshots(), HeatmapGroup() } }
        });
    }

    private static BsonDocument TotalsGroup(bool includePlayers)
    {
        var group = new BsonDocument
        {
            { "_id", BsonNull.Value },
            { "games", new BsonDocument("$sum", 1) },
            { "bullets", Sum("SessionStatisticsPlain.BulletsFired") },
            { "damage", Sum("SessionStatisticsPlain.DamageDone") },
            { "small", Sum("SessionStatisticsPlain.PelletsDestroyedSmall") },
            { "medium", Sum("SessionStatisticsPlain.PelletsDestroyedMedium") },
            { "large", Sum("SessionStatisticsPlain.PelletsDestroyedLarge") },
            { "bulletDamage", Sum("SessionStatisticsPlain.PowerUpBulletDamageCollected") },
            { "bulletSpeed", Sum("SessionStatisticsPlain.PowerUpBulletSpeedCollected") },
            { "playerSpeed", Sum("SessionStatisticsPlain.PowerUpPlayerSpeedCollected") }
        };
        if (includePlayers)
            group.Add("players", new BsonDocument("$addToSet", "$Player.Nickname"));
        return new BsonDocument("$group", group);
    }

    private static BsonDocument LeaderboardGroup()
    {
        return new BsonDocument("$group", new BsonDocument
        {
            { "_id", "$Player.Nickname" },
            { "score", new BsonDocument("$max", "$SessionStatisticsPlain.Score") }
        });
    }

    private static BsonDocument UnwindSnapshots()
    {
        return new BsonDocument("$unwind", "$Snapshots");
    }

    private static BsonDocument HeatmapGroup()
    {
        return new BsonDocument("$group", new BsonDocument
        {
            {
                "_id", new BsonDocument
                {
                    { "x", new BsonDocument("$floor", "$Snapshots.Position.X") },
                    { "z", new BsonDocument("$floor", "$Snapshots.Position.Z") }
                }
            },
            { "count", new BsonDocument("$sum", 1) }
        });
    }

    private static BsonDocument DateTimeExists()
    {
        return new BsonDocument("$match", new BsonDocument("DateTime", new BsonDocument("$type", "date")));
    }

    private static BsonDocument DateLabel(string format)
    {
        return new BsonDocument("$dateToString", new BsonDocument
        {
            { "format", format },
            { "date", "$DateTime" }
        });
    }

    private static BsonDocument Sum(string field)
    {
        return new BsonDocument("$sum", "$" + field);
    }

    private static DashboardChartsResponse ReadTotals(BsonDocument facet)
    {
        var response = new DashboardChartsResponse();
        var totals = First(facet, "totals");
        if (totals == null)
            return response;

        response.GamesPlayed = AsLong(totals, "games");
        response.TotalBulletsFired = AsLong(totals, "bullets");
        response.TotalDamage = AsLong(totals, "damage");
        response.PelletsDestroyed = new List<NamedValue>
        {
            new() { Label = "Large", Value = AsLong(totals, "large") },
            new() { Label = "Medium", Value = AsLong(totals, "medium") },
            new() { Label = "Small", Value = AsLong(totals, "small") }
        };
        response.PowerUpsCollected = new List<NamedValue>
        {
            new() { Label = "Bullet Damage", Value = AsLong(totals, "bulletDamage") },
            new() { Label = "Bullet Speed", Value = AsLong(totals, "bulletSpeed") },
            new() { Label = "Player Speed", Value = AsLong(totals, "playerSpeed") }
        };
        return response;
    }

    private static long UniquePlayerCount(BsonDocument facet)
    {
        var totals = First(facet, "totals");
        if (totals == null || !totals.Contains("players") || totals["players"].IsBsonNull)
            return 0;
        return totals["players"].AsBsonArray.Count(value => !value.IsBsonNull && value.ToString() != "");
    }

    private static List<LeaderboardRow> ReadLeaderboard(BsonDocument facet)
    {
        return Array(facet, "leaderboard").Select(value => ReadLeaderboardRow(value.AsBsonDocument)).ToList();
    }

    private static LeaderboardRow ReadLeaderboardRow(BsonDocument row)
    {
        return new LeaderboardRow
        {
            Player = row.GetValue("_id", "").ToString() ?? "",
            Score = AsLong(row, "score")
        };
    }

    private static List<PlayRow> ReadPlays(BsonDocument facet)
    {
        return Array(facet, "plays").Select(value =>
        {
            var row = value.AsBsonDocument;
            return new PlayRow
            {
                Time = row.GetValue("_id", "").ToString() ?? "",
                Score = AsLong(row, "score")
            };
        }).ToList();
    }

    private static List<NamedValue> ReadActivity(BsonDocument facet)
    {
        return Array(facet, "activity").Select(value =>
        {
            var row = value.AsBsonDocument;
            return new NamedValue
            {
                Label = row.GetValue("_id", "").ToString() ?? "",
                Value = AsLong(row, "value")
            };
        }).ToList();
    }

    private static List<HeatmapPoint> ReadHeatmap(BsonDocument facet)
    {
        return Array(facet, "heatmap").Select(value => ReadHeatmapPoint(value.AsBsonDocument)).ToList();
    }

    private static HeatmapPoint ReadHeatmapPoint(BsonDocument row)
    {
        var id = row["_id"].AsBsonDocument;
        return new HeatmapPoint
        {
            X = (int)AsLong(id, "x"),
            Z = (int)AsLong(id, "z"),
            Count = AsLong(row, "count")
        };
    }

    private static BsonDocument? First(BsonDocument facet, string name)
    {
        var values = Array(facet, name);
        return values.Count == 0 ? null : values[0].AsBsonDocument;
    }

    private static BsonArray Array(BsonDocument facet, string name)
    {
        if (!facet.Contains(name) || facet[name].IsBsonNull)
            return new BsonArray();
        return facet[name].AsBsonArray;
    }

    private static long AsLong(BsonDocument document, string name)
    {
        if (!document.Contains(name) || document[name].IsBsonNull)
            return 0;
        var value = document[name];
        if (value.IsInt32) return value.AsInt32;
        if (value.IsInt64) return value.AsInt64;
        if (value.IsDouble) return (long)value.AsDouble;
        return 0;
    }

    private static AggregateOptions DiskUse()
    {
        return new AggregateOptions { AllowDiskUse = true };
    }
}
