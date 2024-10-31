using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

// public class MouseClickChangeMeshSystem : MonoBehaviour
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

// [RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(CreateCubeswithChangeMaterialAndMeshSceneSystem))]
public partial class MouseClickChangeMeshSystem : SystemBase
{
    private Camera _mainCamera;

    protected override void OnCreate()
    {
        base.OnCreate();

        Debug.Log("MouseClickChangeMeshSystem OnCreate");
        _mainCamera = Camera.main;
        RequireForUpdate<PhysicsWorldSingleton>();
    }

    protected override void OnUpdate()
    {
        // Debug.Log("MouseClickChangeMeshSystem OnUpdate");
        {
            
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            UnityEngine.RaycastHit[] hits = new UnityEngine.RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, hits, 1000) > 0){
                Debug.Log("getGameObject =="+hits[0].collider.name);
            }

        }
        // return;
        if (Input.GetMouseButtonDown(0) ) // 检测左键点击
        {

            Debug.Log("MouseClickChangeMeshSystem GetMouseButtonDown");
            UnityEngine.Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            // 获取 PhysicsWorldSingleton
            // var physicsWorldSingleton = GetSingleton<PhysicsWorldSingleton>();
            var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            
            CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

            RaycastInput raycastInput = new RaycastInput
            {
                Start = ray.origin,
                End = ray.origin + ray.direction * 1000f,
                Filter = CollisionFilter.Default
            };

            if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
            {

                Debug.Log($"点击位置: {hit.RigidBodyIndex}");
                // 获取点击的实体
                Entity clickedEntity = collisionWorld.Bodies[hit.RigidBodyIndex].Entity;

                // 检查实体是否有 LocalTransform 组件
                if (EntityManager.HasComponent<LocalTransform>(clickedEntity))
                {
                    LocalTransform transform = EntityManager.GetComponentData<LocalTransform>(clickedEntity);
                    Debug.Log($"点击的实体位置: {transform.Position}");
                    // 获取实体的名称
                    // testName entityName = EntityManager.GetComponentData<testName>(clickedEntity);
                    // Debug.Log($"点击的实体名称: {entityName.id}");


                    MaterialMeshInfo info = EntityManager.GetComponentData<MaterialMeshInfo>(clickedEntity);

                    Debug.Log($"点击的实体材质: {info}");
                }
            }
        }
    }
}


