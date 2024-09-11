using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using Unity.Entities;
using UnityEngine;

public struct Target : IComponentData
{
    public Entity TargetEntity;
    public float MaxDistance;
}

public class TargetAuthoring : MonoBehaviour
{
    public GameObject TargetGameobject;
    public float MaxDistance = 10f;
}

public class TargetAuthoringBaker : Baker<TargetAuthoring>{
    public override void Bake(TargetAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        // AddComponent(entity, new Target
        // {
        //     TargetEntity = GetEntity(authoring.Targetprefab, TransformUsageFlags.Dynamic),
        //     MaxDistance = authoring.MaxDistance
        // });
        var component  = new Target{
            MaxDistance = authoring.MaxDistance,
            TargetEntity = GetEntity(authoring.TargetGameobject, TransformUsageFlags.Dynamic)
        };

        AddComponent<Target>(entity, component);

    }
}
