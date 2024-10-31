using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public struct testPrefabComponent  : IComponentData
{
    // public Mesh mesh;
    // public Material material;
    public int count;
    public Entity prefabEntity;
}

// public class testPrefabAuthoring : MonoBehaviour
// {

//     public Mesh mesh;
//     public Material material;


// }

public struct testPrefabInfoComponent : IComponentData
{
    public int id;
    // public string name;
}

public struct testLookupComponent : IComponentData
{
    public int code;
}


public class testPrefabAuthoring : MonoBehaviour
{    
    // public Mesh mesh;
    // public Material material;
    public GameObject prefab;
    public int count;
    class Baker : Baker<testPrefabAuthoring>
    {
        public override void Bake(testPrefabAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new testPrefabComponent
            {
                prefabEntity = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                count = authoring.count
            });
            AddComponent(entity, new testLookupComponent{
                code = 123
            });
        }
    }
}



// 
public partial class testPrefabSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "testPrefabScene";
}



[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(testPrefabSceneSystem))]
[BurstCompile]
public partial struct testPrefabSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("testPrefabSystem OnCreate");
        state.RequireForUpdate<testPrefabComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        // Debug.Log("testPrefabSystem OnUpdate");
        state.Enabled = false;



        // 预制体实例化 Entity 第一种方法 ===================
        
        // var testPrefabComponentEntity = SystemAPI.GetSingletonEntity<testPrefabComponent>();
        // // Debug.Log("testPrefabComponent id == " + testPrefabComponentEntity);
        // // var testPrefabComponentEntity = SystemAPI.GetSingletonEntity<testPrefabComponent>();
        // var testPrefabComponent = SystemAPI.GetComponent<testPrefabComponent>(testPrefabComponentEntity);

        // Debug.Log("testPrefabComponent count == " + testPrefabComponent.count);

        // for (int i = 0; i < testPrefabComponent.count; i++){
        //      var entity = state.EntityManager.Instantiate(testPrefabComponent.prefabEntity);

        //      var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        //      state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(randomPosition));
        // }


        // 预制体实例化 Entity  第二种方法 ===================

        var testPrefabComponentEntity = SystemAPI.GetSingleton<testPrefabComponent>();
        var cubes = CollectionHelper.CreateNativeArray<Entity>(testPrefabComponentEntity.count, Allocator.Temp);

        state.EntityManager.Instantiate(testPrefabComponentEntity.prefabEntity, cubes);
        int count = 0;

        foreach (var cube in cubes)
        {
            state.EntityManager.AddComponentData(cube, new testPrefabInfoComponent(){
                id = count
            });
            var randomPosition = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
            state.EntityManager.SetComponentData(cube, LocalTransform.FromPosition(randomPosition));
            // var transform = state.EntityManager.GetComponentData<LocalTransform>(cube);
            count++;
        }

        //  第三种方法 ===================
        // var testPrefabComponentEntity = SystemAPI.GetSingletonEntity<testPrefabComponent>();
        // var prefabLoadResult = SystemAPI.GetComponent<testPrefabComponent>(testPrefabComponentEntity);
        // foreach (var data in SystemAPI.Query<RefRW<testPrefabComponent>>())
        // {
        //     Debug.Log("testPrefabComponent id == " + data.ValueRW.count);
        //     for (int i = 0; i < 10; i++)
        //     {
        //         var entity = state.EntityManager.Instantiate(prefabLoadResult.prefabEntity);
        //         // var sphereCollider = new PhysicsCollider
        //         // {
        //         //     Value = Unity.Physics.SphereCollider.Create(new SphereGeometry
        //         //     {
        //         //         Center = Unity.Mathematics.float3.zero,
        //         //         Radius = 0.5f
        //         //     })
        //         // };
        //         // state.EntityManager.AddComponentData(entity, sphereCollider);
        //         var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        //         state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(randomPosition));
        //     }
        // }
    }
}

[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(testPrefabSceneSystem))]
[BurstCompile]
public partial struct testRotatePrefabSystem : ISystem{

    EntityQuery rotateCubes; // 获取查询
    ComponentTypeHandle<LocalTransform> transformTypeHandle;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<testPrefabComponent>();

        // 获取 句柄 
        transformTypeHandle = state.GetComponentTypeHandle<LocalTransform>();

        var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform>();
        rotateCubes = state.GetEntityQuery(queryBuilder);

        

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 第一种 IQuery
        // foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>())
        // {
        //     transform.ValueRW = transform.ValueRO.RotateY(2 * SystemAPI.Time.DeltaTime);
        // }

