// using System.Collections;
// using System.Collections.Generic;
// using Unity.Entities.UniversalDelegates;
// using UnityEngine;
using Nebukam.Common;
#if UNITY_EDITOR
using Nebukam.Common.Editor;
#endif
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using Random = UnityEngine.Random;
using Nebukam.ORCA;




public class RVOWapper : SingletonAutoMono<RVOWapper>
{
    private AgentGroup<Agent> agents;// 管理 所有的  Agent
    private ObstacleGroup obstacles;// 管理所有 静态障碍物
    private ObstacleGroup dynObstacles;//  管理所有 动态障碍物
    private RaycastGroup raycasts;// 管理所有 射线检测
    private ORCA simulation;// 把所有的 分组 都初始化到 simulation 中

    public AxisPair axis = AxisPair.XZ;



    [Header("Debug")]
    Color staticObstacleColor = Color.red;
    Color dynObstacleColor = Color.yellow;

    public void Awake(){

        
    }


    public void AddObstacleTo(){
        Obstacle o;

        List<float3> vList = new List<float3>();

        Vector3 pos = Vector3.zero;
        for (int i = 0; i < 4; i++)
        {
            pos.x = Random.Range(-10, 10);
            pos.y = 0;
            pos.z = Random.Range(-10, 10);
            vList.Add(pos);
        }
        o = obstacles.Add(vList, true);

        // float3 start = pos;



    }

    public IAgent AddAgent(Vector3 pos, float radius, float radiusObj){
        IAgent a = null;
        a = agents.Add(pos);
        a.radius = radius;
        a.radiusObst = radiusObj;
        a.prefVelocity = 0;
        return a;
    }

    // 静态 障碍物
    public Obstacle AddObstacle(Vector3 pos, List<Vector3> points){
        Obstacle obstacle = null;
        // obstacle = obstacles.Add(pos, points);
        return obstacle;
    }


 

    public void Init(){
        Debug.Log("Init RVO");

        agents = new AgentGroup<Agent>();

        obstacles = new ObstacleGroup();
        dynObstacles = new ObstacleGroup();
        raycasts = new RaycastGroup();

        simulation = new ORCA();
        simulation.plane = axis;
        simulation.agents = agents;
        simulation.staticObstacles = obstacles;
        simulation.dynamicObstacles = dynObstacles;
        simulation.raycasts = raycasts;

        // AddObstacleTo();
    }

    private void RVODebugDraw(){
#if UNITY_EDITOR

        #region update & draw agents

        IAgent agent;
        float3 agentPos;
        for (int i = 0, count = agents.Count; i < count; i++)
        {
            agent = agents[i] as IAgent;
            agentPos = agent.pos;

            //Agent body
            if (axis == AxisPair.XY)
            {
                Draw.Circle2D(agentPos, agent.radius, Color.green, 12);
                Draw.Circle2D(agentPos, agent.radiusObst, Color.cyan.A(0.15f), 12);
            }
            else
            {
                Draw.Circle(agentPos, agent.radius, Color.green, 12);
                Draw.Circle(agentPos, agent.radiusObst, Color.cyan.A(0.15f), 12);

            }
            //Agent simulated velocity (ORCA compliant)
            Draw.Line(agentPos, agentPos + (normalize(agent.velocity) * agent.radius), Color.green);
            //Agent goal vector
            Draw.Line(agentPos, agentPos + (normalize(agent.prefVelocity) * agent.radius), Color.grey);
        }
        #endregion






        #region draw obstacles

        //Draw static obstacles
        Obstacle o;
        int oCount = obstacles.Count, subCount;
        for (int i = 0; i < oCount; i++)
        {
            o = obstacles[i];
            subCount = o.Count;

            //Draw each segment
            for (int j = 1, count = o.Count; j < count; j++)
            {
                Draw.Line(o[j - 1].pos, o[j].pos, staticObstacleColor);
                Draw.Circle(o[j - 1].pos, 0.2f, Color.magenta, 6);
            }
            //Draw closing segment (simulation consider 2+ segments to be closed.)
            if (!o.edge)
                Draw.Line(o[subCount - 1].pos, o[0].pos, staticObstacleColor);
        }

        float delta = Time.deltaTime * 50f;

        //Draw dynamic obstacles
        oCount = dynObstacles.Count;
        for (int i = 0; i < oCount; i++)
        {
            o = dynObstacles[i];
            subCount = o.Count;

            //Draw each segment
            for (int j = 1, count = o.Count; j < count; j++)
            {
                Draw.Line(o[j - 1].pos, o[j].pos, dynObstacleColor);
            }
            //Draw closing segment (simulation consider 2+ segments to be closed.)
            if (!o.edge)
                Draw.Line(o[subCount - 1].pos, o[0].pos, dynObstacleColor);

        }

        #endregion






#endif
    }

    public void Update(){
        // Debug.Log("Update RVO == " + simulation);
        if(simulation != null){
            simulation.Schedule(Time.deltaTime);
            if (simulation.TryComplete())
            {
            }
            this.RVODebugDraw();
        }

    }

    // public void LateUpdate(){
    // }

    public void Exit(){
        simulation.DisposeAll();
    }
}
