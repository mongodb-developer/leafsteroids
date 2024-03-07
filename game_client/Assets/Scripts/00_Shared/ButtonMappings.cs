using UnityEngine;

// PS4 controller map for Unity
// (source: https://www.reddit.com/r/Unity3D/comments/1syswe/ps4_controller_map_for_unity/)
//
// Buttons
// Square  = joystick button 0
// X       = joystick button 1
// Circle  = joystick button 2
// Triangle= joystick button 3
// L1      = joystick button 4
// R1      = joystick button 5
// L2      = joystick button 6
// R2      = joystick button 7
// Share	= joystick button 8
// Options = joystick button 9
// L3      = joystick button 10
// R3      = joystick button 11
// PS      = joystick button 12
// PadPress= joystick button 13
//
// Axes:
// LeftStickX      = X-Axis
// LeftStickY      = Y-Axis (Inverted?)
// RightStickX     = 3rd Axis
//     RightStickY     = 4th Axis (Inverted?)
// L2              = 5th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
// R2              = 6th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
// DPadX           = 7th Axis
//     DPadY           = 8th Axis (Inverted?)

namespace _00_Shared
{
    public class ButtonMappings : MonoBehaviour
    {
        
	public Joystick virtualJoystick;

	public static bool shootButtonActive = false;

	public static bool rotateLeftArrowActive = false;

	public static bool rotateRightArrowActive = false;

	public static DetectedInputDevice DetectedInputDevice;

        public static bool CheckEscapeKey()
        {
            return Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton4);
        }

        public static bool CheckReloadKey()
        {
            return Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.R);
        }

        public static bool CheckConfirmKey()
        {

	    if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                Debug.Log("Touch screen detected");
		DetectedInputDevice = DetectedInputDevice.TouchScreen;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                Debug.Log("Joystick detected.");
                DetectedInputDevice = DetectedInputDevice.Joystick;
                return true;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Keyboard detected.");
                DetectedInputDevice = DetectedInputDevice.Keyboard;
                return true;
            }

            return false;
        }

        public float GetVerticalAxis()
        {
	    if (DetectedInputDevice == DetectedInputDevice.TouchScreen)
		return virtualJoystick.Vertical;
	    else 
	        return Input.GetAxis("Vertical");
        }

        public float GetHorizontalAxis()
        {
            if (DetectedInputDevice == DetectedInputDevice.TouchScreen)
		return virtualJoystick.Horizontal;
	    else 
	        return Input.GetAxis("Horizontal");
        }

        public static bool CheckAnyUpKey()
        {
            return false;
	    //return GetVerticalAxis() > 0.5 || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        }

        public static bool CheckAnyDownKey()
        {
	    return false;
            //return GetVerticalAxis() < -0.5 || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        }

        public static bool CheckRotateLeftKey()
        {
            return rotateLeftArrowActive || Input.GetKey(KeyCode.JoystickButton0) || Input.GetKey(KeyCode.LeftArrow);
        }

        public static bool CheckRotateRightKey()
        {
            return rotateRightArrowActive ||  Input.GetKey(KeyCode.JoystickButton3) || Input.GetKey(KeyCode.RightArrow);
        }

        public static bool CheckShootKey()
        {
            bool status = shootButtonActive || Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Space) ||
                   Input.GetKeyDown(KeyCode.UpArrow);
	    shootButtonActive = false;
	    return status;
        }
    }
}