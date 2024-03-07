using System.Collections.Generic;
using _00_Shared;
using _1_Loading;
using TMPro;
using UnityEngine;
using Event = _3_Main._ReplaySystem.Event;
using System.Runtime.InteropServices;

public class EventSelection : MonoBehaviour
{
    [SerializeField] private TMP_Text slot1;
    [SerializeField] private TMP_Text slot2;
    [SerializeField] private TMP_Text slot3;
    [SerializeField] private TMP_Text slot4;
    [SerializeField] private TMP_Text slot5;
    private int _currentIndex;

    private List<Event> _events;
    private bool _stickMoved;

    private void Awake()
    {
        _events = new List<Event>();
        ReloadEventList();
    }

    private void Update()
    {
        if (ButtonMappings.CheckReloadKey()) ReloadEventList();
        CheckJoystickInput();
        UpdateEventsList();
        if (ButtonMappings.CheckConfirmKey()) SelectEvent();
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

        _currentIndex = Mathf.Clamp(_currentIndex, 0, _events!.Count - 1);
    }

    private void ReloadEventList()
    {
        StartCoroutine(
            AtlasHelper.GetEvents(result =>
            {
                _events = result;
                GameConfigLoader.Instance!.GameConfig!.Event = _events![0];
                SceneNavigation.SwitchToWelcome();
            })
        );
    }

    private void UpdateEventsList()
    {
        var slot1Index = _currentIndex - 2;
        var slot2Index = _currentIndex - 1;
        var slot3Index = _currentIndex;
        var slot4Index = _currentIndex + 1;
        var slot5Index = _currentIndex + 2;

        slot1!.text = slot1Index >= 0 && slot1Index < _events!.Count ? _events[slot1Index]!.Name : "";
        slot2!.text = slot2Index >= 0 && slot2Index < _events!.Count ? _events[slot2Index]!.Name : "";
        slot3!.text = slot3Index >= 0 && slot3Index < _events!.Count
            ? $"===> {_events[slot3Index]!.Name} <==="
            : "";
        slot4!.text = slot4Index >= 0 && slot4Index < _events!.Count ? _events[slot4Index]!.Name : "";
        slot5!.text = slot5Index >= 0 && slot5Index < _events!.Count ? _events[slot5Index]!.Name : "";


    }

    private void SelectEvent()
    {
        if (_events == null || _events.Count == 0) return;
        GameConfigLoader.Instance!.GameConfig!.Event = _events![_currentIndex];
        SceneNavigation.SwitchToWelcome();
    }
}