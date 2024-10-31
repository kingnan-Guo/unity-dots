// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CreateCubeswithChangeMaterialAndMesh : MonoBehaviour
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


using System.Collections.Generic;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;






public partial class CreateCubeswithChangeMaterialAndMeshSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "CubesWithColorAndMeshChange";
}






    // public partial class GraphicsLesson1SystemGroup : SceneSystemGroup
    // {
    //     protected override string SceneName => "CreateWaveCubesRuntime";
    // }
    // [BurstCompile]
    // public struct SpawnJob : IJobParallelFor
    // {
    //     public Entity prototype;
    //     public int halfCountX;
    //     public int halfCountZ;
    //     public EntityCommandBuffer.ParallelWriter Ecb;
    //     public EntityManager entityManager;

    //     public void Execute(int index)
    //     {
    //         var e = Ecb.Instantiate(index, prototype);
    //         Ecb.SetComponent(index, e, new LocalToWorld { Value = ComputeTransform(index) });
        
    //     }

    //     public float4x4 ComputeTransform(int index)
    //     {
    //         int x = index % (halfCountX * 2) - halfCountX;
    //         int z = index / (halfCountX * 2) - halfCountZ;
    //         float4x4 M = float4x4.TRS(
    //             new float3(x*1.1f, 0, z*1.1f),
    //             quaternion.identity,
    //             new float3(1));
    //         return M;
    //     }
    // }
    
    // [BurstCompile]
    // partial struct WaveCubeEntityJob : IJobEntity
    // {
    //     [ReadOnly] public float elapsedTime;
    //     void Execute(ref LocalToWorld transform)
    //     {
    //         var distance = math.distance(transform.Position, float3.zero);
    //         float3 newPos = transform.Position + new float3(0, 1, 0) * math.sin(elapsedTime * 3f + distance * 0.2f);
    //         // float3 newPos = transform.Position;

    //         transform.Value = float4x4.Translate(newPos);
    //     }
    // }
    // [RequireMatchingQueriesForUpdate]
    // [UpdateInGroup(typeof(GraphicsLesson1SystemGroup))]
    // public partial struct WaveCubesMoveSystem : ISystem
    // {
    //     static readonly ProfilerMarker profilerMarker = new ProfilerMarker("WaveCubeEntityJobs");
    //     [BurstCompile]
    //     public void OnUpdate(ref SystemState state)
    //     {
    //         using (profilerMarker.Auto())
    //         {
    //             var job = new WaveCubeEntityJob() { elapsedTime = (float)SystemAPI.Time.ElapsedTime };
    //             job.ScheduleParallel();
    //         }
    //     }
    // }
    
    public class CreateCubeswithChangeMaterialAndMesh : MonoBehaviour
    {
        [Range(10, 100)] public int xHalfCount = 40;
        [Range(10, 100)] public int zHalfCount = 40;
        public Mesh mesh;
        public Material material;

        public Mesh[] changeMeshes;

        public Material[] changeMaterials;
        
        void Start()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = world.EntityManager;

            EntityCommandBuffer ecbJob = new EntityCommandBuffer(Allocator.TempJob);

            var filterSettings = RenderFilterSettings.Default;
            filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
            filterSettings.ReceiveShadows = false;

            var renderMeshArray = new RenderMeshArray(new[] { material }, new[] { mesh });
            var renderMeshDescription = new RenderMeshDescription
            {
                FilterSettings = filterSettings,
                LightProbeUsage = LightProbeUsage.Off,
            };

            var prototype = entityManager.CreateEntity();
            RenderMeshUtility.AddComponents(
                prototype,
                entityManager,
                renderMeshDescription,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            // entityManager.SetComponentData(prototype, new LocalToWorld{Value = float4x4.identity});


            // entityManager.AddComponentData(prototype, new CustomColor{
            //     customColor = new float4(0.0f, 0.0f, 0.0f, 1.0f)
            // });


            entityManager.AddComponentData(prototype, new CustomMeshAndMaterial{
                sphere = changeMeshes[0],   
                capsule = changeMeshes[1],
                cylinder = changeMeshes[2],
                red = changeMaterials[0],
                green = changeMaterials[1],
                blue = changeMaterials[2]
            });

            if(true){
                for (int i = 0; i < 4*xHalfCount*zHalfCount; i++)
                {
                    // Debug.Log(i);
                    var e = entityManager.Instantiate(prototype);
                    //    entityManager.SetComponentData(prototype, new LocalToWorld{Value = float4x4.identity});
                    entityManager.SetComponentData(e, new LocalToWorld{Value = ComputeTransform(i, xHalfCount, zHalfCount)});
                } 

            } else{
                // var spawnJob = new SpawnJob
                // {
                //     entityManager = entityManager,
                //     prototype = prototype,
                //     Ecb = ecbJob.AsParallelWriter(),
                //     halfCountX = xHalfCount,
                //     halfCountZ = zHalfCount
                // };

                // var spawnHandle = spawnJob.Schedule(4*xHalfCount*zHalfCount, 128);
                // spawnHandle.Complete();

                // ecbJob.Playback(entityManager);
                // ecbJob.Dispose();
            }
            




            // entityManager.DestroyEntity(prototype);
        }


        public float4x4 ComputeTransform(int index, int halfCountX, int halfCountZ)
        {
            int x = index % (halfCountX * 2) - halfCountX;
            int z = index / (halfCountX * 2) - halfCountZ;
            float4x4 M = float4x4.TRS(
                new float3(x*1.1f, 0, z*1.1f),
                quaternion.identity,
                new float3(1));
            return M;
        }

    }




