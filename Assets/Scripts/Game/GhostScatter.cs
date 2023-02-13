using UnityEngine;

namespace Game
{
    public class GhostScatter : GhostBehavior
    {
        private void OnDisable()
        {
            Ghost!.Chase!.Enable();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Node node = other!.GetComponent<Node>();

            // Do nothing while the ghost is frightened
            if (node != null && enabled && !Ghost!.Frightened!.enabled)
            {
                // Pick a random available direction
                int index = Random.Range(0, node.AvailableDirections!.Count);

                // Prefer not to go back the same direction so increment the index to
                // the next available direction
                if (node.AvailableDirections.Count > 1 && node.AvailableDirections[index] == -Ghost.Movement!.Direction)
                {
                    index++;

                    // Wrap the index back around if overflowed
                    if (index >= node.AvailableDirections.Count)
                    {
                        index = 0;
                    }
                }

                Ghost.Movement!.SetDirection(node.AvailableDirections[index]);
            }
        }
    }
}