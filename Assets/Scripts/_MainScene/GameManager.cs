using __Shared;
using _LoadingScene;
using _MainScene._ReplaySystem;
using UnityEngine;

namespace _MainScene
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;

        private GameConfig _gameConfig;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
            Debug.Log(_gameConfig!.RoundDuration);
        }

        private void Start()
        {
            Invoke(nameof(PersistRecording), _gameConfig!.RoundDuration);
        }

        private void PersistRecording()
        {
            recorder!.PersistRecording();
        }
    }
}