using Newtonsoft.Json;

namespace _00_Shared
{
    public class RegisteredPlayer
    {
        [JsonProperty("name")] public string Name { get; set; }
    }
}