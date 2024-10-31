using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;



[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(rvoPrefabSceneSystem))]
[BurstCompile]
public partial struct rvoMainPrefabSystem : ISystem, ISystemStartStop
{

    [BurstCompile]
    public void OnCreate(ref SystemState state){
        state.RequireForUpdate<rvoPrefabConfig>();

        state.EntityManager.AddComponentData(state.SystemHandle, new rvoInputEventBridge()
        {
            handler = rvoSingleMethod.getInstance()
        });

    }
    [BurstCompile]
    public void OnStartRunning(ref SystemState state)
    {
        Debug.Log("rvoMainPrefabSystem OnStartRunning");


    }
    [BurstCompile]
    public void OnStopRunning(ref SystemState state)
    {
        Debug.Log("rvoMainPrefabSystem OnStopRunning");
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        state.Enabled = false;
        int number = 10; 
        // Debug.Log("rvoMainPrefabSystem OnUpdate");
        var PrefabConfigComponent = SystemAPI.GetSingleton<rvoPrefabConfig>();
        var cubes = CollectionHelper.CreateNativeArray<Entity>(number, Allocator.Temp);
        state.EntityManager.Instantiate(PrefabConfigComponent.PrefabEntity, cubes);
        int count = 0;

        var inputEventBridge = state.EntityManager.GetComponentData<rvoInputEventBridge>(state.SystemHandle);

        foreach (var cube in cubes)
        {
            var randomPosition = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            LocalTransform lT = new LocalTransform(){
                Position = randomPosition,
                Rotation = Quaternion.identity,
                Scale = 1
            };
            // Debug.Log("add cube " + cube);
            var ag = RVOWapper.GetInstance().AddAgent(randomPosition, 1.0f, 1.2f);
            if (inputEventBridge.handler != null)
            {

                if(inputEventBridge.handler.dic.ContainsKey(count)){
                    inputEventBridge.handler.dic.Remove(count);
                }
                inputEventBridge.handler.dic.Add(count, ag);
            }

            state.EntityManager.AddComponentData(cube, new AgentComponent(){
                seed = 20.0f,
                targetPos = randomPosition,
                id = count,
            });

            state.EntityManager.SetComponentData(cube, lT);
            count++;
        }
        
    }
}

