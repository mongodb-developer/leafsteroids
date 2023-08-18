using _00_Shared;
using _1_Loading;
using TMPro;
using UnityEngine;

namespace _6_Main
{
    public class PlayerRotate : MonoBehaviour
    {
        public float rotateSpeed;

        [SerializeField] private TMP_Text rotateInstructions;

        private bool _hasRotated;

        private void Start()
        {
            rotateSpeed = GameConfigLoader.Instance!.GameConfig!.PlayerRotateSpeed;
            Invoke(nameof(ShowInstructions), 4f);
        }

        private void Update()
        {
            var leftRotatePressed = ButtonMappings.CheckRotateLeftKey();
            var rightRotatePressed = ButtonMappings.CheckRotateRightKey();
            var rotationDirection = leftRotatePressed ? -1 : rightRotatePressed ? 1 : 0;
            var rotationAmount = rotationDirection * rotateSpeed * Time.deltaTime;
            if (Mathf.Abs(rotationAmount) > 0) _hasRotated = true;
            transform.Rotate(0, rotationAmount, 0);
        }

        private void ShowInstructions()
        {
            if (!_hasRotated)
                rotateInstructions!.text = ButtonMappings.DetectedInputDevice switch
                {
                    DetectedInputDevice.Joystick => "Rotate using the X / Y buttons!",
                    DetectedInputDevice.Keyboard => "Rotate using left / right arrow!",
                    _ => rotateInstructions!.text
                };

            Invoke(nameof(HideInstructions), 3f);
        }

        private void HideInstructions()
        {
            rotateInstructions!.text = "";
        }
    }
}