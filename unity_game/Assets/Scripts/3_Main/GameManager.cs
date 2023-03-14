using System.Collections;
using _00_Shared;
using _1_Loading;
using _3_Main._ReplaySystem;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace _3_Main
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeTextField;
        [SerializeField] private TMP_Text playerTextField;
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private TMP_Text gameOverSubText;
        [SerializeField] private GameObject blurPanel;
        [SerializeField] private TMP_Text version;
        [SerializeField] private Recorder recorder;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private PlayableDirector timelineDirector;
        [SerializeField] private Player player;
        [SerializeField] private GameObject enemies;

        private GameConfig _gameConfig;
        private float _timeRemainingS;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
        }

        private void Start()
        {
            StartCoroutine(WaitForTimeline());
        }

        private IEnumerator WaitForTimeline()
        {
            while (timelineDirector!.state == PlayState.Playing) yield return null;
            StartGame();
        }

        private void StartGame()
        {
            version!.text = $"Current version: {Constants.Version}";
            playerTextField!.text = $"Player: {_gameConfig!.Player!.Nickname}";
            ToggleGameOverOverlay(false);

            // Intro
            canvas!.gameObject.SetActive(false);

            // Start
            player!.gameObject!.GetComponent<PlayerMove>()!.enabled = true;
            player!.gameObject!.GetComponent<PlayerRotate>()!.enabled = true;
            player!.gameObject!.GetComponent<PlayerShoot>()!.enabled = true;
            foreach (var enemy in enemies!.gameObject!.GetComponentsInChildren<Enemy>()!)
                enemy!.gameObject.GetComponent<EnemyMovement>()!.enabled = true;
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