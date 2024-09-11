using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

using LocalTransform = Unity.Transforms.LocalTransform;


public class testAuthoring : MonoBehaviour
{
    public float DegreesPerSecond = 9.0f;
}


// 定义 ECS 的 component
public struct RotationSpeed : IComponentData
{
    public float RadiansPerSecond;
}


public struct Additional : IComponentData
{
    public float SomeValue;
}

public struct printComponentData : IComponentData
{
    public float printData;
}

// 定义一个 baker 类
public class SimpleBaker : Baker<testAuthoring>
{
    public override void Bake(testAuthoring authoring)
    {
        // Debug.Log("testAuthoring Bake =1= "+ authoring.DegreesPerSecond);
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        // Debug.Log("testAuthoring Bake =2= "+ authoring.DegreesPerSecond);
        AddComponent(entity, new RotationSpeed
        {
            RadiansPerSecond = authoring.DegreesPerSecond //math.radians(authoring.DegreesPerSecond)
        });
        AddComponent(entity, new printComponentData { printData = authoring.DegreesPerSecond });
        var additionalA = CreateAdditionalEntity(TransformUsageFlags.Dynamic, entityName: "Additional A");
        // var additionalB = CreateAdditionalEntity(TransformUsageFlags.Dynamic, entityName: "Additional B");

        AddComponent(additionalA, new Additional { SomeValue = 123 });
        // AddComponent(additionalB, new Additional { SomeValue = 234 });

        // AddComponent(entity, new Additional { SomeValue = 123 });
    }
}


public partial struct MyRotationSpeedSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        // Debug.Log("MyRotationSpeedSystem OnUpdate");
        foreach (var (transform, speed, pri) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>, RefRW<printComponentData>>()){
            // Debug.Log("MyRotationSpeedSystem foreach");
            transform.ValueRW = transform.ValueRO.RotateY(speed.ValueRO.RadiansPerSecond * deltaTime * 0.1f);
            // transform.ValueRW.Position = Vector3.one * deltaTime;
            // Debug.Log("MyRotationSpeedSystem speed == " + speed.ValueRO.RadiansPerSecond.ToString() );
            // Debug.Log("MyRotationSpeedSystem  printComponentData == " + pri.ValueRW.printData.ToString());
        }
    }
}


