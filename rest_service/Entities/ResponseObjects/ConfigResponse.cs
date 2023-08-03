using RestService.Entities.Atlas;

namespace RestService.Entities.ResponseObjects
{
    public class ConfigResponse
    {
        public int? RoundDuration { get; set; }
        public int? BulletDamage { get; set; }
        public int? BulletSpeed { get; set; }
        public int? PlayerMoveSpeed { get; set; }
        public int? PlayerRotateSpeed { get; set; }
        public int? BulletLifespan { get; set; }
        public int? PelletHealthSmall { get; set; }
        public int? PelletHealthLarge { get; set; }
        public int? PelletHealthMedium { get; set; }

        public ConfigResponse(ConfigAtlas configAtlas)
        {
            RoundDuration = configAtlas.RoundDuration;
            BulletDamage = configAtlas.BulletDamage;
            BulletSpeed = configAtlas.BulletSpeed;
            PlayerMoveSpeed = configAtlas.PlayerMoveSpeed;
            PlayerRotateSpeed = configAtlas.PlayerRotateSpeed;
            BulletLifespan = configAtlas.BulletLifespan;
            PelletHealthSmall = configAtlas.PelletHeatlhSmall;
            PelletHealthLarge = configAtlas.PelletHeatlhLarge;
            PelletHealthMedium = configAtlas.PelletHeatlhMedium;
        }
    }
}