using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receiveDataFromJson : baseManager<receiveDataFromJson>
{
    public receiveDataFromJson(){
        Debug.Log("receiveDataFromJson Start");
        this.getJsonData();
    }
    public void getJsonData(){
        globalUtils.getInstance().receiveJsonDateFormResources<deviceInfoData>("json/dotsData", (res) =>{
            Debug.Log("getJsonData == "+ res);
        });
    }
}
