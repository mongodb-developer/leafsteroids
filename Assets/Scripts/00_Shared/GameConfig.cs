using JetBrains.Annotations;

namespace _00_Shared
{
    public class GameConfig
    {
        public float BulletDamage;
        public float BulletLifespan;
        public float BulletSpeed;
        public string Id;
        public float PelletHeatlhLarge;
        public float PelletHeatlhMedium;
        public float PelletHeatlhSmall;
        [CanBeNull] public RegisteredPlayer Player;
        public float PlayerMoveSpeed;
        public float PlayerRotateSpeed;
        public float RoundDuration;
    }
}