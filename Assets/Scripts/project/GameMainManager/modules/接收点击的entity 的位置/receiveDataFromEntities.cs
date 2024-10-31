using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class receiveDataFromEntities : baseManager<receiveDataFromEntities>
{
    public GameObject AxesParentGameObject;

    public receiveDataFromEntities(){

    }

    public void addAxesParentGameObject(GameObject model){
        // 创建空物体
        // GameObject gameObject = new GameObject("AxesParent");
        // gameObject.transform.parent = model.transform;

    }

    public void addAXES(Vector3 pos){
        Debug.Log("addAXES "+ pos);

        if(AxesParentGameObject == null){
            AxesParentGameObject = new GameObject("AxesParent");
        }
        AxesParentGameObject.transform.position = pos;
            // GameObject gameObject = null;


            // GameObject AxesParentGameObject = new GameObject("AxesParent");
            // // gameObject.transform.parent = model.transform;
            // AxesParentGameObject.transform.position = pos;

        // Debug.Log(" ==== " + AxesParentGameObject.TryGetComponent<moveAxes>());


        // Debug.Log("AxesParentGameObject moveAxes ==== " + AxesParentGameObject.GetComponentInChildren<moveAxes>());
        if(AxesParentGameObject.GetComponentInChildren<moveAxes>() == null){
            GameObject MoveAxesGameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/Axes/MoveAxes");

            // gameObject = ResourcesMgr.getInstance().Load<GameObject>("Prefabs/Axes/MoveAxes");
            MoveAxesGameObject.transform.parent = AxesParentGameObject.transform;
            MoveAxesGameObject.transform.localPosition = Vector3.zero;
            // currentAxesParent = model.transform;
            // 添加 坐标轴后 给 坐标轴的 moveAxes 脚本 中的 currentAxisParent 赋值
            moveAxes moveAxes = AxesParentGameObject.GetComponentInChildren<moveAxes>();
            moveAxes.currentAxisParent = AxesParentGameObject.transform;
        }

            

        






    }
}


// public class receiveDataFromEntities : SingletonAutoMono<receiveDataFromEntities>
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
