using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[BurstCompile]
public unsafe class BatchRendererGroupSetupTwo : MonoBehaviour
{
    [Range(1, 100)] public int xHalfCount = 10;
    [Range(1, 100)] public int zHalfCount = 10;
    public bool cullTest = false;
    public bool motionVectorTest = false;
    [Range(0.1f,3.0f)]public float spacingFactor = 1.1f;
    [Range(1, 16)]public int jobsPerBatch = 2;

    public Mesh mesh;
    public Material material;

    // opengle和opengles上只能使用ConstantBuffer，其他的都是使用RawBuffer
    private bool useConstantBuffer => BatchRendererGroup.BufferTarget == BatchBufferTarget.ConstantBuffer;
    
    private GraphicsBuffer gpuPersistentInstanceData;
    private BatchMeshID meshID;
    private BatchMaterialID materialID;
    private BatchRendererGroup brg;

    private uint instanceCount;         //生成多少个instance
    private uint batchCount;            //生成多少个batch
    private uint maxInstancePerBatch;   //每个batch最多多少个instance
    private BatchID[] batchIDs;          //每个batch的ID
    public struct SRPBatch
    {
        public uint rawBufferOffsetInFloat4;
        public uint instanceCount;
    };
    private SRPBatch[] srpBatches;   //每个batch的信息，包括在InstanceGraphicsBuffer的偏移和instance数量
    private NativeArray<float4> sysmemBuffer; //系统内存的Buffer，用于填充GPU的InstanceGraphicsBuffer
    
    private bool initialized = false;

    //创建一个NativeArray，用于填充GPU的InstanceGraphicsBuffer
    public static T* Malloc<T>(uint count) where T : unmanaged
    {
        return (T*)UnsafeUtility.Malloc(
            UnsafeUtility.SizeOf<T>() * count,
            UnsafeUtility.AlignOf<T>(),
            Allocator.TempJob);
    }
    
    //创建MetadataValue
    // static MetadataValue CreateMetadataValue(int nameID, int gpuAddress, bool isOverridden)
    // {
    //     const uint kIsOverriddenBit = 0x80000000;   //最高位是1，表示是Overridden的，0x80000000是2^31
    //     return new MetadataValue
    //     {
    //         NameID = nameID,
    //         Value = (uint)gpuAddress | (isOverridden ? kIsOverriddenBit : 0),
    //     };
    // }
    
    private void Start()
    {
        brg = new BatchRendererGroup(this.OnPerformCulling, IntPtr.Zero);
        
        meshID = brg.RegisterMesh(mesh);
        materialID = brg.RegisterMaterial(material);

        Debug.Log("meshID: " + meshID + " materialID: " + materialID);
        
        const int kFloat4Size = 16;
        uint kBRGBufferMaxWindowSize = 128 * 1024 * 1024;   //128MB
        uint kBRGBufferAlignment = 16;                      //16字节对齐

        //创建InstanceGraphicsBuffer，创建一个或者多个Batch，取决于UBO的大小限制
        instanceCount = (uint)(xHalfCount * zHalfCount * 4);
        Debug.Log("instanceCount: " + instanceCount);


        //计算每个Instance数据，包括Matrix和Color，有两个4*3的矩阵和一个Color，矩阵是unity_ObjectToWorld，unity_WorldToObject
        const uint kPerInstanceSize = (3*2 + 1);

        Debug.Log("kPerInstanceSize: " + kPerInstanceSize);
        //减4个float4是因为BRG的限制，前64个字节必须是0，计算每个Batch最多能放多少个Instance
        maxInstancePerBatch = ((kBRGBufferMaxWindowSize / kFloat4Size) - 4) / kPerInstanceSize;

        Debug.Log("maxInstancePerBatch: " + maxInstancePerBatch);
        if (maxInstancePerBatch > instanceCount)
            maxInstancePerBatch = instanceCount;
        //计算需要多少个Batch，-1是因为C#的除法是向下取整
        batchCount = (instanceCount + maxInstancePerBatch - 1) / maxInstancePerBatch;
        Debug.Log("batchCount: " + batchCount);
        //计算每个Batch的大小，需要对齐到16字节
        uint batchAlignedSizeInBytes = (((4 + maxInstancePerBatch * kPerInstanceSize)* kFloat4Size) + kBRGBufferAlignment - 1) & (~(kBRGBufferAlignment - 1)); // 4是前64字节必须是0
        Debug.Log("batchAlignedSizeInBytes: " + batchAlignedSizeInBytes);

        //计算InstanceGraphicsBuffer的大小
        uint totalRawBufferSizeInBytes = batchCount * batchAlignedSizeInBytes;
        Debug.Log("totalRawBufferSizeInBytes: " + totalRawBufferSizeInBytes);

        
        Debug.Log("useConstantBuffer: " + useConstantBuffer);

        //创建InstanceGraphicsBuffer
        // if (useConstantBuffer){
        //     gpuPersistentInstanceData = new GraphicsBuffer(GraphicsBuffer.Target.Constant, (int)totalRawBufferSizeInBytes / kFloat4Size, kFloat4Size);
        // }
        // else
        gpuPersistentInstanceData = new GraphicsBuffer(GraphicsBuffer.Target.Raw, (int)totalRawBufferSizeInBytes / 4, 4);
        Debug.Log("gpuPersistentInstanceData: " + gpuPersistentInstanceData);


        
        // //创建Batch的MetaData
        var batchMetadata = new NativeArray<MetadataValue>(3, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
        // //Batch的MetaData
        // int objectToWorldID = Shader.PropertyToID("unity_ObjectToWorld");
        // int worldToObjectID = Shader.PropertyToID("unity_WorldToObject");
        // int colorID = Shader.PropertyToID("_BaseColor");
            
        // // 创建系统内存的Buffer，用于填充GPU的InstanceGraphicsBuffer
        // sysmemBuffer = new NativeArray<float4>((int)(totalRawBufferSizeInBytes/kFloat4Size), Allocator.Persistent, NativeArrayOptions.ClearMemory);
        // // 创建每个Batch的信息，包括在InstanceGraphicsBuffer的偏移和instance数量
        // srpBatches = new SRPBatch[batchCount];
        // // 创建每个Batch的ID
        // batchIDs = new BatchID[batchCount];
        // uint left = instanceCount;

    }
    


    void Update()
    {
        //UpdatePositionsAndColors(Vector3.zero);
        // JobHandle updateInstanceDataJobHandle = default;
        // updateInstanceDataJobHandle = UpdatePositionsAndColorsWithJob(Vector3.zero);
        // updateInstanceDataJobHandle.Complete();
        // gpuPersistentInstanceData.SetData(sysmemBuffer);
    }
    
    private void OnDestroy()
    {
        if (initialized)
        {
            // 清理Batch
            for (uint b=0;b<batchCount;b++)
            {
                brg.RemoveBatch(batchIDs[b]);
            }
            // 清理Mesh和Material
            if (material) 
                brg.UnregisterMaterial(materialID);
            if (mesh) 
                brg.UnregisterMesh(meshID);

            // 清理BatchRendererGroup
            brg.Dispose();
            // 清理GPU的InstanceGraphicsBuffer
            gpuPersistentInstanceData.Dispose();
            // 清理系统内存的Buffer
            sysmemBuffer.Dispose();
        }
    }
    
    [BurstCompile]
    public unsafe JobHandle OnPerformCulling(
        BatchRendererGroup rendererGroup,
        BatchCullingContext cullingContext,
        BatchCullingOutput cullingOutput,
        IntPtr userContext)
    {
        return new JobHandle();
    }


}

