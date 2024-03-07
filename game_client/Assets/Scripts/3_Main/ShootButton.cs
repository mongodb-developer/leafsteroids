using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using _00_Shared;


namespace _3_Main
{

    public class ShootButton : MonoBehaviour, IPointerDownHandler
    {   
	
	public void OnPointerDown(PointerEventData pointerEventData)
        {
            ButtonMappings.shootButtonActive = true;
        }
 

    }
}
