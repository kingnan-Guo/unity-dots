using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 让没有继承 MonoBehaviour的 类 可以开启 携程，可以 upDate 更新 ，统一管理 upDate
/// 过场景 不移除
/// </summary>

public class MonoController : MonoBehaviour
{

    private event UnityAction upDateEvent;
    // Start is called before the first frame update
    void Start()
    {
        // 会把游戏对象在场景切换的时候保留下来,包括子物体
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        // 不为空 执行
        if(upDateEvent != null){
            upDateEvent();
        }
    }
    

    /// <summary>
    /// 给外部 添加更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun){
        upDateEvent += fun;
    }

    
    /// <summary>
    /// 移除 事件
    /// </summary>
    /// <param name="fun"></param>
    public void removeUpdateListener(UnityAction fun){
        upDateEvent -= fun;
    }
}
