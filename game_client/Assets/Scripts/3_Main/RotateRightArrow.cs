using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using _00_Shared;


namespace _3_Main
{

    public class RotateRightArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {   

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            ButtonMappings.rotateRightArrowActive = true;
        }
 
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            ButtonMappings.rotateRightArrowActive = false;
        }
    }
}
