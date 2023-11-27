using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using _00_Shared;

namespace _3_Main
{
    public class QRCode : MonoBehaviour
    {
        [SerializeField] private Image image;
        // Start is called before the first frame update
        void Start()
        {
            string path = @"eventQRCode.png";
            Sprite sprite = LoadSprite.LoadNewSprite(path);
            image!.sprite = sprite;
        }
    }
}