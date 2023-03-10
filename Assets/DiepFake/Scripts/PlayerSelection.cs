using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using TMPro;
using UnityEngine;

public class Team
{
    public ObjectId Id { get; set; }
    public string Nickname { get; set; }
    public string TeamName { get; set; }
    public string Slogan { get; set; }
}

public class PlayerSelection : MonoBehaviour
{
    public List<GameObject> players;
    private int currentIndex;
    private IMongoCollection<Team> teams;

    private void Start()
    {
        var client =
            new MongoClient(
                "mongodb+srv://dbUser:dbUserPassword@cluster0.c8y2i2q.mongodb.net/?retryWrites=true&w=majority");
        var database = client.GetDatabase("registration");
        teams = database.GetCollection<Team>("players");

        var teamList = teams.Find(_ => true).ToList();
        Debug.Log(teamList.ToList().ToString());
        foreach (var team in teamList)
            Debug.Log($"Nickname: {team.Nickname}, Team Name: {team.TeamName}, Slogan: {team.Slogan}");

        for (var i = 0; i < players.Count; i++)
        {
            if (teamList.Count <= i) break;
            var team = teamList[i];
            var player = players[i];
            player.GetComponent<TMP_Text>().text = team.Nickname;
        }

        if (players.Count > 0) players[currentIndex].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangePlayer(-1);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangePlayer(1);
        else if (Input.GetKeyDown(KeyCode.Return)) SelectPlayer();
    }

    private void ChangePlayer(int direction)
    {
        players[currentIndex].SetActive(false);
        currentIndex = (currentIndex + direction + players.Count) % players.Count;
        players[currentIndex].SetActive(true);
    }

    private void SelectPlayer()
    {
        Debug.Log("Selected player: " + players[currentIndex].name);
        // Do whatever you want to do with the selected player
    }
}