using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RestService.Dtos.ResponseObjects;
using RestService.Entities;

namespace RestService.Controllers
{
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
             *  Use players_unique collection to query by Nickname
             *  This avoids a collscan against the players globally sharded collection
             *  whose shard key is {location:1, Nickname:1}
            */
            _playersCollection = Database!.GetCollection<Player>(Constants.PlayersUniqueCollectionName);
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

            var newRecording = new Recording(recordingRequest);
            await AddLocation(newRecording);
            await AddPlayer(newRecording);

            await _recordingsCollection.InsertOneAsync(newRecording);

            return Ok(new { Message = "Recording created successfully." });
        }

        private async Task AddLocation(Recording recording)
        {
            var eventId = recording.Event.Id;
            var eventFilter = Builders<Event>.Filter.Eq("_id", eventId);
            var currentEvent = await _eventsCollection.Find(eventFilter).FirstAsync();
            recording.Location = currentEvent.Location;
        }

        private async Task AddPlayer(Recording recording)
        {
            var playerNickname = recording.Player.Nickname;
            var playerFilter = Builders<Player>.Filter.Eq("Nickname", playerNickname);
            var currentPlayer = await _playersCollection.Find(playerFilter).FirstAsync();
            recording.Player.Id = currentPlayer.Id;
        }
    }
}