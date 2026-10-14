using Newtonsoft.Json;

namespace website.Data;

public class NamedValue
{
    [JsonProperty("label")] public string Label { get; set; } = "";
    [JsonProperty("value")] public long Value { get; set; }
}

public class LeaderboardRow
{
    [JsonProperty("player")] public string Player { get; set; } = "";
    [JsonProperty("score")] public long Score { get; set; }
}

public class PlayRow
{
    [JsonProperty("time")] public string Time { get; set; } = "";
    [JsonProperty("score")] public long Score { get; set; }
}

public class HeatmapPoint
{
    [JsonProperty("x")] public int X { get; set; }
    [JsonProperty("z")] public int Z { get; set; }
    [JsonProperty("count")] public long Count { get; set; }
}

public class DashboardData
{
    [JsonProperty("gamesPlayed")] public long GamesPlayed { get; set; }
    [JsonProperty("uniquePlayers")] public long? UniquePlayers { get; set; }
    [JsonProperty("totalBulletsFired")] public long TotalBulletsFired { get; set; }
    [JsonProperty("totalDamage")] public long TotalDamage { get; set; }
    [JsonProperty("pelletsDestroyed")] public List<NamedValue> PelletsDestroyed { get; set; } = new();
    [JsonProperty("powerUpsCollected")] public List<NamedValue> PowerUpsCollected { get; set; } = new();
    [JsonProperty("leaderboard")] public List<LeaderboardRow>? Leaderboard { get; set; }
    [JsonProperty("plays")] public List<PlayRow>? Plays { get; set; }
    [JsonProperty("activity")] public List<NamedValue> Activity { get; set; } = new();
    [JsonProperty("activityValueLabel")] public string ActivityValueLabel { get; set; } = "";
    [JsonProperty("heatmap")] public List<HeatmapPoint> Heatmap { get; set; } = new();
}
