using System.Linq;
using UnityEngine;

namespace Recording
{
    public class Recorder : MonoBehaviour
    {
        [SerializeField] public Pacman pacman;
        [SerializeField] public Ghost chuck;
        [SerializeField] public Ghost nic;
        [SerializeField] public Ghost hubert;
        [SerializeField] public Ghost dominic;

        public Recording Recording;

        private void Awake()
        {
            Recording = new Recording();
        }

        private void Start()
        {
            InvokeRepeating(nameof(CreateSnapshot), 0f, 0.1f);
        }

        private void CreateSnapshot()
        {
            if (pacman == null || chuck == null || nic == null || hubert == null || dominic == null)
            {
                Debug.LogError("Objects must not be null.");
                return;
            }

            var pacmanPosition = new Position(pacman.transform.position);
            var chuckPosition = new Position(chuck.transform.position);
            var nicPosition = new Position(nic.transform.position);
            var hubertPosition = new Position(hubert.transform.position);
            var dominicPosition = new Position(dominic.transform.position);

            var snapshot = new Snapshot(pacmanPosition, chuckPosition, nicPosition, hubertPosition, dominicPosition);
            _ = Recording!.Snapshots.Append(snapshot);
        }
    }
}