using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//继承 MonoBehaviour 只有 挂载到 GameObject 上的时候 被 实例化，不能自己实例化
// 继承 MonoBehaviour 的单例 需要我们 自己保证 唯一性
// 切换 场景 会被删除
public class singletonMono<T> : MonoBehaviour where T: MonoBehaviour
{
    private static T instance;

    public static T GetInstance(){

        return instance;
    }


    // 虚函数
    protected virtual void Awake(){
        instance =  this as T;
    }
}
