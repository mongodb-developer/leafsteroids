using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

    public int ghostMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    private const string dataApiUrlInsertOne =
        "https://data.mongodb-api.com/app/data-mmwob/endpoint/data/v1/action/insertOne";

    private readonly HttpClient client = new();

    private string username = "";

    private void Start()
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("api-key", "JX0THWuEm9AniPG9fx6B7E8dXg7GhRYcWLS312kvrscu8S0066R16t3TwXqzTQkl");
        SetScore(0);
        SetLives(3);
    }

    public void StartButtonClicked()
    {
        username = textField.text;
        Debug.Log($"username.Length: {username.Length}, username: >>>{username}<<<");
        if (username.Length <= 3)
        {
            Debug.Log("StartButtonClicked returned.");
            return;
        }

        Debug.Log("StartButtonClicked starts a new game.");

        ResetState();
        NewGame();
    }


    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            Debug.Log("Update fired new game.");
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }


    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        Task.Run(() => PostLocations());

        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0)
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
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            Task.Run(() => PostLocations());

            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    private async Task PostLocations()
    {
        var locations = pacman.locations.ToArray();
        var roundResultWithoutId = new RoundResultWithoutId { username = username, locations = locations };

        var insertOneValues = new PayloadWithDocument
        {
            dataSource = "Cluster0",
            database = "pacman",
            collection = "round-results",
            document = roundResultWithoutId
        };

        var jsonStringNewDocument = JsonUtility.ToJson(insertOneValues);
        await PostRequest(dataApiUrlInsertOne, jsonStringNewDocument);
        pacman.locations.Clear();
    }

    public async Task PostRequest(string url, string jsonString)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        stringContent.Headers.ContentType.CharSet = null;
        httpRequest.Content = stringContent;

        httpRequest.Content.Headers.Remove("Content-Type");
        httpRequest.Content.Headers.Add("Content-Type", "application/json");

        var response = await client.SendAsync(httpRequest);
        var resultString = await response.Content.ReadAsStringAsync();
        Debug.Log(resultString);
    }
}