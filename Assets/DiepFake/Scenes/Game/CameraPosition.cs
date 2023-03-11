using DiepFake.Scripts;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public PlayerController playerController;

    private void Update()
    {
        transform.position = playerController!.transform.position + new Vector3(0, 11, 0);
    }
}