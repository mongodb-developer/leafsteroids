using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Variables to control movement and rotation speed
    public float moveSpeed = 3f;
    public float rotateSpeed = 100f;

    // Variables to control shooting
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    // Update is called once per frame
    private void Update()
    {
        // Rotation control
        var rotation = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        // Movement control
        var forwardMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, forwardMovement);

        // Shooting control
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
    }

    private void Shoot()
    {
        // Create the bullet
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint!.position, bulletSpawnPoint.rotation);

        // Apply force to the bullet
        bullet!.GetComponent<Rigidbody>()!.AddForce(bulletSpawnPoint.forward * 500f);
    }
}