using _00_Shared;
using _1_Loading;
using _3_Main._ReplaySystem;
using UnityEngine;

namespace _3_Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Recorder recorder;

        private GameConfig _gameConfig;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
        }

        private void Start()
        {
            Invoke(nameof(PersistRecording), _gameConfig!.RoundDuration);
        }

        private void PersistRecording()
        {
            recorder!.PersistRecording();
            SessionStatistics.Instance!.Reset();
        }
    }
}