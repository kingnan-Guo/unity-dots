using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelHelp : Attribute
{
    public bool IsCreated { get; set; }// 是否可以创建 字段
    public string FieldName { get; set; }// 字段名
    public string Type { get; set; }// 对应到 数据库中的 类型
    public bool IsPrimaryKey { get; set; }// 是否是主键

    public bool IsCanBeNUll { get; set; }// 是否可以为空 

    // IsUnique
    public bool IsUnique { get; set; }// 是否可以为空 
    // public string TableName { get; set; }
    // public string PrimaryKey { get; set; }
    // public string[] Columns { get; set; }
    // public string[] Types { get; set; }
    // public string[] Constraints { get; set; }
    // public string[] Indexes { get; set; }


    public ModelHelp(bool isCreated, string fieldName, string type, bool isPrimaryKey, bool isCanBeNull = false, bool isUnique = false){
        IsCreated = isCreated;
        FieldName = fieldName;
        Type = type;
        IsPrimaryKey = isPrimaryKey;
        IsCanBeNUll = isCanBeNull;
        IsUnique = isUnique;
    }
}
