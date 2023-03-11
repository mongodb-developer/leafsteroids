using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiepFake.Scenes.Game.ReplaySystem
{
    public class Replayer : MonoBehaviour
    {
        private ReplayPacman _replayPacman;
        private List<Snapshot> _snapshots;

        public void ReplayGame(List<Snapshot> s, ReplayPacman rPacman, ReplayGhost[] rGhosts)
        {
            _snapshots = s;
            _replayPacman = rPacman;
            InvokeRepeating(nameof(PlayNextSnapshot), 0f, Constants.RecordingSpeed);
        }

        private void PlayNextSnapshot()
        {
            if (_snapshots!.Count == 0) return;

            var nextSnapshot = _snapshots!.First();

            _replayPacman!.transform.position = Helper.InvertY(nextSnapshot.PacManPosition!.ToVector3());

            _snapshots.RemoveAt(0);
        }
    }
}