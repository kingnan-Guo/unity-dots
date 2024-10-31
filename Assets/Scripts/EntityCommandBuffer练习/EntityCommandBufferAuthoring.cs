using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;




public struct EntityCommandBufferComponent  : IComponentData
{
    public int count;
    // public Entity prefabEntity;
    public Entity cubeProtoType;
    public bool useScheduleParallel;
}



public class EntityCommandBufferAuthoring : MonoBehaviour
{
    public GameObject redCubePrefab = null;
    public GameObject blueCubePrefab = null;
    public bool useScheduleParallel = true;
    [Range(2, 10000)] public int count = 10;
    class Baker : Baker<EntityCommandBufferAuthoring>
    {
        public override void Bake(EntityCommandBufferAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntityCommandBufferComponent
            {
                // cubeProtoType = authoring.useScheduleParallel ? GetEntity(authoring.redCubePrefab, TransformUsageFlags.Dynamic) : GetEntity(authoring.blueCubePrefab, TransformUsageFlags.Dynamic),
                 cubeProtoType = GetEntity(authoring.blueCubePrefab, TransformUsageFlags.Dynamic),
                useScheduleParallel = authoring.useScheduleParallel,
                count = authoring.count
            });
        }
    }
}

public partial class EntityCommandBufferSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "EntityCommandBufferScene";
}




[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(EntityCommandBufferSceneSystem))]
[BurstCompile]
public partial struct EntityCommandBufferSystem : ISystem
{
    private int totalCubes;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("EntityCommandBufferSystem OnCreate");
        state.RequireForUpdate<EntityCommandBufferComponent>();
        totalCubes = 0;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Debug.Log("EntityCommandBufferSystem OnUpdate");
        var generator = SystemAPI.GetSingleton<EntityCommandBufferComponent>();
        if(totalCubes > generator.count){
            state.Enabled = false;
            return;
        }



        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
        EntityCommandBuffer.ParallelWriter ecbParallel = ecb.AsParallelWriter();

        var cubes = CollectionHelper.CreateNativeArray<Entity>(1, Allocator.TempJob);

                var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));


        {
            


            var job = new testEntityCommandBufferJob
            {
                deltaTime = SystemAPI.Time.DeltaTime,
                ecbParallel = ecbParallel,
                cubeProtoType = generator.cubeProtoType,
                cubes = cubes,
                randomPosition = randomPosition
            };

            // state.Dependency = job.ScheduleParallel(state.Dependency);
            state.Dependency = job.ScheduleParallel(cubes.Length, 1, state.Dependency);


        }


        


        state.Dependency.Complete();
        ecb.Playback(state.EntityManager);
        totalCubes += cubes.Length;
        ecb.Dispose();


    }
}

partial struct testEntityCommandBufferJob : IJobFor
{
    public EntityCommandBuffer.ParallelWriter ecbParallel;
    public float deltaTime;
    public Entity cubeProtoType;
    public NativeArray<Entity> cubes;
    public Vector3 randomPosition;
    // public void Execute([ChunkIndexInQuery] int chunkIndex, Entity entity){
    // }

    // public void Execute(int index){
    //     cubes[index] = ecbParallel.Instantiate(index, cubeProtoType);

    //     // ecbParallel.AddComponent
    // }

    public void Execute(int index){
        // Debug.Log("testEntityCommandBufferJob  =  " +  index);
        cubes[index] = ecbParallel.Instantiate(index, cubeProtoType);


        ecbParallel.AddComponent(
            index, 
            cubes[index], 
            LocalTransform.FromPosition(randomPosition)
            // new Translation{Value = new float3(index, index, index)}
        );

        
    }



}






