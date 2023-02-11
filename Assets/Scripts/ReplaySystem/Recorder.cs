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

        public void PersistResorcing()
        {
            var recording = new Recording(Snapshots!.ToArray());
            Task.Run(async () => await atlasHelper!.PersistRecording(recording));
        }

        private void Start()
        {
            InvokeRepeating(nameof(CreateSnapshot), 0f, 0.1f);
        }

        private void CreateSnapshot()
        {
            var pacmanPosition = pacman!.transform.position;
            var chuckPosition = chuck!.transform.position;
            var nicPosition = nic!.transform.position;
            var hubertPosition = hubert!.transform.position;
            var dominicPosition = dominic!.transform.position;

            var snapshot = new Snapshot(pacmanPosition, chuckPosition, nicPosition, hubertPosition, dominicPosition);
            Snapshots!.Add(snapshot);
        }
    }
}