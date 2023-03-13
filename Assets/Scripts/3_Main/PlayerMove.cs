using _00_Shared;
using _1_Loading;
using _3_Main._ReplaySystem;
using UnityEngine;

namespace _3_Main
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private ButtonMappings buttonMappings;

        public float moveSpeed;

        private void Start()
        {
            moveSpeed = GameConfigLoader.Instance!.GameConfig!.PlayerMoveSpeed;
        }

        private void Update()
        {
            var verticalMovement = buttonMappings!.GetVerticalAxis() * moveSpeed * Time.deltaTime;
            var horizontalMovement = buttonMappings.GetHorizontalAxis() * moveSpeed * Time.deltaTime;
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
    }
}