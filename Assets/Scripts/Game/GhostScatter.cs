using UnityEngine;

namespace Game
{
    public class GhostScatter : GhostBehavior
    {
        private void OnDisable()
        {
            ghost.Chase.Enable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Node node = other.GetComponent<Node>();

            // Do nothing while the ghost is frightened
            if (node != null && enabled && !ghost.Frightened.enabled)
            {
                // Pick a random available direction
                int index = Random.Range(0, node.availableDirections.Count);

                // Prefer not to go back the same direction so increment the index to
                // the next available direction
                if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.Movement.direction)
                {
                    index++;

                    // Wrap the index back around if overflowed
                    if (index >= node.availableDirections.Count)
                    {
                        index = 0;
                    }
                }

                ghost.Movement.SetDirection(node.availableDirections[index]);
            }
        }
    }
}