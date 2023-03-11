using System.Collections.Generic;
using __Shared;
using Newtonsoft.Json;
using UnityEngine;

namespace _MainScene._ReplaySystem
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private PlayerController player;

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
            var recording = new Recording { Snapshots = _snapshots };
            StartCoroutine(AtlasHelper.RecordSnapshot(JsonConvert.SerializeObject(recording),
                result => { Debug.Log($"Finished saving recording: {result}"); }));
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                PlayerPosition = new Position(Helper.InvertY(player!.transform.position))
            };
            _snapshots!.Add(snapshot);
        }
    }
}