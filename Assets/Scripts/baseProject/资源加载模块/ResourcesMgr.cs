using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesMgr : baseManager<ResourcesMgr>
{
    public T Load<T>(string path) where T : Object
    {
        //加载资源
        T res = Resources.Load<T>(path);
        if (res == null)
        {
            Debug.LogError("在Resources中没有找到资源" + path);
        }
        else if(res is GameObject){
            return GameObject.Instantiate(res) as T;
        }
        return res;
    }
    public T LoadPrefab<T>(string path) where T : Object
    {
        //加载资源
        T res = Resources.Load<T>(path);
        if (res == null)
        {
            Debug.LogError("在Resources中没有找到资源" + path);
        }
        else if(res is GameObject){
            return res as T;
        }
        return res;
    }

    // 异步加载资源

    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        MonoManager.getInstance().StartCoroutine(ReallyLoadAsync<T>(path, callback));

    }

    private IEnumerator ReallyLoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        yield return request;

        if(request.asset is GameObject){

            // GameObject.Instantiate(request.asset) as T ； 要将 基类 GameObject 转换为 子类 T
            callback(GameObject.Instantiate(request.asset) as T);
        } else {
            callback((request.asset) as T);
        }

        //加载资源
        
    }
}


