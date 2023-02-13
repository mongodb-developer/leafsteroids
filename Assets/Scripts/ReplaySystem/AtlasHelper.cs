using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace ReplaySystem
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

        public async Task<string> PersistRecording(Recording recording)
        {
            var payload = new Payload
            {
                dataSource = "Cluster0",
                database = "pacman",
                collection = "recordings",
                document = recording
            };
            var json = JsonConvert.SerializeObject(payload);
            return await PostRequest(DataApiUrlInsertOne, json);
        }

        private async Task<string> PostRequest(string url, string jsonString)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            stringContent.Headers!.ContentType!.CharSet = null;
            httpRequest.Content = stringContent;

            httpRequest.Content.Headers!.Remove("Content-Type");
            httpRequest.Content.Headers.Add("Content-Type", "application/json");

            var response = await client!.SendAsync(httpRequest)!;
            var resultString = await response!.Content!.ReadAsStringAsync();
            Debug.Log(resultString);
            return resultString;
        }
    }
}