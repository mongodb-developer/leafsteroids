using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game;
using UnityEngine;

namespace ReplaySystem
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] private Pacman pacman;
        [SerializeField] private Ghost chuck;
        [SerializeField] private Ghost nic;
        [SerializeField] private Ghost hubert;
        [SerializeField] private Ghost dominic;

        public readonly List<Snapshot> Snapshots = new();
        private readonly AtlasHelper atlasHelper = new();

        public void StartNewRecording()
        {
            Snapshots!.Clear();
        }

        public void PersistRecording()
        {
            var recording = new Recording
            {
                DateTime = DateTime.Now,
                Snapshots = Snapshots
            };
            Task.Run(async () => await atlasHelper!.PersistRecording(recording));
        }

        private void Start()
        {
            InvokeRepeating(nameof(CreateSnapshot), 0f, Constants.RecordingSpeed);
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                PacManPosition = Helper.InvertY(pacman!.transform.position),
                ChuckPosition = Helper.InvertY(chuck!.transform.position),
                NicPosition = Helper.InvertY(nic!.transform.position),
                HubertPosition = Helper.InvertY(hubert!.transform.position),
                DominicPosition = Helper.InvertY(dominic!.transform.position)
            };
            Snapshots!.Add(snapshot);
        }
    }
}