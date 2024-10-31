using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Entities.Serialization;
using Unity.Transforms;
using UnityEngine;


public struct MeshComponentData : IComponentData
{
    public bool startedLoad;
    public WeakObjectReference<Mesh> mesh;
    public WeakObjectReference<Material> material;
}

public class createEntityFormMeshAuthoring : MonoBehaviour
{
    public WeakObjectReference<Mesh> mesh;
    public WeakObjectReference<Material> material;
    class Baker : Baker<createEntityFormMeshAuthoring>
    {
        public override void Bake(createEntityFormMeshAuthoring authoring)
        {

            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MeshComponentData { mesh = authoring.mesh, material = authoring.material, startedLoad = false});
        }
    }
}


// using Unity.Entities;
// using Unity.Entities.Content;
// using UnityEngine;

// public class MeshRefSample : MonoBehaviour
// {
//     public WeakObjectReference<Mesh> mesh;
//     class MeshRefSampleBaker : Baker<MeshRefSample>
//     {
//         public override void Bake(MeshRefSample authoring)
//         {
//             var entity = GetEntity(TransformUsageFlags.Dynamic);
//             AddComponent(entity, new MeshComponentData { mesh = authoring.mesh });
//         }
//     }
// }

// public struct MeshComponentData : IComponentData
// {
//     public WeakObjectReference<Mesh> mesh;
// }



public partial class createEntityFormMeshSceneSystem : createEntityFormMeshSceneSystemGroup
{
    protected override string AuthoringSceneName => "createEntityFormMesh";
}




