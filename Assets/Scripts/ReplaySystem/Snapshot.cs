using System;
using UnityEngine;

namespace ReplaySystem
{
    [Serializable]
    public struct Snapshot
    {
        public Vector3 PacManPosition { get; }
        public Vector3 ChuckPosition { get; }
        public Vector3 NicPosition { get; }
        public Vector3 HubertPosition { get; }
        public Vector3 DominicPosition { get; }

        public Snapshot(Vector3 pacManPosition, Vector3 chuckPosition, Vector3 nicPosition, Vector3 hubertPosition,
            Vector3 dominicPosition)
        {
            PacManPosition = pacManPosition;
            ChuckPosition = chuckPosition;
            NicPosition = nicPosition;
            HubertPosition = hubertPosition;
            DominicPosition = dominicPosition;
        }

        public override string ToString()
        {
            return $"\n{PacManPosition}\n{ChuckPosition}\n{NicPosition}\n{HubertPosition}\n{DominicPosition}";
        }
    }
}