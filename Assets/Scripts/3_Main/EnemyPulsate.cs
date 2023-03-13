using UnityEngine;

public class EnemyPulsate : MonoBehaviour
{
    public float minScale = 0.7f;
    public float maxScale = 1.3f;
    public float pulsateSpeed = 1.5f;

    private float currentScale;
    private float scaleDirection = 1f;
    private float startYPosition;

    private void Start()
    {
        // Set a random starting scale within the min/max range
        currentScale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        // Save the starting y position
        startYPosition = transform.position.y;
    }

    private void Update()
    {
        // Change the current scale by pulsate speed in the current direction
        currentScale += pulsateSpeed * Time.deltaTime * scaleDirection;

        // If the current scale is greater than the max scale or less than the min scale, reverse the direction
        if (currentScale > maxScale || currentScale < minScale) scaleDirection *= -1f;

        // Set the scale of the GameObject to the current scale in all dimensions
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        // Ensure the GameObject stays at the same y coordinate
        transform.position = new Vector3(transform.position.x, startYPosition, transform.position.z);
    }
}