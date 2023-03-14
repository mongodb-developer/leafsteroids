using _00_Shared;
using _1_Loading;
using UnityEngine;

namespace _3_Main
{
    public class PlayerRotate : MonoBehaviour
    {
        public float rotateSpeed;

        private void Start()
        {
            rotateSpeed = GameConfigLoader.Instance!.GameConfig!.PlayerRotateSpeed;
        }

        private void Update()
        {
            var leftRotatePressed = ButtonMappings.CheckRotateLeftKey();
            var rightRotatePressed = ButtonMappings.CheckRotateRightKey();
            var rotationDirection = leftRotatePressed ? -1 : rightRotatePressed ? 1 : 0;
            var rotationAmount = rotationDirection * rotateSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }
}