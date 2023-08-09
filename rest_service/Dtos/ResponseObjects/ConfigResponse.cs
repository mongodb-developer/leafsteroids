using System.Diagnostics.CodeAnalysis;
using RestService.Entities;

namespace RestService.Dtos.ResponseObjects;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class ConfigResponse
{
    public float RoundDuration { get; set; }
    public float BulletDamage { get; set; }
    public float BulletSpeed { get; set; }
    public float PlayerMoveSpeed { get; set; }
    public float PlayerRotateSpeed { get; set; }
    public float BulletLifespan { get; set; }
    public float PelletHealthSmall { get; set; }
    public float PelletHealthLarge { get; set; }
    public float PelletHealthMedium { get; set; }

    public ConfigResponse(Config config)
    {
        RoundDuration = config.RoundDuration;
        BulletDamage = config.BulletDamage;
        BulletSpeed = config.BulletSpeed;
        PlayerMoveSpeed = config.PlayerMoveSpeed;
        PlayerRotateSpeed = config.PlayerRotateSpeed;
        BulletLifespan = config.BulletLifespan;
        PelletHealthSmall = config.PelletHealthSmall;
        PelletHealthMedium = config.PelletHealthMedium;
        PelletHealthLarge = config.PelletHealthLarge;
    }
}