using JetBrains.Annotations;
using Newtonsoft.Json;

namespace _00_Shared
{
    public class RegisteredPlayer
    {
        [JsonProperty("_id")]
        public string Id;
        public string Nickname { get; set; }
        [JsonProperty("location")]
        [CanBeNull] public string Location;
        [CanBeNull] public string TeamName;
        [CanBeNull] public string Email;
    }
}