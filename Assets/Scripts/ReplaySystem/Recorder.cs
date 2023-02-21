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

        private readonly AtlasHelper atlasHelper = new();
        public readonly List<Snapshot> Snapshots = new();

        private void Start()
        {
            InvokeRepeating(nameof(CreateSnapshot), 0f, Constants.RecordingSpeed);
        }

        public void StartNewRecording()
        {
            Snapshots!.Clear();
        }

        public async Task PersistRecording()
        {
            var recording = new Recording { Snapshots = Snapshots };
            await atlasHelper!.PersistRecording(recording);
        }

        private void CreateSnapshot()
        {
            var snapshot = new Snapshot
            {
                ChuckPosition = new Position(Helper.InvertY(chuck!.transform.position)),
                DominicPosition = new Position(Helper.InvertY(dominic!.transform.position)),
                HubertPosition = new Position(Helper.InvertY(nic!.transform.position)),
                NicPosition = new Position(Helper.InvertY(hubert!.transform.position)),
                PacManPosition = new Position(Helper.InvertY(pacman!.transform.position))
            };
            Snapshots!.Add(snapshot);
        }
    }
}