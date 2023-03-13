using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2.5f;
    public float minMoveDuration = 0.5f;
    public float maxMoveDuration = 1.5f;

    private float currentMoveDuration;
    private Vector3 moveDirection;
    private float timer;

    private void Start()
    {
        // Set a random move direction at the start
        SetRandomMoveDirection();
    }

    private void Update()
    {
        // Move the GameObject in the current direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Increase the timer by the time passed since the last frame
        timer += Time.deltaTime;

        // If the timer has exceeded the current move duration, set a new random move direction and duration
        if (timer > currentMoveDuration) SetRandomMoveDirection();
    }

    private void SetRandomMoveDirection()
    {
        // Set a new random move direction
        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        // Set a new random move duration
        currentMoveDuration = Random.Range(minMoveDuration, maxMoveDuration);

        // Reset the timer
        timer = 0f;
    }
}