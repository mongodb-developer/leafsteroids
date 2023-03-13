using UnityEngine;

public class PowerUpAnimation : MonoBehaviour
{
    private const float PowerUpAnimationSpeed = 100;

    private void Update()
    {
        var rotation = PowerUpAnimationSpeed * Time.deltaTime;
        transform.Rotate(rotation, 2 * rotation, 3 * rotation);
    }
}