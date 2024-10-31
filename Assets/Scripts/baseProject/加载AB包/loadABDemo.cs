using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadABDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string name = "Assets/Prefabs/mainMap.prefab";
        // name = "Assets/dahuyuahqu 1.fbx";

        GameObject obj = ABManager.GetInstance().LoadRes("mainmap", name) as GameObject;
        obj.transform.position = new Vector3(0, 0, 0);
        obj.transform.tag = gloab_TagName.MAIN_MAP;

        // Debug.Log("obj.transform.childCount =="+ obj.transform.childCount);
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if(obj.transform.GetChild(i).name.Contains("floor")){
                obj.transform.GetChild(i).tag ="building";
            }
        }
        
        // 将主场景 传给 GameMainManager 
        EventCenterOptimize.getInstance().EventTrigger<Transform>(gloab_EventCenter_Name.GLOBAL_CURRENT_MAIN_PARENT_TRANSFORM, obj.transform);


        EventCenterOptimize.getInstance().EventTrigger<string>(gloab_EventCenter_Name.MAIN_MAP_LOAD_DONE, "true");

        // mainMap(Clone)

        string tree = "Assets/Prefabs/tree.prefab";
        GameObject treeObj = ABManager.GetInstance().LoadRes("treemap", tree) as GameObject;
        treeObj.transform.position = new Vector3(-7.26f, -4.2f, -4.22f);





    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
