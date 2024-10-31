using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class UIEventHandler : MonoBehaviour
{
    public Mesh mesh;
    public Material material;

    // private UIInteropManagedSystem _uiInteropManagedSystem;
    public Action OnLoadEntityPrefab;
    // public Action OnLoadGameObjectPrefab;
    // public Action OnUnloadEntityPrefab;
    // public Action OnUnloadGameObjectPrefab;


    // Start is called before the first frame update
    void Start()
    {
        // var uiInteropSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<transferMessageSystem>();
        // World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>().AddSystemToUpdateList(uiInteropSystem);
    
    
        // Debug.Log("UIEventHandler Start");
        // OnLoadEntityPrefab?.Invoke();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
