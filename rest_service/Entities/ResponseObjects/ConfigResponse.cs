using RestService.Entities.Atlas;

namespace RestService.Entities.ResponseObjects
{
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

        public ConfigResponse(ConfigAtlas configAtlas)
        {
            RoundDuration = configAtlas.RoundDuration;
            BulletDamage = configAtlas.BulletDamage;
            BulletSpeed = configAtlas.BulletSpeed;
            PlayerMoveSpeed = configAtlas.PlayerMoveSpeed;
            PlayerRotateSpeed = configAtlas.PlayerRotateSpeed;
            BulletLifespan = configAtlas.BulletLifespan;
            PelletHealthSmall = configAtlas.PelletHealthSmall;
            PelletHealthMedium = configAtlas.PelletHealthMedium;
            PelletHealthLarge = configAtlas.PelletHealthLarge;
        }
    }
}