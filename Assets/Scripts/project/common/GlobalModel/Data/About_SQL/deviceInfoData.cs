using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class deviceInfoData : BaseDataClass
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
    [ModelHelp(true, "deviceName", "TEXT", false, false)]
    public string deviceName {
        get{
            return _deviceName;
        } set{
            _deviceName = value;
        }
    }

    private string _deviceId;
    [ModelHelp(true, "deviceId", "TEXT", false, true)]
    public string deviceId {
        get{
            return _deviceId;
        } set{
            _deviceId = value;
        }
    }
    private string _imei;
    [ModelHelp(true, "imei", "TEXT", false, false, true)]
    public string imei {
        get{
            return _imei;
        } set{
            _imei = value;
        }
    }
    //设备大类
    private long? _deviceCategory;
    [ModelHelp(true, "deviceCategory", "INTEGER", false, false)]
    public long? deviceCategory {
        get{
            return _deviceCategory;
        } set{
            _deviceCategory = value;
        }
    }

    private string _deviceStatus;
    [ModelHelp(true, "deviceStatus", "TEXT", false, true)]
    public string deviceStatus {
        get{
            return _deviceStatus;
        } set{
            _deviceStatus = value;
        }
    }

    private string? _isOnline;
    [ModelHelp(true, "isOnline", "TEXT", false, true)]
    public string? isOnline {
        get{
            return _isOnline;
        } set{
            _isOnline = value;
        }
    }



    // private string _orgCode;
    // [ModelHelp(true, "orgCode", "TEXT", false, true)]
    // public string orgCode {
    //     get{
    //         return _orgCode;
    //     } set{
    //         _orgCode = value;
    //     }
    // }

    // private string _orgName;

    // [ModelHelp(true, "orgName", "TEXT", false, true)]
    // public string orgName {
    //     get{
    //         return _orgName;
    //     } set{
    //         _orgName = value;
    //     }
    // }

    private string _modelType;
    [ModelHelp(true, "modelType", "TEXT", false, false)]
    public string modelType {
        get{
            return _modelType;
        } set{
            _modelType = value;
        }
    }

    private string _modelTypeName;
    [ModelHelp(true, "modelTypeName", "TEXT", false, false)]
    public string modelTypeName {
        get{
            return _modelTypeName;
        } set{
            _modelTypeName = value;
        }
    }


    private string _parentModelId;
    [ModelHelp(true, "parentModelId", "TEXT", false, false)]
    public string parentModelId {
        get{
            return _parentModelId;
        } set{
            _parentModelId = value;
        }
    }

    private string _parentModelName;
    [ModelHelp(true, "parentModelName", "TEXT", false, false)]
    public string parentModelName {
        get{
            return _parentModelName;
        } set{
            _parentModelName = value;
        }
    }




    private Vector3 _position;
    [ModelHelp(false, "Position", "Vector3", false, true)]
    public Vector3 position {
        get{
            return _position;
        } set{
            _position = value;
        }
    }
    private Vector3 _localPosition;
    [ModelHelp(false, "localPosition", "Vector3", false, true)]
    public Vector3 localPosition {
        get{
            return _localPosition;
        } set{
            _localPosition = value;
        }
    }
    
    private float? _positionX;
    [ModelHelp(true, "positionX", "REAL", false, true)]
    public float? positionX {
        get{
            return _positionX;
        } set{
            _positionX = value;
        }
    }

    private float? _positionY;
    [ModelHelp(true, "positionY", "REAL", false, true)]
    public float? positionY {
        get{
            return _positionY;
        } set{
            _positionY = value;
        }
    }
    private float? _positionZ;
    [ModelHelp(true, "positionZ", "REAL", false, true)]
    public float? positionZ {
        get{
            return _positionZ;
        } set{
            _positionZ = value;
        }
    }
    private Vector3 _scale;
    [ModelHelp(false, "scale", "Vector3", false, true)]
    public Vector3 scale {
        get{
            return _scale;
        } set{
            _scale = value;
        }
    }

    private Vector3 _localScale;
    [ModelHelp(false, "localScale", "Vector3", false, true)]
    public Vector3 localScale {
        get{
            return _localScale;
        } set{
            _localScale = value;
        }
    }


    private float? _scaleY;
    [ModelHelp(true, "scaleY", "REAL", false, true)]
    public float? scaleY {
        get{
            return _scaleY;
        } set{
            _scaleY = value;
        }
    }

    private float? _scaleX;
    [ModelHelp(true, "scaleX", "REAL", false, true)]
    public float? scaleX {
        get{
            return _scaleX;
        } set{
            _scaleX = value;
        }
    }

    private float? _scaleZ;
    [ModelHelp(true, "scaleZ", "REAL", false, true)]
    public float? scaleZ {
        get{
            return _scaleZ;
        } set{
            _scaleZ = value;
        }
    }

    private Vector3 _rotate;
    [ModelHelp(false, "rotate", "Vector3", false, true)]
    public Vector3 rotate {
        get{
            return _rotate;
        } set{
            _rotate = value;
        }
    }

    private Vector3 _localRotate;
    [ModelHelp(false, "localRotate", "Vector3", false, true)]
    public Vector3 localRotate {
        get{
            return _localRotate;
        } set{
            _localRotate = value;
        }
    }

    private float? _rotateX;
    [ModelHelp(true, "rotateX", "REAL", false, true)]
    public float? rotateX {
        get{
            return _rotateX;
        } set{
            _rotateX = value;
        }
    }

    private float? _rotateY;
    [ModelHelp(true, "rotateY", "REAL", false, true)]
    public float? rotateY {
        get{
            return _rotateY;
        } set{
            _rotateY = value;
        }
    }

    private float? _rotateZ;
    [ModelHelp(true, "rotateZ", "REAL", false, true)]
    public float? rotateZ {
        get{
            return _rotateZ;
        } set{
            _rotateZ = value;
        }
    }

    

}

// deviceIcon 会有单独 的字典 数据库 使用 deviceType 对应 路径
