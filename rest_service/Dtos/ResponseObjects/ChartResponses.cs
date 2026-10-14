namespace RestService.Dtos.ResponseObjects;

public class NamedValue
{
    public string Label { get; set; } = "";
    public long Value { get; set; }
}

public class LeaderboardRow
{
    public string Player { get; set; } = "";
    public long Score { get; set; }
}

public class PlayRow
{
    public string Time { get; set; } = "";
    public long Score { get; set; }
}

public class HeatmapPoint
{
    public int X { get; set; }
    public int Z { get; set; }
    public long Count { get; set; }
}

public class DashboardChartsResponse
{
    public long GamesPlayed { get; set; }
    public long? UniquePlayers { get; set; }
    public long TotalBulletsFired { get; set; }
    public long TotalDamage { get; set; }
    public List<NamedValue> PelletsDestroyed { get; set; } = new();
    public List<NamedValue> PowerUpsCollected { get; set; } = new();
    public List<LeaderboardRow>? Leaderboard { get; set; }
    public List<PlayRow>? Plays { get; set; }
    public List<NamedValue> Activity { get; set; } = new();
    public string ActivityValueLabel { get; set; } = "";
    public List<HeatmapPoint> Heatmap { get; set; } = new();
}
