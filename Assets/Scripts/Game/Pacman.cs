using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Movement))]
    public class Pacman : MonoBehaviour
    {
        public AnimatedSprite deathSequence;

        private SpriteRenderer SpriteRenderer { get; set; }
        private Collider2D Collider { get; set; }
        private Movement Movement { get; set; }

        private void Awake()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Collider = GetComponent<Collider2D>();
            Movement = GetComponent<Movement>();

            if (SpriteRenderer == null || Collider == null || Movement == null) Debug.Log("null");
        }

        private void Update()
        {
            // Set the new direction based on the current input
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Movement!.SetDirection(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Movement!.SetDirection(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Movement!.SetDirection(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Movement!.SetDirection(Vector2.right);
            }

            // Rotate pacman to face the movement direction
            float angle = Mathf.Atan2(Movement!.Direction.y, Movement.Direction.x);
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }

        public void ResetState()
        {
            enabled = true;
            SpriteRenderer!.enabled = true;
            Collider!.enabled = true;
            deathSequence!.enabled = false;
            deathSequence.SpriteRenderer!.enabled = false;
            Movement!.ResetState();
            gameObject.SetActive(true);
        }

        public void DeathSequence()
        {
            enabled = false;
            SpriteRenderer!.enabled = false;
            Collider!.enabled = false;
            Movement!.enabled = false;
            deathSequence!.enabled = true;
            deathSequence.SpriteRenderer!.enabled = true;
            deathSequence.Restart();
        }
    }
}