using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 开启输入检测
        InputMgr.getInstance().StartOrEndCheck(true);

        EventCenter.getInstance().AddEventListener("KeyDown", InputEvent);
        EventCenter.getInstance().AddEventListener("KeyUp", InputEvent);
    }

    void InputEvent(object key)
    {
        Debug.Log("InputEvent = " + key);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
