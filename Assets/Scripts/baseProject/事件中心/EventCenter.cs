using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
///事件中心  单例模式 对象
///</summary>
public class EventCenter : baseManager<EventCenter>
{
    private Dictionary<string, UnityAction<object>> eventDic = new Dictionary<string, UnityAction<object>>();

    
    ///<summary>
    ///添加事件
    ///</summary>
    ///<param name="name">事件的名字</param>
    ///<param name="action">准备用来处理事件的委托</param>
    public void AddEventListener(string name, UnityAction<object> action){
        if(eventDic.ContainsKey(name)){
            eventDic[name] += action;
        }else{
            eventDic.Add(name, action);
        }
    }

    ///<summary>
    ///移除事件
    ///</summary>
    ///<param name="name">事件的名字</param>
    ///<param name="action">准备用来处理事件的委托</param>
    public void RemoveEventListener(string name, UnityAction<object> action){
        if(eventDic.ContainsKey(name)){
            eventDic[name] -= action;
        }
    }

    ///<summary>
    ///触发事件
    ///</summary>
    ///<param name="name"> 事件名字 出发 </param>
    public void EventTrigger(string name, object info){
        // 有美誉对应的 事件 监听
        if(eventDic.ContainsKey(name)){
            eventDic[name].Invoke(info);
        }
    }


    public void Clear(){
        eventDic.Clear();
    }
}
