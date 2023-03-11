using System.Collections;
using UnityEngine;

namespace DiepFake.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public float damage = 15f;

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
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}