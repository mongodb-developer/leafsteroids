using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace _00_Shared.Map
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class PowerUpDto
    {
        [JsonProperty("position")] public ObjectPosition Position { get; set; }
    }
}