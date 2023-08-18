using _00_Shared;
using _6_Main._ReplaySystem;
using UnityEngine;

namespace _1_Loading
{
    public class GameConfigLoader : MonoBehaviour
    {
        public enum SceneName
        {
            EventSelection,
            Welcome,
            PlayerSelection,
            MainDynamic
        }

        public SceneName sceneToSwitchTo;
        public static GameConfigLoader Instance;
        public GameConfig GameConfig;
        public LocalConfig LocalConfig;

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

        private void LoadRemoteConfig()
        {
            Debug.Log(nameof(LoadRemoteConfig));
            if (Instance != null && Instance.GameConfig != null) return;

            StartCoroutine(
                AtlasHelper.GetConfig(result =>
                {
                    GameConfig = result;
                    switch (sceneToSwitchTo)
                    {
                        case SceneName.EventSelection:
                            SceneNavigation.SwitchToEventSelection();
                            break;
                        case SceneName.Welcome:
                            SceneNavigation.SwitchToWelcome();
                            break;
                        case SceneName.PlayerSelection:
                            SceneNavigation.SwitchToPlayerSelection();
                            break;
                        case SceneName.MainDynamic:
                            if (sceneToSwitchTo.Equals(SceneName.MainDynamic))
                            {
                                // This is a playground case.
                                GameConfig!.RoundDuration = 5;
                                GameConfig!.Player = new RegisteredPlayer
                                {
                                    Name = "Player 1"
                                };
                                GameConfig!.Conference = new Conference
                                {
                                    Id = "mdb-internal",
                                    Name = "MDB Internal"
                                };
                            }

                            SceneNavigation.SwitchToMainDynamic();
                            break;
                    }
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
                RestServicePort = envVars["REST_SERVICE_PORT"]
            };
        }
    }
}