using System;
using UnityEngine;

public class SphereMovement : MonoBehaviour
{
    public GameManagerNew GameManagerNew;

    public bool isPoweredUp = false;
    public bool hasWeapon = false;

    [SerializeField] private Vector3 currentMovementDirection = Vector3.zero;
    [SerializeField] private int speed = 3000;
    [SerializeField] private float directionChangeRateS = 1.0f;

    private readonly System.Random random = new();

    private Rigidbody thisRigidbody;

    private void Start()
    {
        thisRigidbody = gameObject.GetComponent<Rigidbody>();
        InvokeRepeating(nameof(RandomMovementChange), 0f, directionChangeRateS);
    }

    private void Update()
    {
        Push();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var wallComponent = collision.gameObject.GetComponent<Wall>();
        if (wallComponent != null)
        {
            currentMovementDirection = -currentMovementDirection;
            Push(5);
        }
        
        var playerComponent = collision.gameObject.GetComponent<SphereMovement>();
        if (playerComponent != null)
        {
            GameManagerNew.PlayerHitOtherPlayer(this, playerComponent);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var extractionZoneComponent = other.gameObject.GetComponent<ExtractionZone>();
        if (extractionZoneComponent != null)
        {
            GameManagerNew.PlayerReachedGoal(this);
        }

        var deadZoneComponent = other.gameObject.GetComponent<DeadZone>();
        if (deadZoneComponent != null)
        {
            GameManagerNew.PlayerHitDeadZone(this);
        }

        var powerUpZoneComponent = other.gameObject.GetComponent<PowerUpZone>();
        if (powerUpZoneComponent != null)
        {
            GameManagerNew.PlayerHitPowerUpZone(this);
        }

        var weaponZoneComponent = other.gameObject.GetComponent<WeaponZone>();
        if (weaponZoneComponent != null)
        {
            GameManagerNew.PlayerHitWeaponZone(this);
        }
    }

    private void Push(int multiplier = 1)
    {
        thisRigidbody.AddForce(currentMovementDirection * (speed * multiplier));
    }

    private void RandomMovementChange()
    {
        currentMovementDirection = new Vector3(random.Next(-1, 2), 0, random.Next(-1, 2));
    }
}