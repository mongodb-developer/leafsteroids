using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReplaySystem
{
    public class Replayer : MonoBehaviour
    {
        private ReplayGhost[] _replayGhosts;
        private ReplayPacman _replayPacman;
        private List<Snapshot> _snapshots;

        public void ReplayGame(List<Snapshot> s, ReplayPacman rPacman, ReplayGhost[] rGhosts)
        {
            _snapshots = s;
            _replayPacman = rPacman;
            _replayGhosts = rGhosts;
            InvokeRepeating(nameof(PlayNextSnapshot), 0f, Constants.RecordingSpeed);
        }

        private void PlayNextSnapshot()
        {
            if (_snapshots!.Count == 0) return;

            var nextSnapshot = _snapshots!.First();

            _replayPacman!.transform.position = Helper.InvertY(nextSnapshot.PacManPosition!.ToVector3());
            _replayGhosts![0]!.transform.position = Helper.InvertY(nextSnapshot.ChuckPosition!.ToVector3());
            _replayGhosts![1]!.transform.position = Helper.InvertY(nextSnapshot.NicPosition!.ToVector3());
            _replayGhosts![2]!.transform.position = Helper.InvertY(nextSnapshot.HubertPosition!.ToVector3());
            _replayGhosts![3]!.transform.position = Helper.InvertY(nextSnapshot.DominicPosition!.ToVector3());

            _snapshots.RemoveAt(0);
        }
    }
}