using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(PrefabSceneSystem))]
[UpdateInGroup(typeof(InitializationSystemGroup))]
// [UpdateAfter(typeof(SceneSystemGroup))]
[BurstCompile]
public partial struct changeLocalTransformSystem : ISystem, ISystemStartStop
{
    public void OnCreate(ref SystemState state){
        // state.RequireForUpdate<PrefabConfig>();
        Debug.Log("changeLocalTransformSystem OnCreate");
    }
    public void OnStartRunning(ref SystemState state)
    {
        Debug.Log("changeLocalTransformSystem OnStartRunning");

    }
    public void OnStopRunning(ref SystemState state)
    {
        Debug.Log("changeLocalTransformSystem OnStopRunning");
    }
    public void OnUpdate(ref SystemState state){

        Debug.Log("changeLocalTransformSystem OnUpdate");
        
        // state.Enabled = false;
    }
}

