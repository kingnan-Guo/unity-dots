using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;

// public class changeLocalTransformSystemBase : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

[DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(InitializationSystemGroup))]
[UpdateAfter(typeof(SceneSystemGroup))]
[BurstCompile]
public partial class changeLocalTransformSystemBase : SystemBase
{
    private customEventHandler _handler;
    EntityQuery isActiveEntityQuery; // 获取查询
    Vector3 Vector3Data;
    [BurstCompile]
    public void SetUIEventHandler(customEventHandler handler)
    {
        _handler = handler;
    }
    [BurstCompile]
    protected override void OnCreate()
    {
        Debug.Log("changeLocalTransformSystemBase OnCreate");

        var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, SharingGroup, Config>().WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
        isActiveEntityQuery = GetEntityQuery(queryBuilder);
        isActiveEntityQuery.SetSharedComponentFilter(new SharingGroup { isActive = 1 });
    }
    [BurstCompile]
    protected override void OnUpdate()
    {

        
        
        // Debug.Log("changeLocalTransformSystemBase changePosition == "+ vector3);
        // Vector3Data = vector3;




        // Debug.Log("changeLocalTransformSystemBase OnUpdate");

        // isActiveEntityQuery.SetSharedComponentFilter(new SharingGroup { isActive = 1 });

        // var isActiveEntities = isActiveEntityQuery.ToEntityArray(Allocator.Temp);
        // // 注释： cubeEntities 是 一个数组，包含了所有满足查询条件的实体
        // // Debug.Log("isActiveEntities == " + isActiveEntities.Length);
        // var localTransforms = isActiveEntityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
        // Debug.Log("localTransforms = " + localTransforms.Length);


        // foreach (var entity in isActiveEntities)
        // {

        //     Debug.Log("isActiveEntities entity == " + entity);
            
        //     // // EntityManager.HasComponent<LocalTransform>(entity);
        //     // LocalTransform transform = EntityManager.GetComponentData<LocalTransform>(entity);
        //     // Debug.Log("changeLocalTransformSystemBase transform == " + transform.Position);
        //     // transform.Position = new Vector3(0, 0, 0);
        //     EntityManager.AddComponentData<LocalTransform>(entity, LocalTransform.FromPosition(Vector3Data));
        // }


        // foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>())
        // {
        //     transform.ValueRW = transform.ValueRO.RotateY(2 * SystemAPI.Time.DeltaTime);
        // }

        // isActiveEntities

        // foreach (var localTransform in localTransforms)
        // {
        //     localTransform = 
        // }

        // var configs = isActiveEntityQuery.ToComponentDataArray<Config>(Allocator.Temp);
        
    }
    [BurstCompile]
    public void changePosition(Vector3 vector3){

        // isActiveEntityQuery.SetSharedComponentFilter(new SharingGroup { isActive = 1 });
        // Debug.Log("changeLocalTransformSystemBase changePosition == "+ vector3);
        Vector3Data = vector3;
        // var isActiveEntities = isActiveEntityQuery.ToEntityArray(Allocator.Temp);

        // foreach (var entity in isActiveEntities)
        // {
        //     Debug.Log("isActiveEntities entity == " + entity);
        //     EntityManager.AddComponentData<LocalTransform>(entity, LocalTransform.FromPosition(vector3));
        // }
        

        var isActiveEntities = isActiveEntityQuery.ToEntityArray(Allocator.Temp);

        foreach (var entity in isActiveEntities)
        {
            Debug.Log("isActiveEntities entity == " + entity);
            EntityManager.AddComponentData<LocalTransform>(entity, LocalTransform.FromPosition(Vector3Data));
            // EntityManager.AddComponentData<LocalTransform>(entity, LocalTransform.FromPosition(Vector3Data));
        }
        // isActiveEntityQuery.Dispose();

    }
}

