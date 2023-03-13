using System.Linq;
using _00_Shared;
using MongoDB.Driver;
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
            var client = new MongoClient(Constants.ConnectionString);
            var database = client.GetDatabase("game");
            var collection = database!.GetCollection<GameConfig>("config");
            GameConfig = collection.Find(_ => true).ToList()!.First();
            SceneManager.LoadScene("2_PlayerSelection");
        }
    }
}