using UnityEngine;

namespace _MainScene
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float rotateSpeed = 100f;

        public GameObject bulletPrefab;
        public Transform bulletSpawnPoint;

        private void Update()
        {
            var verticalMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            var horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            transform.Translate(horizontalMovement, 0, verticalMovement, Space.World);

            var leftRotatePressed = Input.GetKey(KeyCode.Joystick1Button0);
            var rightRotatePressed = Input.GetKey(KeyCode.Joystick1Button2);
            var rotationDirection = leftRotatePressed ? -1 : rightRotatePressed ? 1 : 0;
            var rotationAmount = rotationDirection * rotateSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);

            if (Input.GetKeyDown(KeyCode.Joystick1Button1)) Shoot();
        }

        private void Shoot()
        {
            // Create the bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint!.position, bulletSpawnPoint.rotation);

            // Apply force to the bullet
            bullet!.GetComponent<Rigidbody>()!.AddForce(bulletSpawnPoint.forward * 500f);
        }
    }
}