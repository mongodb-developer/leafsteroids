using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace _6_Main._ReplaySystem
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Position
    {
        public Position(Vector3 position)
        {
            X = position.x;
            Y = position.y;
            Z = position.z;
        }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return $"{{ x={X}, y={Y}, z={Z} }}";
        }
    }
}