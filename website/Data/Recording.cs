using MongoDB.Bson;

namespace website.Data;

public class Recording
{
    public ObjectId Id { get; set; }
    public string? location { get; set; }
    public SessionStatisticsPlain? SessionStatisticsPlain { get; set; }
    public DateTime DateTime { get; set; }
    public Player? Player { get; set; }
    public Event? Event { get; set; }
    public List<Snapshot>? Snapshots { get; set; }
}

public class SessionStatisticsPlain
{
    public int? BulletsFired { get; set; }
    public int? DamageDone { get; set; }
    public int? PelletsDestroyedSmall { get; set; }
    public int? PelletsDestroyedMedium{ get; set; }
    public int? PelletsDestroyedLarge{ get; set; }
    public int? Score{ get; set; }
    public int? PowerUpBulletDamageCollected{ get; set; }
    public int? PowerUpBulletSpeedCollected{ get; set; }
    public int? PowerUpPlayerSpeedCollected{ get; set; }
}

public class Snapshot
{
    public PlayerPosition? PlayerPosition{ get; set; }
}

public class PlayerPosition
{
    public double? X{ get; set; }
    public double? Y{ get; set; }
    public double? Z{ get; set; }
}