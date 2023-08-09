using System.Diagnostics.CodeAnalysis;

namespace RestService.Entities;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
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