
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;


// [DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(rvoPrefabSceneSystem))]
public partial struct rvoInputSystem : ISystem
{

    EntityQuery isActiveEntityQuery; // 获取查询
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
        state.EntityManager.AddComponentData(state.SystemHandle, new rvoInputEventBridge()
        {
            // handler = Object.FindObjectOfType<receiveDataFromEntities>()
            handler = rvoSingleMethod.getInstance()
        });
        // var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, SharingGroup, Config>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
        // isActiveEntityQuery = state.GetEntityQuery(queryBuilder);
    }
    
    public void OnUpdate(ref SystemState state)
    {
        // Debug.Log("rvoInputSystem OnUpdate");
        if (Input.GetMouseButtonUp(0))
        {
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance = 1000f;
            Entity entity = Raycast(ray.origin, ray.direction * rayDistance);
            if (entity.Equals(Entity.Null))
            {
                // Debug.Log("Hit gameObject");
                var inputEventBridge = state.EntityManager.GetComponentData<rvoInputEventBridge>(state.SystemHandle);
                if (inputEventBridge.handler != null)
                {

                    UnityEngine.RaycastHit hit= inputEventBridge.handler.RayCast(ray, rayDistance);
                    // Debug.Log("Hit gameObject: " + hit.point);
                    // 控制所有的点移动
                    
                }
            }
            else
            {
                Debug.Log("Hit entity: " + entity);
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