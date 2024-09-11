using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public struct MovingBody : IComponentData{
    public float Velocity;
}

public class MoveingBodyAthoring : MonoBehaviour
{
    public float Velocity;
}

class MoveingBodyAthoringBaker : Baker<MoveingBodyAthoring>{
    public override void Bake(MoveingBodyAthoring authoring){
        var entity = this.GetEntity(TransformUsageFlags.Dynamic);
        var component = new MovingBody{
            Velocity = authoring.Velocity
        };
        this.AddComponent<MovingBody>(entity, component);
    }
}

public partial struct MovingBodySystem : ISystem
{

    public void OnUpdate(ref SystemState state){

        foreach (var (target, transform, moving, velocity) in SystemAPI.Query<
            RefRO<Target>, 
            RefRO<LocalTransform>, 
            RefRO<MovingBody>, 
            RefRW<PhysicsVelocity>
        >().WithAll<MovingBody>()){

            var targetPosition  = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.TargetEntity).Position;
            var direction = math.normalize(targetPosition - transform.ValueRO.Position);

            if(math.distance(transform.ValueRO.Position, targetPosition) < target.ValueRO.MaxDistance){
                velocity.ValueRW.Linear = moving.ValueRO.Velocity * direction;
            } else {
                velocity.ValueRW.Linear = float3.zero;
            }



        }
    }

}


    // public void OnUpdate(ref SystemState state){
    //     foreach (var (target, transform, moving, velocity) in SystemAPI.Query<RefRO<Target>, RefRO<LocalTransform>, RefRO<MovingBody>, RefRW<PhysicsVelocity>>().WithAll<MovingBody>()){
    //     {
    //         var targetPosition  = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.TargetEntity).Position;
    //         // 计算方向
    //         var direction = math.normalize(targetPosition - transform.ValueRO.Position);

    //         if(math.distance(transform.ValueRO.Position, targetPosition) < target.ValueRO.MaxDistance){
    //             velocity.ValueRW.Linear = moving.ValueRO.Velocity * direction;
    //         } else {
    //             velocity.ValueRW.Linear = float3.zero;
    //         }
    //     }
    // }