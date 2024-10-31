using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// interface BaseData
// {
// }
[System.Serializable]
public class BaseDataClass: BaseData
{
    private Int64 _id;

    [ModelHelp(true, "id", "INTEGER", true, false)]
    public Int64 id {
        get{
            return _id;
        } set{
            _id = value;
        }
    }
}
