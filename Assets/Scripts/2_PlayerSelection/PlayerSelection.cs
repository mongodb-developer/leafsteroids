using System;
using System.Collections.Generic;
using _00_Shared;
using _1_Loading;
using MongoDB.Driver;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _2_PlayerSelection
{
    public class PlayerSelection : MonoBehaviour
    {
        [SerializeField] private TMP_Text slot1;
        [SerializeField] private TMP_Text slot2;
        [SerializeField] private TMP_Text slot3;
        [SerializeField] private TMP_Text slot4;
        [SerializeField] private TMP_Text slot5;
        private int _currentIndex;

        private List<RegisteredPlayer> _players;
        private bool _stickMoved;

        private void Awake()
        {
            var mongoClient = new MongoClient(Constants.ConnectionString);
            var registrationDatabase = mongoClient.GetDatabase("registration");
            var playersCollection = registrationDatabase!.GetCollection<RegisteredPlayer>("players");
            _players = playersCollection.Find(_ => true).ToList();
            _players!.Sort((x, y) => string.Compare(x!.Nickname!, y!.Nickname, StringComparison.Ordinal));

            UpdatePlayerList();
        }

        private void Update()
        {
            var verticalInput = Input.GetAxis("Vertical");
            if (verticalInput > 0.5 && !_stickMoved)
            {
                _currentIndex--;
                _stickMoved = true;
            }
            else if (verticalInput < -0.5 && !_stickMoved)
            {
                _currentIndex++;
                _stickMoved = true;
            }
            else if (verticalInput == 0)
            {
                _stickMoved = false;
            }

            _currentIndex = Mathf.Clamp(_currentIndex, 0, _players!.Count - 1);
            UpdatePlayerList();

            if (Input.GetKeyDown(KeyCode.Joystick1Button1)) SelectPlayer();
        }

        private void UpdatePlayerList()
        {
            var slot1Index = _currentIndex - 2;
            var slot2Index = _currentIndex - 1;
            var slot3Index = _currentIndex;
            var slot4Index = _currentIndex + 1;
            var slot5Index = _currentIndex + 2;

            slot1!.text = slot1Index >= 0 && slot1Index < _players!.Count ? _players[slot1Index]!.Nickname : "";
            slot2!.text = slot2Index >= 0 && slot2Index < _players!.Count ? _players[slot2Index]!.Nickname : "";
            slot3!.text = slot3Index >= 0 && slot3Index < _players!.Count
                ? $"===> {_players[slot3Index]!.Nickname} <==="
                : "";
            slot4!.text = slot4Index >= 0 && slot4Index < _players!.Count ? _players[slot4Index]!.Nickname : "";
            slot5!.text = slot5Index >= 0 && slot5Index < _players!.Count ? _players[slot5Index]!.Nickname : "";
        }

        private void SelectPlayer()
        {
            GameConfigLoader.Instance!.GameConfig!.Player = _players![_currentIndex];
            SceneManager.LoadScene("3_Main");
        }
    }
}