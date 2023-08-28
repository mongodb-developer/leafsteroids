using Microsoft.AspNetCore.Mvc;

namespace RestService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : BaseController
{
    // private readonly IMongoCollection<Map> _mapsCollection;

    public MapsController(ILogger<MapsController> logger) : base(logger)
    {
        // _mapsCollection = Database!.GetCollection<Map>(Constants.MapsCollectionName);
    }

    [HttpGet(Name = "GetMaps")]
    public string GetMaps()
    {
        Logger.LogDebug($"Route {nameof(GetMaps)} called.");

        var json = """
                     [
                       {
                         "pallets": [
                           {
                             "position": {
                               "x": 3,
                               "y": 0.05,
                               "z": 3
                             },
                             "pallet_type": "Large"
                           },
                           {
                             "position": {
                               "x": 4,
                               "y": 0.05,
                               "z": 4
                             },
                             "pallet_type": "Medium"
                           }
                         ],
                         "power_ups": [
                           {
                             "position": {
                               "x": 50,
                               "y": 60,
                               "z": 60
                             }
                           }
                         ],
                         "enemies": [
                           {
                             "position": {
                               "x": 70,
                               "y": 80,
                               "z": 60
                             }
                           },
                           {
                             "position": {
                               "x": 90,
                               "y": 100,
                               "z": 60
                             }
                           }
                         ]
                       }
                     ]
                   """;

        return json;
    }
}