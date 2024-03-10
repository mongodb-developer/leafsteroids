using Newtonsoft.Json;
namespace website.Data;

public class SessionStatisticsPlain
{
    public int BulletsFired { get; set; }
    public int DamageDone { get; set; }
    public int PelletsDestroyedSmall { get; set; }
    public int PelletsDestroyedMedium { get; set; }
    public int PelletsDestroyedLarge { get; set; }
    public int Score { get; set; }
    public int PowerUpBulletDamageCollected { get; set; }
    public int PowerUpBulletSpeedCollected { get; set; }
    public int PowerUpPlayerSpeedCollected { get; set; }
}

public class SimilarRecordings
{
    [JsonProperty("id")]
    public string? Id { get; set; }
    [JsonProperty("sessionStatisticsPlain")]
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
}