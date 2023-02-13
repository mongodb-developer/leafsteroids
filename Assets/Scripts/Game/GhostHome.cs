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
            if (gameObject.activeInHierarchy) StartCoroutine(ExitTransition());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Reverse direction everytime the ghost hits a wall to create the
            // effect of the ghost bouncing around the home
            if (enabled && collision!.gameObject!.layer == LayerMask.NameToLayer("Obstacle"))
                Ghost!.Movement!.SetDirection(-Ghost.Movement.Direction);
        }

        private IEnumerator ExitTransition()
        {
            // Turn off movement while we manually animate the position
            Ghost!.Movement!.SetDirection(Vector2.up, true);
            Ghost.Movement.Rigidbody!.isKinematic = true;
            Ghost.Movement.enabled = false;

            var position = transform.position;

            const float d = 0.5f;
            var elapsed = 0f;

            // Animate to the starting point
            while (elapsed < d)
            {
                Ghost.SetPosition(Vector3.Lerp(position, inside!.position, elapsed / d));
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            // Animate exiting the ghost home
            while (elapsed < d)
            {
                Ghost.SetPosition(Vector3.Lerp(inside!.position, outside!.position, elapsed / d));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Pick a random direction left or right and re-enable movement
            Ghost.Movement!.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
            Ghost.Movement.Rigidbody!.isKinematic = false;
            Ghost.Movement.enabled = true;
        }
    }
}