using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
// using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;
using UnityEngine.UI;
// using Unity.Transforms;
// using UnityEngine;




public struct PrefabConfig : IComponentData
{
    public Entity PrefabEntity;
}
public class PrefabConfigAuthoring : MonoBehaviour
{
    public GameObject Prefab;

    class Baker : Baker<PrefabConfigAuthoring>
    {
        public override void Bake(PrefabConfigAuthoring authoring)
        {
            var prefabEntity = GetEntity(authoring.Prefab, TransformUsageFlags.None);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PrefabConfig { PrefabEntity = prefabEntity });
        }
    }
}



// using Unity.Entities;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

public partial struct SpawnEntitiesSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PrefabConfig>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<PrefabConfig>();
        var prefabEntity = config.PrefabEntity;

        for (int i = 0; i < 2; i++)
        {
            var instance = state.EntityManager.Instantiate(prefabEntity);
            var randomPosition = new float3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));

            state.EntityManager.SetComponentData(instance, LocalTransform.FromPosition(randomPosition));
        }

        // 禁用系统，避免重复生成
        state.Enabled = false;
    }
}



// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Mathematics;
// using Unity.Transforms;
// using UnityEngine;

// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using UnityEngine;

public partial class MouseClickSystem : SystemBase
{
    private Camera _mainCamera;

    protected override void OnCreate()
    {
        base.OnCreate();
        _mainCamera = Camera.main;
        RequireForUpdate<PhysicsWorldSingleton>();
    }

    protected override void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0)) // 检测左键点击
        {
            UnityEngine.Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // 获取 PhysicsWorldSingleton
            // var physicsWorldSingleton = GetSingleton<PhysicsWorldSingleton>();
            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            
            CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

            RaycastInput raycastInput = new RaycastInput
            {
                Start = ray.origin,
                End = ray.origin + ray.direction * 1000f,
                Filter = CollisionFilter.Default
            };

            if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
            {

                Debug.Log($"点击位置: {hit.RigidBodyIndex}");
                // 获取点击的实体
                Entity clickedEntity = collisionWorld.Bodies[hit.RigidBodyIndex].Entity;

                // 检查实体是否有 LocalTransform 组件
                if (EntityManager.HasComponent<LocalTransform>(clickedEntity))
                {
                    LocalTransform transform = EntityManager.GetComponentData<LocalTransform>(clickedEntity);
                    Debug.Log($"点击的实体位置: {transform.Position}");
                    // 获取实体的名称
                    string entityName = EntityManager.GetName(clickedEntity);
                    Debug.Log($"点击的实体名称: {entityName}");
                }
            }
        }
    }
}
