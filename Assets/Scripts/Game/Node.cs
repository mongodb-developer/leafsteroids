using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Node : MonoBehaviour
    {
        public LayerMask obstacleLayer;
        public List<Vector2> AvailableDirections { get; private set; }

        private void Start()
        {
            AvailableDirections = new List<Vector2>();

            // We determine if the direction is available by box casting to see if
            // we hit a wall. The direction is added to list if available.
            CheckAvailableDirection(Vector2.up);
            CheckAvailableDirection(Vector2.down);
            CheckAvailableDirection(Vector2.left);
            CheckAvailableDirection(Vector2.right);
        }

        private void CheckAvailableDirection(Vector2 direction)
        {
            var hit =
                Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

            // If no collider is hit then there is no obstacle in that direction
            if (hit.collider == null) AvailableDirections!.Add(direction);
        }
    }
}