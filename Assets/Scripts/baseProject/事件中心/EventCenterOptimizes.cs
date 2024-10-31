using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 定义事件参数类
public class MyEventArgs <T, S>: EventArgs
{
    public T KEYWORD;
    public S MESSAGE;
    public object EXTENDDATA;
    public string METHODNAME;
    public MyEventArgs(T keyword, S message, object extendData = null, string MethodName = null)
    {
        KEYWORD = keyword;
        MESSAGE = message;
        EXTENDDATA = extendData;
        METHODNAME = MethodName;
    }


}


public delegate void MyEventAction<T, S>(T param1, S param2);
public delegate void MyEventAction();

// 定义 空 接口

interface IEventInfos
{
}

// 容器
public class EventInfos<T, S>: IEventInfos
{
    public MyEventAction<T, S> actions;
    public EventInfos(MyEventAction<T, S> action){
        this.actions += action;
    }
}

// 同名 不同类 容器； 普通类
public class EventInfos: IEventInfos
{
    public MyEventAction actions;
    public EventInfos(MyEventAction action){
        this.actions += action;
    }
}


///<summary>
///事件中心  单例模式 对象
///可以传递 两个参数
///</summary>
public class EventCenterOptimizes : baseManager<EventCenterOptimizes>
{
    private Dictionary<string, IEventInfos> eventDic = new Dictionary<string, IEventInfos>();
    
    ///<summary>
    ///添加事件
    ///</summary>
    ///<param name="name">事件的名字</param>
    ///<param name="action">准备用来处理事件的委托</param>
    public void AddEventListener<T, S>(string name, MyEventAction<T, S> action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfos<T, S>).actions += action;
        }else{
            // eventDic.Add(name, action);
            eventDic.Add(name, new EventInfos<T, S>(action));
        }
    }

    /// <summary>
    /// 重载  无泛型
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void AddEventListener(string name, MyEventAction action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfos).actions += action;
        }else{
            // eventDic.Add(name, action);
            eventDic.Add(name, new EventInfos(action));
        }
    }



    ///<summary>
    ///移除事件
    ///</summary>
    ///<param name="name">事件的名字</param>
    ///<param name="action">准备用来处理事件的委托</param>
    public void RemoveEventListener<T, S>(string name, MyEventAction<T, S> action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfos<T, S>).actions -= action;
        }
    }
    /// <summary>
    /// 移出不需要 参数的事件
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, MyEventAction action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfos).actions -= action;
        }
    }



    ///<summary>
    ///触发事件
    ///</summary>
    ///<param name="name"> 事件名字 出发 </param>
    public void EventTrigger<T, S>(string name, T info, S data){
        // 有美誉对应的 事件 监听
        if(eventDic.ContainsKey(name)){
            if((eventDic[name] as EventInfos<T, S>).actions != null){
                (eventDic[name] as EventInfos<T, S>).actions.Invoke(info, data);
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
            if((eventDic[name] as EventInfos).actions != null){
                (eventDic[name] as EventInfos).actions.Invoke();
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