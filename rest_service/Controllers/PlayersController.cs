using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Dtos.RequestObjects;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayersController : BaseController
{
    private readonly IMongoCollection<Player> _playersCollection;
    private readonly IMongoCollection<PlayerUnique> _playersUniqueCollection;
    private readonly IMongoCollection<PlayerUnique> _playersUniqueCollectionOnSecondary;

    public PlayersController(ILogger<PlayersController> logger) : base(logger)
    {
        _playersCollection = Database!.GetCollection<Player>(Constants.PlayersCollectionName);

        _playersUniqueCollection = Database!.GetCollection<PlayerUnique>(Constants.PlayersUniqueCollectionName);

        _playersUniqueCollectionOnSecondary = Database!.GetCollection<PlayerUnique>(
            Constants.PlayersUniqueCollectionName,
            new MongoCollectionSettings() { ReadPreference = ReadPreference.SecondaryPreferred }
        );
    }

    [HttpGet(Name = "GetPlayers")]
    public async Task<List<PlayerResponse>> GetPlayers([FromQuery] PlayerRequest playerRequest)
    {
        Logger.LogDebug($"Route {nameof(GetPlayers)} called.");

        FilterDefinition<Player> filter = Builders<Player>.Filter.Empty;
        if (!string.IsNullOrEmpty(playerRequest.Name))
            filter &= Builders<Player>.Filter.Eq("Name", playerRequest.Name);
        if (!string.IsNullOrEmpty(playerRequest.Location))
            filter &= Builders<Player>.Filter.Eq("Location", playerRequest.Location);
        if (!string.IsNullOrEmpty(playerRequest.Team))
            filter &= Builders<Player>.Filter.Eq("Team", playerRequest.Team);
        if (!string.IsNullOrEmpty(playerRequest.Id))
            filter &= Builders<Player>.Filter.Eq("Id", playerRequest.Id);

        // If Name but no Location, then get Location from players_unique
        if (!string.IsNullOrEmpty(playerRequest.Name) && string.IsNullOrEmpty(playerRequest.Location))
        {
            var playerUnique = _playersUniqueCollection
                .Find(Builders<PlayerUnique>
                    .Filter.Eq(x => x.Name, playerRequest.Name))
                .First<PlayerUnique>();

            if (playerUnique != null)
                filter &= Builders<Player>.Filter.Eq(x => x.Location, playerUnique.Location);
        }

        var players = await _playersCollection.FindAsync(filter, new FindOptions<Player,Player>() { Limit = 10 });

        var playersResponse =
            players.ToList().Select(player => new PlayerResponse(player)).ToList();
            
        return playersResponse;
    }

    [HttpPost(Name = "CreatePlayer")]
    public async Task<ActionResult<PlayerResponse>> CreatePlayer(PlayerRequest playerRequest)
    {
        Logger.LogDebug($"Route {nameof(CreatePlayer)} called.");

        var player = new Player()
        {
            Id = ObjectId.GenerateNewId(),
            Name = playerRequest.Name,
            Team = playerRequest.Team,
            Email = playerRequest.Email,
            Location = playerRequest.Location
        };

        var playerUnique = new PlayerUnique()
        {
            Name = playerRequest.Name,
            Location = playerRequest.Location
        };

        // ACID Transaction: player + player_unique
        using (var session = await Client.StartSessionAsync())
        {
            try
            {
                session.StartTransaction();
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine("Client does not support transactions.");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            try
            {
                await _playersCollection.InsertOneAsync(session, player);
                
                await _playersUniqueCollection.InsertOneAsync(session, playerUnique);

                if (session.IsInTransaction)
                    await session.CommitTransactionAsync();
            }
            catch (Exception e)
            {
                if (session.IsInTransaction)
                    await session.AbortTransactionAsync();

                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        return CreatedAtRoute("GetPlayers", null, null);
    }

    [HttpGet("autocomplete", Name = "GetPlayerAutoComplete")]
    public async Task<List<string>> PlayerAutoComplete([FromQuery] string Name)
    {
        Logger.LogDebug($"Route {nameof(PlayerAutoComplete)} called.");

        var pipeline = new List<IPipelineStageDefinition>
        {
            new JsonPipelineStageDefinition<PlayerUnique, BsonDocument>(
                "{ $search: { index: 'autocomplete', autocomplete: { query: '" + Name + "', path: '_id', fuzzy: { maxEdits: 2, prefixLength: 1, maxExpansions: 256 } } } }"
            ),
            new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(
                "{ $limit: 5 }"
            ),
            new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(
                "{ $group: { _id: 1, matches: {$push: '$_id' } } }"
            ),
            new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(
                "{ $project: { _id: 0, matches: 1 } }"
            )
        };

        try
        {
            var result = await _playersUniqueCollectionOnSecondary.AggregateAsync<BsonDocument>(pipeline);

            BsonDocument arrMatches = result.First();

            return arrMatches.GetElement("matches").Value.AsBsonArray
                             .Select(x => x.ToString())
                             .ToList();
        } catch (Exception e)
        {
            Logger.LogError("GetPlayerAutoComplete did not find matches");
            Logger.LogError(e.Message);

            return new List<string>();
        }
    }

    [HttpGet("search", Name = "GetPlayerSearch")]
    public async Task<List<PlayerResponse>> PlayerSearch([FromQuery] PlayerRequest playerRequest)
    {
        Logger.LogDebug($"Route {nameof(PlayerSearch)} called.");

        var input = playerRequest.Name!.ToString()!;

        /* $search with $lookup in JS form for dev/test purposes:
        [
            {
            $search: {
                index: "default",
                query: "hbrt",
                path: "*"
                },
            },
            },
            {
            $lookup: {
                from: "players",
                let: {
                lookupNickname: "$_id",
                lookupLocation: "$location",
                },
                pipeline: [
                {
                    $match: {
                    $expr: {
                        $and: [
                        {
                            $eq: [
                            "$Nickname",
                            "$$lookupNickname",
                            ],
                        },
                        {
                            $eq: [
                            "$location",
                            "$$lookupLocation",
                            ],
                        },
                        ],
                    },
                    },
                },
                ],
                as: "playerInfo",
            },
            },
            {
            $unwind: "$playerInfo",
            },
            {
            $replaceRoot: {
                newRoot: "$playerInfo",
            },
            },
        ]
        */

        var pipeline = new List<IPipelineStageDefinition>
        {
            // TO-DO: $search could be used on a variety of fields or use a dynamic index
            new JsonPipelineStageDefinition<PlayerUnique, BsonDocument>(
                "{$search: {index: 'default', text:{query: '" + input + "', path:{wildcard: '*'}, fuzzy: { maxEdits: 2, prefixLength: 1, maxExpansions: 256 }}}}"
            ),
            new BsonDocumentPipelineStageDefinition<BsonDocument, BsonDocument>(
                new BsonDocument("$limit", 5)
            ),
            new BsonDocumentPipelineStageDefinition<BsonDocument, BsonDocument>(
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "players" },
                    { "let", new BsonDocument
                        {
                            { "lookupNickname", "$_id" },
                            { "lookupLocation", "$location" }
                        }
                    },
                    { "pipeline", new BsonArray
                        {
                            new BsonDocument("$match", new BsonDocument
                            {
                                { "$expr", new BsonDocument
                                    {
                                        { "$and", new BsonArray
                                            {
                                                new BsonDocument("$eq", new BsonArray { "$Nickname", "$$lookupNickname" }),
                                                new BsonDocument("$eq", new BsonArray { "$location", "$$lookupLocation" })
                                            }
                                        }
                                    }
                                }
                            })
                        }
                    },
                    { "as", "playerInfo" }
                })
            ),
            new JsonPipelineStageDefinition<BsonDocument, BsonDocument>(
                "{ $unwind: '$playerInfo' }"
            ),
            new JsonPipelineStageDefinition<BsonDocument, Player>(
                "{ $replaceRoot: { newRoot: '$playerInfo' } }"
            )
        };

        var result = await _playersUniqueCollectionOnSecondary.AggregateAsync<Player>(pipeline);

        var playersResponse =
            result.ToList().Select(player => new PlayerResponse(player)).ToList();

        return playersResponse;
    }
}