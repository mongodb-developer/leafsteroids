using _00_Shared;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Event = _3_Main._ReplaySystem.Event;

namespace _1_Loading
{
    public class GameConfigLoader : MonoBehaviour
    {
        public enum SceneName
        {
            Welcome,
            PlayerSelection,
            Playground
        }

        public SceneName sceneToSwitchTo;
        public static GameConfigLoader Instance;
        public GameConfig GameConfig;
        public LocalConfig LocalConfig;

        private List<Event> _events;

        [DllImport("__Internal")]
        private static extern string GetPlayerName();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            LoadLocalConfig();
            LoadRemoteConfig();
        }

        private void GetEventDetails()
        {
            StartCoroutine(
                AtlasHelper.GetEvents(result =>
                {
                    _events = result;
                    GameConfig.Event = _events![0];

                    var playerName = GetPlayerName();
                    GameConfig.Player.Name = playerName;

                    SceneNavigation.SwitchToWelcome();
                })
            );
        }

        private void LoadRemoteConfig()
        {
            Debug.Log(nameof(LoadRemoteConfig));
            if (Instance != null && Instance.GameConfig != null) return;

            StartCoroutine(
                AtlasHelper.GetConfig(result =>
                {
                    GameConfig = result;
                    GetEventDetails();
                })
            );
        }

        private void LoadLocalConfig()
        {
            var envVars = DotEnv.Read();
            Debug.Log(envVars);
            LocalConfig = new LocalConfig
            {
                RestServiceIp = envVars["REST_SERVICE_IP"],
                RestServicePort = envVars["REST_SERVICE_PORT"],
                EventId = envVars["EVENT_ID"]
            };
        }
    }
}