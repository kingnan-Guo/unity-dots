using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using AXES;
using UnityEngine.Events;




public class AxesBaseClass : AxesBase
{

    protected override void GetComponent(RaycastHit hit, UnityAction<AxisType> action){
        Axis axis = hit.transform.gameObject.GetComponent<Axis>();
        axisType = axis.axisType;
        // axis.isActive = true;
        axis.setActive(true);
        action(axisType);
    }


    // protected override bool UIElementsBlockRaycast(List<string> IgnoreRaycastTagList)
    // {
    //     Debug.Log("UIElementsBlockRaycast == " + IgnoreRaycastTagList.Count);
    //     // PointerEventData eventData = new PointerEventData(EventSystem.current);
    //     // eventData.pressPosition = Input.mousePosition;
    //     // eventData.position = Input.mousePosition;
    //     // List<RaycastResult> list = new List<RaycastResult>();
    //     // // gr.Raycast(eventData, list);
    //     // EventSystem.current.RaycastAll(eventData, list);
    //     // if(IgnoreRaycastTagList.Count > 0){
    //     //     foreach (RaycastResult item in list){
    //     //         if (IgnoreRaycastTagList.Contains(item.gameObject.tag)){
    //     //             return true;
    //     //         }
    //     //     }
    //     //     return false;
    //     // } else {
    //     //     return list.Count > 0;
    //     // }
    //     return false;
    // }
}
