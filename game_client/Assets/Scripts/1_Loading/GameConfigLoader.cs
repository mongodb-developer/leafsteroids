using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using _00_Shared;
using _6_Main._ReplaySystem;
using UnityEngine;

namespace _1_Loading
{
    public class GameConfigLoader : MonoBehaviour
    {
        public enum SceneName
        {
            ConferenceSelection,
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

        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private async Task Start()
        {
            LoadLocalConfig();
            await LoadRemoteConfig();
            await LoadMaps();
            SwitchToNextScene();
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

        private async Task LoadRemoteConfig()
        {
            Debug.Log(nameof(LoadRemoteConfig));
            if (Instance != null && Instance.GameConfig != null) return;

            var configs = await RestClient.GetConfig();
            GameConfig = configs!.First();
        }

        private async Task LoadMaps()
        {
            var maps = await RestClient.GetMaps();
            GameConfig!.Maps = maps;
        }

        private void SwitchToNextScene()
        {
            switch (sceneToSwitchTo)
            {
                case SceneName.ConferenceSelection:
                    SceneNavigation.SwitchToConferenceSelection();
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
        }
    }
}