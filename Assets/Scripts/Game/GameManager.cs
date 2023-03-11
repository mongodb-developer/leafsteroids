using DiepFake.Scenes.Game.ReplaySystem;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public Ghost[] ghosts;
        public Pacman pacman;
        public ReplayGhost[] replayGhosts;
        public ReplayPacman replayPacman;
        public Transform pellets;

        public Text gameOverText;
        public Text scoreText;
        public Text livesText;

        [SerializeField] private Recorder recorder;
        [SerializeField] private Replayer replayer;

        private readonly AtlasHelper _atlasHelper = new();

        private int GhostMultiplier { get; set; } = 1;
        private int Score { get; set; }
        private int Lives { get; set; }

        private void Start()
        {
            StartCoroutine(_atlasHelper!.GetSnapshot("640ba8ed160f99e8826f1040",
                result => { Debug.Log(JsonConvert.SerializeObject(result)); }));
            DisableAllGameObjects();
        }

        public void StartButtonPressed()
        {
            DisableAllGameObjects();

            recorder!.StartNewRecording();
            NewGame();
        }

        public void ReplayButtonPressed()
        {
            DisableAllGameObjects();
            ToggleReplayGameObjectState(true);
            replayer!.ReplayGame(recorder!.Snapshots, replayPacman, replayGhosts);
        }

        private void NewGame()
        {
            SetScore(0);
            SetLives(3);
            NewRound();
        }

        private void NewRound()
        {
            gameOverText!.enabled = false;

            foreach (Transform pellet in pellets!) pellet!.gameObject.SetActive(true);

            ResetState();
        }

        private void ResetState()
        {
            for (var i = 0; i < ghosts!.Length; i++) ghosts[i]!.ResetState();

            pacman!.ResetState();
        }

        private void GameOver()
        {
            DisableAllGameObjects();

            gameOverText!.enabled = true;

            recorder!.PersistRecording();
        }

        private void DisableAllGameObjects()
        {
            gameOverText!.enabled = false;

            for (var i = 0; i < ghosts!.Length; i++) ghosts[i]!.gameObject.SetActive(false);
            pacman!.gameObject.SetActive(false);

            ToggleReplayGameObjectState(false);
        }

        private void ToggleReplayGameObjectState(bool activate)
        {
            for (var i = 0; i < replayGhosts!.Length; i++) replayGhosts[i]!.gameObject.SetActive(activate);
            replayPacman!.gameObject.SetActive(activate);
        }

        private void SetLives(int lives)
        {
            Lives = lives;
            livesText!.text = $"x{lives}";
        }

        private void SetScore(int score)
        {
            Score = score;
            scoreText!.text = score.ToString().PadLeft(2, '0');
        }

        public void PacmanEaten()
        {
            pacman!.DeathSequence();

            SetLives(Lives - 1);

            if (Lives > 0)
                Invoke(nameof(ResetState), 3f);
            else
                GameOver();
        }

        public void GhostEaten(Ghost ghost)
        {
            var points = ghost!.points * GhostMultiplier;
            SetScore(Score + points);

            GhostMultiplier++;
        }

        public void PelletEaten(Pellet pellet)
        {
            pellet!.gameObject.SetActive(false);

            SetScore(Score + pellet.points);

            if (HasRemainingPellets()) return;

            pacman!.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
            recorder!.PersistRecording();
        }

        public void PowerPelletEaten(PowerPellet pellet)
        {
            for (var i = 0; i < ghosts!.Length; i++) ghosts[i]!.Frightened!.Enable(pellet!.duration);

            PelletEaten(pellet);
            CancelInvoke(nameof(ResetGhostMultiplier));
            Invoke(nameof(ResetGhostMultiplier), pellet!.duration);
        }

        private bool HasRemainingPellets()
        {
            foreach (Transform pellet in pellets!)
                if (pellet!.gameObject.activeSelf)
                    return true;

            return false;
        }

        private void ResetGhostMultiplier()
        {
            GhostMultiplier = 1;
        }
    }
}