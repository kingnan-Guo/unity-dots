using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 创建 fileName 文件名字 menuName 菜单名字 order 菜单顺序
[CreateAssetMenu(fileName = "deviceTableData", menuName = "inventory/deviceTableData", order = 1)]
public class deviceTableData : ScriptableObject
{
    // 物品有 公开的变量 来存储 属性
    public string deviceName;
    public string deviceType;
    public Sprite deviceIcon;
    public int devicePrice;
    [TextArea]
    public string deviceInfo;

    public int deviceID;
    public bool E;
    public int Count;
}
