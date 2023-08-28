using System.Diagnostics.CodeAnalysis;
using _00_Shared;
using Newtonsoft.Json;

namespace _6_Main._ReplaySystem
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Snapshot
    {
        [JsonProperty("position")] public ObjectPosition ObjectPosition { get; set; }
    }
}