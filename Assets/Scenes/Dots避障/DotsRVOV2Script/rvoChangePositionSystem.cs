using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Mathematics.math;


[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(rvoPrefabSceneSystem))]
[UpdateAfter(typeof(rvoMainPrefabSystem))]
[BurstCompile]
public partial struct rvoChangePositionSystem : ISystem, ISystemStartStop
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
        // Debug.Log("rvoMainPrefabSystem OnStartRunning");
    }

    [BurstCompile]
    public void OnStopRunning(ref SystemState state)
    {
        // Debug.Log("rvoMainPrefabSystem OnStopRunning");
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        // Debug.Log("rvoMainPrefabSystem OnUpdate" );
        var inputEventBridge = state.EntityManager.GetComponentData<rvoInputEventBridge>(state.SystemHandle);
        if (inputEventBridge.handler != null)
        {

            // Debug.Log("rvoMainPrefabSystem targetPos" +inputEventBridge.handler.curHit.point);
            RaycastHit raycastHit = inputEventBridge.handler.curHit;
            var targetPos = raycastHit.point;
            // Debug.Log("rvoMainPrefabSystem targetPos" +targetPos);
            foreach (var (transform, AgentData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<AgentComponent>>())
            {
                // Debug.Log("rvoMainPrefabSystem OnUpdate transform" + transform);
                
                // Debug.Log(AgentData.ValueRO.id + " === " +inputEventBridge.handler.dic[AgentData.ValueRO.id]);

                var  agent = inputEventBridge.handler.dic[AgentData.ValueRO.id];
                Vector3 pos = agent.pos;
                Vector3 dir = targetPos - pos;
                if(dir.magnitude < float.Epsilon){
                    agent.pos = targetPos;
                    agent.prefVelocity = 0.0f;// 理想速度
                    return;
                } else{
                    // 算出 速度
                    
                    float mspd = AgentData.ValueRO.seed; // 移动速度
                    float3 tr = targetPos;
                    float s = math.min(1f, distance(agent.pos, tr) / mspd);
                    float agentSpeed = mspd * s;
                    agent.maxSpeed = agentSpeed * s;
                    agent.prefVelocity = normalize(dir) * agentSpeed;
                }
                //  agent.pos float3 转 LocalTransform
                LocalTransform lT = new LocalTransform(){
                    Position = agent.pos,
                    Rotation = Quaternion.identity,
                    Scale = 1
                };
                transform.ValueRW = lT;
            }


            // Debug.Log("rvoMainPrefabSystem OnUpdate" + inputEventBridge.handler.dic.Count);
            // Debug.Log("rvoMainPrefabSystem OnUpdate" + inputEventBridge.handler.dic[0]);

            // foreach (var item in inputEventBridge.handler.dic){
            //     // Debug.Log("rvoMainPrefabSystem OnUpdate" + item.Key + " " + item.Value);
            // }

        }
    }
}
