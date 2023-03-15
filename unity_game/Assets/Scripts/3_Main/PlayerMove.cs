using _00_Shared;
using _1_Loading;
using _3_Main._ReplaySystem;
using TMPro;
using UnityEngine;

namespace _3_Main
{
    public class PlayerMove : MonoBehaviour
    {
        public float moveSpeed;

        [SerializeField] private TMP_Text moveInstructions;

        private bool _hasMoved;

        private void Start()
        {
            moveSpeed = GameConfigLoader.Instance!.GameConfig!.PlayerMoveSpeed;
            Invoke(nameof(ShowInstructions), 1f);
        }

        private void Update()
        {
            var verticalMovement = ButtonMappings.GetVerticalAxis() * moveSpeed * Time.deltaTime;
            var horizontalMovement = ButtonMappings.GetHorizontalAxis() * moveSpeed * Time.deltaTime;
            if (Mathf.Abs(verticalMovement) > 0 || Mathf.Abs(horizontalMovement) > 0) _hasMoved = true;
            transform.Translate(horizontalMovement, 0, verticalMovement, Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == null) return;

            var powerUp = other.GetComponent<PowerUpPlayerSpeed>();
            if (powerUp == null) return;

            moveSpeed = (float)(moveSpeed * 1.2);
            SessionStatistics.Instance!.PowerUpPlayerSpeedCollected++;

            Destroy(other.gameObject);
        }

        private void ShowInstructions()
        {
            if (!_hasMoved)
                moveInstructions!.text = ButtonMappings.DetectedInputDevice switch
                {
                    DetectedInputDevice.Joystick => "Move using the Joystick!",
                    DetectedInputDevice.Keyboard => "Move using WASD!",
                    _ => moveInstructions!.text
                };

            Invoke(nameof(HideInstructions), 3f);
        }

        private void HideInstructions()
        {
            moveInstructions!.text = "";
        }
    }
}