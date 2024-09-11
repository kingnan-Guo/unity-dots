using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Physics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;



// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
using Unity.Physics.Systems;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

public struct Config : IComponentData
{
    public EntityPrefabReference PrefabReference;
    // public string EntityName;
}


public class ConfigAuthoring : MonoBehaviour
{
    public GameObject Prefab;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var prefabEntity = new EntityPrefabReference(authoring.Prefab);

            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Config
            {
                PrefabReference = prefabEntity,
            });
        }
    }
}


public partial struct LoadPrefabSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var config = SystemAPI.GetSingleton<Config>();
        var configEntity = SystemAPI.GetSingletonEntity<Config>();

        state.EntityManager.AddComponentData(configEntity, new RequestEntityPrefabLoaded
        {
            Prefab = config.PrefabReference
        });
    }
}



    public partial struct SpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Config>();
            state.RequireForUpdate<PrefabLoadResult>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var config = SystemAPI.GetSingleton<Config>();

            var configEntity = SystemAPI.GetSingletonEntity<Config>();
            if (!SystemAPI.HasComponent<PrefabLoadResult>(configEntity))
            {
                return;
            }
            state.Enabled = false;




            var prefabLoadResult = SystemAPI.GetComponent<PrefabLoadResult>(configEntity);


            
            for (int i = 0; i < 100; i++)
            {
                var entity = state.EntityManager.Instantiate(prefabLoadResult.PrefabRoot);

                // var sphereCollider = new PhysicsCollider
                // {
                //     Value = Unity.Physics.SphereCollider.Create(new SphereGeometry
                //     {
                //         Center = Unity.Mathematics.float3.zero,
                //         Radius = 0.5f
                //     })
                // };
                // state.EntityManager.AddComponentData(entity, sphereCollider);

                var randomPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
                state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(randomPosition));

                
                
            
            
            }
        }
    }








// public partial struct MouseClickSystem : ISystem
// {
//     private Camera _mainCamera;
//     private SystemHandle _buildPhysicsWorldHandle;

//     public void OnCreate(ref SystemState state)
//     {
//         _mainCamera = Camera.main;

//         // 获取 BuildPhysicsWorld 的 SystemHandle
//         _buildPhysicsWorldHandle = state.World.Unmanaged.GetExistingUnmanagedSystem<BuildPhysicsWorld>();
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         // 通过 SystemAPI.GetSingleton 获取 PhysicsWorld
//         var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;

//         if (Input.GetMouseButtonDown(0)) // 左键点击
//         {
//             UnityEngine.Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
//             RaycastInput raycastInput = new RaycastInput
//             {
//                 Start = ray.origin,
//                 End = ray.origin + ray.direction * 1000f,  // 光线长度，设置为一个较大的值
//                 Filter = CollisionFilter.Default // 默认的碰撞过滤器
//             };

//             // 进行光线投射
//             if (physicsWorld.CollisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
//             {
//                 // 获取点击的实体
//                 Entity hitEntity = physicsWorld.Bodies[hit.RigidBodyIndex].Entity;

//                 // 获取该实体的相关信息
//                 if (state.EntityManager.HasComponent<LocalTransform>(hitEntity))
//                 {
//                     var entityTransform = state.EntityManager.GetComponentData<LocalTransform>(hitEntity);
//                     Debug.Log($"点击的实体位置: {entityTransform.Position}");

//                     // 如果实体上有其他组件，可以继续获取它们
//                     // if (state.EntityManager.HasComponent<YourComponent>(hitEntity))
//                     // {
//                     //     var yourComponentData = state.EntityManager.GetComponentData<YourComponent>(hitEntity);
//                     //     Debug.Log($"实体有组件 YourComponent，值为: {yourComponentData.SomeField}");
//                     // }
//                 }
//             }
//         }
//     }
// }






