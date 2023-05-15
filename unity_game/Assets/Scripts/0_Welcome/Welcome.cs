using _00_Shared;
using CandyCoded.env;
using TMPro;
using UnityEngine;

namespace _0_Welcome
{
    public class Welcome : MonoBehaviour
    {
        [SerializeField] private TMP_Text welcomeTextField;

        private void Start()
        {
            if (env.TryParseEnvironmentVariable(Constants.DotEnvFileKeys.EventName, out string eventName))
            {
                welcomeTextField!.text = $"Welcome to\n{eventName}";
            }
        }
    }
}