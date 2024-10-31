using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class floorList{
    public string name;
    public string type;
}

[System.Serializable]
public class buildingInfo{
    public string name;
    public string type;
    public Vector3 position;
    public int direction;
    public floorList[] floorList;
}
[System.Serializable]
public class buildingList{
    public buildingInfo[] data;
    public int code;
    public string mainModelName;
    public string floorKeyWord;
}
