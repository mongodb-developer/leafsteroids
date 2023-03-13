using UnityEngine;

public class ButtonMappings : MonoBehaviour
{
    public bool CheckEscapeKeu()
    {
        return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton3);
    }

    public bool CheckConfirmKey()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button1);
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
        return Input.GetKey(KeyCode.Joystick1Button0);
    }

    public bool CheckRotateRightKey()
    {
        return Input.GetKey(KeyCode.Joystick1Button2);
    }

    public bool CheckShootKey()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Space);
    }
}