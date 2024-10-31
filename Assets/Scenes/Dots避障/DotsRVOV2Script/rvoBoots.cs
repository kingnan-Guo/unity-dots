using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rvoBoots : MonoBehaviour
{
    public void Awake(){
        RVOWapper.GetInstance().Init();
    }
    void Start()
    {
    }

    public void NavToDest(Vector3 targetPos){
        // Debug.Log("rvoBoots 导航到目标点 "+ targetPos);
    }

    void Update()
    {
        // if(Input.GetMouseButtonDown(0)){
        //     var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if(Physics.Raycast(ray, out hit)){
        //         // Debug.Log(hit.collider.gameObject.name);
        //         Vector3 targetPos = hit.point;
        //         NavToDest(targetPos);
        //     }
        // }
    }

    void OnApplicationQuit(){
        Debug.Log("退出游戏");
        RVOWapper.GetInstance().Exit();
    }

}
