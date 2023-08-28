using Newtonsoft.Json;

namespace _00_Shared.Map
{
    public class PalletDto
    {
        [JsonProperty("position")] public ObjectPosition Position { get; set; }
        [JsonProperty("pallet_type")] public PalletType PalletType { get; set; }
    }
}