        // 第二种  IASpect
        // foreach (var aspect in SystemAPI.Query<testPrefabASpect>())
        // {
        //     aspect.Rotate(SystemAPI.Time.DeltaTime);
        //     aspect.move(SystemAPI.Time.ElapsedTime);
        // }


        // 第三种 IJobEntity
        // var job = new RotatePrefabWithJobEntity { deltaTime = SystemAPI.Time.DeltaTime };
        // job.ScheduleParallel();


        transformTypeHandle.Update(ref state);


        // 第四种 IJobChunk
        var job = new testIJobChunk
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            TransformTypeHandle = transformTypeHandle,
        };


        // SystemState.Dependency存储了当前系统以来的jobhandler，最好等待其完成在执行本系统的内容，同时如果本系统执行完毕，返回之前，如果有希望后面的系统以来的jobhandler，则可以赋值给SystemState.Dependency，这样系统就能够有一个安全可靠的依赖关系，顺序执行，当然这不是必须的，需要使用者自行判断。
        // state.Dependency = job.ScheduleParallel(rotateCubes, state.Dependency);
        job.ScheduleParallel(rotateCubes, state.Dependency).Complete();


    }
}


/// <summary>
/// 使用 IJobEntity 实现 旋转
/// </summary>
partial struct RotatePrefabWithJobEntity : IJobEntity{
    public float deltaTime;
    public void Execute(ref LocalTransform  transform)
    {
        transform = transform.RotateY(2 * deltaTime);
        // transform.ValueRW = transform.ValueRO.RotateY(2 * SystemAPI.Time.DeltaTime);
        // quaternion.Value = math.mul(quaternion.Value, quaternion.Value * deltaTime);
    }
}






/// <summary>
/// https://www.bilibili.com/read/cv36019837/?jump_opus=1 文档
/// 
/// 使用IjobChunk的主要流程 : 在DOTS中迭代处理Entity的组件数据先找到对应符合条件的ArcheType,然后再找到ArcheType对应的所有的Chunks，再遍历每个Chunk里面的每个Entity的组件数据。有了这个思路以后，我们先来看下如何来使用IJobChunk机制来进行数据迭代 
/// 一 : 
/// 1. 定义一个EntityQuery对象，它根据 筛选条件 而生成，当IJobChunk来筛选Entity的时候就基于这个对象的筛选条件。
/// 2. EntityQuery对象是基于EntityQueryBuilder对象创建出来。EntityQueryBuilder负责将筛选条件转成最后的EntityQuery对象
/// 
///         var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform>();
///         EntityQuery rotateCubes = state.GetEntityQuery(queryBuilder);
/// 
/// 
/// 二: 
/// 1 . IJobChunk迭代处理数据的时候都是一个一个Chunk来处理的，每个Chunk调用一次Execute函数。在Execute函数中你需要什么样的数据就可以定义在Job的结构体里面。
/// 2 . 定义在Job中Execute中所使用的数据,这些数据在System Update迭代的时候传入。IJobChunk迭代处理数据的时候都是一个一个Chunk来处理的，每个Chunk调用一次Execute函数。在Execute函数中你需要什么样的数据就可以定义在Job的结构体里面。
/// 3 . 利用 ComponentTypeHandle 可以把chunk内存块里面所有的对应的组件的数据放到NativeArray里面给Execute迭代使用。普通的数据可以直接定义即可
/// 
/// 4 .  实现Interface IjobChunk的Execute函数的具体迭代逻辑，用于具体的处理。处理组件数据的时候，基于ComponentTypeHandle把Chunk里面的组件数据获取到一个NativeArray里面。Execute有4个参数:
///         chunk,类型是ArcheTypeChunk,就是我们的chunk内存块，存放数据地方;
///         unfilteredChunkIndex: 我们当前chunk,所在所有chunk的索引; 
///         
///         bool useEnabledMask, in v128 chunkEnabledMask, 是 enableable component的bitmap,给我们查询使用。
///  
/// 5 . 在Execute里面我们使用 ChunkEntityEnumerator 来遍历chunk里面的每个entity的Component
/// 
/// 
/// 
/// </summary>
partial struct testIJobChunk : IJobChunk{
    public float deltaTime;
    public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask){
        // 获取chunk内存块里面所有的对应的组件的数据放到NativeArray里面给Execute迭代使用
        var chunkTransforms =  chunk.GetNativeArray(ref TransformTypeHandle);

        //  enumerator 是一个迭代器 ，用来遍历chunk里面的每个entity的Component ; chunk.Count 是chunk里面entity的数量
        var enumerator = new ChunkEntityEnumerator(useEnabledMask, chunkEnabledMask, chunk.Count);
        while (enumerator.NextEntityIndex(out var i))
        {
            chunkTransforms[i] = chunkTransforms[i].RotateY(3 * deltaTime);
        }
    }
}


