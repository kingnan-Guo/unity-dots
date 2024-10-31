using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gloab_static_data{
    public static List<string> UIIgnoreRaycastTagList = new List<string>() { "2dBackground" }; //忽略射线检测的标签


    private static buildingList _buildingListInfo = new buildingList();
    public static buildingList buildingListInfo{
        get{
            return _buildingListInfo;
        } set{
            _buildingListInfo = value;
        }
    }


    /// <summary>
    /// 当前的楼幢 楼层的信息 
    /// </summary>
    public static Dictionary<string, object> _buildingDictionary = new Dictionary<string, object>();
    public static Dictionary<string, object> buildingDictionary{
        get{
            return _buildingDictionary;
        } set{
            _buildingDictionary = value;
        }
    }

}