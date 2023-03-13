using System.Collections;
using _00_Shared;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class Bullet : MonoBehaviour
    {
        private GameConfig _gameConfig;

        private void Awake()
        {
            SessionStatistics.Instance!.BulletsFired++;
        }

        private void Start()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
            StartCoroutine(DestroyBullet());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision!.gameObject!.CompareTag("wall")) Destroy(gameObject);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(_gameConfig!.BulletLifespan);
            Destroy(gameObject);
        }
    }
}