using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class device_info_data  : BaseDataClass
{

    private string _device_name;
    [ModelHelp(true, "device_name", "TEXT", false, false)]
    public string device_name {
        get{
            return _device_name;
        } set{
            _device_name = value;
        }
    }

    private string _device_id;
    [ModelHelp(true, "device_id", "TEXT", false, false)]
    public string deviceId {
        get{
            return _device_id;
        } set{
            _device_id = value;
        }
    }

    private string _device_type;
    [ModelHelp(true, "device_type", "TEXT", false, false)]
    public string deviceType {
        get{
            return _device_type;
        } set{
            _device_type = value;
        }
    }

    private string _device_status;
    [ModelHelp(true, "device_status", "TEXT", false, true)]
    public string device_status {
        get{
            return _device_status;
        } set{
            _device_status = value;
        }
    }

    private string _imei;
    [ModelHelp(true, "imei", "TEXT", false, false)]
    public string imei {
        get{
            return _imei;
        } set{
            _imei = value;
        }
    }

    private string _org_code;
    [ModelHelp(true, "org_code", "TEXT", false, false)]
    public string org_code {
        get{
            return _org_code;
        } set{
            _org_code = value;
        }
    }

    private string _org_name;

    [ModelHelp(true, "org_name", "TEXT", false, false)]
    public string org_name {
        get{
            return _org_name;
        } set{
            _org_name = value;
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
    
    private float? _x;
    [ModelHelp(true, "x", "REAL", false, true)]
    public float? x {
        get{
            return _x;
        } set{
            _x = value;
        }
    }

    private float? _y;
    [ModelHelp(true, "y", "REAL", false, true)]
    public float? y {
        get{
            return _y;
        } set{
            _y = value;
        }
    }
    private float? _z;
    [ModelHelp(true, "z", "REAL", false, true)]
    public float? z {
        get{
            return _z;
        } set{
            _z = value;
        }
    }



}
