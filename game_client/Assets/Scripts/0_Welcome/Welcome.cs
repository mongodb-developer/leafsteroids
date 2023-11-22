using System;
using _1_Loading;
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

        public Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
        {
            // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
            Texture2D spriteTexture = LoadTexture(filePath);
            Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit, 0, spriteType);
            return newSprite;
        }
        public Texture2D LoadTexture(string filePath)
        {
            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails
            Texture2D tex2D;
            byte[] fileData;
            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (tex2D.LoadImage(fileData))           // Load the imagedata into the texture (size is set automatically)
                    return tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }
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

                    Sprite sprite = LoadNewSprite(path);
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