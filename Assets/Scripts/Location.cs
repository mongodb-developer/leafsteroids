using System;

[Serializable]
public class Location
{
    public float x;
    public float y;

    public bool Equals(Location other)
    {
        return x == other.x && y == other.y;
    }

    public override string ToString()
    {
        return $"({x},{y})";
    }
}