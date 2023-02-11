using System;

[Serializable]
public class Location
{
    public float x;
    public float y;

    public bool Equals(Location other)
    {
        return Math.Abs(x - other!.x) < 0.001f && Math.Abs(y - other.y) < 0.001f;
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }
}