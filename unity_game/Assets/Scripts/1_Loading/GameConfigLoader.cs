using System.Collections.Generic;
using System.IO;
using _00_Shared;
using Newtonsoft.Json;
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
            Playground
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
            // InvokeRepeating(nameof(LoadRemoteConfig), 0f, 3f);
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
                        case SceneName.Playground:
                            SceneNavigation.SwitchToPlayground();
                            break;
                    }
                })
            );
        }

        private void LoadLocalConfig()
        {
            var configJson = Application.dataPath + "/local_config.json";
            StreamReader streamReader = new StreamReader(configJson);
            var configJsonContent = streamReader.ReadToEnd();
            Debug.Log(configJsonContent);
            LocalConfig = JsonConvert.DeserializeObject<LocalConfig>(configJsonContent);
            if (LocalConfig != null)
            {
                Debug.Log(LocalConfig.GameServerIp);
                Debug.Log(LocalConfig.GameServerPort);
            }
            else
            {
                Debug.Log("No local config found.");
            }
        }
    }
}