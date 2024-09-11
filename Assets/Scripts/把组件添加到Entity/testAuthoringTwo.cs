// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class testAuthoringTwo : MonoBehaviour
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


// using Unity.Entities;
// using Unity.Collections;
// using Unity.Burst;
// using Unity.Mathematics;
// using UnityEngine;
// public class EntityGenerator : MonoBehaviour
// {
//     private EntityManager entityManager;
//     void Start()
//     {
//         // 创建一个新的World（通常在你的应用程序中只有一个或少数几个World）
//         var world = World.CreateDefaultWorld(new WorldSettings
//         {
//             // 你可以在这里设置World的选项
//         });
//         entityManager = world.EntityManager;
//         // 创建一个NativeArray来存储EntityArrayHandle，稍后用于批量添加组件
//         var entities = new NativeArray<Entity>(1000, Allocator.TempJob);
//         // 创建一个EntityArchetype，它包含了你的实体需要的所有组件类型
//         var archetype = entityManager.CreateArchetype(typeof(PositionComponent));
//         // 使用EntityManager的Instantiate方法来创建实体
//         entityManager.Instantiate(archetype, entities);
//         // 现在你可以遍历entities数组，并给每个实体设置位置组件的值
//         for (int i = 0; i < entities.Length; i++)
//         {
//             entityManager.SetComponentData(entities[i], new PositionComponent { Value = new float3(i, 0, 0) });
//         }
//         // 不要忘记释放NativeArray的内存
//         entities.Dispose();
//     }
// }
