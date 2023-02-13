using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        public float speed = 8f;
        public float speedMultiplier = 1f;
        public Vector2 initialDirection;
        public LayerMask obstacleLayer;

        public Rigidbody2D Rigidbody { get; private set; }
        public Vector2 Direction { get; private set; }
        private Vector2 NextDirection { get; set; }
        private Vector3 StartingPosition { get; set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            StartingPosition = transform.position;
        }

        private void Start()
        {
            ResetState();
        }

        private void Update()
        {
            // Try to move in the next direction while it's queued to make movements
            // more responsive
            if (NextDirection != Vector2.zero) SetDirection(NextDirection);
        }

        private void FixedUpdate()
        {
            var position = Rigidbody!.position;
            var translation = Direction * (speed * speedMultiplier * Time.fixedDeltaTime);

            Rigidbody.MovePosition(position + translation);
        }

        public void ResetState()
        {
            speedMultiplier = 1f;
            Direction = initialDirection;
            NextDirection = Vector2.zero;
            transform.position = StartingPosition;
            Rigidbody!.isKinematic = false;
            enabled = true;
        }

        public void SetDirection(Vector2 direction, bool forced = false)
        {
            // Only set the direction if the tile in that direction is available
            // otherwise we set it as the next direction so it'll automatically be
            // set when it does become available
            if (forced || !Occupied(direction))
            {
                Direction = direction;
                NextDirection = Vector2.zero;
            }
            else
            {
                NextDirection = direction;
            }
        }

        private bool Occupied(Vector2 direction)
        {
            // If no collider is hit then there is no obstacle in that direction
            var hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f,
                obstacleLayer);
            return hit.collider != null;
        }
    }
}