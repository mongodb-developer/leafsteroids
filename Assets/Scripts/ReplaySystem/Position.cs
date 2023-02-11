using System;
using UnityEngine;

namespace ReplaySystem
{
    [Serializable]
    public struct Position
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }

        public Position(Vector3 position)
        {
            X = position.x;
            Y = position.y;
            Z = position.z;
        }

        public override string ToString()
        {
            return $"\n{X}\n{Y}\n{Z}";
        }
    }
}