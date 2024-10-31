using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScripTableObjectDemo : MonoBehaviour
{
    public deviceTableData deviceTableData;
    public tableDataTest tableDataTest;

    private void onTrggerEnter2d(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            // Debug.Log("deviceTableData: " + deviceTableData);
            // Debug.Log("tableDataTest: " + tableDataTest);
            
            AddNewDeviceData();
            Destroy(gameObject);
        }
    }

    public void AddNewDeviceData(){
        //如果 包里 没有此 物体
        if(!tableDataTest.tableDatas.Contains(deviceTableData)){
            tableDataTest.tableDatas.Add(deviceTableData);
        } else {
            // Debug.Log("已经存在");
            deviceTableData.Count += 1;
        }
    }
}
