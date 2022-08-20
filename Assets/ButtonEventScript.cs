using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEventScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int buttonType;
    public void OnPointerDown (PointerEventData eventData)
    {
        InGameMgr.Instance.ButtonClickAction(buttonType);
        //Debug.Log("Button down");
        //InGameMgr.Instance.purpleTimeCount = 0;

        if (buttonType == 0)
        {
            InGameMgr.Instance.isBtnLDown = true;
        }
        else
        {
            InGameMgr.Instance.isBtnRDown = true;
        }
    }
 
    public void OnPointerUp (PointerEventData eventData)
    {
        
            //isBtnDown = false;
            
            //Debug.Log("Button Up");
            //InGameMgr.Instance.purpleTimeCount = 0;
            if (buttonType == 0)
            {
                InGameMgr.Instance.isBtnLDown = false;
            }
            else
            {
                InGameMgr.Instance.isBtnRDown = false;
            }
    }
}
