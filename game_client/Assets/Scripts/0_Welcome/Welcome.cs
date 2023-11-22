using System;
using _1_Loading;
using _00_Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

namespace _0_Welcome
{
    public class Welcome : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeTextField;
        [SerializeField] private Image image;

        IEnumerator GetEventQR(string uri)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(uri))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Error While Sending: " + uwr.error);
                }
                else
                {
                    Debug.Log("Received: " + uwr.downloadHandler.text);

                    string path = @"eventQRCode.png";
                    File.WriteAllBytes(path, uwr.downloadHandler.data);
                    Debug.Log("File saved at: " + path);

                    Sprite sprite = LoadSprite.LoadNewSprite(path);
                    image!.sprite = sprite;

                    welcomeTextField!.text = $"Welcome to\n{GameConfigLoader.Instance!.GameConfig!.Event!.Name}";
                }
            }
        }

        private void Start()
        {
            StartCoroutine(GetEventQR($"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=https://leafsteroids.net/?event={GameConfigLoader.Instance!.GameConfig!.Event!.Id}"));
        }
    }
}