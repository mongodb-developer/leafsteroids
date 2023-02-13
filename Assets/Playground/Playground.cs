using System;
using UnityEngine;

namespace Playground
{
    public class Playground : MonoBehaviour
    {
        private void Start()
        {
            InvokeRepeating(nameof(Log), 0f, 3f);
        }

        private void Log()
        {
            Debug.Log("foo");
            Console.WriteLine("foo");
        }
    }
}