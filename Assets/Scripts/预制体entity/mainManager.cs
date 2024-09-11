using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            // EventCenter.getInstance().EventTrigger("鼠标点击左键", Input.mousePosition);
            Debug.Log("鼠标点击左键");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;//   碰撞点 是 世界 坐标系
            bool res = Physics.Raycast(ray,out hit);
            if(res){
                Debug.Log("getGameObject =="+hit.collider.name);
            }



        }

    }
}
