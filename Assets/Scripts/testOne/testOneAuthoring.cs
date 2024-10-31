using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public struct testOneComponent  : IComponentData
{
    public int id;
}


public class testOneAuthoring : MonoBehaviour
{
    public int id = 1;
    class Baker : Baker<testOneAuthoring>
    {
        public override void Bake(testOneAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new testOneComponent
            {
                id = authoring.id,
            });
        }
    }
}
