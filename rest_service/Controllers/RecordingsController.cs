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

            Logger.LogDebug(recordingRequest.ToString());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRecordingAtlas = new RecordingAtlas(recordingRequest);

            await _recordingsCollection.InsertOneAsync(newRecordingAtlas);

            return Ok(new { Message = "Recording created successfully." });
        }
    }
}