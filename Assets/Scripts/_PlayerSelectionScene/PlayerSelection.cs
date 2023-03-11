using System.Collections.Generic;
using System.Linq;
using __Shared;
using MongoDB.Driver;
using TMPro;
using UnityEngine;

namespace _PlayerSelectionScene
{
    public class PlayerSelection : MonoBehaviour
    {
        public List<GameObject> players;
        private int _currentIndex;
        private IMongoCollection<Player> _teams;

        private void Start()
        {
            var client = new MongoClient(Constants.ConnectionString);
            var database = client.GetDatabase("registration");
            _teams = database!.GetCollection<Player>("players");

            var teamList = _teams.Find(_ => true).ToList();
            Debug.Log(teamList!.ToList().ToString());
            foreach (var team in teamList)
                Debug.Log($"Nickname: {team!.Nickname}, Team Name: {team.Team}, Slogan: {team.Slogan}");

            for (var i = 0; i < players!.Count; i++)
            {
                if (teamList.Count <= i) break;
                var team = teamList[i];
                var player = players[i];
                player!.GetComponent<TMP_Text>()!.text = team!.Nickname;
            }

            if (players.Count > 0) players[_currentIndex]!.SetActive(true);
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
            players![_currentIndex]!.SetActive(false);
            _currentIndex = (_currentIndex + direction + players.Count) % players.Count;
            players[_currentIndex]!.SetActive(true);
        }

        private void SelectPlayer()
        {
            Debug.Log("Selected player: " + players![_currentIndex]!.name);
            // Do whatever you want to do with the selected player
        }
    }
}