using System.Collections.Generic;
using Game;
using Newtonsoft.Json;
using UnityEngine;

namespace DiepFake.Scenes.Game.ReplaySystem
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private Pacman pacman;

        private readonly AtlasHelper _atlasHelper = new();
        public readonly List<Snapshot> Snapshots = new();

        private void Start()
        {
            InvokeRepeating(nameof(CreateSnapshot), 0f, Constants.RecordingSpeed);
        }

        public void StartNewRecording()
        {
            Snapshots!.Clear();
        }

        public void PersistRecording()
        {
            var recording = new Recording { Snapshots = Snapshots };
            //await atlasHelper!.PersistRecording(recording);
            StartCoroutine(_atlasHelper!.RecordSnapshot(JsonConvert.SerializeObject(recording), result =>
            {
                if (result) Debug.Log("Saved!");
                Debug.Log(result);
            }));
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                PacManPosition = new Position(Helper.InvertY(pacman!.transform.position))
            };
            Snapshots!.Add(snapshot);
        }
    }
}