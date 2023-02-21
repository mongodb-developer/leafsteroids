using UnityEngine;

namespace DiepFake
{
    public class WeaponDirection : MonoBehaviour
    {
        public Transform weapon; // reference to the weapon
        public float rotationSpeed = 10.0f; // speed of rotation

        private void Update()
        {
            // Get the position of the mouse cursor in the screen space
            var mousePos = Input.mousePosition;

            // Cast a ray from the camera to the mouse cursor position in the world space
            var ray = Camera.main!.ScreenPointToRay(mousePos);

            // Create a plane at the height of the weapon to intersect with the ray
            var plane = new Plane(Vector3.up, weapon!.position);

            // Find the point of intersection between the ray and the plane
            if (plane.Raycast(ray, out var distance))
            {
                // Get the position of the intersection point
                var targetPoint = ray.GetPoint(distance);

                // Calculate the direction to the target point from the weapon's position
                var targetDir = targetPoint - weapon.position;
                targetDir.y = 0; // ignore the y component

                // Smoothly rotate the weapon towards the target direction
                var targetRot = Quaternion.LookRotation(targetDir);
                weapon.rotation = Quaternion.Slerp(weapon.rotation, targetRot, Time.deltaTime * rotationSpeed);
            }
        }
    }
}