using System;
using _1_Loading;
using TMPro;
using UnityEngine;

namespace _0_Welcome
{
    public class Welcome : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeTextField;

        private void Start()
        {
            // var foo = Environment.GetEnvironmentVariable("foo");
            Debug.Log(GameConfigLoader.Instance!.GameConfig!.Event!.Name);
            // Environment.SetEnvironmentVariable("foo", "bar");
            welcomeTextField!.text = $"Welcome to\n{GameConfigLoader.Instance!.GameConfig!.Event!.Name}";
        }
    }
}