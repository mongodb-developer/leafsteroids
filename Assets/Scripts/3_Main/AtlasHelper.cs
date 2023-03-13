using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using _00_Shared;
using _3_Main._ReplaySystem;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace _3_Main
{
    public static class AtlasHelper
    {
        public static IEnumerator GetSnapshots(Action<List<Recording>> callback = null)
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

        public static IEnumerator GetSnapshot(string id, Action<Recording> callback = null)
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

        public static IEnumerator RecordSnapshot(string data, Action<bool> callback = null)
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
                Debug.Log(request.downloadHandler!.text);
                callback?.Invoke(request.downloadHandler!.text != "{}");
            }
        }
    }
}