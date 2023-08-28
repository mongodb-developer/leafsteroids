using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using UnityEngine;

namespace _6_Main
{
    public class PlayerHitsEnemy : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        private bool playerHitEnemy;

        [SuppressMessage("ReSharper", "Unity.IncorrectMethodSignature")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private async Task OnCollisionEnter(Collision collision)
        {
            if (collision == null) return;
            if (collision.gameObject == null) return;
            if (collision.gameObject.GetComponentInParent<Enemy>() == null) return;

            if (playerHitEnemy) return;
            playerHitEnemy = true;

            SessionStatistics.Instance!.Score = 0;
            await gameManager!.GameOver();
        }
    }
}