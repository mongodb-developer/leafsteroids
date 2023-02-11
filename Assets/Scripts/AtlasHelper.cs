using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Recording
{
    public class AtlasHelper
    {
        private const string DataApiUrlInsertOne =
            "https://data.mongodb-api.com/app/data-mmwob/endpoint/data/v1/action/insertOne";

        private readonly HttpClient client = new();

        public AtlasHelper()
        {
            if (client == null || client.DefaultRequestHeaders == null) return;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("api-key",
                "JX0THWuEm9AniPG9fx6B7E8dXg7GhRYcWLS312kvrscu8S0066R16t3TwXqzTQkl");
        }

        public async Task PersistRecording(Recording recording)
        {
            var insertOneValues = new PayloadWithDocument
            {
                dataSource = "Cluster0",
                database = "pacman",
                collection = "round-results",
                document = recording
            };

            var jsonStringNewDocument = JsonUtility.ToJson(insertOneValues);
            await PostRequest(DataApiUrlInsertOne, jsonStringNewDocument);
        }

        private async Task PostRequest(string url, string jsonString)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            stringContent.Headers!.ContentType!.CharSet = null;
            httpRequest.Content = stringContent;

            httpRequest.Content.Headers!.Remove("Content-Type");
            httpRequest.Content.Headers.Add("Content-Type", "application/json");

            var response = await client!.SendAsync(httpRequest)!;
            if (response is { Content: { } })
            {
                var resultString = await response.Content.ReadAsStringAsync();
                Debug.Log(resultString);
            }
        }
    }
}