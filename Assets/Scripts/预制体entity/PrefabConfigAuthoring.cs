using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
// using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Burst;
using UnityEngine.UI;
using System.ComponentModel;
using Unity.Rendering;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.Entities.Content;
// using Unity.Transforms;
// using UnityEngine;




public struct PrefabConfig : IComponentData
{
    public Entity PrefabEntity;
}

// class myCustomMeshAndMaterial : IComponentData
// {
//     // public WeakObjectReference<Mesh> Mesh;
//     public WeakObjectReference<Mesh> mesh;

//     // public UnityEngine.Material red;

// }


class myCustomMeshAndMaterial : IComponentData
{
    public Mesh mesh;
}


// [Unity.Entities.GenerateAuthoringComponent]
// public struct MeshData : IComponentData {
//     public Mesh Mesh;
//     // public Material Material;
// }


public struct testName : IComponentData{
    public  int id;
}
public class PrefabConfigAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    // public Mesh mesh;
    public Mesh mesh;

    // public UnityEngine.Material red;

    class Baker : Baker<PrefabConfigAuthoring>
    {
        public override void Bake(PrefabConfigAuthoring authoring)
        {
            var prefabEntity = GetEntity(authoring.Prefab, TransformUsageFlags.None);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PrefabConfig { PrefabEntity = prefabEntity});

            // var world = World.DefaultGameObjectInjectionWorld;
            // var entityManager = world.EntityManager;
            // entityManager.CreateEntity();
            // var prototype = entityManager.CreateEntity();
            // entityManager.AddComponentData(prototype, new testName { id = 1 });
            
            // entityManager.AddComponentData(entity, new MeshData { mesh = authoring.mesh });

            // AddComponent(entity, new MeshData { Mesh = authoring.mesh });

            // AddComponent(entity, new myCustomMeshAndMaterial { mesh = authoring.mesh });


            // AddComponent(entity, new myCustomMeshAndMaterial { mesh = authoring.mesh});



        }
    }
}



// using Unity.Entities;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

public partial struct SpawnEntitiesSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PrefabConfig>();


 
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<PrefabConfig>();
        var prefabEntity = config.PrefabEntity;

        for (int i = 0; i < 10; i++)
        {
            var instance = state.EntityManager.Instantiate(prefabEntity);
            var randomPosition = new float3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));


            // 添加新的组件 testName 
            // state.EntityManager.AddComponentData(instance, new testName { id = i });
            state.EntityManager.SetComponentData(instance, LocalTransform.FromPosition(randomPosition));
        }

        // 禁用系统，避免重复生成
        state.Enabled = false;
    }
}



// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using Unity.Mathematics;
// using UnityEngine;

// using Unity.Burst;
// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Mathematics;
// using Unity.Transforms;
// using UnityEngine;

// using Unity.Entities;
// using Unity.Physics;
// using Unity.Physics.Systems;
// using Unity.Transforms;
// using UnityEngine;


class CustomMeshData : IComponentData
{
    public Mesh mesh1;
    public Mesh mesh2;
    public Mesh mesh3;

}

public partial class PrefabConfigSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "PrefabConfigScene";
}


[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(PrefabConfigSceneSystem))]
public partial class MouseClickSystem : SystemBase
{
    private Camera _mainCamera;

    public BatchMeshID batchMeshID1;
    public BatchMeshID batchMeshID2;
    public BatchMeshID batchMeshID3;

    public Mesh MyMesh1;
    public Mesh MyMesh2;
    public Mesh MyMesh3;

    public BatchMaterialID batchMaterialID1;
    public BatchMaterialID batchMaterialID2;
    public BatchMaterialID batchMaterialID3;
    protected override void OnCreate()
    {
        base.OnCreate();
        _mainCamera = Camera.main;
        RequireForUpdate<PhysicsWorldSingleton>();

        
        // Debug.Log("data.mesh   " + data.mesh1);
        // EntityManager.AddComponentData(this.SystemHandle, new CustomMeshData  {mesh1 = data.mesh1 });
        // MyMesh1 = data.mesh1;
        // MyMesh2 = data.mesh2;
        // MyMesh3 = data.mesh3;



        UIEventHandlerData data = Object.FindObjectOfType<UIEventHandlerData>();// 获取UIEventHandlerData组件
        // Debug.Log("data ===   " + data);

        if(data){
            var entitiesGraphicsSystem = World.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();

            batchMeshID1 = entitiesGraphicsSystem.RegisterMesh(data.mesh1);
            batchMeshID2 = entitiesGraphicsSystem.RegisterMesh(data.mesh2);
            batchMeshID3 = entitiesGraphicsSystem.RegisterMesh(data.mesh3);


            batchMaterialID1 = entitiesGraphicsSystem.RegisterMaterial(data.material1);
            batchMaterialID2 = entitiesGraphicsSystem.RegisterMaterial(data.material2);
            batchMaterialID3 = entitiesGraphicsSystem.RegisterMaterial(data.material3);

            Debug.Log("batchMeshID   " + batchMeshID1.value + "   " + batchMeshID2.value + "   " + batchMeshID3.value);
            Debug.Log("batchMaterialID1   " + batchMaterialID1.value + "   " + batchMaterialID2.value + "   " + batchMaterialID3.value);
        }





    }



