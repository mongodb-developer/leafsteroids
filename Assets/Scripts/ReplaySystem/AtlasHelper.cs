using System.Threading.Tasks;
using MongoDB.Driver;

namespace ReplaySystem
{
    public class AtlasHelper
    {
        private readonly MongoClient _mongoClient = new(Constants.ConnectionString);

        public async Task PersistRecording(Recording recording)
        {
            var database = _mongoClient!.GetDatabase(Constants.Database);
            // await database!.DropCollectionAsync(Constants.RecordingsCollection)!;
            var collection = database!.GetCollection<Recording>(Constants.RecordingsCollection);
            // Debug.Log(recording!.ToString());
            await collection!.InsertOneAsync(recording)!;
            // var documents = await collection.Find(new BsonDocument()).ToListAsync()!;
            // Debug.Log(documents);
        }
    }
}