// ========================================

/// <summary>
/// IAspect 只能 包含 Entity（对 Entity id 进行索引 ）  RefRW<T> RefRo<T> EnabledRefRO EnabledRefRW（用来访问组件状态）   DynamicBuffer<T> 和 其他 Aspect 类型
/// </summary>

readonly partial struct testPrefabASpect : IAspect
{
    readonly RefRW<LocalTransform> localTransform;

    public void Rotate(float deltaTime){
        localTransform.ValueRW = localTransform.ValueRO.RotateY(2 * deltaTime);
    }

    public void move(double deltaTime){
        localTransform.ValueRW.Position.y += (float)Unity.Mathematics.math.sin(deltaTime) * 0.1f;
    }
}




// ===========================================================================================
// entity  随机访问 ,在 任意时刻  通过 entity id 访问 entity 对应的 component 数据,
// entityManager 提供了这样的能力, 但是 entityManager 的访问方式是  非线程安全的 ,所以不能在 job 中使用 entityManager
// ComponentLookup 提供了线程安全的访问方式, 可以在 job 中使用


[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(testPrefabSceneSystem))]
[BurstCompile]
public partial struct testComponentLookupSystem : ISystem{

    private EntityQuery query;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<testPrefabComponent>();
        // // 用来查询 testPrefabComponent 的数据
        ComponentLookup<testPrefabComponent> testPrefabComponentLookup = SystemAPI.GetComponentLookup<testPrefabComponent>(true);

        
        // Debug.Log("testConfigComponentLookup " + testConfigComponentLookup);

        // IJobEntity


        var queryBuilder = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, testPrefabInfoComponent>();
        query = state.GetEntityQuery(queryBuilder);

        // query = this.GetEntityQuery(typeof(LocalTransform), ComponentType.ReadOnly<Target>());




    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        ComponentLookup<testPrefabInfoComponent> testPrefabInfoComponentLookup = SystemAPI.GetComponentLookup<testPrefabInfoComponent>(true);
        Debug.Log("testPrefabInfoComponentLookup ==" + testPrefabInfoComponentLookup);

        var job = new testConfigComponentLookupJob();

        job.deltaTime = SystemAPI.Time.DeltaTime;

        job.EntityPrefabInfoComponent = testPrefabInfoComponentLookup;


        ComponentLookup<testLookupComponent>  testLookupComponentArray= SystemAPI.GetComponentLookup<testLookupComponent>(true);

        Debug.Log("testLookupComponentArray ==" + testLookupComponentArray);
        // job.testLookupComponent = testLookupComponentArray;

        // job.ScheduleParallel(query, state.Dependency).Complete();



        // Entity e = state.EntityManager.CreateEntity(typeof(testLookupComponent));
        // Entity e = ;
        var e = SystemAPI.GetSingletonEntity<testLookupComponent>();

        
        Debug.Log("e ==" + testLookupComponentArray[e].code);

    }
}




// private partial struct testConfigComponentLookupJob : IJobEntity
// {

//     // // Read-only data stored (potentially) in other chunks
//     // #region lookup-ijobchunk-declare
//     // [ReadOnly]
//     // public ComponentLookup<LocalToWorld> EntityPositions;
//     // #endregion

//     // // Non-entity data
//     // public float deltaTime;

//     public void Execute(ref LocalTransform transform, in LocalToWorld entityPosition)
//     {
//         // // Get the target Entity object
//         // Entity targetEntity = target.entity;

//         // // Check that the target still exists
//         // if (!EntityPositions.HasComponent(targetEntity))
//         //     return;

//         // // Update translation to move the chasing entity toward the target
//         // float3 targetPosition = entityPosition.Position;
//         // float3 chaserPosition = transform.Position;

//         // float3 displacement = targetPosition - chaserPosition;
//         // transform.Position = chaserPosition + displacement * deltaTime;
//     }
// }


partial struct testConfigComponentLookupJob : IJobEntity{
    [ReadOnly]
    public ComponentLookup<testPrefabInfoComponent> EntityPrefabInfoComponent;
    [ReadOnly]
    public ComponentLookup<testLookupComponent> testLookupComponent;
    public float deltaTime;
    public void Execute(ref LocalTransform  transform, in testPrefabInfoComponent PrefabInfoComponent, Entity entity)
    {
        Debug.Log("testConfigComponentLookupJob PrefabInfoComponent == " + PrefabInfoComponent.id + " = testLookupComponent ==" + testLookupComponent);
    }
}





