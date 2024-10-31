using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 自动 再 场景中 创建 一个 GameObject


public class SingletonAutoMono<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T GetInstance(){
        // Debug.Log("SingletonAutoMono =="+ instance);

        if(instance == null){
            GameObject obj = new GameObject();
            //设置 对象名字 为脚本的名字
            obj.name = typeof(T).ToString();

            // 切换 场景 不 会被删除
            // 因为 往往是 存在 整个程序 生命周期的
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<T>();
        }

        return instance;
    }



}
