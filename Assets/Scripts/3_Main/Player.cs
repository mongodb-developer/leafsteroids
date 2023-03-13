using _00_Shared;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private ButtonMappings buttonMappings;

        public float moveSpeed;
        public float rotateSpeed;

        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;

        private GameConfig _gameConfig;

        private void Start()
        {
            _gameConfig = GameConfigLoader.Instance!.GameConfig;
            moveSpeed = _gameConfig!.PlayerMoveSpeed;
            rotateSpeed = _gameConfig.PlayerRotateSpeed;
        }

        private void Update()
        {
            Move();
            Rotate();
            Shoot();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null) return;
            if (string.IsNullOrEmpty(other.tag)) return;
            if (!other.tag.Equals("powerup")) return;
            Destroy(other.gameObject);
        }

        private void Move()
        {
            var verticalMovement = buttonMappings!.GetVerticalAxis() * moveSpeed * Time.deltaTime;
            var horizontalMovement = buttonMappings.GetHorizontalAxis() * moveSpeed * Time.deltaTime;
            transform.Translate(horizontalMovement, 0, verticalMovement, Space.World);
        }

        private void Rotate()
        {
            var leftRotatePressed = buttonMappings!.CheckRotateLeftKey();
            var rightRotatePressed = buttonMappings!.CheckRotateRightKey();
            var rotationDirection = leftRotatePressed ? -1 : rightRotatePressed ? 1 : 0;
            var rotationAmount = rotationDirection * rotateSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }

        private void Shoot()
        {
            if (!buttonMappings!.CheckShootKey()) return;

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint!.position, bulletSpawnPoint.rotation);
            var bulletForce = bulletSpawnPoint.forward * _gameConfig!.BulletSpeed;
            bullet!.GetComponent<Rigidbody>()!.AddForce(bulletForce);
        }
    }
}