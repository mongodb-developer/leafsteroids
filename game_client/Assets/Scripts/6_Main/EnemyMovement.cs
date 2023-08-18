using UnityEngine;

namespace _6_Main
{
    public class EnemyMovement : MonoBehaviour
    {
        public float moveSpeed = 2.5f;
        public float minMoveDuration = 0.5f;
        public float maxMoveDuration = 1.5f;

        private float _currentMoveDuration;
        private Vector3 _moveDirection;
        private float _timer;

        private void Start()
        {
            SetRandomMoveDirection();
        }

        private void Update()
        {
            transform.Translate(_moveDirection * (moveSpeed * Time.deltaTime));
            _timer += Time.deltaTime;
            if (_timer > _currentMoveDuration) SetRandomMoveDirection();
        }

        private void SetRandomMoveDirection()
        {
            _moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
            _currentMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);
            _timer = 0f;
        }
    }
}