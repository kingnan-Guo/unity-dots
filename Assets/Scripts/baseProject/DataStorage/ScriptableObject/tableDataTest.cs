using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "deviceTableDataList", menuName = "inventory/deviceTableDataList", order = 1)]
public class tableDataTest : ScriptableObject
{
    public List<deviceTableData> tableDatas = new List<deviceTableData>();
}