// 更换材质 
// systemBase
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(CreateCubeswithChangeMaterialAndMeshSceneSystem))]
public partial class ChangeMaterialSystemBase : SystemBase{

    static readonly ProfilerMarker profilerMarker = new ProfilerMarker("ChangeMaterialSystemBase");

    public Dictionary<Mesh, BatchMeshID> m_MeshMapping;
    private Dictionary<Material, BatchMaterialID> m_MaterialMapping;

    private Camera _mainCamera;

    protected override void OnStartRunning()
    {

        _mainCamera = Camera.main;


        RequireForUpdate<Unity.Physics.PhysicsWorldSingleton>();

        
        UnityEngine.Debug.Log("ChangeMaterialSystemBase OnStartRunning");
        RequireForUpdate<CustomMeshAndMaterial>();

        var entitiesGraphicsSystem = World.GetOrCreateSystemManaged<EntitiesGraphicsSystem>();
        m_MeshMapping = new Dictionary<Mesh, BatchMeshID>();
        m_MaterialMapping = new Dictionary<Material, BatchMaterialID>();


        // 注册材质 和 网格 
        Entities.WithoutBurst().ForEach(( in CustomMeshAndMaterial changer) => {
            // UnityEngine.Debug.Log("ChangeMaterialSystemBase OnStartRunning   " + localTransform);

            m_MeshMapping[changer.sphere] = entitiesGraphicsSystem.RegisterMesh(changer.sphere);
            m_MeshMapping[changer.capsule] = entitiesGraphicsSystem.RegisterMesh(changer.capsule);
            m_MeshMapping[changer.cylinder] = entitiesGraphicsSystem.RegisterMesh(changer.cylinder);
            m_MaterialMapping[changer.red] = entitiesGraphicsSystem.RegisterMaterial(changer.red);
            m_MaterialMapping[changer.green] = entitiesGraphicsSystem.RegisterMaterial(changer.green);
            m_MaterialMapping[changer.blue] = entitiesGraphicsSystem.RegisterMaterial(changer.blue);
        
        }).Run();

    
    }


