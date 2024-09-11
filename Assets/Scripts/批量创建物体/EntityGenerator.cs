using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct PositionComponent : IComponentData{
    public float3 Value;
}

public class EntityGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EntityGenerator Start");
        // 创建一个 world 
        var world = World.DefaultGameObjectInjectionWorld;
        // 获取 EntityManager
        var entityManager = world.EntityManager;
        // 创建一个 NativeArray 来存储 EntityArrayHandle，稍后用于批量添加组件
        var entities = new NativeArray<Entity>(1000, Allocator.TempJob);
        // 创建一个 EntityArchetype，它包含了你的实体需要的所有组件类型
        var archetype = entityManager.CreateArchetype(typeof(PositionComponent));
        // 使用 EntityManager 的 Instantiate 方法来创建实体
        // entityManager.Instantiate(archetype, entities);
        // entityManager.Instantiate();
        // 现在你可以遍历 entities 数组，并给每个实体设置位置组件的值
        for (int i = 0; i < entities.Length; i++)
        {
            // entityManager.SetComponentData(entities[i], new PositionComponent { Value = new float3(i, 0, 0) });

            // var additionalA = CreateAdditionalEntity(TransformUsageFlags.Dynamic, entityName: "Additional A");
            // // var additionalB = CreateAdditionalEntity(TransformUsageFlags.Dynamic, entityName: "Additional B");

            // AddComponent(additionalA, new Additional { SomeValue = 123 });


            // EntityManager.CreateEntity(typeof(Transform))

            // entityManager.SetComponentData(entities[i], new PositionComponent { Value = new float3(i, 0, 0) });
        }
        Debug.Log("entities.Length: " + entities.Length);
        // 不要忘记释放 NativeArray 的内存
        entities.Dispose();
    }


}


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

