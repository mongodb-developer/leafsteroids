using System.Collections;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class Bullet : MonoBehaviour
    {
        public float damage;

        private float _lifespan;

        private void Awake()
        {
            SessionStatistics.Instance!.BulletsFired++;
        }

        private void Start()
        {
            _lifespan = GameConfigLoader.Instance!.GameConfig!.BulletLifespan;
            StartCoroutine(DestroyBullet());
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision!.gameObject!.CompareTag("wall")) Destroy(gameObject);
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(_lifespan);
            Destroy(gameObject);
        }
    }
}