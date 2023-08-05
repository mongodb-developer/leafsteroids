using UnityEngine;

namespace _3_Main
{
    public class SessionStatistics : MonoBehaviour
    {
        public static SessionStatistics Instance;

        public int BulletsFired { get; set; }
        public int DamageDone { get; set; }
        public int PelletsDestroyedSmall { get; set; }
        public int PelletsDestroyedMedium { get; set; }
        public int PelletsDestroyedLarge { get; set; }
        public int Score { get; set; }
        public int PowerUpBulletDamageCollected { get; set; }
        public int PowerUpBulletSpeedCollected { get; set; }
        public int PowerUpPlayerSpeedCollected { get; set; }


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Reset()
        {
            BulletsFired = 0;
            DamageDone = 0;
            PelletsDestroyedSmall = 0;
            PelletsDestroyedMedium = 0;
            PelletsDestroyedLarge = 0;
            Score = 0;
        }

        public SessionStatisticsPlain GetPlainCopy()
        {
            return new SessionStatisticsPlain
            {
                BulletsFired = BulletsFired,
                DamageDone = DamageDone,
                PelletsDestroyedSmall = PelletsDestroyedSmall,
                PelletsDestroyedMedium = PelletsDestroyedMedium,
                PelletsDestroyedLarge = PelletsDestroyedLarge,
                Score = Score,
                PowerUpBulletDamageCollected = PowerUpBulletDamageCollected,
                PowerUpBulletSpeedCollected = PowerUpBulletSpeedCollected,
                PowerUpPlayerSpeedCollected = PowerUpPlayerSpeedCollected
            };
        }
    }
}