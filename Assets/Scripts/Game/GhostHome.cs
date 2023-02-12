using System.Collections;
using UnityEngine;

namespace Game
{
    public class GhostHome : GhostBehavior
    {
        public Transform inside;
        public Transform outside;

        private void OnEnable()
        {
            StopAllCoroutines();
        }

        private void OnDisable()
        {
            // Check for active self to prevent error when object is destroyed
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(ExitTransition());
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Reverse direction everytime the ghost hits a wall to create the
            // effect of the ghost bouncing around the home
            if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                ghost.Movement.SetDirection(-ghost.Movement.direction);
            }
        }

        private IEnumerator ExitTransition()
        {
            // Turn off movement while we manually animate the position
            ghost.Movement.SetDirection(Vector2.up, true);
            ghost.Movement.rigidbody.isKinematic = true;
            ghost.Movement.enabled = false;

            Vector3 position = transform.position;

            float duration = 0.5f;
            float elapsed = 0f;

            // Animate to the starting point
            while (elapsed < duration)
            {
                ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            // Animate exiting the ghost home
            while (elapsed < duration)
            {
                ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Pick a random direction left or right and re-enable movement
            ghost.Movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
            ghost.Movement.rigidbody.isKinematic = false;
            ghost.Movement.enabled = true;
        }
    }
}