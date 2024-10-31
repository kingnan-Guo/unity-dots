using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;



public struct testComponentData : IComponentData
{
    public int id;
    // public string EntityName;
}

public class testAddAuthoring : MonoBehaviour
{

    public GameObject Prefab;
    public string EntityName;

    class Baker : Baker<testAddAuthoring>
    {
        public override void Bake(testAddAuthoring authoring)
        {
            // var prefabEntity = new EntityPrefabReference(authoring.Prefab);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new testComponentData
            {
                id = 1,
            });
        }
    }
}
