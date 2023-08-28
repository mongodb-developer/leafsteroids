using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _1_Loading;
using _6_Main._ReplaySystem;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _00_Shared
{
    public static class RestClient
    {
        public static async Task<List<RegisteredPlayer>> GetPlayers()
        {
            return await Get<List<RegisteredPlayer>>(Constants.GameServerEndpoints.GetPlayers);
        }

        public static async Task<List<Conference>> GetConferences()
        {
            return await Get<List<Conference>>(Constants.GameServerEndpoints.GetConferences);
        }

        public static async Task<List<Map.Map>> GetMaps()
        {
            return await Get<List<Map.Map>>(Constants.GameServerEndpoints.GetMaps);
        }

        public static async Task<List<GameConfig>> GetConfig()
        {
            return await Get<List<GameConfig>>(Constants.GameServerEndpoints.GetConfig);
        }

        public static async Task<string> PostRecording(Recording recording)
        {
            return await PostJson<string>(Constants.GameServerEndpoints.PostRecording, recording);
        }

        [ItemCanBeNull]
        private static async Task<T> Get<T>(string url)
        {
            return await SendRequest<T>(CreateServerSpecificUrl(url), UnityWebRequest.Get);
        }

        [ItemCanBeNull]
        private static async Task<T> PostJson<T>(string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            Debug.Log(json);
            return await SendRequest<T>(CreateServerSpecificUrl(url), (u) => CreatePostRequest(u, json));
        }

        private static string CreateServerSpecificUrl(string url)
        {
            var ip = GameConfigLoader.Instance!.LocalConfig.RestServiceIp;
            var port = GameConfigLoader.Instance!.LocalConfig.RestServicePort;
            return string.Format(url, ip, port);
        }

        private static async Task<T> SendRequest<T>(string url, Func<string, UnityWebRequest> requestFactory)
        {
            var webRequest = requestFactory(url);
            var taskCompletionSource = new TaskCompletionSource<string>();
            var asyncOperation = webRequest.SendWebRequest();
            asyncOperation.completed += _ =>
            {
                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    taskCompletionSource.SetResult(webRequest.downloadHandler.text);
                }
                else
                {
                    Debug.LogError("Request failed: " + webRequest.error);
                    taskCompletionSource.SetResult(null);
                }
            };

            var result = await taskCompletionSource.Task;

            var data = JsonConvert.DeserializeObject<T>(result);

            return data;
        }

        private static UnityWebRequest CreatePostRequest(string url, string jsonData)
        {
            var webRequest = new UnityWebRequest(url, "POST");
            var bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            return webRequest;
        }
    }
}