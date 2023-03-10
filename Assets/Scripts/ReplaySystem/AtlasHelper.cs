using System.Threading.Tasks;
using MongoDB.Driver;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace ReplaySystem
{
    public class AtlasHelper
    {
        private readonly MongoClient mongoClient = new(Constants.ConnectionString);

        public async Task PersistRecording(Recording recording)
        {
            var database = mongoClient!.GetDatabase(Constants.Database);
            // await database!.DropCollectionAsync(Constants.RecordingsCollection)!;
            var collection = database!.GetCollection<Recording>(Constants.RecordingsCollection);
            // Debug.Log(recording!.ToString());
            await collection!.InsertOneAsync(recording)!;
            // var documents = await collection.Find(new BsonDocument()).ToListAsync()!;
            // Debug.Log(documents);
        }

        public IEnumerator GetSnapshots(System.Action<List<Recording>> callback = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(Constants.DataApiUrlGetMany))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("apiKey", Constants.DataApiKey);
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError) {
                    Debug.Log(request.error);
                    if(callback != null) {
                        callback.Invoke(null);
                    }
                }
                else {
                    if(callback != null) {
                        callback.Invoke(JsonConvert.DeserializeObject<List<Recording>>(request.downloadHandler.text));
                    }
                }
            }
        }

        public IEnumerator GetSnapshot(string id, System.Action<Recording> callback = null)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(Constants.DataApiUrlGetOne + id))
            {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("apiKey", Constants.DataApiKey);
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError) {
                    Debug.Log(request.error);
                    if(callback != null) {
                        callback.Invoke(null);
                    }
                }
                else {
                    if(callback != null) {
                        callback.Invoke(JsonConvert.DeserializeObject<Recording>(request.downloadHandler.text));
                    }
                }
            }
        }

        public IEnumerator RecordSnapshot(string data, System.Action<bool> callback = null) {
            using (UnityWebRequest request = new UnityWebRequest(Constants.DataApiUrlInsertOne, "POST")) {
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("apiKey", Constants.DataApiKey);
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(data);
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError) {
                    Debug.Log(request.error);
                    if(callback != null) {
                        callback.Invoke(false);
                    }
                } else {
                    if(callback != null) {
                        callback.Invoke(request.downloadHandler.text != "{}");
                    }
                }
            }
        }
    }
}