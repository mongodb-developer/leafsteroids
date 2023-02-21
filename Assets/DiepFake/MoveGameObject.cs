using UnityEngine;

namespace DiepFake
{
    public class MoveGameObject : MonoBehaviour
    {
        public float speed = 10.0f; // speed of movement
        public float rotationSpeed = 100.0f; // speed of rotation

        private void Update()
        {
            // Get input from arrow keys and WASD
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            // Calculate movement and rotation based on input
            var movement = new Vector3(horizontal, 0, vertical) * (speed * Time.deltaTime);
            var rotation = Quaternion.Euler(0, horizontal * rotationSpeed * Time.deltaTime, 0);

            // Apply movement and rotation to the GameObject
            Transform transform1;
            (transform1 = transform).Translate(movement, Space.World);
            transform1.rotation *= rotation;
        }
    }
}