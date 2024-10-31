using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nebukam.ORCA;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using Random = UnityEngine.Random;

public class Charactor : MonoBehaviour
{
    private IAgent agent;
    private float seed;
    private Vector3 targetPos;// 目标点
    public void Init(Vector3 pos, float seed){
        // Debug.Log("init ==", RVOWapper.GetInstance());
        this.agent = RVOWapper.GetInstance().AddAgent(pos, 1.0f, 1.2f);
        this.seed = seed;
        this.gameObject.transform.position = pos;

        this.targetPos = pos ;
    }

    public void NavTo(Vector3 targetPos){
        this.targetPos = targetPos;
    }

    public void Update(){
        Vector3 pos = this.agent.pos;
        Vector3 dir = this.targetPos - pos;
        if(dir.magnitude < float.Epsilon){
            this.agent.pos = this.targetPos;
            this.agent.prefVelocity = 0.0f;// 理想速度
            return;
        } else{
            // 算出 速度
            
            float mspd = this.seed; // 移动速度
            float3 tr = this.targetPos;
            float s = math.min(1f, distance(agent.pos, tr) / mspd);
            float agentSpeed = mspd * s;
            this.agent.maxSpeed = agentSpeed * s;
            this.agent.prefVelocity = normalize(dir) * agentSpeed;

        }
    }

    public void LateUpdate(){
        this.gameObject.transform.position = this.agent.pos;
    }
}
