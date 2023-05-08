using UnityEngine;

namespace _3_Main
{
    public class PlayerHitsEnemy : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null) return;
            if (collision.gameObject == null) return;
            if (collision.gameObject.GetComponentInParent<Enemy>() == null) return;
            SessionStatistics.Instance!.Score = 0;
            gameManager!.GameOver();
        }
    }
}