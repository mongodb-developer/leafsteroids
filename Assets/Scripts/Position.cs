using UnityEngine;

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
}