using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

// public class customEventHandler : MonoBehaviour
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



public class customEventHandler : baseManager<customEventHandler>
{

    changeLocalTransformSystemBase  _uiInteropManagedSystem;
    public customEventHandler(){
        systemBaseEventHandler();
        EventCenterOptimize.getInstance().AddEventListener<Vector3>(gloab_EventCenter_Name.CHANGE_AXIS_PARENT_POSITION, (res) =>{
            // Debug.Log("gloab_EventCenter_Name.CHANGE_AXIS_PARENT_POSITION == " + res);
            this.changePosition(res);
        });
    }



    public void systemEventHandler(){
        var uiInteropSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<changeLocalTransformSystem>();
        World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>().AddSystemToUpdateList(uiInteropSystem);

    }


    public void systemBaseEventHandler(){
        _uiInteropManagedSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged(typeof(changeLocalTransformSystemBase)) as changeLocalTransformSystemBase;
        if (_uiInteropManagedSystem != null)
        {
            _uiInteropManagedSystem.SetUIEventHandler(this);
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>().AddSystemToUpdateList(_uiInteropManagedSystem);
        }
    }

    public void changePosition(Vector3 res){
        Debug.Log("changePosition == " + res);

        _uiInteropManagedSystem.changePosition(res);

    }
}

