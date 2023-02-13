using UnityEngine;

namespace Game
{
    public class GhostFrightened : GhostBehavior
    {
        public SpriteRenderer body;
        public SpriteRenderer eyes;
        public SpriteRenderer blue;
        public SpriteRenderer white;

        private bool HasBeenEaten { get; set; }

        public override void Enable(float d)
        {
            base.Enable(d);

            body!.enabled = false;
            eyes!.enabled = false;
            blue!.enabled = true;
            white!.enabled = false;

            Invoke(nameof(Flash), d / 2f);
        }

        public override void Disable()
        {
            base.Disable();

            body!.enabled = true;
            eyes!.enabled = true;
            blue!.enabled = false;
            white!.enabled = false;
        }

        private void Eaten()
        {
            HasBeenEaten = true;
            Ghost!.SetPosition(Ghost.Home!.inside!.position);
            Ghost.Home.Enable(duration);

            body!.enabled = false;
            eyes!.enabled = true;
            blue!.enabled = false;
            white!.enabled = false;
        }

        private void Flash()
        {
            if (!HasBeenEaten)
            {
                blue!.enabled = false;
                white!.enabled = true;
                white.GetComponent<AnimatedSprite>()!.Restart();
            }
        }

        private void OnEnable()
        {
            blue!.GetComponent<AnimatedSprite>()!.Restart();
            Ghost!.Movement!.speedMultiplier = 0.5f;
            HasBeenEaten = false;
        }

        private void OnDisable()
        {
            Ghost!.Movement!.speedMultiplier = 1f;
            HasBeenEaten = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Node node = other!.GetComponent<Node>();

            if (node != null && enabled)
            {
                Vector2 direction = Vector2.zero;
                float maxDistance = float.MinValue;

                // Find the available direction that moves farthest from pacman
                foreach (Vector2 availableDirection in node.AvailableDirections!)
                {
                    // If the distance in this direction is greater than the current
                    // max distance then this direction becomes the new farthest
                    Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                    float distance = (Ghost!.target!.position - newPosition).sqrMagnitude;

                    if (distance > maxDistance)
                    {
                        direction = availableDirection;
                        maxDistance = distance;
                    }
                }

                Ghost!.Movement!.SetDirection(direction);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision!.gameObject!.layer == LayerMask.NameToLayer("Pacman"))
            {
                if (enabled)
                {
                    Eaten();
                }
            }
        }
    }
}