    protected override void OnUpdate()
    {
        // UnityEngine.Debug.Log("ChangeMaterialSystemBase OnUpdate");
        Entities.WithoutBurst().ForEach(( ref MaterialMeshInfo info, in LocalToWorld trans, in CustomMeshAndMaterial changer) => {

            // UnityEngine.Debug.Log("ChangeMaterialSystemBase OnUpdate  trans == "+ trans.Position.x);

            info.MeshID = m_MeshMapping[changer.capsule];
            info.MaterialID = m_MaterialMapping[changer.red];
        }).Run();




                // if (Input.GetMouseButtonDown(0) ) // 检测左键点击
                // {

                //     UnityEngine.Debug.Log("ChangeMaterialSystemBase OnUpdate GetMouseButtonDown");
                //     UnityEngine.Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                //     // 获取 PhysicsWorldSingleton
                //     // var physicsWorldSingleton = GetSingleton<PhysicsWorldSingleton>();
                //     var physicsWorldSingleton = SystemAPI.GetSingleton<Unity.Physics.PhysicsWorldSingleton>();
                    
                //     Unity.Physics.CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

                //     Unity.Physics.RaycastInput raycastInput = new Unity.Physics.RaycastInput
                //     {
                //         Start = ray.origin,
                //         End = ray.origin + ray.direction * 1000f,
                //         Filter = Unity.Physics.CollisionFilter.Default
                //     };

                //     if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
                //     {

                //         UnityEngine.Debug.Log($"点击位置: {hit.RigidBodyIndex}");
                //         // 获取点击的实体
                //         Entity clickedEntity = collisionWorld.Bodies[hit.RigidBodyIndex].Entity;

                //         // // 检查实体是否有 LocalTransform 组件
                //         if (EntityManager.HasComponent<LocalTransform>(clickedEntity))
                //         {
                //             LocalTransform transform = EntityManager.GetComponentData<LocalTransform>(clickedEntity);
                //             UnityEngine.Debug.Log($"点击的实体位置: {transform.Position}");
  
                //         }
                //     }
                // }



    }
}


// public partial class MouseClickSystem2 : SystemBase
// {
//     private Camera _mainCamera;

//     protected override void OnCreate()
//     {
//         base.OnCreate();
//         _mainCamera = Camera.main;
//         RequireForUpdate<PhysicsWorldSingleton>();
//     }

//     protected override void OnUpdate()
//     {

//         {

//             UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             UnityEngine.RaycastHit[] hits = new UnityEngine.RaycastHit[1];
//             if (Physics.RaycastNonAlloc(ray, hits, 1000) > 0){
//                 Debug.Log("getGameObject =="+hits[0].collider.name);
//             }

//         }
//         // return;
//         if (Input.GetMouseButtonDown(0) ) // 检测左键点击
//         {
//             UnityEngine.Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

//             // 获取 PhysicsWorldSingleton
//             // var physicsWorldSingleton = GetSingleton<PhysicsWorldSingleton>();
//             var physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            
//             CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

//             RaycastInput raycastInput = new RaycastInput
//             {
//                 Start = ray.origin,
//                 End = ray.origin + ray.direction * 1000f,
//                 Filter = CollisionFilter.Default
//             };

//             if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
//             {

//                 Debug.Log($"点击位置: {hit.RigidBodyIndex}");
//                 // 获取点击的实体
//                 Entity clickedEntity = collisionWorld.Bodies[hit.RigidBodyIndex].Entity;

//                 // 检查实体是否有 LocalTransform 组件
//                 if (EntityManager.HasComponent<LocalTransform>(clickedEntity))
//                 {
//                     LocalTransform transform = EntityManager.GetComponentData<LocalTransform>(clickedEntity);
//                     Debug.Log($"点击的实体位置: {transform.Position}");
//                     // 获取实体的名称
//                     // testName entityName = EntityManager.GetComponentData<testName>(clickedEntity);
//                     // Debug.Log($"点击的实体名称: {entityName.id}");


//                     MaterialMeshInfo info = EntityManager.GetComponentData<MaterialMeshInfo>(clickedEntity);

//                     Debug.Log($"点击的实体材质: {info}");
//                 }
//             }
//         }
//     }
// }




class CustomMeshAndMaterial : IComponentData
{
    public Mesh sphere;
    public Mesh capsule;
    public Mesh cylinder;
    public Material red;
    public Material green;
    public Material blue;
}


[MaterialProperty("_BaseColor")]
struct CustomColor : IComponentData
{
    public float4 customColor;
}

