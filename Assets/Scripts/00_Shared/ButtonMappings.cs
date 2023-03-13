using UnityEngine;

namespace _00_Shared
{
    public class ButtonMappings : MonoBehaviour
    {
        public bool CheckEscapeKey()
        {
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton3);
        }

        public bool CheckConfirmKey()
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Space) ||
                   Input.GetKeyDown(KeyCode.Return);
        }

        public float GetVerticalAxis()
        {
            return Input.GetAxis("Vertical");
        }

        public float GetHorizontalAxis()
        {
            return Input.GetAxis("Horizontal");
        }

        public bool CheckRotateLeftKey()
        {
            return Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.LeftArrow);
        }

        public bool CheckRotateRightKey()
        {
            return Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.RightArrow);
        }

        public bool CheckShootKey()
        {
            return Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Space);
        }
    }
}