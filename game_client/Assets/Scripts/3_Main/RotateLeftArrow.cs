using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using _00_Shared;


namespace _3_Main
{

    public class RotateLeftArrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {   

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            ButtonMappings.rotateLeftArrowActive = true;
        }
 
        public void OnPointerUp(PointerEventData pointerEventData)
        {
            ButtonMappings.rotateLeftArrowActive = false;
        }
    }
}
