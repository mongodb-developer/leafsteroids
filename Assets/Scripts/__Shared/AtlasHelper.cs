using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _MainScene._ReplaySystem;
using MongoDB.Driver;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace __Shared
{
    public class AtlasHelper
    {
        private readonly MongoClient _mongoClient = new(Constants.ConnectionString);

        public async Task PersistRecording(Recording recording)
        {
            var database = _mongoClient!.GetDatabase(Constants.DatabaseName);
            // await database!.DropCollectionAsync(Constants.RecordingsCollectionName)!;
            var collection = database!.GetCollection<Recording>(Constants.RecordingsCollectionName);
            // Debug.Log(recording!.ToString());
            await collection!.InsertOneAsync(recording)!;
            // var documents = await collection.Find(new BsonDocument()).ToListAsync()!;
            // Debug.Log(documents);
        }

        public IEnumerator GetSnapshots(Action<List<Recording>> callback = null)
        {
            using var request = UnityWebRequest.Get(Constants.DataApiUrlGetMany);
            request!.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apiKey", Constants.DataApiKey);
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(JsonConvert.DeserializeObject<List<Recording>>(request.downloadHandler!.text!));
            }
        }

        public IEnumerator GetSnapshot(string id, Action<Recording> callback = null)
        {
            using var request = UnityWebRequest.Get(Constants.DataApiUrlGetOne + id);
            request!.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apiKey", Constants.DataApiKey);
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            }
            else
            {
                callback?.Invoke(JsonConvert.DeserializeObject<Recording>(request.downloadHandler!.text!));
            }
        }

        public IEnumerator RecordSnapshot(string data, Action<bool> callback = null)
        {
            using var request = new UnityWebRequest(Constants.DataApiUrlInsertOne, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("apiKey", Constants.DataApiKey);
            var bodyRaw = Encoding.UTF8.GetBytes(data!);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                callback?.Invoke(false);
            }
            else
            {
                callback?.Invoke(request.downloadHandler!.text != "{}");
            }
        }
    }
}