using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// 定义 空 接口

interface IEventInfo
{
    
}

// 容器
public class EventInfo<T>: IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action){
        this.actions += action;
    }
}

// 同名 不同类 容器； 普通类
public class EventInfo: IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action){
        this.actions += action;
    }
}


///<summary>
///事件中心  单例模式 对象
///</summary>
public class EventCenterOptimize : baseManager<EventCenterOptimize>
{
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    
    ///<summary>
    ///添加事件
    ///</summary>
    ///<param name="name">事件的名字</param>
    ///<param name="action">准备用来处理事件的委托</param>
    public void AddEventListener<T>(string name, UnityAction<T> action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfo<T>).actions += action;
        }else{
            // eventDic.Add(name, action);
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 重载  无泛型
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, UnityAction action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfo).actions += action;
        }else{
            // eventDic.Add(name, action);
            eventDic.Add(name, new EventInfo(action));
        }
    }



    ///<summary>
    ///移除事件
    ///</summary>
    ///<param name="name">事件的名字</param>
    ///<param name="action">准备用来处理事件的委托</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
    /// <summary>
    /// 移出不需要 参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfo).actions -= action;
        }
    }



    ///<summary>
    ///触发事件
    ///</summary>
    ///<param name="name"> 事件名字 出发 </param>
    public void EventTrigger<T>(string name, T info){
        // 有美誉对应的 事件 监听
        if(eventDic.ContainsKey(name)){
            if((eventDic[name] as EventInfo<T>).actions != null){
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
            }
        }
    }
    /// <summary>
    /// 触发事件
    /// 没有 参数
    /// </summary>
    /// <param name="name"></param>

    public void EventTrigger(string name){
        // 有美誉对应的 事件 监听
        if(eventDic.ContainsKey(name)){
            if((eventDic[name] as EventInfo).actions != null){
                (eventDic[name] as EventInfo).actions.Invoke();
            }
        }
    }


    public void Clear(){
        eventDic.Clear();
    }
}



// 优化版本
// 优化 拆箱 

// object 存在 装箱拆箱的 ，使用 接口 配合 泛型 优化

// 类 以内的 成员 变量 是泛型，那么这个 类也需要是泛型，也就是 UnityAction<T> 要带有泛型 那么  EventCenterOptimize<T>

// 里是转换 原则 ： 基类装 子类 
// IEventInfo作为 基类  可以 储存 子类