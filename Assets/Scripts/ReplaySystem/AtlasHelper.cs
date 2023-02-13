using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace ReplaySystem
{
    public class AtlasHelper : MonoBehaviour
    {
        private const string DataApiUrlInsertOne =
            "https://data.mongodb-api.com/app/data-mmwob/endpoint/data/v1/action/insertOne";

        private HttpClient client;

        private void Start()
        {
            Debug.Log("AtlasHelper.Start");

            client = new();
            client!.DefaultRequestHeaders!.Clear();
            client.DefaultRequestHeaders.Add("api-key",
                "JX0THWuEm9AniPG9fx6B7E8dXg7GhRYcWLS312kvrscu8S0066R16t3TwXqzTQkl");
        }

        public string PersistRecording(Recording recording)
        {
            var payload = new Payload
            {
                dataSource = "Cluster0",
                database = "pacman",
                collection = "recordings",
                document = recording
            };
            var json = JsonConvert.SerializeObject(payload);
            Debug.Log("AtlasHelper.PersistRecording");

            var result = PostRequest(DataApiUrlInsertOne, json);

            Debug.Log(result);

            return result;
        }

        private string PostRequest(string url, string jsonString)
        {
            Debug.Log("AtlasHelper.PostRequest");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            Debug.Log("A");
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            Debug.Log("B");
            stringContent.Headers!.ContentType!.CharSet = null;
            Debug.Log("C");
            httpRequest.Content = stringContent;
            Debug.Log("D");

            httpRequest.Content.Headers!.Remove("Content-Type");
            Debug.Log("E");
            httpRequest.Content.Headers.Add("Content-Type", "application/json");
            Debug.Log("F");

            Debug.Log(client == null ? "null" : "all good");

            // var response = client!.SendAsync(httpRequest)!;
            var responseTask = client!.SendAsync(httpRequest);
            responseTask!.Wait();
            var response = responseTask.Result;
            Debug.Log("G");

            Debug.Log(response);

            var readAsStringAsync = response!.Content!.ReadAsStringAsync();
            readAsStringAsync.Wait();
            var resultString = readAsStringAsync.Result;
            Debug.Log("H");

            Debug.Log(resultString);

            return resultString;
        }
    }
}