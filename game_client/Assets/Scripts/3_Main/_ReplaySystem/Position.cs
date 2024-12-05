using UnityEngine;

namespace _3_Main._ReplaySystem
{
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

        public Vector2 ToVector3()
        {
            return new Vector2(X, Z);
        }
 
        public override string ToString()
        {
            return $"{{ x={X},z={Z} }}";
        }
    }
}