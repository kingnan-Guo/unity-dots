using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// public class BaseDataClass
// {
//     private Int64 _id;

//     [ModelHelp(true, "id", "INTEGER", true, false)]
//     public Int64 id {
//         get{
//             return _id;
//         } set{
//             _id = value;
//         }
//     }
// }

public class deviceInfoClass : BaseDataClass
{
    // private int _id;

    // [ModelHelp(true, "id", "int", true, false)]
    // public int id {
    //     get{
    //         return _id;
    //     } set{
    //         _id = value;
    //     }
    // }

    private string _deviceName;
    [ModelHelp(true, "deviceName", "string", false, false)]
    public string deviceName {
        get{
            return _deviceName;
        } set{
            _deviceName = value;
        }
    }

    private string _deviceId;
    [ModelHelp(true, "deviceId", "string", false, false)]
    public string deviceId {
        get{
            return _deviceId;
        } set{
            _deviceId = value;
        }
    }

    private string _deviceType;
    [ModelHelp(true, "deviceType", "string", false, false)]
    public string deviceType {
        get{
            return _deviceType;
        } set{
            _deviceType = value;
        }
    }

    private string _deviceStatus;
    [ModelHelp(true, "deviceStatus", "string", false, true)]
    public string deviceStatus {
        get{
            return _deviceStatus;
        } set{
            _deviceStatus = value;
        }
    }

    private string _imei;
    [ModelHelp(true, "imei", "string", false, false)]
    public string imei {
        get{
            return _imei;
        } set{
            _imei = value;
        }
    }

    private string _orgCode;
    [ModelHelp(true, "orgCode", "string", false, false)]
    public string orgCode {
        get{
            return _orgCode;
        } set{
            _orgCode = value;
        }
    }

    private string _orgName;

    [ModelHelp(true, "orgName", "string", false, false)]
    public string orgName {
        get{
            return _orgName;
        } set{
            _orgName = value;
        }
    }

    // // public position:{
    // //     x: 85,
    // //     y: 78,
    // //     z: 55
    // // }
    // [ModelHelp(true, "Position", "string", false, false)]
    private Vector3 _position;
    [ModelHelp(false, "Position", "Vector3", false, true)]
    public Vector3 position {
        get{
            return _position;
        } set{
            _position = value;
        }
    }
    
    private float _x;
    [ModelHelp(true, "x", "float", false, true)]
    public float x {
        get{
            return _x;
        } set{
            _x = value;
        }
    }

    private float _y;
    [ModelHelp(true, "y", "float", false, true)]
    public float y {
        get{
            return _y;
        } set{
            _y = value;
        }
    }
    private float _z;
    [ModelHelp(true, "z", "float", false, true)]
    public float z {
        get{
            return _z;
        } set{
            _z = value;
        }
    }



}

// deviceIcon 会有单独 的字典 数据库 使用 deviceType 对应 路径
