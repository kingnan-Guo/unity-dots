using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;


public static class EquipmentStatusDictionary{
    public static string NULL = "0";
    public static string ALARM = "1";
    public static string OFFLINE = "2";
    public static string SHIELD = "3";
    public static string FAULT = "4";
    public static string NORMAL = "5";
}


[System.Serializable]
public  enum EquipmentEventType{
    UPDATE = 0,
    ADD = 1,
    REMOVE = 2
}

[System.Serializable]
public class EquipmentEvent {
    public string keyword;
    public EquipmentEventType equipmentEventType;
}


/// <summary>
/// 设备 基类  可能是 消防设备 可能是  视频设备 
/// </summary>
public class EquipmentBaseClass : EquipmentBase
{
    public string keyword;
    public BaseData baseData;
    public EquipmentEvent equipmentEvent;
}
public interface EquipmentBase
{
}