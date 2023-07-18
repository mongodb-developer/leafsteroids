using UnityEngine;

namespace _3_Main
{
    public class EnemyPulsate : MonoBehaviour
    {
        public float minScale = 0.7f;
        public float maxScale = 1.3f;
        public float pulsateSpeed = 1f;

        private float _currentScale;
        private float _scaleDirection = 1f;
        private float _startYPosition;

        private void Start()
        {
            _currentScale = Random.Range(minScale, maxScale);
            var transform1 = transform;
            transform1.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
            _startYPosition = transform1.position.y;
        }

        private void Update()
        {
            _currentScale += pulsateSpeed * Time.deltaTime * _scaleDirection;
            if (_currentScale > maxScale || _currentScale < minScale) _scaleDirection *= -1f;
            var transform1 = transform;
            transform1.localScale = new Vector3(_currentScale, _currentScale, _currentScale);
            var position = transform1.position;
            position = new Vector3(position.x, _startYPosition, position.z);
            transform1.position = position;
        }
    }
}