using System.Diagnostics;
using ReplaySystem;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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

        private int GhostMultiplier { get; set; } = 1;
        private int Score { get; set; }
        private int Lives { get; set; }

        public void StartButtonPressed()
        {
            var stackTrace = new StackTrace();
            var methodBase = stackTrace.GetFrame(1)!.GetMethod();
            var methodName = methodBase!.Name;
            Debug.Log(methodName);
            recorder!.StartNewRecording();
            NewGame();
        }

        public void ReplayButtonPressed()
        {
            Debug.Log("ReplayButtonPressed");
        }

        private void Awake()
        {
            DisablePlayGameObjects();
            DisableReplayGameObjects();
        }

        // private void Update()
        // {
        //     if (Lives <= 0 && Input.anyKeyDown)
        //     {
        //         NewGame();
        //     }
        // }

        private void NewGame()
        {
            Debug.Log("NewGame");
            SetScore(0);
            SetLives(3);
            NewRound();
        }

        private void NewRound()
        {
            gameOverText!.enabled = false;

            foreach (Transform pellet in pellets!)
            {
                pellet!.gameObject.SetActive(true);
            }

            ResetState();
        }

        private void ResetState()
        {
            for (int i = 0; i < ghosts!.Length; i++)
            {
                ghosts[i]!.ResetState();
            }

            pacman!.ResetState();
        }

        private void GameOver()
        {
            DisablePlayGameObjects();

            gameOverText!.enabled = true;

            recorder!.PersistRecording();
        }

        private void DisablePlayGameObjects()
        {
            gameOverText!.enabled = false;

            for (int i = 0; i < ghosts!.Length; i++)
            {
                ghosts[i]!.gameObject.SetActive(false);
            }

            pacman!.gameObject.SetActive(false);
        }

        private void DisableReplayGameObjects()
        {
            gameOverText!.enabled = false;

            for (int i = 0; i < replayGhosts!.Length; i++)
            {
                replayGhosts[i]!.gameObject.SetActive(false);
            }

            replayPacman!.gameObject.SetActive(false);
        }

        private void SetLives(int lives)
        {
            Lives = lives;
            livesText!.text = "x" + lives.ToString();
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
            {
                Invoke(nameof(ResetState), 3f);
            }
            else
            {
                GameOver();
            }
        }

        public void GhostEaten(Ghost ghost)
        {
            int points = ghost!.points * GhostMultiplier;
            SetScore(Score + points);

            GhostMultiplier++;
        }

        public void PelletEaten(Pellet pellet)
        {
            pellet!.gameObject.SetActive(false);

            SetScore(Score + pellet.points);

            if (!HasRemainingPellets())
            {
                pacman!.gameObject.SetActive(false);
                Invoke(nameof(NewRound), 3f);
                recorder!.PersistRecording();
            }
        }

        public void PowerPelletEaten(PowerPellet pellet)
        {
            for (int i = 0; i < ghosts!.Length; i++)
            {
                ghosts[i]!.frightened!.Enable(pellet!.duration);
            }

            PelletEaten(pellet);
            CancelInvoke(nameof(ResetGhostMultiplier));
            Invoke(nameof(ResetGhostMultiplier), pellet!.duration);
        }

        private bool HasRemainingPellets()
        {
            foreach (Transform pellet in pellets!)
            {
                if (pellet!.gameObject.activeSelf)
                {
                    return true;
                }
            }

            return false;
        }

        private void ResetGhostMultiplier()
        {
            GhostMultiplier = 1;
        }
    }
}