using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Scenes;
using UnityEngine;


// [RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(transferMessageSceneSystem))]


// [DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(SceneSystemGroup))]
public partial class transferMessageSystemBase : SystemBase
{
    AssetsReferences assetsRef;

    int target = 0;
    protected override void OnCreate()
    {
        Debug.Log("transferMessageSystemBase OnCreate");
        RequireForUpdate<AssetsReferences>();
        // EntityManager.
        EntityManager.AddComponentData(this.SystemHandle, new LoadedEntityAssets() { entity = Entity.Null, entityInstance = Entity.Null});
        EntityManager.AddComponentData(this.SystemHandle, new LoadedGoAssets(){ gameObject = null, gameObjectInstance = null});

        
        // LoadEntityPrefab();
    }

    protected override void OnUpdate()
    {
        Debug.Log("transferMessageSystemBase");

        if(target == 0){
            this.LoadEntityPrefab();
            // this.LoadGoPrefab();
        }
        // Debug.Log("transferMessageSystemBase OnUpdate = = " );
        var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
        if (assetsRef.entityPrefabReference.IsReferenceValid)
        {
            LoadedEntityAssets asset = EntityManager.GetComponentData<LoadedEntityAssets>(this.SystemHandle);

            Debug.Log("EntityManager.HasComponent<PrefabLoadResult>(asset.entity) "+ EntityManager.HasComponent<PrefabLoadResult>(asset.entity));
             Debug.Log("LoadedEntityAssets  asset OnUpdate ==" + asset.entity);
            if (asset.entity != Entity.Null && EntityManager.HasComponent<PrefabLoadResult>(asset.entity))
            {
                var data = EntityManager.GetComponentData<PrefabLoadResult>(asset.entity);

               
                if (data.PrefabRoot != Entity.Null)
                {
                    EntityManager.DestroyEntity(asset.entity);
                    asset.entity = data.PrefabRoot;
                    if (asset.entityInstance == Entity.Null){
                        asset.entityInstance = EntityManager.Instantiate(asset.entity);
                    }
                    EntityManager.SetComponentData<LoadedEntityAssets>(this.SystemHandle, asset);
                    // if (_handler != null)
                    //     _handler.OnEntityPrefabLoaded();
                }
            }
        }
        // this.LoadEntityPrefab();


        // this.LoadGoPrefab();
        
        // Debug.Log("transferMessageSystemBase OnUpdate = = " + assetsRef.gameObjectPrefabReference.IsReferenceValid);
        // if (assetsRef.gameObjectPrefabReference.IsReferenceValid)
        // {
        //     Debug.Log("transferMessageSystemBase OnUpdate ==" + assetsRef.gameObjectPrefabReference.LoadingStatus + "  ObjectLoadingStatus.Completed ="+ ObjectLoadingStatus.Completed);
        //     Debug.Log("assetsRef.gameObjectPrefabReference.Result =="+ assetsRef.gameObjectPrefabReference.Result);
        //     if(assetsRef.gameObjectPrefabReference.LoadingStatus == ObjectLoadingStatus.Completed)
        //     { 
        //         LoadedGoAssets asset = EntityManager.GetComponentData<LoadedGoAssets>(this.SystemHandle);
        //         if (asset != null && asset.gameObject == null)
        //         {
        //             asset.gameObject = assetsRef.gameObjectPrefabReference.Result;
        //             if(asset.gameObjectInstance == null){
        //                 asset.gameObjectInstance = Object.Instantiate(asset.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        //             }

        //             Debug.Log("LoadedGoAssets: " + asset.gameObject.name);
                        
        //             // if (_handler != null)
        //             //     _handler.OnGameObjectPrefabLoaded();
        //         }
        //         assetsRef.gameObjectPrefabReference.Release();  //弱引用要手动释放
        //     }
        // }


    }



    public void LoadEntityPrefab(){
        target = 1;
        assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
        if(assetsRef.entityPrefabReference.IsReferenceValid){
            LoadedEntityAssets asset = EntityManager.GetComponentData<LoadedEntityAssets>(this.SystemHandle);
            if(asset.entity == Entity.Null)
            {
                asset.entity = EntityManager.CreateEntity();
                EntityManager.AddComponentData<RequestEntityPrefabLoaded>(asset.entity, new RequestEntityPrefabLoaded{
                    Prefab = assetsRef.entityPrefabReference
                });
                EntityManager.SetComponentData<LoadedEntityAssets>(this.SystemHandle, asset);
                Debug.Log("asset =="+ asset.entity);
            }
        }
    }


    private void LoadGoPrefab()
    {
        target = 1;
        assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
        if (assetsRef.gameObjectPrefabReference.IsReferenceValid)
        {
            // Debug.Log("assetsRef.gameObjectPrefabReference.LoadingStatus == "+ assetsRef.gameObjectPrefabReference.LoadingStatus);
            if (assetsRef.gameObjectPrefabReference.LoadingStatus == ObjectLoadingStatus.None)
            {
                assetsRef.gameObjectPrefabReference.LoadAsync();
            }
        }
    }













}