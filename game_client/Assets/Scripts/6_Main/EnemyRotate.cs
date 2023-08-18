using UnityEngine;

namespace _6_Main
{
    public class EnemyRotate : MonoBehaviour
    {
        public float minRotationSpeed = 0.1f;
        public float maxRotationSpeed = 0.25f;

        private float rotateSpeedX;
        private float rotateSpeedY;
        private float rotateSpeedZ;

        private void Start()
        {
            rotateSpeedX = Random.Range(minRotationSpeed, maxRotationSpeed);
            rotateSpeedY = Random.Range(minRotationSpeed, maxRotationSpeed);
            rotateSpeedZ = Random.Range(minRotationSpeed, maxRotationSpeed);
            if (Random.value > 0.5f) rotateSpeedX = -rotateSpeedX;
            if (Random.value > 0.5f) rotateSpeedY = -rotateSpeedY;
            if (Random.value > 0.5f) rotateSpeedZ = -rotateSpeedZ;
        }

        private void Update()
        {
            gameObject.transform.Rotate(rotateSpeedX, rotateSpeedY, rotateSpeedZ);
        }
    }
}