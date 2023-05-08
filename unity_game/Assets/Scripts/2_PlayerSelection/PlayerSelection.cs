using System;
using System.Collections.Generic;
using _00_Shared;
using _1_Loading;
using TMPro;
using UnityEngine;

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
            _players = new List<RegisteredPlayer>();
            ReloadPlayerList();
        }

        private void Update()
        {
            if (ButtonMappings.CheckReloadKey()) ReloadPlayerList();

            if (ButtonMappings.CheckAnyUpKey() && !_stickMoved)
            {
                _currentIndex--;
                _stickMoved = true;
            }
            else if (ButtonMappings.CheckAnyDownKey() && !_stickMoved)
            {
                _currentIndex++;
                _stickMoved = true;
            }
            else if (!ButtonMappings.CheckAnyUpKey() && !ButtonMappings.CheckAnyDownKey())
            {
                _stickMoved = false;
            }

            _currentIndex = Mathf.Clamp(_currentIndex, 0, _players!.Count - 1);
            UpdatePlayerList();

            if (ButtonMappings.CheckConfirmKey()) SelectPlayer();
        }

        private void ReloadPlayerList()
        {
            StartCoroutine(
                AtlasHelper.GetPlayers(result =>
                {
                    _players = result;
                    _players!.Sort((x, y) => string.Compare(x!.Nickname!, y!.Nickname, StringComparison.Ordinal));
                    UpdatePlayerList();
                })
            );
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
            if (_players == null || _players.Count == 0) return;
            GameConfigLoader.Instance!.GameConfig!.Player = _players![_currentIndex];
            SceneNavigation.SwitchToInstructions();
        }
    }
}