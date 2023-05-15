using _00_Shared;
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
            // InvokeRepeating(nameof(LoadConfig), 0f, 3f);
            LoadConfig();
        }

        private void LoadConfig()
        {
            Debug.Log(nameof(LoadConfig));
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
    }
}