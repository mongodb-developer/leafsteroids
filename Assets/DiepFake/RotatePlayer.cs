using UnityEngine;

namespace DiepFake
{
    public class RotatePlayer : MonoBehaviour
    {
        public float rotationSpeed = 10.0f; // speed of rotation

        private void Update()
        {
            // Get input from arrow keys and WASD
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            // Calculate the direction the player is moving in
            var moveDir = new Vector3(horizontal, 0, vertical).normalized;

            if (moveDir != Vector3.zero)
            {
                // Calculate the rotation towards the movement direction
                var targetRot = Quaternion.LookRotation(moveDir, Vector3.up);

                // Smoothly rotate the player towards the target direction
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
            }
        }
    }
}