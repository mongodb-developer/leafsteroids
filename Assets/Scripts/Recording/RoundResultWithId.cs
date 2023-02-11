using System;

namespace Recording
{
    [Serializable]
    public class RoundResultWithId : RoundResultWithoutId
    {
        public string id;

        public override string ToString()
        {
            if (locations!.Length == 1) return $"{locations[0]}";

            var locationsString = $"{locations[0]}";
            for (var i = 1; i < locations.Length; i++) locationsString += $", {locations[i]}";
            return $"{id} | {locationsString}";
        }
    }
}