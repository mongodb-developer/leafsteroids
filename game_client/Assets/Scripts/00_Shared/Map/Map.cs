using System.Collections.Generic;
using Newtonsoft.Json;

namespace _00_Shared.Map
{
    public class Map
    {
        [JsonProperty("pallets")] public List<PalletDto> Pallets;
        [JsonProperty("power_ups")] public List<PowerUpDto> PowerUps;
        [JsonProperty("enemies")] public List<EnemyDto> Enemies;
    }
}