using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public class createEntityWithMonobehavior : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    // public GameObject entity;
    void Start()
    {
        var world= World.DefaultGameObjectInjectionWorld;
        EntityManager entityManager = world.EntityManager;

        var renderMeshArray = new RenderMeshArray(new[] {material}, new[] {mesh});
        var renderMeshArrayEntity = entityManager.CreateEntity(typeof(RenderMeshArray));

        var renderMeshDescription = new RenderMeshDescription{
            FilterSettings = RenderFilterSettings.Default,
            LightProbeUsage = LightProbeUsage.Off,
        };

        var cubeEntity = entityManager.CreateEntity();
        RenderMeshUtility.AddComponents(
            cubeEntity,
            entityManager,
            renderMeshDescription,
            renderMeshArray,
            MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
        );

        entityManager.SetComponentData(cubeEntity, new LocalToWorld{Value = float4x4.identity});


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
