using UnityEngine;

namespace Playground
{
    public class Playground : MonoBehaviour
    {
        private void Start()
        {
            InvokeRepeating(nameof(Log), 0f, 1f);
        }

        private void Log()
        {
            Debug.Log("foo");
        }
    }
}