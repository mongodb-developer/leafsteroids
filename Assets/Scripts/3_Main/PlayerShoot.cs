using _00_Shared;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private ButtonMappings buttonMappings;

        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;
        private float _bulletDamage;
        private float _bulletSpeed;

        private void Start()
        {
            _bulletSpeed = GameConfigLoader.Instance!.GameConfig!.BulletSpeed;
            _bulletDamage = GameConfigLoader.Instance!.GameConfig!.BulletDamage;
        }

        private void Update()
        {
            if (!buttonMappings!.CheckShootKey()) return;

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint!.position, bulletSpawnPoint.rotation);
            bullet!.GetComponent<Bullet>()!.damage = _bulletDamage;
            var bulletForce = bulletSpawnPoint.forward * _bulletSpeed;
            bullet!.GetComponent<Rigidbody>()!.AddForce(bulletForce);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null) return;

            if (other.GetComponent<PowerUpBulletSpeed>() != null)
            {
                _bulletSpeed = (float)(_bulletSpeed * 1.2);
                SessionStatistics.Instance!.PowerUpBulletSpeedCollected++;
            }

            if (other.GetComponent<PowerUpBulletDamage>() != null)
            {
                _bulletDamage = (float)(_bulletDamage * 1.5);
                SessionStatistics.Instance!.PowerUpBulletDamageCollected++;
            }

            Destroy(other.gameObject);
        }
    }
}