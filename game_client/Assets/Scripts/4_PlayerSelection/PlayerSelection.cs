using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using _00_Shared;
using _1_Loading;
using TMPro;
using UnityEngine;

namespace _4_PlayerSelection
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

        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private async Task Awake()
        {
            _players = new List<RegisteredPlayer>();
            await ReloadPlayerList();
        }

        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private async Task Update()
        {
            if (ButtonMappings.CheckReloadKey()) await ReloadPlayerList();
            CheckJoystickInput();
            CheckKeyboardInput();
            UpdatePlayerList();
            if (ButtonMappings.CheckConfirmKey()) SelectPlayer();
        }

        private void CheckKeyboardInput()
        {
            if (Input.inputString != null
                && Input.inputString.Length == 1)
            {
                char pressedCharacter = Input.inputString[0];

                if (char.IsLetter(pressedCharacter) && _players is { Count: > 0 })
                {
                    Debug.Log("Detected key code: " + pressedCharacter);
                    _currentIndex = _players.FindIndex(p => p!.Name!.StartsWith(pressedCharacter));
                }
            }
        }

        private void CheckJoystickInput()
        {
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
        }

        private async Task ReloadPlayerList()
        {
            _players = await RestClient.GetPlayers();
            _players!.Sort((x, y) => string.Compare(x!.Name!, y!.Name, StringComparison.Ordinal));
            UpdatePlayerList();
        }

        private void UpdatePlayerList()
        {
            var slot1Index = _currentIndex - 2;
            var slot2Index = _currentIndex - 1;
            var slot3Index = _currentIndex;
            var slot4Index = _currentIndex + 1;
            var slot5Index = _currentIndex + 2;

            slot1!.text = slot1Index >= 0 && slot1Index < _players!.Count ? _players[slot1Index]!.Name : "";
            slot2!.text = slot2Index >= 0 && slot2Index < _players!.Count ? _players[slot2Index]!.Name : "";
            slot3!.text = slot3Index >= 0 && slot3Index < _players!.Count
                ? $"===> {_players[slot3Index]!.Name} <==="
                : "";
            slot4!.text = slot4Index >= 0 && slot4Index < _players!.Count ? _players[slot4Index]!.Name : "";
            slot5!.text = slot5Index >= 0 && slot5Index < _players!.Count ? _players[slot5Index]!.Name : "";
        }

        private void SelectPlayer()
        {
            if (_players == null || _players.Count == 0) return;
            GameConfigLoader.Instance!.GameConfig!.Player = _players![_currentIndex];
            SceneNavigation.SwitchToInstructions();
        }
    }
}