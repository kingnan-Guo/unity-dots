using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class deviceInfoParams{
    // "pageNum\": 1,\"pageSize\": 0, \"isLabeled\": 1 "parentModelId\": \"123\"
    private int _pageNum = 1;
    public int pageNum {
        get{
            return _pageNum;
        } set{
            _pageNum = value;
        }
    }
    private int _pageSize = 10;
    public int pageSize {
        get{
            return _pageSize;
        } set{
            _pageSize = value;
        }
    }


    public string? imei{
        get;
        set;
    }
    public string? deviceName{
        get;
        set;
    }
    public int? deviceCategory{
        get;
        set;
    }
    public int? isLabeled{
        get;
        set;
    }
    public string? parentModelId{
        get;
        set;
    }
    public string? modelType{
        get;
        set;
    }
}

