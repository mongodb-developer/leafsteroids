using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using _00_Shared;
using _1_Loading;
using _6_Main._ReplaySystem;
using TMPro;
using UnityEngine;

namespace _2_ConferenceSelection
{
    public class ConferenceSelection : MonoBehaviour
    {
        [SerializeField] private TMP_Text slot1;
        [SerializeField] private TMP_Text slot2;
        [SerializeField] private TMP_Text slot3;
        [SerializeField] private TMP_Text slot4;
        [SerializeField] private TMP_Text slot5;
        private int _currentIndex;

        private List<Conference> _conferences;
        private bool _stickMoved;

        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private async Task Awake()
        {
            _conferences = new List<Conference>();
            await ReloadConferenceList();
        }

        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private async Task Update()
        {
            if (ButtonMappings.CheckReloadKey()) await ReloadConferenceList();
            CheckJoystickInput();
            UpdateConferenceList();
            if (ButtonMappings.CheckConfirmKey()) SelectConference();
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

            _currentIndex = Mathf.Clamp(_currentIndex, 0, _conferences!.Count - 1);
        }

        private async Task ReloadConferenceList()
        {
            _conferences = await RestClient.GetConferences();
            UpdateConferenceList();
        }

        private void UpdateConferenceList()
        {
            var slot1Index = _currentIndex - 2;
            var slot2Index = _currentIndex - 1;
            var slot3Index = _currentIndex;
            var slot4Index = _currentIndex + 1;
            var slot5Index = _currentIndex + 2;

            slot1!.text = slot1Index >= 0 && slot1Index < _conferences!.Count ? _conferences[slot1Index]!.Name : "";
            slot2!.text = slot2Index >= 0 && slot2Index < _conferences!.Count ? _conferences[slot2Index]!.Name : "";
            slot3!.text = slot3Index >= 0 && slot3Index < _conferences!.Count
                ? $"===> {_conferences[slot3Index]!.Name} <==="
                : "";
            slot4!.text = slot4Index >= 0 && slot4Index < _conferences!.Count ? _conferences[slot4Index]!.Name : "";
            slot5!.text = slot5Index >= 0 && slot5Index < _conferences!.Count ? _conferences[slot5Index]!.Name : "";
        }

        private void SelectConference()
        {
            if (_conferences == null || _conferences.Count == 0) return;
            GameConfigLoader.Instance!.GameConfig!.Conference = _conferences![_currentIndex];
            SceneNavigation.SwitchToWelcome();
        }
    }
}