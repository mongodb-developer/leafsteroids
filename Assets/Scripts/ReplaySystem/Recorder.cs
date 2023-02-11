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

        private List<Snapshot> Snapshots { get; } = new();
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
            InvokeRepeating(nameof(CreateSnapshot), 0f, 0.1f);
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                PacManPosition = InvertY(pacman!.transform.position),
                ChuckPosition = InvertY(chuck!.transform.position),
                NicPosition = InvertY(nic!.transform.position),
                HubertPosition = InvertY(hubert!.transform.position),
                DominicPosition = InvertY(dominic!.transform.position)
            };
            Snapshots!.Add(snapshot);
        }

        private static Vector3 InvertY(Vector3 vector)
        {
            return new Vector3(vector.x, -vector.y, vector.z);
        }
    }
}