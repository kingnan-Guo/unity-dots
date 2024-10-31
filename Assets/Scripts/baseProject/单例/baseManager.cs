using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseManager<T> where T: new()
{
    private static T instance;


    // private baseManager 可以避免 外部 new ； 保护类型 构造函数
    // private baseManager(){
    // }

    public static T getInstance(){
        if(instance == null){
            instance = new T();
        }
        return instance;
    }
}



public class GameManager: baseManager<GameManager>
{
    void test(){
        GameManager aa = GameManager.getInstance();
    }
}



public class Test{

    void Main(){
    //    GameManager a = GameManager.getInstance();
        // GameManager aa  = GameManager.getInstance();
    }

}