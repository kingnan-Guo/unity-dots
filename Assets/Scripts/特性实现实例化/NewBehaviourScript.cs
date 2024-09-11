using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


class EntityObject : Attribute{
    // 存储实例
    private static Dictionary<Type, object> _dic = new Dictionary<Type, object>();
    public EntityObject(Type T){
        // Debug.Log("GameEntityObject");
        Activator.CreateInstance(T);
        if(_dic.ContainsKey(T)){
            _dic.Add(T, Activator.CreateInstance(T));
        }
    }
    public static T Get<T>() where T : class{
        return _dic[typeof(T)] as T;
    }
}


[EntityObject(typeof(GameEntityObject))]
class GameEntityObject{
    public GameEntityObject(){
        Debug.Log("GameEntityObject");
    }
}

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        
        // 获取所有的 类型
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in types){
            // 判断是否有装饰器
            // if (type.GetCustomAttribute<EntityObject>() != null){
            //     // 获取实例
            //     // GameEntityObject gameEntityObject = EntityObject.Get<GameEntityObject>();
            //     // Debug.Log(gameEntityObject);
            //     // Debug.Log(type.GetCustomAttribute<EntityObject>().GetType());
            //     //  Debug.Log(type);
            // }
            type.GetCustomAttribute<EntityObject>();
            // Debug.Log(type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
// 通过装饰器  实例化对象