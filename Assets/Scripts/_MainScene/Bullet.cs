using System.Collections;
using __Shared;
using _LoadingScene;
using UnityEngine;

namespace _MainScene
{
    public class Bullet : MonoBehaviour
    {
        private GameConfig _gameConfig;

        private void Awake()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
        }

        private void Start()
        {
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