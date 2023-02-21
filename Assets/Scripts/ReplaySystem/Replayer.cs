using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReplaySystem
{
    public class Replayer : MonoBehaviour
    {
        private ReplayGhost[] replayGhosts;
        private ReplayPacman replayPacman;
        private List<Snapshot> snapshots;

        public void ReplayGame(List<Snapshot> s, ReplayPacman rPacman, ReplayGhost[] rGhosts)
        {
            snapshots = s;
            replayPacman = rPacman;
            replayGhosts = rGhosts;
            InvokeRepeating(nameof(PlayNextSnapshot), 0f, Constants.RecordingSpeed);
        }

        private void PlayNextSnapshot()
        {
            if (snapshots!.Count == 0) return;

            var nextSnapshot = snapshots!.First();

            replayPacman!.transform.position = Helper.InvertY(nextSnapshot.PacManPosition!.ToVector3());
            replayGhosts![0]!.transform.position = Helper.InvertY(nextSnapshot.ChuckPosition!.ToVector3());
            replayGhosts![1]!.transform.position = Helper.InvertY(nextSnapshot.NicPosition!.ToVector3());
            replayGhosts![2]!.transform.position = Helper.InvertY(nextSnapshot.HubertPosition!.ToVector3());
            replayGhosts![3]!.transform.position = Helper.InvertY(nextSnapshot.DominicPosition!.ToVector3());

            snapshots.RemoveAt(0);
        }
    }
}