using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;




[InternalBufferCapacity(8)]// 定义默认 8 个元素
struct WayPoint : IBufferElementData
{
    public float3 point;
}


public class DynamicBufferComponentAutoring : MonoBehaviour
{
    public List<Vector3> wayPoints;
    class Baker : Baker<DynamicBufferComponentAutoring>
    {
        public override void Bake(DynamicBufferComponentAutoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            DynamicBuffer<WayPoint> wayPoints = AddBuffer<WayPoint>(entity);
            // wayPoints.Add(new WayPoint { point = new float3(0, 0, 0) });
            wayPoints.Length = authoring.wayPoints.Count;
            for (int i = 0; i < authoring.wayPoints.Count; i++){
                wayPoints[i] = new WayPoint{
                    point = authoring.wayPoints[i]
                };
            }

        }
    }


    // private void Update()
    // {

    //     Debug.Log("mouse down");
    //     // if (Input.GetMouseButtonDown(0))
    //     // {
    //     //     Debug.Log("mouse down");
    //     //     Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     //     float3 newWayPoint = new float3(worldPos.x, worldPos.y, 0);
    //     //     if (wayPoints.Count > 0)
    //     //     {
    //     //         float mindist = float.MaxValue;
    //     //         int index = wayPoints.Count;
    //     //         for (int i = 0; i < wayPoints.Count; i++)
    //     //         {
    //     //             float dist = math.distance(wayPoints[i], newWayPoint);
    //     //             if (dist < mindist)
    //     //             {
    //     //                 mindist = dist;
    //     //                 index = i;
    //     //             }
    //     //         }

    //     //         wayPoints.Insert(index, new Vector3(newWayPoint.x, newWayPoint.y, newWayPoint.z));
    //     //     }
    //     // }
    // }



}

public partial class DynamicBufferComponentSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "DynamicBufferComponentScene";
}



[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(DynamicBufferComponentSceneSystem))]
[BurstCompile]
public partial struct DynamicBufferSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        Debug.Log("DynamicBufferSystem OnCreate");
        state.RequireForUpdate<WayPoint>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Debug.Log("DynamicBufferSystem OnUpdate");、
            if (Input.GetMouseButtonDown(0))
            {
                DynamicBuffer<WayPoint> path = SystemAPI.GetSingletonBuffer<WayPoint>();
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("mouse down" + worldPos.x + " " + worldPos.y + " "+ worldPos.z);
                float3 newWayPoint = new float3(worldPos.x, worldPos.y, 0);
                if (path.Length > 0)
                {
                    float mindist = float.MaxValue;
                    int index = path.Length;
                    for (int i = 0; i < path.Length; i++)
                    {
                        float dist = math.distance(path[i].point, newWayPoint);
                        if (dist < mindist)
                        {
                            mindist = dist;
                            index = i;
                        }
                    }
                    path.Insert(index, new WayPoint(){ point = newWayPoint });
                }
            }

            // Debug.Log("path count:" + SystemAPI.GetSingletonBuffer<WayPoint>().Length);



    }
}





// IBufferElementData 动态大小的  buffer

