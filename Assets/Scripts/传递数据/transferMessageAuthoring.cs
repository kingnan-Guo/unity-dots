using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Entities.Serialization;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 信息传输
/// 1、 创建多个 entity，每个entity对应一个物体
/// 2、 点击 entity 通知 monoBehaviour 在  entitiy 上添加 坐标轴 
/// 3、 拖动坐标轴  同时 传输信息给 entity， entity 修改位
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
    public EntityPrefabReference entityPrefabReference;
    public WeakObjectReference<GameObject> gameObjectPrefabReference;
}

public class transferMessageAuthoring : MonoBehaviour
{
    [SerializeField]
    public AssetsReferences assetsReferences;
    class Baker : Baker<transferMessageAuthoring>
    {
        public override void Bake(transferMessageAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, authoring.assetsReferences);
        }
    }
}


public partial class transferMessageSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "transferMessageScene";
}

