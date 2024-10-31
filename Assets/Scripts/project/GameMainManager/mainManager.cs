using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 在 mainManager 中准备数据并且 entity 会 获取这里的数据  
/// 
/// 主要流程如下：
/// 1、mainManager 调取 https 接口 获取到数据， 测试数据可以是 json 数据
/// 2、entity system 获取到数据后，将数据添加到场景中
/// 3、entityManger 点击 entity 后，通知 mainManager 添加坐标轴
/// 4、mainManager 添加坐标轴
/// 5、拖动 坐标轴 后 通知 entity 跟着 坐标轴移动
/// 6、entity 跟着 坐标轴 移动，并且更新 mainManager 中的数据
/// 
/// </summary>
public class mainManager : MonoBehaviour
{   
    void Start()
    {
        GameMainManager.GetInstance();
    }
    void Update()
    {
        
    }
}
