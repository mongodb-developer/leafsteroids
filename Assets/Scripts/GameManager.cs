using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;

    public TMP_Text textField;

    // public Recorder recorder;
    // private readonly AtlasHelper atlasHelper = new();

    private string username = "";

    private int GhostMultiplier { get; set; } = 1;
    private int Score { get; set; }
    private int Lives { get; set; }

    private void Start()
    {
        SetScore(0);
        SetLives(3);
    }


    private void Update()
    {
        if (Lives <= 0 && Input.anyKeyDown)
        {
            Debug.Log("Update fired new game.");
            NewGame();
        }
    }

    public void StartButtonClicked()
    {
        username = textField!.text;
        Debug.Log($"username.Length: {username!.Length}, username: >>>{username}<<<");
        if (username.Length <= 3)
        {
            Debug.Log("StartButtonClicked returned.");
            return;
        }

        Debug.Log("StartButtonClicked starts a new game.");

        ResetState();
        NewGame();
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
        gameOverText!.enabled = true;

        for (var i = 0; i < ghosts!.Length; i++) ghosts[i]!.gameObject.SetActive(false);

        pacman!.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        Lives = lives;
        livesText!.text = "x" + lives;
    }

    private void SetScore(int score)
    {
        Score = score;
        scoreText!.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        // Task.Run(() => atlasHelper!.PersistRecording(recorder!.Recording));

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

        if (!HasRemainingPellets())
        {
            // Task.Run(() => atlasHelper!.PersistRecording(recorder!.Recording));

            pacman!.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
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