using Newtonsoft.Json;

namespace ReplaySystem
{
    public struct Payload
    {
        [JsonProperty("dataSource")] public string DataSource;
        [JsonProperty("database")] public string Database;
        [JsonProperty("collection")] public string Collection;
        [JsonProperty("document")] public Recording Document;
    }
}ª