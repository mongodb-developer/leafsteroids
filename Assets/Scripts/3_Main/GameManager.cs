using _00_Shared;
using _1_Loading;
using _3_Main._ReplaySystem;
using TMPro;
using UnityEngine;

namespace _3_Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeTextField;
        [SerializeField] private TMP_Text playerTextField;
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private TMP_Text gameOverSubText;
        [SerializeField] private Recorder recorder;

        private GameConfig _gameConfig;
        private float _timeRemainingS;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
            playerTextField!.text = $"Player: {_gameConfig!.Player!.Nickname}";
            gameOverText!.gameObject.SetActive(false);
            gameOverSubText!.gameObject.SetActive(false);
        }

        private void Start()
        {
            _timeRemainingS = _gameConfig!.RoundDuration;
            InvokeRepeating(nameof(UpdateTimer), 0f, 1f);
        }

        private void UpdateTimer()
        {
            _timeRemainingS--;
            _timeRemainingS = Mathf.Clamp(_timeRemainingS, 0, _gameConfig!.RoundDuration);
            timeTextField!.text = $"Time remaining: {_timeRemainingS}";
            if (_timeRemainingS > 0) return;

            Time.timeScale = 0f;
            gameOverText!.gameObject.SetActive(true);
            gameOverSubText!.gameObject.SetActive(true);
            PersistRecording();
        }

        private void PersistRecording()
        {
            recorder!.PersistRecording();
            SessionStatistics.Instance!.Reset();
        }
    }
}