    protected override void OnStartRunning()
    {

        _mainCamera = Camera.main;





        // 获取所有带有MeshData的Entity
        // Entities.ForEach((ref MeshData meshData, ref RenderMesh renderMesh) => {
        //     // 这里可以根据需要修改meshData.Mesh或者renderMesh.Material
        //     // 例如，这里我们只是简单地将MeshData的Mesh赋值给RenderMesh的Mesh
        //     renderMesh.mesh = meshData.Mesh;
        // }).ScheduleParallel();
        
        // RequireForUpdate<myCustomMeshAndMaterial>();

        // var entitiesGraphicsSystem = World.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
        // // // m_MeshMapping = new Dictionary<Mesh, BatchMeshID>();
        // // // m_MaterialMapping = new Dictionary<Material, BatchMaterialID>();
        // // // 注册材质 和 网格 
        // Entities.WithoutBurst().ForEach(( in CustomMeshData changer) => {
        //     // UnityEngine.Debug.Log("ChangeMaterialSystemBase OnStartRunning   " + localTransform);
        //     // m_MeshMapping[changer.Mesh] = entitiesGraphicsSystem.RegisterMesh(changer.Mesh);
        //     // m_MaterialMapping[changer.red] = entitiesGraphicsSystem.RegisterMaterial(changer.red);
        //     batchMeshID = entitiesGraphicsSystem.RegisterMesh(changer.mesh);
        //     // batchMaterialID = entitiesGraphicsSystem.RegisterMaterial(changer.red);
        //     Debug.Log("batchMeshID   " + batchMeshID);
        
        // }).Run();
        // entitiesGraphicsSystem.RegisterMesh(MyMesh);


        // batchMeshID1 = entitiesGraphicsSystem.RegisterMesh(MyMesh1);
        // batchMeshID2 = entitiesGraphicsSystem.RegisterMesh(MyMesh2);
        // batchMeshID3 = entitiesGraphicsSystem.RegisterMesh(MyMesh3);

        // Debug.Log("batchMeshID   " + batchMeshID1.value + "   " + batchMeshID2.value + "   " + batchMeshID3.value);

        // batchMaterialID = entitiesGraphicsSystem.RegisterMesh(MyMesh);


    }


    protected override void OnUpdate()
    {


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

                    // info.MeshID = batchMeshID;

                    Debug.Log($"点击的实体材质: {info}");

                    // EntityManager.AddComponentData(clickedEntity, new CustomColor { customColor = new float4(0, 1, 1, 1) });


                    // clickedEntity中找到  MaterialMeshInfo 
                    MaterialMeshInfo infoData = EntityManager.GetComponentData<MaterialMeshInfo>(clickedEntity);
                    Debug.Log("infoData =="+ infoData);

                    Debug.Log("infoData =="+ infoData.Mesh);
                    // infoData.Mesh = 2; // 修改 MeshID

                    EntityManager.AddComponentData(clickedEntity, new MaterialMeshInfo { Mesh = (int)batchMeshID1.value, Material = (int)batchMaterialID1.value });
                    // Debug.Log("infoData  = "+ infoData + " ===MeshID  "+ infoData.MeshID + "  infoData Mesh " + infoData.Mesh);
                    // infoData.MeshID = batchMeshID;

                    // infoData.Mesh = 1;




                    // clickedEntity 切换网格
                    // EntityManager.SetComponentData(clickedEntity, new MaterialMeshInfo { MeshID = batchMeshID, MaterialID = batchMaterialID });


                    // EntityManager.AddComponentData(clickedEntity, new CustomColor { customColor = new float4(0, 1, 1, 1) });
                }
            }
        }
    }
}
