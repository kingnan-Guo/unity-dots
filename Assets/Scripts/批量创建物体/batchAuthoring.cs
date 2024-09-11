using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor;
using UnityEngine;



public class batchAuthoring : MonoBehaviour
{
    public GameObject TargetGameobject;
}

public struct EntityData : IComponentData{
    public Entity TargetEntity;
    public int id;
}


public class batchAuthoringBaker : Baker<batchAuthoring>{
    public override void Bake(batchAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        var component  = new EntityData{
            TargetEntity = GetEntity(authoring.TargetGameobject, TransformUsageFlags.Dynamic)
        };
        AddComponent<EntityData>(entity, component);


    }
}

