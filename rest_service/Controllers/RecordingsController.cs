using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RestService.Entities.Atlas;
using RestService.Entities.ResponseObjects;

namespace RestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecordingsController : BaseController
    {
        private readonly IMongoCollection<RecordingAtlas> _recordingsCollection;
        private readonly IMongoCollection<EventAtlas> _eventsCollection;
        private readonly IMongoCollection<PlayerAtlas> _playersCollection; 

        public RecordingsController(ILogger<RecordingsController> logger) : base(logger)
        { 
            _recordingsCollection = Database!.GetCollection<RecordingAtlas>(Constants.RecordingsCollectionName);
            _eventsCollection = Database!.GetCollection<EventAtlas>(Constants.EventsCollectionName);
            /*  
             *  Use players_unique collection to query by Nickname
             *  This avoids a collscan against the players globally sharded collection
             *  whose shard key is {location:1, Nickname:1}
            */
            _playersCollection = Database!.GetCollection<PlayerAtlas>(Constants.PlayersUniqueCollectionName);
        }

        [HttpPost(Name = "PostRecording")]
        public async Task<IActionResult> PostRecording([FromBody] RecordingRequest recordingRequest)
        {
            Logger.LogDebug($"Route {nameof(PostRecording)} called.");

            Logger.LogDebug(recordingRequest.ToString());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRecordingAtlas = new RecordingAtlas(recordingRequest);
            await AddLocation(newRecordingAtlas);
            await AddPlayer(newRecordingAtlas);

            await _recordingsCollection.InsertOneAsync(newRecordingAtlas);

            return Ok(new { Message = "Recording created successfully." });
        }

        private async Task AddLocation(RecordingAtlas recordingAtlas)
        {
            var eventId = recordingAtlas.Event.Id;
            var eventFilter = Builders<EventAtlas>.Filter.Eq("_id", eventId);
            var currentEvent = await _eventsCollection.Find(eventFilter).FirstAsync();
            recordingAtlas.Location = currentEvent.Location;
        }

        private async Task AddPlayer(RecordingAtlas recordingAtlas)
        {
            var playerNickname = recordingAtlas.Player.Nickname;
            var playerFilter = Builders<PlayerAtlas>.Filter.Eq("Nickname", playerNickname);
            var currentPlayer = await _playersCollection.Find(playerFilter).FirstAsync();
            recordingAtlas.Player.Id = currentPlayer.Id;
        }
    }
}