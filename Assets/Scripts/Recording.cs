using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Recorder : MonoBehaviour
    {
        public Recording Recording = new();

        private void FixedUpdate()
        {
            var position = transform.position;
            var snapshot = new Snapshot(position.x, position.y, position.z);
            Recording.snapshots.Add(snapshot);
        }
    }

    public class Recording : MonoBehaviour
    {
        public readonly List<Snapshot> snapshots = new();
    }

    public class Snapshot
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Snapshot(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
}