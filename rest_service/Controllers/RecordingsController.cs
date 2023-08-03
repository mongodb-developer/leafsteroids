using Microsoft.AspNetCore.Mvc;
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

        public RecordingsController(ILogger<RecordingsController> logger) : base(logger)
        {
            _recordingsCollection = Database!.GetCollection<RecordingAtlas>(Constants.RecordingsCollectionName);
        }

        [HttpPost(Name = "PostRecording")]
        public async Task<IActionResult> PostRecording([FromBody] RecordingRequest recordingRequest)
        {
            Logger.LogDebug($"Route {nameof(PostRecording)} called.");

            if (recordingRequest == null)
            {
                return BadRequest("Invalid request data.");
            }

            // Assuming you have a constructor in RecordingAtlas to create a new recording from the request data
            var newRecordingAtlas = new RecordingAtlas(recordingRequest);

            // Insert the new recording into the database
            await _recordingsCollection.InsertOneAsync(newRecordingAtlas);

            // Return a response indicating success
            return Ok(new { Message = "Recording created successfully.", RecordingId = newRecordingAtlas.Id });
        }
    }
}