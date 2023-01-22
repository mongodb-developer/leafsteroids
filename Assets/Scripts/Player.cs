using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject validPlayingField;
    public GameManager gameManager;

    [SerializeField] private int speedInMS = 20;
    [SerializeField] private int overlappingColliderCount;
    [SerializeField] private Vector3 currentDirection = Vector3.zero;

    private readonly System.Random random = new();


    private void Start()
    {
        InvokeRepeating(nameof(RandomizeDirection), 0f, 0.5f);
    }


    private void Update()
    {
        gameObject.transform.Translate(currentDirection * (speedInMS * Time.deltaTime), Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other == null || other.gameObject == null) return;

        if (other.gameObject.name.Equals("Deadzone"))
        {
            gameManager.playerHitDeadzone(overlappingColliderCount);
        }

        var goalComponent = other.gameObject.GetComponent<Goal>();
        if (goalComponent != null)
        {
            gameManager.playerReachedGoal(this);
        }

        var wallComponent = other.gameObject.GetComponent<Wall>();
        if (wallComponent == null) return;

        overlappingColliderCount++;

        if (overlappingColliderCount != 2) TurnAround();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject == validPlayingField)
        {
            gameManager.playerLeftMap(this);
        }

        var wallComponent = other.gameObject.GetComponent<Wall>();
        if (wallComponent == null) return;

        overlappingColliderCount--;
    }

    private void RandomizeDirection()
    {
        if (overlappingColliderCount > 0) return;

        currentDirection = new Vector3(NextFloat(-1, 1), 0, NextFloat(-1, 1)).normalized;
    }

    private void TurnAround()
    {
        var currentPosition = gameObject.transform.position;
        currentDirection = new Vector3(-currentPosition.x, 0, -currentPosition.z).normalized;
    }

    private float NextFloat(float min, float max)
    {
        var val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}