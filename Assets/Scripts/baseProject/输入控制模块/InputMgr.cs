using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : baseManager<InputMgr>
{

    public static bool isStart = false;
    public InputMgr(){
        //初始化
        MonoManager.getInstance().AddUpdateListener(myUpdate);
    }


    /// <summary>
    /// 是否开启  输入检测
    /// </summary>
    /// <param name=""></param>
    public void StartOrEndCheck(bool isOpen){
        isStart = isOpen;
    }


    /// <summary>
    /// 检测按键
    /// 1.按下
    /// 2.弹起
    /// </summary>
    /// <param name="keyCode"></param>
    public void checkedKeyCode(KeyCode keyCode){
        if(Input.GetKeyDown(keyCode)){
            Debug.Log("按下 = "+keyCode);
            EventCenter.getInstance().EventTrigger("KeyDown", keyCode);
        }

        if(Input.GetKeyUp(keyCode)){
            Debug.Log("弹起 = "+keyCode);
            EventCenter.getInstance().EventTrigger("KeyUp", keyCode);
        }
    }

    private void myUpdate(){
        if(!isStart){
            return;
        }
        checkedKeyCode(KeyCode.W);
        checkedKeyCode(KeyCode.S);
        checkedKeyCode(KeyCode.A);
        checkedKeyCode(KeyCode.D);
    }
}
