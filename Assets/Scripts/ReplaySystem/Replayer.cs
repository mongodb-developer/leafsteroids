using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReplaySystem
{
    public class Replayer : MonoBehaviour
    {
        private List<Snapshot> snapshots;
        private ReplayPacman replayPacman;
        private ReplayGhost[] replayGhosts;

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

            replayPacman!.transform.position = Helper.InvertY(nextSnapshot.PacManPosition);
            replayGhosts![0]!.transform.position = Helper.InvertY(nextSnapshot.ChuckPosition);
            replayGhosts![1]!.transform.position = Helper.InvertY(nextSnapshot.NicPosition);
            replayGhosts![2]!.transform.position = Helper.InvertY(nextSnapshot.HubertPosition);
            replayGhosts![3]!.transform.position = Helper.InvertY(nextSnapshot.DominicPosition);

            snapshots.RemoveAt(0);
        }
    }
}