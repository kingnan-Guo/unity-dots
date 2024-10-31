using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Scenes;
using UnityEngine;

// [DisableAutoCreation]
// [RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(transferMessageSceneSystem))]

// [DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(SceneSystemGroup))]
public partial struct transferMessageSystem : ISystem, ISystemStartStop
{
    // public int targetNumber = 0;
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<AssetsReferences>();
        state.EntityManager.AddComponentData(state.SystemHandle, new LoadedEntityAssets() { entity = Entity.Null, entityInstance = Entity.Null });
        state.EntityManager.AddComponentData(state.SystemHandle, new LoadedGoAssets(){ gameObject = null, gameObjectInstance = null });
        state.EntityManager.AddComponentData(state.SystemHandle, new UIEventBridge()
        {
            handler = Object.FindObjectOfType<UIEventHandler>()
        });
    }



    public void OnStartRunning(ref SystemState state)
    {
        Debug.Log("transferMessageSystem OnStartRunning");
        var eventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
        if ( eventBridge.handler != null)
        {
            eventBridge.handler.OnLoadEntityPrefab += LoadEntityPrefab;
            // eventBridge.handler.OnLoadGameObjectPrefab += LoadGoPrefab;

        }

        LoadEntityPrefab();
    }

    public void OnStopRunning(ref SystemState state)
    {
        var eventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
        if ( eventBridge.handler != null)
        {
            eventBridge.handler.OnLoadEntityPrefab -= LoadEntityPrefab;
            // eventBridge.handler.OnLoadGameObjectPrefab -= LoadGoPrefab;
            // eventBridge.handler.OnUnloadEntityPrefab -= UnloadEntityPrefab;
            // eventBridge.handler.OnUnloadGameObjectPrefab -= UnloadGoPrefab;
        }
    }


    public void OnUpdate(ref SystemState state){
        Debug.Log("transferMessageSystem OnUpdate");



        return;
        var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
        if (assetsRef.entityPrefabReference.IsReferenceValid)
        {
            LoadedEntityAssets asset = state.EntityManager.GetComponentData<LoadedEntityAssets>(state.SystemHandle);
            if (asset.entity != Entity.Null && state.EntityManager.HasComponent<PrefabLoadResult>(asset.entity))
            {
                var data = state.EntityManager.GetComponentData<PrefabLoadResult>(asset.entity);
                if (data.PrefabRoot != Entity.Null)
                {
                    state.EntityManager.DestroyEntity(asset.entity);
                    asset.entity = data.PrefabRoot;
                    if (asset.entityInstance == Entity.Null){
                        asset.entityInstance = state.EntityManager.Instantiate(asset.entity);
                    }
                    state.EntityManager.SetComponentData<LoadedEntityAssets>(state.SystemHandle, asset);

                    Debug.Log("transferMessageSystem OnUpdate end");

                    // var uiEventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
                    // if (uiEventBridge.handler != null)
                    //     uiEventBridge.handler.OnEntityPrefabLoaded();
                }
            }
        }
        
        // var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
        // if (assetsRef.entityPrefabReference.IsReferenceValid)
        // {
        //     LoadedEntityAssets asset = state.EntityManager.GetComponentData<LoadedEntityAssets>(state.SystemHandle);
        //     var data = state.EntityManager.GetComponentData<PrefabLoadResult>(asset.entity);
        //     if (asset.entity != Entity.Null && state.EntityManager.HasComponent<PrefabLoadResult>(asset.entity))
        //     {
                
        //     }
        // }
        // if(targetNumber == 0){

        // }
        // this.LoadEntityPrefab();
    }

    private void LoadEntityPrefab()
    {
        Debug.Log("transferMessageSystem LoadEntityPrefab");
        var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
        if (assetsRef.entityPrefabReference.IsReferenceValid)
        {
            var systemHandle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<transferMessageSystem>();
            LoadedEntityAssets asset = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LoadedEntityAssets>(systemHandle);
            if(asset.entity == Entity.Null)
            {
                asset.entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
                World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(asset.entity, new RequestEntityPrefabLoaded{
                    Prefab = assetsRef.entityPrefabReference
                });
                World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData<LoadedEntityAssets>(systemHandle, asset);

                Debug.Log("transferMessageSystem LoadEntityPrefab end");
            }
        }
    }

}
