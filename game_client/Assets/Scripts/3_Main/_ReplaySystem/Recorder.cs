using System.Collections.Generic;
using _00_Shared;
using _1_Loading;
using Newtonsoft.Json;
using UnityEngine;

namespace _3_Main._ReplaySystem
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private Player player;

        private readonly List<Snapshot> _snapshots = new();

        private void Start()
        {
            InvokeRepeating(nameof(CreateSnapshot), 0f, Constants.RecordingSpeed);
        }

        public void StartNewRecording()
        {
            _snapshots!.Clear();
        }

        public void PersistRecording()
        {
            var gameConfig = GameConfigLoader.Instance!.GameConfig!;
            Debug.Log("PlayerName: " + gameConfig.Player!.Name);
            Debug.Log("EventId: " + gameConfig.Event!.Id);
            var recording = new Recording
            {
                Snapshots = _snapshots,
                PlayerName = gameConfig.Player!.Name,
                EventId = gameConfig.Event!.Id
            };
            StartCoroutine(
                AtlasHelper.RecordSnapshot(
                    JsonConvert.SerializeObject(recording),
                    _ => { Debug.Log("Recording persisted."); })
            );
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                Position = new Position(Helper.InvertY(player!.transform.position))
            };
            _snapshots!.Add(snapshot);
        }
    }
}