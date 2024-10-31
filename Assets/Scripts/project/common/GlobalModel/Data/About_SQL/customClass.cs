using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class customClass : BaseData
{
    // private string _sql;
    // public string sql {
    //     get{
    //         return _sql;
    //     } set{
    //         _sql = value;
    //     }
    // }

    public string? sql{
        get;
        set;
    }

    public int? id{
        get;
        set;
    }
}
