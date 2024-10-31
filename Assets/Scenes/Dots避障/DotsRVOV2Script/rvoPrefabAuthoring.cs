using System.Collections;
using System.Collections.Generic;
using Nebukam.ORCA;
using Unity.Entities;
using UnityEngine;

public struct rvoPrefabConfig : IComponentData
{
    public Entity PrefabEntity;
}

public struct AgentComponent : IComponentData
{
    public float seed;// 速度 随机种子
    public Vector3 targetPos;
    public int id;
}


public class rvoPrefabAuthoring : MonoBehaviour
{
    // [SerializeField]
    // public AssetsReferences assetsReferences;
    public GameObject Prefab;
    class Baker : Baker<rvoPrefabAuthoring>
    {
        public override void Bake(rvoPrefabAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var prefabEntity = GetEntity(authoring.Prefab, TransformUsageFlags.None);
            AddComponent(entity, new rvoPrefabConfig{PrefabEntity = prefabEntity});
            // AddComponent(entity, new AgentComponent{seed = 20.0f, targetPos = Vector3.zero});
        }
    }
}

public partial class rvoPrefabSceneSystem : CustomSceneSystemGroup
{
    protected override string AuthoringSceneName => "DotsRVO2";
}
