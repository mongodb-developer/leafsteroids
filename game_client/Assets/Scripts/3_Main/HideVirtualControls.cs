using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using _00_Shared;


namespace _3_Main
{
    public class HideVirtualControls : MonoBehaviour
    {   

        void Start() 
        {
	    if (ButtonMappings.DetectedInputDevice != DetectedInputDevice.TouchScreen)
	       	gameObject.SetActive(false);

	}
    }
}
