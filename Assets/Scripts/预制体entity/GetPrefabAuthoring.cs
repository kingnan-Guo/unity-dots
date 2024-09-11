using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Scenes;
using UnityEngine;


class GetPrefabAuthoring : MonoBehaviour{
    public GameObject prefab;
}
// // public class GetPrefabAuthoring : MonoBehaviour
// // {
// //     // Start is called before the first frame update
// //     void Start()
// //     {
        
// //     }

// //     // Update is called once per frame
// //     void Update()
// //     {
        
// //     }
// // }
// public struct EntityPrefabComponent : IComponentData
// {
//     public Entity Value;
// }

// public class GetPrefabAuthoring : MonoBehaviour
// {
//     public GameObject Prefab;
// }

// public class GetPrefabBaker : Baker<GetPrefabAuthoring>
// {
//     public override void Bake(GetPrefabAuthoring authoring)
//     {
//         // Register the Prefab in the Baker
//         var entityPrefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic);
//         // Add the Entity reference to a component for instantiation later
//         var entity = GetEntity(TransformUsageFlags.Dynamic);
//         AddComponent(entity, new EntityPrefabComponent() {Value = entityPrefab});
//     }
// }


// // public struct EntityPrefabReferenceComponent : IComponentData
// // {
// //     public EntityPrefabReference Value;
// // }

// // public class GetPrefabReferenceAuthoring : MonoBehaviour
// // {
// //     public GameObject Prefab;
// // }

// // public class GetPrefabReferenceBaker : Baker<GetPrefabReferenceAuthoring>
// // {
// //     public override void Bake(GetPrefabReferenceAuthoring authoring)
// //     {
// //         // Create an EntityPrefabReference from a GameObject. This will allow the
// //         // serialization process to serialize the prefab in its own entity scene
// //         // file instead of duplicating the prefab ECS content everywhere it is used
// //         var entityPrefab = new EntityPrefabReference(authoring.Prefab);
// //         var entity = GetEntity(TransformUsageFlags.Dynamic);
// //         AddComponent(entity, new EntityPrefabReferenceComponent() {Value = entityPrefab});
// //     }
// // }

// // public partial struct InstantiatePrefabSystem : ISystem
// // {
// //     public void OnUpdate(ref SystemState state)
// //     {
// //         var ecb = new EntityCommandBuffer(Allocator.Temp);

// //         // Get all Entities that have the component with the Entity reference
// //         foreach (var prefab in
// //                  SystemAPI.Query<RefRO<EntityPrefabComponent>>())
// //         {
// //             // Instantiate the prefab Entity
// //             var instance = ecb.Instantiate(prefab.ValueRO.Value);
// //             // Note: the returned instance is only relevant when used in the ECB
// //             // as the entity is not created in the EntityManager until ECB.Playback
// //             ecb.AddComponent<ComponentA>(instance);
// //         }

// //         ecb.Playback(state.EntityManager);
// //         ecb.Dispose();
// //     }
// // }




// // public partial struct InstantiatePrefabSystem : ISystem
// // {
// //     public void OnUpdate(ref SystemState state)
// //     {
// //         Debug.Log("InstantiatePrefabSystem ===" );
// //         var ecb = new EntityCommandBuffer(Allocator.Temp);

// //         // Get all Entities that have the component with the Entity reference
// //         foreach (var prefab in SystemAPI.Query<RefRO<EntityPrefabComponent>>())
// //         {
// //             // 判断 prefab 是不是 Entity.Null
// //             if (prefab.ValueRO.Value != Entity.Null){
// //                 Debug.Log(prefab.ValueRO.Value);
// //                 // Instantiate the prefab Entity
// //                 var instance = ecb.Instantiate(prefab.ValueRO.Value);
// //                 // Note: the returned instance is only relevant when used in the ECB
// //                 // as the entity is not created in the EntityManager until ECB.Playback
// //                 ecb.AddComponent<EntityPrefabComponent>(instance);
// //             }

// //         }




// //         ecb.Playback(state.EntityManager);
// //         ecb.Dispose();
// //         // state.Enabled = false;
// //     }
// // }









