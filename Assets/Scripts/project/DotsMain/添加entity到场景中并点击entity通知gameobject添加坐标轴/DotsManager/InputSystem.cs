
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


// [DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateInGroup(typeof(PrefabSceneSystem))]
public partial struct InputSystem : ISystem
{

    EntityQuery isActiveEntityQuery; // 获取查询
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
        state.EntityManager.AddComponentData(state.SystemHandle, new InputEventBridge()
        {
            // handler = Object.FindObjectOfType<receiveDataFromEntities>()
            handler = receiveDataFromEntities.getInstance()
        });

        var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, SharingGroup, Config>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
        isActiveEntityQuery = state.GetEntityQuery(queryBuilder);
    }
    
    public void OnUpdate(ref SystemState state)
    {
        if (Input.GetMouseButtonUp(0))
        {
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance = 1000f;
            Entity entity = Raycast(ray.origin, ray.direction * rayDistance);
            if (entity.Equals(Entity.Null))
            {
                Debug.Log("Hit gameObject");
                // var inputEventBridge = state.EntityManager.GetComponentData<InputEventBridge>(state.SystemHandle);
                // if (inputEventBridge.handler != null)
                // {
                //     GameObject go = inputEventBridge.handler.RayCast(ray, rayDistance);
                //     if (go != null)
                //     {
                //         go.GetComponent<MeshRenderer>().material.color = Color.red;
                //     }
                // }
            }
            else
            {
                Debug.Log("Hit entity: " + entity);

                // 查询 所有 Config isActive = 1 的 entity
                // var query = state.EntityManager.CreateEntityQuery(typeof(Config));
                
                // query.SetFilter(new EntityQueryDesc{ All = new ComponentType[] { typeof(Config) } });
                // query.SetFilter(new EntityQueryDesc { All = new ComponentType[] { typeof(Config), typeof(LocalTransform) } });

                isActiveEntityQuery.SetSharedComponentFilter(new SharingGroup { isActive = 1 });

                

                var isActiveEntities = isActiveEntityQuery.ToEntityArray(Allocator.Temp);
                // 注释： cubeEntities 是 一个数组，包含了所有满足查询条件的实体
                Debug.Log("isActiveEntities == " + isActiveEntities.Length);

                // var localTransforms = isActiveEntityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);

                var configs = isActiveEntityQuery.ToComponentDataArray<Config>(Allocator.Temp);

                // foreach (var config in configs){
                    
                // }

                if(configs.Length == 1){
                    Config config = state.EntityManager.GetComponentData<Config>(entity);
    
                    if(configs[0].id == config.id){


                        return;


                    } else {

                        foreach (var isActiveEntity in isActiveEntities)
                        {
                            state.EntityManager.AddSharedComponent<SharingGroup>(isActiveEntity, new SharingGroup { 
                                isActive = 0, 
                            });
                        }

                    }

                }
   


                // if(state.EntityManager.HasComponent<Config>(entity)){
                //     // state.EntityManager.、
                //     Config config = state.EntityManager.GetComponentData<Config>(entity);
                //     // config.isActive = 1;
                //     state.EntityManager.SetComponentData(entity, config);
                // }
                if(state.EntityManager.HasComponent<SharingGroup>(entity)){
                    SharingGroup sharingGroup = state.EntityManager.GetSharedComponent<SharingGroup>(entity);
                    // sharingGroup.isActive = 1;
                    // state.EntityManager.SetSharedComponentData(entity, sharingGroup);
                    state.EntityManager.AddSharedComponent<SharingGroup>(entity, new SharingGroup { 
                        isActive = 1, 
                    });
                }

                if (state.EntityManager.HasComponent<LocalTransform>(entity))
                {
                    LocalTransform transform = state.EntityManager.GetComponentData<LocalTransform>(entity);
                    Debug.Log($"点击的实体位置: {transform.Position}");
                    // Debug.Log(" Config == " + state.EntityManager.HasComponent<Config>(entity));
                    // Transform data  = new Vector3(transform.Position.x, transform.Position.y, transform.Position.z);
                    var inputEventBridge = state.EntityManager.GetComponentData<InputEventBridge>(state.SystemHandle);
                    if (inputEventBridge.handler != null)
                    {
                        inputEventBridge.handler.addAXES(new Vector3(transform.Position.x, transform.Position.y, transform.Position.z));
                    }
                }





                // 这里要通知 gameobject 添加坐标轴
                // state.EntityManager.AddComponentData(entity, new CustomColor { customColor = new float4(0, 1, 1, 1) });
            }
        }
    }
    private Entity Raycast(float3 frompos, float3 topos)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

        RaycastInput raycastInput = new RaycastInput
        {
            Start = frompos,
            End = topos,
            Filter = new CollisionFilter
            {
                BelongsTo =  ~0u,  
                CollidesWith = ~0u,
                GroupIndex = 0
            }

        };

        Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();
        if (collisionWorld.CastRay(raycastInput, out raycastHit))
        {
            Entity hitEntity = collisionWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            return hitEntity;
        }
        else
        {
            return Entity.Null;
        }
    }
}