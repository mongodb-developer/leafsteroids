using JetBrains.Annotations;
using Newtonsoft.Json;

namespace _00_Shared
{
    public class RegisteredPlayer
    {
        [CanBeNull] public string Email;
        [JsonProperty("_id")]
        public string Id;
        // [CanBeNull] public string location;
        public string Nickname;
        [CanBeNull] public string TeamName;
    }
}