using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


public struct SharingGroup : ISharedComponentData
{
    //0 red, 1 green, 2 blue
    public int group;
}

struct CubesGenerator : IComponentData
{
    public Entity redCubeProtoType;
    public Entity blueCubeProtoType;

    public int count;
    public float rotateSpeed;
    public float moveSpeed;
}


struct RotateSpeed : IComponentData, IEnableableComponent
{
    public float rotateSpeed;
}
struct MovementSpeed : IComponentData, IEnableableComponent
{
    public float movementSpeed;
}


public class EnableableComponentAutoring : MonoBehaviour
{
        public GameObject redCubeProtoType = null;
        public GameObject blueCubeProtoType = null;
        [Range(1, 10000)] public int count = 10;
        public float rotateSpeed = 180.0f;
        public float moveSpeed = 5.0f;


    class Baker : Baker<EnableableComponentAutoring>
    {
        public override void Bake(EnableableComponentAutoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);


            var data = new CubesGenerator
            {
                redCubeProtoType = GetEntity(authoring.redCubeProtoType, TransformUsageFlags.Dynamic),
                blueCubeProtoType = GetEntity(authoring.blueCubeProtoType, TransformUsageFlags.Dynamic),
                count = authoring.count,
                rotateSpeed = authoring.rotateSpeed,
                moveSpeed = authoring.moveSpeed
            };
            AddComponent(entity, data);


        }
    }
}
public partial class EnableableComponentSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "EnableableComponentScene";
}






[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(EnableableComponentSceneSystem))]
[BurstCompile]
public partial struct EnableableComponentSystem : ISystem
{
    private int totalCubes;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("DynamicBufferSystem OnCreate");
        state.RequireForUpdate<CubesGenerator>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var generator = SystemAPI.GetSingleton<CubesGenerator>();
        if (totalCubes >= generator.count)
        {
            state.Enabled = false;
            return;
        }

        Entity redCube = state.EntityManager.Instantiate(generator.redCubeProtoType);
        Entity blueCube = state.EntityManager.Instantiate(generator.blueCubeProtoType);
               
        state.EntityManager.AddSharedComponent<SharingGroup>(redCube, new SharingGroup { group = 0 });
        state.EntityManager.AddSharedComponent<SharingGroup>(blueCube, new SharingGroup { group = 2 });


        state.EntityManager.AddComponentData<RotateSpeed>(redCube, new RotateSpeed
        {
            rotateSpeed = Unity.Mathematics.math.radians(generator.rotateSpeed)
        });
        state.EntityManager.AddComponentData<RotateSpeed>(blueCube, new RotateSpeed
        {
            rotateSpeed = Unity.Mathematics.math.radians(generator.rotateSpeed)
        });

        // redCube 的 RotateSpeed 组件  EnableableComponent false
        
        // if(totalCubes > 5){
        //     state.EntityManager.SetComponentEnabled<RotateSpeed>(redCube, false);
        // }
        



        // var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            // state.EntityManager.SetComponentData(cube, LocalTransform.FromPosition(randomPosition));

        var redCubeTransform = SystemAPI.GetComponentRW<LocalTransform>(redCube);
        redCubeTransform.ValueRW.Position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));;
        
        var blueCubeTransform = SystemAPI.GetComponentRW<LocalTransform>(blueCube);
        blueCubeTransform.ValueRW.Position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));;
        totalCubes += 1;

    }
}





[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(EnableableComponentSceneSystem))]
[UpdateAfter(typeof(EnableableComponentSystem))]
// [BurstCompile]
public partial struct  EnableableComponentRotatePrefabSystem : ISystem{

    EntityQuery rotateCubes; // 获取查询
    ComponentTypeHandle<LocalTransform> transformTypeHandle;
    ComponentTypeHandle<RotateSpeed> rotateSpeedTypeHandle;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CubesGenerator>();

        // // 获取 句柄 
        transformTypeHandle = state.GetComponentTypeHandle<LocalTransform>();


        //  WithOptions(EntityQueryOptions.IgnoreComponentEnabledState); 可以过滤到已经  被禁用的组件
        var queryBuilder = new EntityQueryBuilder(Allocator.Temp)
        .WithAll<LocalTransform, SharingGroup, RotateSpeed>()
        .WithOptions(EntityQueryOptions.IgnoreComponentEnabledState);
        
        rotateCubes = state.GetEntityQuery(queryBuilder);

        rotateSpeedTypeHandle = state.GetComponentTypeHandle<RotateSpeed>();

        

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // // 第一种 IQuery
        // foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>())
        // {
        //     transform.ValueRW = transform.ValueRO.RotateY(2 * SystemAPI.Time.DeltaTime);
        // }

        rotateCubes.SetSharedComponentFilter(new SharingGroup { group = 0 });

        var cubeEntities = rotateCubes.ToEntityArray(Allocator.Temp);
        // 注释： cubeEntities 是 一个数组，包含了所有满足查询条件的实体
        Debug.Log(cubeEntities.Length);


        transformTypeHandle.Update(ref state);
        rotateSpeedTypeHandle.Update(ref state);


        // 第四种 IJobChunk
        var job = new EnableableComponentIJobChunk
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            TransformTypeHandle = transformTypeHandle,
            rotateSpeedTypeHandle = rotateSpeedTypeHandle
        };


        // SystemState.Dependency存储了当前系统以来的jobhandler，最好等待其完成在执行本系统的内容，同时如果本系统执行完毕，返回之前，如果有希望后面的系统以来的jobhandler，则可以赋值给SystemState.Dependency，这样系统就能够有一个安全可靠的依赖关系，顺序执行，当然这不是必须的，需要使用者自行判断。
        // state.Dependency = job.ScheduleParallel(rotateCubes, state.Dependency);
        job.ScheduleParallel(rotateCubes, state.Dependency).Complete();


    }
}



partial struct EnableableComponentIJobChunk : IJobChunk
{
    public float deltaTime;
    public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
    public ComponentTypeHandle<RotateSpeed> rotateSpeedTypeHandle;
    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask){
        // 获取chunk内存块里面所有的对应的组件的数据放到NativeArray里面给Execute迭代使用
        var chunkTransforms =  chunk.GetNativeArray(ref TransformTypeHandle);
        // var chunkRotateSpeeds = chunk.GetNativeArray(ref rotateSpeedTypeHandle);


        //  enumerator 是一个迭代器 ，用来遍历chunk里面的每个entity的Component ; chunk.Count 是chunk里面entity的数量
        var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        while (enumerator.NextEntityIndex(out var i))
        {

            bool enabled = chunk.IsComponentEnabled(ref rotateSpeedTypeHandle, i);

           if (chunkTransforms[i].Position.x > 5)
            {
                chunk.SetComponentEnabled(ref rotateSpeedTypeHandle, i, false);
            }


            if(enabled){
                chunkTransforms[i] = chunkTransforms[i].RotateY(3 * deltaTime);
            }

            // chunkTransforms[i] = chunkTransforms[i].RotateY(3 * deltaTime);



        }
    }


}



