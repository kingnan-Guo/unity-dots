using System.Collections;
using System.Collections.Generic;
using Nebukam.ORCA;
using UnityEngine;

public class rvoSingleMethod : baseManager<rvoSingleMethod>
{

    public Dictionary<int, IAgent> dic = new Dictionary<int, IAgent>();

    public RaycastHit curHit;
    public rvoSingleMethod(){
        
    }

    public RaycastHit RayCast(Ray ray, float rayDistance){
        RaycastHit hit;
        Vector3 targetPos = Vector3.zero;
        if(Physics.Raycast(ray, out hit)){
            // Debug.Log(hit.collider.gameObject.name);
            // targetPos = hit.point;
            hit = hit;
            curHit = hit;
            // NavToDest(targetPos);
        }
        return hit;
    }

    
}
