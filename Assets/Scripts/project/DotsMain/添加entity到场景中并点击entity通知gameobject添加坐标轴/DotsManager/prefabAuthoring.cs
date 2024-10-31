using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Entities.Serialization;
using UnityEngine;


public struct PrefabConfig : IComponentData
{
    public Entity PrefabEntity;
}

// 添加组件 到实体， 标记设备在那一层
public struct Config : IComponentData
{
    public int id;
    public int parentsId;
}

public struct SharingGroup : ISharedComponentData
{

    
    public int isActive; //0 false, 1 true
}

public class prefabAuthoring : MonoBehaviour
{
    [SerializeField]
    public AssetsReferences assetsReferences;
    public GameObject Prefab;
    class Baker : Baker<prefabAuthoring>
    {
        public override void Bake(prefabAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            // AddComponent(entity, authoring.assetsReferences);
            var prefabEntity = GetEntity(authoring.Prefab, TransformUsageFlags.None);
            AddComponent(entity, new PrefabConfig{PrefabEntity = prefabEntity});
            AddComponent(entity, new Config{id = 0});
        }
    }
}

public partial class PrefabSceneSystem : CustomSceneSystemGroup
{
    protected override string AuthoringSceneName => "mainScene";
}


/// <summary>
/// 
/// </summary>
public class LoadedGoAssets : IComponentData
{
    public GameObject gameObject;
    public GameObject gameObjectInstance;
}
public struct LoadedEntityAssets : IComponentData
{
    public Entity entity;
    public Entity entityInstance;
}
[Serializable]
public struct AssetsReferences : IComponentData
{
    // public Entity PrefabEntity;
    public EntityPrefabReference entityPrefabReference;
    public WeakObjectReference<GameObject> gameObjectPrefabReference;
}


