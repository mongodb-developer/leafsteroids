using UnityEngine;

namespace _3_Main
{
    public class SessionStatistics : MonoBehaviour
    {
        public static SessionStatistics Instance;

        public int BulletsFired { get; set; }
        public float DamageDone { get; set; }
        public float PelletsDestroyedSmall { get; set; }
        public float PelletsDestroyedMedium { get; set; }
        public float PelletsDestroyedLarge { get; set; }
        public float Score { get; set; }

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
            DamageDone = 0f;
            PelletsDestroyedSmall = 0f;
            PelletsDestroyedMedium = 0f;
            PelletsDestroyedLarge = 0f;
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
                Score = Score
            };
        }
    }
}