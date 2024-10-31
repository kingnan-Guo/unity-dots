using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Scenes;
using UnityEngine;

public struct JobsConfig : IComponentData
{
    public int id;
    // public string EntityName;
}



public class jobTestAuthoring : MonoBehaviour
{
    public int id = 1;
    class Baker : Baker<jobTestAuthoring>
    {
        public override void Bake(jobTestAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new JobsConfig
            {
                id = authoring.id,
            });
        }
    }
}



// public class testOneSystemGroup : ComponentSystemGroup{}
// [DisableAutoCreation] // 控制是否自动执行
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class SceneSystemGroupTest : ComponentSystemGroup{
}

[UpdateInGroup(typeof(SceneSystemGroupTest))]
public partial class cubeSystemGroupOne : SceneSystemGroup
{
    protected override string SceneName => "jobSubScene";
    // protected override string SceneName => "jobSubScene123";
    
}



[UpdateInGroup(typeof(SceneSystemGroupTest))]

public partial class cubeSystemGroupTwo : SceneSystemGroup
{
    protected override string SceneName => "subScene";
    //  protected override string SceneName => "dddd";
    // protected override void OnCreate(){
    //     base.OnCreate();
    // }

    // [UpdateInGroup(typeof(cubeSystemGroupTwo))]
    // public partial struct MovingBodySystem : ISystem{
    //     public void OnUpdate(ref SystemState state){

    //         Debug.Log("subScene test");
    //     }
    // }
}


// [UpdateInGroup(typeof(cubeSystemGroupTwo))]
// public partial class LoadingSystem : SystemBase
// {
//     protected override void OnUpdate()
//     {
//         // 系统组中的加载逻辑
//     }
// }



[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(cubeSystemGroupOne))]
// [UpdateAfter(typeof(cubeSystemGroupOne))]
// [DisableAutoCreation]
// [RequireMatchingQueriesForUpdate]
public partial struct MovingBodySystemOne : ISystem{
    // public void OnCreate(ref SystemState state){
    //     Debug.Log("OnCreate  test");
    // }
    public void OnCreate(ref SystemState state)
    {
        // RequireForUpdate<>();
        Debug.Log("MovingBodySystemOne OnCreate");

       
        // state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        Debug.Log("MovingBodySystemOne  OnUpdate");

        

        //  =============

        // new testJobFor().Run(2);


        // JobHandle jobHandle0 = new JobHandle();
        // // new testJobFor().Schedule(2, jobHandle);
        // // 多线程
        // new testJobFor().Schedule(2, jobHandle0).Complete(); 
        // // new testJobFor().Schedule(2, default);

        // // 分批次 
        // new testJobFor().ScheduleParallel(100, 10, jobHandle0).Complete();





        // Debug.Log("testSystemBase  OnUpdate");

        // // var job = new testJob();
        // // job.Schedule();
        // new MyJob().Schedule();
        // int[] testArray = new int[]{1, 2, 3 };


        // NativeArray<int> nativeArrayA = new NativeArray<int>(testArray, Allocator.TempJob);
        // NativeArray<int> nativeArrayB = new NativeArray<int>(testArray, Allocator.TempJob);

        // Debug.Log(nativeArrayB[0]);

        // sumJob sumJobI = new sumJob(){
        //     arrayA = nativeArrayA,
        //     arrayB = nativeArrayB,
        //     index = 0
        // };
        // JobHandle jobHandle = sumJobI.Schedule();
        // jobHandle.Complete();
        
        // Debug.Log(nativeArrayB[0]);

        // // int l = 2;
        


        // nativeArrayA.Dispose();
        // nativeArrayB.Dispose();





        // =====
        new testIJobEntity().ScheduleParallel();

        // state.Enabled = false;



    }
}


[UpdateInGroup(typeof(cubeSystemGroupTwo))]
public partial class testSystemBase : SystemBase
{

    protected override void OnCreate()
    {
        // RequireForUpdate<cubeSystemGroupTwo>();
        base.OnCreate();





        Debug.Log("testSystemBase  OnCreate");
    }

    protected override void OnUpdate()
    {
 


    }
}


    struct MyJob : IJob
    {

        public void Execute()
        {
            Debug.Log("MyJob  OnUpdate");
        }
    }


    struct sumJob : IJob
    {
        public NativeArray<int> arrayA;
        public NativeArray<int> arrayB;
        public int index;
        public void Execute(){
            Debug.Log("sumJob  OnUpdate");
            arrayB[index] = arrayA[index] + 1;
        }
    }

struct testJobFor : IJobFor
{
    public void Execute(int index)
    {
        Debug.Log("testJobFor  OnUpdate" + index);
        // throw new System.NotImplementedException();
    }
}


partial struct testIJobEntity : IJobEntity{
    public void Execute(in JobsConfig JobsConfig){
        // Debug.Log("testIJobEntity  OnUpdate  " + e + "  jobsConfig  " + jobsConfig.id);
        Debug.Log("testIJobEntity  OnUpdate  " + JobsConfig.id);
    }
}


// partial struct testJob : IJobEntity{
    
//     void Execute(Entity e){
//         Debug.Log("testJob  OnUpdate");
//     }
// }

// public partial struct TestJob : IJobEntity
// {
//     public Entity managerEntity;
//     public EntityCommandBuffer ecb;

//     [BurstCompile]
//     void Execute(Entity entity, in ClickComponent c)
//     {
//         // TODO...
//         UnityEngine.Debug.Log("接收到按钮点击的消息");

//         ecb.DestroyEntity(entity);
//     }
// }

// [BurstCompile]
// public partial struct OtherSystem : ISystem
// {
//     void OnUpdate(ref SystemState state)
//     {
//         var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
//         EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
//         // Entity managerEntity = SystemAPI.GetSingletonEntity<CharacterManager>();

//         TestJob job = new TestJob()
//         {
//             ecb = ecb
//         };
//         job.Schedule();
//     }
// }





