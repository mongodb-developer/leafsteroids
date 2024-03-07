using _00_Shared;
using _1_Loading;
using _3_Main._ReplaySystem;
using TMPro;
using UnityEngine;

namespace _3_Main
{
    public class PlayerShoot : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;

        [SerializeField] private TMP_Text shootInstructions;

        private float _bulletDamage;
        private float _bulletSpeed;
        private bool _hasShoot;

        private void Start()
        {
            _bulletSpeed = GameConfigLoader.Instance!.GameConfig!.BulletSpeed;
            _bulletDamage = GameConfigLoader.Instance!.GameConfig!.BulletDamage;
            Invoke(nameof(ShowInstructions), 7f);
        }

        private void Update()
        {
            if (!ButtonMappings.CheckShootKey()) return;

            _hasShoot = true;

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

        private void ShowInstructions()
        {
            if (!_hasShoot)
                shootInstructions!.text = ButtonMappings.DetectedInputDevice switch
                {
                    DetectedInputDevice.Joystick => "Shoot with A!",
                    DetectedInputDevice.Keyboard => "Shoot using Space / Enter / Up Arrow!",
		    DetectedInputDevice.TouchScreen => "Shoot using the round button on the right!",
                    _ => shootInstructions!.text
                };

            Invoke(nameof(HideInstructions), 3f);
        }

        private void HideInstructions()
        {
            shootInstructions!.text = "";
        }
    }
}