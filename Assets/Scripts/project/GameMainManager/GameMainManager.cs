using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

[System.Serializable]
public class infoData{
        public  string deviceName;
        public  string deviceId;
        public  int deviceCategory;
        public  int? deviceStatus;
        public  int? isOnline;
        public  string modelType;
        public  string modelTypeName;
        public  string imei;
        public  int? parentModelId;
        public  string? parentModelName;
        public  Vector3? position;
        public  float positionX;
        public  float positionY;
        public  float positionZ;
        public  Vector3? scale;
        public  float scaleX;
        public  float scaleY;
        public  float scaleZ;
        public  Vector3? rotate;
        public  float rotateX;
        public  float rotateY;
        public  float rotateZ;
}


[System.Serializable]
public  class JsonData {
    public infoData[] data;
    public int code;
}
public class GameMainManager : SingletonAutoMono<GameMainManager>
{
    public JsonData jsonData;
    void Start()
    {
        Debug.Log("GameMainManager Start");
        // receiveDataFromJson.getInstance();

        globalUtils.getInstance().receiveJsonDateFormResources<JsonData>("json/dotsData", (res) =>{
            jsonData = res;
            Debug.Log("this.jsonData == "+ jsonData.data[0].deviceName);
        });


        customEventHandler.getInstance();


        // var uiInteropSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<changeLocalTransformSystem>();
        // World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>().AddSystemToUpdateList(uiInteropSystem);



        // changeLocalTransformSystemBase  _uiInteropManagedSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged(typeof(changeLocalTransformSystemBase)) as changeLocalTransformSystemBase;
        // if (_uiInteropManagedSystem != null)
        // {
        //     _uiInteropManagedSystem.SetUIEventHandler(this);
        //     World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>().AddSystemToUpdateList(_uiInteropManagedSystem);
        // }

        
        

        for (int i = 0; i < 0; i++){
            var randomPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            Vector3 rotation = new Vector3(1, 1, 1);
            GameObject device = GameObject.Instantiate(
                ResourcesMgr.getInstance().LoadPrefab<GameObject>("Models/yangan2"), 
                randomPosition,
                Quaternion.Euler(rotation)
            );
        }
    }
}
