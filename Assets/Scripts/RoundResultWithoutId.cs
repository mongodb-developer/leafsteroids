using System;

[Serializable]
public class RoundResultWithoutId
{
    public Location[] locations;

    public override string ToString()
    {
        if (locations.Length == 1) return $"{locations[0]}";

        var locationsString = $"{locations[0]}";
        for (var i = 1; i < locations.Length; i++) locationsString += $", {locations[i]}";
        return locationsString;
    }
}