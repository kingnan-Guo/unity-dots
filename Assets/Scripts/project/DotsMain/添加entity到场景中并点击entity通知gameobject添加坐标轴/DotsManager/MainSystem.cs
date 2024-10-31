using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

// [DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(PrefabSceneSystem))]
[BurstCompile]
public partial struct MainSystem : ISystem, ISystemStartStop
{
    
    // [BurstCompile]
    // public GameMainManager gameMainManager;
    // public JsonData jsonData;
    // public int dataLength;
    [BurstCompile]
    public void OnCreate(ref SystemState state){
        // state.RequireForUpdate<AssetsReferences>();

        // state.EntityManager.AddComponentData(state.SystemHandle, new UIEventBridge()
        // {
        //     handler = Object.FindObjectOfType<GameMainManager>()
        // });

        state.RequireForUpdate<PrefabConfig>();
        Debug.Log("MainSystem OnCreate");
    }
    [BurstCompile]
    public void OnStartRunning(ref SystemState state)
    {
        Debug.Log("MainSystem OnStartRunning");


        // dataLength = Object.FindObjectOfType<GameMainManager>().jsonData.data.Count();
        // Debug.Log("gameMainManager dataLength== " + dataLength);
        // if(jsonData){
        //     Debug.Log("gameMainManager.jsonData OnUpdate== " + jsonData.data.Count());
        // }

    }
    [BurstCompile]
    public void OnStopRunning(ref SystemState state)
    {
        Debug.Log("MainSystem OnStopRunning");
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        
        state.Enabled = false;
        JsonData jsonData = Object.FindObjectOfType<GameMainManager>().jsonData;
        Debug.Log("gameMainManager dataLength== " + jsonData.data.Count());
        // if(jsonData){
        //     Debug.Log("gameMainManager.jsonData OnUpdate== " + jsonData.data.Count());
        // }
        
        int number = jsonData.data.Count();

        // number = jsonData.data.Count();

        number = 20000;

        Debug.Log("MainSystem OnUpdate");
        // TryGetSingleton
        var PrefabConfigComponent = SystemAPI.GetSingleton<PrefabConfig>();
        var cubes = CollectionHelper.CreateNativeArray<Entity>(number, Allocator.Temp);
        state.EntityManager.Instantiate(PrefabConfigComponent.PrefabEntity, cubes);
        int count = 0;
        foreach (var cube in cubes)
        {
            // Debug.Log("count == " + count);
            // Debug.Log("jsonData Vector3 == " + jsonData.data[count].positionX + " " +(float)jsonData.data[count].positionY + " " +(float)jsonData.data[count].positionZ);


            // Debug.Log("jsonData Vector3 == "  + (jsonData.data)[count].positionX);
            state.EntityManager.AddComponentData(cube, new Config(){
                id = count,
                parentsId = count > 5 ? 1 : 2
            });

            state.EntityManager.AddSharedComponent<SharingGroup>(cube, new SharingGroup { 
                isActive = 0, 
            });
            var randomPosition = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

            // var randomPosition = new Vector3((float)jsonData.data[count].positionX, (float)jsonData.data[count].positionY, (float)jsonData.data[count].positionZ);
            // state.EntityManager.SetComponentData(cube,)
            // 缩小 0.1
            // state.EntityManager.SetComponentData(cube, new NonUniformScale { Value = new Vector3(0.1f, 0.1f, 0.1f) });
            
            // LocalTransform.FromPosition(randomPosition);
            LocalTransform lT = new LocalTransform(){
                Position = randomPosition,
                Rotation = Quaternion.identity,
                Scale = 1
            };

            state.EntityManager.SetComponentData(cube, lT);
            count++;
        }
        
    }
}
