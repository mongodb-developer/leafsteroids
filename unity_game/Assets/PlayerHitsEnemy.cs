using _3_Main;
using UnityEngine;

public class PlayerHitsEnemy : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        if (collision.gameObject == null) return;
        if (collision.gameObject.GetComponent<Enemy>() == null) return;
        SessionStatistics.Instance!.Score = 0;
        gameManager!.GameOver();
    }
}