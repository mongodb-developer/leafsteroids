using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using RestService.Entities;
using RestService.Dtos.ResponseObjects;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigsController : BaseController
{
    private readonly IMongoCollection<Config> _configsCollection;

    public ConfigsController(ILogger<ConfigsController> logger) : base(logger)
    {
        _configsCollection = Database!.GetCollection<Config>(Constants.ConfigsCollectionName);
    }

    [HttpGet(Name = "GetConfigs")]
    public async Task<ConfigResponse[]> GetConfigs()
    {
        Logger.LogDebug($"Route {nameof(GetConfigs)} called.");

        var configs = await _configsCollection.FindAsync(new BsonDocument());
        var configsResponse = configs.ToList().Select(config => new ConfigResponse(config)).First();

        return new[] { configsResponse };
    }
}