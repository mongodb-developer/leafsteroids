using _3_Main._ReplaySystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3_Main
{
    public class CameraPosition : MonoBehaviour
    {
        [FormerlySerializedAs("playerController")]
        public Player player;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        private void Update()
        {
            transform.position = player!.transform.position + new Vector3(0, 11, 0);
        }
    }
}