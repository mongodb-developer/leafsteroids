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
        [SerializeField] private GameObject blurPanel;
        [SerializeField] private Recorder recorder;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;

        private GameConfig _gameConfig;
        private float _timeRemainingS;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;

            Debug.Log("GameManager - PlayerName: " + _gameConfig.Player!.Name);
            Debug.Log("GameManager - EventId: " + _gameConfig.Event!.Id);
            Debug.Log("GameManager - EventName: " + _gameConfig.Event!.Name);
        }

        private void Start()
        {
            playerTextField!.text = $"Player: {_gameConfig!.Player!.Name}";
            ToggleGameOverOverlay(false);

            // Intro
            canvas!.gameObject.SetActive(false);

            // Start
            canvas!.gameObject.SetActive(true);
            mainCamera!.GetComponent<CameraPosition>()!.enabled = true;
            InvokeRepeating(nameof(UpdateTimer), 0f, 1f);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _timeRemainingS = _gameConfig!.RoundDuration;
        }

        public void GameOver()
        {
            Time.timeScale = 0f;
            ToggleGameOverOverlay(true);
            Debug.Log("PlayerName: " + _gameConfig!.Player!.Name);
            Debug.Log("EventId: " + _gameConfig!.Event!.Id);
            recorder!.PersistRecording();
            recorder!.StartNewRecording();
        }

        private void UpdateTimer()
        {
            _timeRemainingS--;
            _timeRemainingS = Mathf.Clamp(_timeRemainingS, 0, _gameConfig!.RoundDuration);
            timeTextField!.text = $"Time remaining: {_timeRemainingS}";
            if (_timeRemainingS <= 0)
                GameOver();
        }

        private void ToggleGameOverOverlay(bool shouldShow)
        {
            gameOverText!.gameObject.SetActive(shouldShow);
            gameOverSubText!.gameObject.SetActive(shouldShow);
            blurPanel!.SetActive(shouldShow);
        }
    }
}