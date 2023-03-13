using _00_Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1_Loading
{
    public class GameConfigLoader : MonoBehaviour
    {
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
            if (GameConfig != null) return;
            StartCoroutine(
                AtlasHelper.GetConfig(result =>
                {
                    GameConfig = result;
                    SceneManager.LoadScene("2_PlayerSelection");
                })
            );
        }
    }
}