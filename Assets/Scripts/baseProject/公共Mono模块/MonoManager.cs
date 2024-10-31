using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 可以给外部 添加 帧更新事件 的方法
/// 可以给外部 添加下协程  的方法
/// </summary>
public class MonoManager : baseManager<MonoManager>
{
    private MonoController controller;


    /// <summary>
    /// 构造函数 ， 实例化 new 时 自动调用 
    /// 
    /// baseManager 中 getInstance 会 new 这时 会 调用此 构造函数； 而且 只会 进入一次
    /// </summary>
    public MonoManager(){
        // 实例化 MonoController ； 保证 MonoController 的唯一性
        GameObject gameObject = new GameObject("MonoController");
        controller = gameObject.AddComponent<MonoController>();
        // DontDestroyOnLoad(gameObject);
    }



    /// <summary>
    /// 获取 MonoController 实例
    /// 此方法 是为了 方便 调用； 因为 此 方法 只 会在 构造函数 中 调用 一次； 之后 就不会 调用 了
    /// </summary>
    /// <returns></returns>
    public MonoController getController(){
        return controller;
    }


    // 因为 外部调用 应用的是  MonoManager 所以这里开始封装 添加 移除 监听
        /// <summary>
    /// 给外部 添加更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun){
        controller.AddUpdateListener(fun);
    }
    
    /// <summary>
    /// 移除 事件
    /// </summary>
    /// <param name="fun"></param>
    public void removeUpdateListener(UnityAction fun){
        controller.removeUpdateListener(fun);
    }

    /// <summary>
    /// 协程调用
    /// </summary>
    /// <param name="routine"></param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine){
        return controller.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value){
        return controller.StartCoroutine(methodName, value);
    }

    public Coroutine StartCoroutine(string methodName){
        return controller.StartCoroutine(methodName);
    }




    // 停止协程
    public void StopCoroutine(IEnumerator routine){
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine){
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(string methodName){
        controller.StopCoroutine(methodName);
    }

    public void StopAllCoroutines(){
        controller.StopAllCoroutines();
    }


}
