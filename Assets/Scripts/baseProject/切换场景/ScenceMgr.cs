using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScenceMgr : baseManager<ScenceMgr>
{
    // Start is called before the first frame update
    public void LoadScence(string name, UnityAction callback)
    {
        //加载场景
        SceneManager.LoadScene(name);
        callback();
        
    }

    public void UnloadScence(string name, UnityAction callback)
    {
        //卸载场景
        SceneManager.UnloadSceneAsync(name);
        callback();
    }


    /// <summary>
    /// 异步加载场景 ； 提供 给外部加载的 接口
    /// 1.异步加载场景
    /// 2.加载完成之后执行回调函数
    /// </summary>
    /// <param name="name"></param>
    public void LoadScenceAsync(string name, UnityAction callback)
    {
        //异步加载场景
        // SceneManager.LoadSceneAsync(name);
        MonoManager.getInstance().StartCoroutine(ReallyLoadScenceAsync(name, callback));

    }

    /// <summary>
    /// 协程 异步 加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadScenceAsync(string name, UnityAction callback)
    {

        //异步加载场景
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);

        while (!ao.isDone)
        {
            // ao.progress = ao.progress / ao.totalProgress;
            Debug.Log("异步加载场景进度：" + ao.progress);
            // 事件中心 触发 进度条更新事件； 向外分发进度
            EventCenter.getInstance().EventTrigger("LoadScenceProgress 进度条更新", ao.progress);
            yield return ao;
        }
        
        callback();
    }


    public void UnloadScenceAsync(string name)
    {
        //异步卸载场景
    }
}
