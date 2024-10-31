using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;


struct RespawnController : IComponentData
{
    public float timer;
}

struct PrefabBufferElement : IBufferElementData
{
    public EntityPrefabReference prefab;
}

struct RespawnCleanupComponent : ICleanupComponentData
{
}

public class cleanUpComponentAuthoring : MonoBehaviour
{
    public GameObject[] spawners = null;
    [Range(1, 5)]public float timer = 3.0f;
    class Baker : Baker<cleanUpComponentAuthoring>
    {
        public override void Bake(cleanUpComponentAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            var data = new RespawnController
            {
                timer = authoring.timer
            };
            AddComponent(entity, data);


            var buffer = AddBuffer<PrefabBufferElement>(entity);
            // foreach (var spawner in authoring.spawners){
            //     var element = new PrefabBufferElement{
            //         prefab = new EntityPrefabReference(spawner)
            //     };
            //     buffer.Add(element);
            // }
                for (int i = 0; i < authoring.spawners.Length; i++)
                {
                    var elem = new PrefabBufferElement
                    {
                        prefab = new EntityPrefabReference(authoring.spawners[i])
                    };
                    buffer.Add(elem);
                }
        }
    }
}


public partial class cleanUpComponentSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "cleanUpComponentScene";
}


[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(cleanUpComponentSceneSystem))]
[BurstCompile]
public partial struct cleanUpComponentSystem : ISystem, ISystemStartStop
{

    private Entity controllerEntity;
    private Entity instanceEntity;
    private int totalCubes;
    private int index;

    private float timer;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("cleanUpComponentSystem OnCreate");
        state.RequireForUpdate<RespawnController>();
    }

    public void OnStartRunning(ref SystemState state)
    {
        Debug.Log("cleanUpComponentSystem OnStartRunning");
        // throw new System.NotImplementedException();
         index = 0;
         timer = 0;
        controllerEntity = default;
        instanceEntity = default;
        var prefabs = SystemAPI.GetSingletonBuffer<PrefabBufferElement>(true);

        Debug.Log(prefabs.Length);
        controllerEntity = state.EntityManager.CreateEntity();

        state.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(
            controllerEntity, new RequestEntityPrefabLoaded(){
                Prefab = prefabs[0].prefab
            }
        );
        // state.EntityManager.AddComponent<RespawnCleanupComponent>(controllerEntity);
        
        index++;
    }

    public void OnStopRunning(ref SystemState state)
    {
        Debug.Log("cleanUpComponentSystem OnStopRunning");
        // throw new System.NotImplementedException();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!controllerEntity.Equals(default))
        {
            if (state.EntityManager.HasComponent<PrefabLoadResult>(controllerEntity))
            {
                if(!instanceEntity.Equals(default)){
                    state.EntityManager.DestroyEntity(instanceEntity);
                }
                    
                var data = state.EntityManager.GetComponentData<PrefabLoadResult>(controllerEntity);
                instanceEntity = state.EntityManager.Instantiate(data.PrefabRoot);
                state.EntityManager.DestroyEntity(controllerEntity);
                timer = 0;
            }

            var controller = SystemAPI.GetSingleton<RespawnController>();
            timer += SystemAPI.Time.DeltaTime;
            if (timer >= controller.timer)
            {
                var prefabs = SystemAPI.GetSingletonBuffer<PrefabBufferElement>(true);
                state.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(controllerEntity, new RequestEntityPrefabLoaded
                {
                    Prefab = prefabs[index % prefabs.Length].prefab
                });
                index++;
                timer = 0;
            }
        }
    }
}

// [RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(cleanUpComponentSceneSystem))]
// public partial struct EntityRespawnSystem : ISystem, ISystemStartStop
// {
//     private int index;
//     private float timer;
//     private Entity controllerEntity;
//     private Entity instanceEntity;
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         state.RequireForUpdate<RespawnController>();
//     }


//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         if (!controllerEntity.Equals(default))
//         {
//             if (state.EntityManager.HasComponent<PrefabLoadResult>(controllerEntity))
//             {
//                 if(!instanceEntity.Equals(default)){
//                     state.EntityManager.DestroyEntity(instanceEntity);
//                 }
                    
//                 var data = state.EntityManager.GetComponentData<PrefabLoadResult>(controllerEntity);
//                 instanceEntity = state.EntityManager.Instantiate(data.PrefabRoot);
//                 state.EntityManager.DestroyEntity(controllerEntity);
//                 timer = 0;
//             }

//             var controller = SystemAPI.GetSingleton<RespawnController>();
//             timer += SystemAPI.Time.DeltaTime;
//             if (timer >= controller.timer)
//             {
//                 var prefabs = SystemAPI.GetSingletonBuffer<PrefabBufferElement>(true);
//                 state.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(controllerEntity, new RequestEntityPrefabLoaded
//                 {
//                     Prefab = prefabs[index % prefabs.Length].prefab
//                 });
//                 index++;
//                 timer = 0;
//             }
//         }
//     }
//     [BurstCompile]
//     public void OnStartRunning(ref SystemState state)
//     {
//         index = 0;
//         timer = 0;
//         controllerEntity = default;
//         instanceEntity = default;
//         var prefabs = SystemAPI.GetSingletonBuffer<PrefabBufferElement>(true);
//         controllerEntity = state.EntityManager.CreateEntity();
//         state.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(controllerEntity, new RequestEntityPrefabLoaded
//         {
//             Prefab = prefabs[index % prefabs.Length].prefab
//         });
//         state.EntityManager.AddComponent<RespawnCleanupComponent>(controllerEntity);
//         index++;
//     }
//     [BurstCompile]
//     public void OnStopRunning(ref SystemState state)
//     {
//         // if (!instanceEntity.Equals(default))
//         // {
//         //     state.EntityManager.DestroyEntity(instanceEntity);
//         //     instanceEntity = default;
//         // }

//         // if (!controllerEntity.Equals(default))
//         // {
//         //     state.EntityManager.DestroyEntity(controllerEntity);
//         //     index = 0;
//         //     timer = 0;
//         //     controllerEntity = default;
//         // }
//     }
// }



