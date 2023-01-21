using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnCollisionExit(Collision other)
    {
        // Destroy(other.gameObject);
    }
}