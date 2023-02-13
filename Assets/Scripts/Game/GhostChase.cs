using UnityEngine;

namespace Game
{
    public class GhostChase : GhostBehavior
    {
        private void OnDisable()
        {
            Ghost!.Scatter!.Enable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var node = other!.GetComponent<Node>();

            // Do nothing while the ghost is frightened
            if (node != null && enabled && !Ghost!.Frightened!.enabled)
            {
                var direction = Vector2.zero;
                var minDistance = float.MaxValue;

                // Find the available direction that moves closet to pacman
                foreach (var availableDirection in node.AvailableDirections!)
                {
                    // If the distance in this direction is less than the current
                    // min distance then this direction becomes the new closest
                    var newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                    var distance = (Ghost.target!.position - newPosition).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                }

                Ghost.Movement!.SetDirection(direction);
            }
        }
    }
}