// [RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(testPrefabSceneSystem))]
// [BurstCompile]
// public partial struct createEntityFormMeshSystem : ISystem
// {
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         Debug.Log("testPrefabSystem OnCreate");
//         state.RequireForUpdate<testPrefabComponent>();
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {

//         // Debug.Log("testPrefabSystem OnUpdate");
//         state.Enabled = false;





//         // 预制体实例化 Entity  第二种方法 ===================

//         var testPrefabComponentEntity = SystemAPI.GetSingleton<MeshComponentData>();
//         var cubes = CollectionHelper.CreateNativeArray<Entity>(10, Allocator.Temp);

//         state.EntityManager.Instantiate(testPrefabComponentEntity.mesh, cubes);
//         // int count = 0;

//         // foreach (var cube in cubes)
//         // {
//         //     state.EntityManager.AddComponentData(cube, new testPrefabInfoComponent(){
//         //         id = count
//         //     });
//         //     var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
//         //     state.EntityManager.SetComponentData(cube, LocalTransform.FromPosition(randomPosition));
//         //     // var transform = state.EntityManager.GetComponentData<LocalTransform>(cube);
//         //     count++;
//         // }


//     }
// }



// [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
// [UpdateInGroup(typeof(PresentationSystemGroup))]
public partial struct RenderFromWeakObjectReferenceSystem : ISystem
{
    public int count;
    public void OnCreate(ref SystemState state) {
        count = 0;
    }
    public void OnDestroy(ref SystemState state) { }
    public void OnUpdate(ref SystemState state)
    {
        
        
        foreach (var (transform, dec) in SystemAPI.Query<RefRW<LocalToWorld>, RefRW<MeshComponentData>>())
        {
            if (!dec.ValueRW.startedLoad)
            {
                dec.ValueRW.mesh.LoadAsync();
                dec.ValueRW.material.LoadAsync();
                dec.ValueRW.startedLoad = true;
            }
            if (dec.ValueRW.mesh.LoadingStatus == ObjectLoadingStatus.Completed &&
                dec.ValueRW.material.LoadingStatus == ObjectLoadingStatus.Completed)
            {

                // Graphics.DrawMesh(dec.ValueRO.mesh.Result, transform.ValueRO.Value, dec.ValueRO.material.Result, 0);

                // NativeArray<Matrix4x4> matrices = new NativeArray<Matrix4x4>(10, Allocator.TempJob);
        
                // for (int i = 0; i < 10; i++)
                // {
                //     matrices[i] = new Matrix4x4
                //     {
                //         m0 = transform.ValueRO.Value.c0.x,
                //         m1 = transform.ValueRO.Value.c0.y,
                //         m2 = transform.ValueRO.Value.c0.z,
                //         m3 = transform.ValueRO.Value.c0.w,
                //         m4 = transform.ValueRO.Value.c1.x,
                //         m5 = transform.ValueRO.Value.c1.y,
                //         m6 = transform.ValueRO.Value.c1.z,
                //         m7 = transform.ValueRO.Value.c1.w,
                //         m8 = transform.ValueRO.Value.c2.x,
                //         m9 = transform.ValueRO.Value.c2.y,
                //         m10 = transform.ValueRO.Value.c2.z,
                //         m11 = transform.ValueRO.Value.c2.w,
                //         m12 = transform.ValueRO.Value.c3.x,
                //         m13 = transform.ValueRO.Value.c3.y,
                //         m14 = transform.ValueRO.Value.c3.z,
                //         m15 = transform.ValueRO.Value.c3.w
                //     };
                // }






                NativeArray<Matrix4x4> matrices = new NativeArray<Matrix4x4>(10, Allocator.TempJob);

                for (int i = 0; i < 10; i++)
                {
                    matrices[i] = new Matrix4x4
                    {
                        m00 = transform.ValueRO.Value.c0.x,
                        m01 = transform.ValueRO.Value.c0.y,
                        m02 = transform.ValueRO.Value.c0.z,
                        m03 = transform.ValueRO.Value.c0.w,
                        m10 = transform.ValueRO.Value.c1.x,
                        m11 = transform.ValueRO.Value.c1.y,
                        m12 = transform.ValueRO.Value.c1.z,
                        m13 = transform.ValueRO.Value.c1.w,
                        m20 = transform.ValueRO.Value.c2.x,
                        m21 = transform.ValueRO.Value.c2.y,
                        m22 = transform.ValueRO.Value.c2.z,
                        m23 = transform.ValueRO.Value.c2.w,
                        m30 = transform.ValueRO.Value.c3.x,
                        m31 = transform.ValueRO.Value.c3.y,
                        m32 = transform.ValueRO.Value.c3.z,
                        m33 = transform.ValueRO.Value.c3.w
                    };
                }




            // Matrix4x4[] matrices = new Matrix4x4[10];
            // for (int i = 0; i < matrices.Length; i++)
            // {
            //     matrices[i] = Matrix4x4.TRS(
            //             new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)),
            //             Quaternion.identity,
            //             Vector3.one);
            // }

                // Graphics.DrawMeshInstanced(dec.ValueRO.mesh.Result, matrices, dec.ValueRO.material.Result, 0);
                //  Graphics.DrawMeshInstanced(dec.ValueRO.mesh.Result, 0,  dec.ValueRO.material.Result, matrices);

                // Graphics.DrawMeshInstanced(dec.ValueRO.mesh.Result, 0, dec.ValueRO.material.Result, matrices);


                Graphics.DrawMeshInstanced(dec.ValueRO.mesh.Result, 0, dec.ValueRO.material.Result, matrices.ToArray());
                // state.Enabled = false;
                // Debug.Log("RenderFromWeakObjectReferenceSystem OnUpdate = "+ count);
                // if(count > 100){
                //     state.Enabled = false;
                // }
                // count++;
                // foreach (var cube in cubes)
                // {
                //     state.EntityManager.AddComponentData(cube, new testPrefabInfoComponent(){
                //         id = count
                //     });
                //     var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
                //     state.EntityManager.SetComponentData(cube, LocalTransform.FromPosition(randomPosition));
                //     // var transform = state.EntityManager.GetComponentData<LocalTransform>(cube);
                //     count++;
                // }

                // for (int i = 0; i < 5; i++)
                // {
                //     Debug.Log("RenderFromWeakObjectReferenceSystem OnUpdate");
                //     var randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

                //     // Graphics.DrawMesh(dec.ValueRO.mesh.Result, randomPosition, dec.ValueRO.material.Result, 0);

                //     Graphics.DrawMesh(dec.ValueRO.mesh.Result, transform.ValueRO.Value, dec.ValueRO.material.Result, 10);
                    
                // }


            }
        }
    }
}


