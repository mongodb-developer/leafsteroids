using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _1_Loading;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Event = _3_Main._ReplaySystem.Event;

namespace _00_Shared
{
    public static class AtlasHelper
    {
        public static IEnumerator GetConfig(Action<GameConfig> callback = null)
        {
            Debug.Log(nameof(GetConfig));
            var url = string.Format(Constants.GameServerEndpoints.GetConfig,
                GameConfigLoader.Instance!.LocalConfig!.GameServerIp,
                GameConfigLoader.Instance.LocalConfig.GameServerPort);
            Debug.Log(url);
            using var request = UnityWebRequest.Get(url);
            request!.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            }
            else
            {
                var gameConfigs = JsonConvert.DeserializeObject<List<GameConfig>>(request.downloadHandler!.text!);
                Debug.Log(gameConfigs);
                var config = gameConfigs.First();
                callback?.Invoke(config);
            }
        }

        public static IEnumerator GetEvents(Action<List<Event>> callback = null)
        {
            Debug.Log(nameof(GetEvents));
            var url = string.Format(Constants.GameServerEndpoints.GetEvents,
                GameConfigLoader.Instance!.LocalConfig!.GameServerIp,
                GameConfigLoader.Instance.LocalConfig.GameServerPort);
            Debug.Log(url);
            using var request = UnityWebRequest.Get(url);
            // request!.SetRequestHeader("Content-Type", "application/json");
            // request.SetRequestHeader("apiKey", Constants.DataApiKey);
            yield return request!.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            }
            else
            {
                var events = JsonConvert.DeserializeObject<List<Event>>(request.downloadHandler!.text!);
                callback?.Invoke(events);
            }
        }

        public static IEnumerator GetPlayers(Action<List<RegisteredPlayer>> callback = null)
        {
            Debug.Log(nameof(GetPlayers));
            var url = string.Format(Constants.GameServerEndpoints.GetPlayers,
                GameConfigLoader.Instance!.LocalConfig!.GameServerIp,
                GameConfigLoader.Instance.LocalConfig.GameServerPort);
            Debug.Log(url);
            using var request = UnityWebRequest.Get(url);
            request!.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                callback?.Invoke(null);
            }
            else
            {
                var players = JsonConvert.DeserializeObject<List<RegisteredPlayer>>(request.downloadHandler!.text!);
                callback?.Invoke(players);
            }
        }

        // public static IEnumerator GetSnapshots(Action<List<Recording>> callback = null)
        // {
        //     using var request = UnityWebRequest.Get(Constants.DataApiUrlGetMany);
        //     request!.SetRequestHeader("Content-Type", "application/json");
        //     yield return request.SendWebRequest();
        //     if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        //     {
        //         Debug.Log(request.error);
        //         callback?.Invoke(null);
        //     }
        //     else
        //     {
        //         callback?.Invoke(JsonConvert.DeserializeObject<List<Recording>>(request.downloadHandler!.text!));
        //     }
        // }
        //
        // public static IEnumerator GetSnapshot(string id, Action<Recording> callback = null)
        // {
        //     using var request = UnityWebRequest.Get(Constants.DataApiUrlGetOne + id);
        //     request!.SetRequestHeader("Content-Type", "application/json");
        //     yield return request.SendWebRequest();
        //     if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
        //     {
        //         Debug.Log(request.error);
        //         callback?.Invoke(null);
        //     }
        //     else
        //     {
        //         callback?.Invoke(JsonConvert.DeserializeObject<Recording>(request.downloadHandler!.text!));
        //     }
        // }

        public static IEnumerator RecordSnapshot(string data, Action<bool> callback = null)
        {
            var url = string.Format(Constants.GameServerEndpoints.PostInsertOne,
                GameConfigLoader.Instance!.LocalConfig!.GameServerIp,
                GameConfigLoader.Instance.LocalConfig.GameServerPort);
            Debug.Log(url);
            using var request = new UnityWebRequest(url, "POST");
            request.SetRequestHeader("Content-Type", "application/json");
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
                Debug.Log(request.downloadHandler!.text);
                callback?.Invoke(request.downloadHandler!.text != "{}");
            }
        }
    }
}