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

namespace DOTS.DOD.GRAPHICS.LESSON1
{
    // public partial class GraphicsLesson1SystemGroup : SceneSystemGroup
    // {
    //     protected override string SceneName => "CreateWaveCubesRuntime";
    // }

    public partial class GraphicsLesson1SystemGroup : testPrefabSceneSystemGroup
    {
        protected override string AuthoringSceneName => "CreateWaveCubesRuntime";
    }




    [BurstCompile]
    public struct SpawnJob : IJobParallelFor
    {
        public Entity prototype;
        public int halfCountX;
        public int halfCountZ;
        public EntityCommandBuffer.ParallelWriter Ecb;
        public EntityManager entityManager;

        public void Execute(int index)
        {
            var e = Ecb.Instantiate(index, prototype);
            Ecb.SetComponent(index, e, new LocalToWorld { Value = ComputeTransform(index) });
        
        }

        public float4x4 ComputeTransform(int index)
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
    
    [BurstCompile]
    partial struct WaveCubeEntityJob : IJobEntity
    {
        [ReadOnly] public float elapsedTime;
        void Execute(ref LocalToWorld transform)
        {
            var distance = math.distance(transform.Position, float3.zero);
            float3 newPos = transform.Position + new float3(0, 1, 0) * math.sin(elapsedTime * 3f + distance * 0.2f);
            // float3 newPos = transform.Position;

            transform.Value = float4x4.Translate(newPos);
        }
    }
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(GraphicsLesson1SystemGroup))]
    public partial struct WaveCubesMoveSystem : ISystem
    {
        static readonly ProfilerMarker profilerMarker = new ProfilerMarker("WaveCubeEntityJobs");
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            using (profilerMarker.Auto())
            {
                var job = new WaveCubeEntityJob() { elapsedTime = (float)SystemAPI.Time.ElapsedTime };
                job.ScheduleParallel();
            }
        }
    }
    
    public class CreateWaveCubesWithMonobehavior : MonoBehaviour
    {
        [Range(10, 100)] public int xHalfCount = 40;
        [Range(10, 100)] public int zHalfCount = 40;
        public Mesh mesh;
        public Material material;
        
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


            if(true){
                for (int i = 0; i < 4*xHalfCount*zHalfCount; i++)
                {
                    Debug.Log(i);
                    var e = entityManager.Instantiate(prototype);
                    //    entityManager.SetComponentData(prototype, new LocalToWorld{Value = float4x4.identity});
                    entityManager.SetComponentData(e, new LocalToWorld{Value = ComputeTransform(i, xHalfCount, zHalfCount)});
                } 

            } else{
                var spawnJob = new SpawnJob
                {
                    entityManager = entityManager,
                    prototype = prototype,
                    Ecb = ecbJob.AsParallelWriter(),
                    halfCountX = xHalfCount,
                    halfCountZ = zHalfCount
                };

                var spawnHandle = spawnJob.Schedule(4*xHalfCount*zHalfCount, 128);
                spawnHandle.Complete();

                ecbJob.Playback(entityManager);
                ecbJob.Dispose();

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
}
