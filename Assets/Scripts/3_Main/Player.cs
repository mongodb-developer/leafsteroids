using _00_Shared;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class Player : MonoBehaviour
    {
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
            var verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            var horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            transform.Translate(horizontalMovement, 0, verticalMovement, Space.World);
        }

        private void Rotate()
        {
            var leftRotatePressed = Input.GetKey(KeyCode.Joystick1Button0);
            var rightRotatePressed = Input.GetKey(KeyCode.Joystick1Button2);
            var rotationDirection = leftRotatePressed ? -1 : rightRotatePressed ? 1 : 0;
            var rotationAmount = rotationDirection * rotateSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }

        private void Shoot()
        {
            if (!Input.GetKeyDown(KeyCode.Joystick1Button1) && !Input.GetKeyDown(KeyCode.Space)) return;

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint!.position, bulletSpawnPoint.rotation);
            var bulletForce = bulletSpawnPoint.forward * _gameConfig!.BulletSpeed;
            bullet!.GetComponent<Rigidbody>()!.AddForce(bulletForce);
        }
    }
}