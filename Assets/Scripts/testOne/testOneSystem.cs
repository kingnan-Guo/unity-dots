// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Scenes;
using UnityEngine;
// using DOTS.DOD;
// public class testOneSystem : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(testOneSystemGroup))]
    [BurstCompile]
    public partial struct testOneSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            Debug.Log("testOneSystem OnCreate");
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            Debug.Log("testOneSystem OnUpdate");
            // float deltaTime = SystemAPI.Time.DeltaTime;
            foreach (var data in SystemAPI.Query<RefRW<testOneComponent>>())
            {
                Debug.Log("testOneComponent id == " + data.ValueRW.id);
                data.ValueRW.id += 1;
                
            }
        }
    }

