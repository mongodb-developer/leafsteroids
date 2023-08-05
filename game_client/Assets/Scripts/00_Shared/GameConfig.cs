using _3_Main._ReplaySystem;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace _00_Shared
{
    public class GameConfig
    {
        [JsonProperty("bulletDamage")] public int BulletDamage { get; set; }

        [JsonProperty("bulletLifespan")] public int BulletLifespan { get; set; }
        [JsonProperty("bulletSpeed")] public int BulletSpeed { get; set; }
        [JsonProperty("pelletHealthSmall")] public int PelletHealthSmall { get; set; }
        [JsonProperty("pelletHealthMedium")] public int PelletHealthMedium { set; get; }
        [JsonProperty("pelletHealthLarge")] public int PelletHealthLarge { get; set; }
        [CanBeNull] public RegisteredPlayer Player { get; set; }
        [CanBeNull] public Event Event { get; set; }
        [JsonProperty("playerMoveSpeed")] public int PlayerMoveSpeed { get; set; }
        [JsonProperty("playerRotateSpeed")] public int PlayerRotateSpeed { get; set; }
        [JsonProperty("roundDuration")] public int RoundDuration { get; set; }
    }
}