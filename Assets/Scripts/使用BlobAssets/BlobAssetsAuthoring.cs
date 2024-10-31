using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;



struct EntitySpawnerComponent : IComponentData{
    public float data;
}

public enum myType //建筑类型
{
    DEVICE,
    BUILDING
}

struct EntitySpawnerBlobData{
    public Entity EntityProtoType;
    public myType type;
}


struct EntitySpawnerSettings : IComponentData
{
    public BlobAssetReference<EntitySpawnerBlobData> blobSettings;
}



public class BlobAssetsAuthoring : MonoBehaviour
{

    public GameObject protoTypePrefab = null;
    public myType type = myType.DEVICE;

    public float data;
    class Baker : Baker<BlobAssetsAuthoring>
    {
        public override void Bake(BlobAssetsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);


            AddComponent(entity, new EntitySpawnerComponent { 
                data = authoring.data
            });

            var settings = CreateSpawnerBlobSettings(authoring);
            // AddBlobAsset 是 用来 创建 BlobAssetReference 的
            AddBlobAsset(ref settings, out var hash);
            AddComponent(entity, new EntitySpawnerSettings
            {
                blobSettings = settings
            });

        }



        BlobAssetReference<EntitySpawnerBlobData> CreateSpawnerBlobSettings(BlobAssetsAuthoring authoring){
            var builder = new BlobBuilder(Allocator.Temp);

            // return result;
            ref EntitySpawnerBlobData spawnerBlobData = ref builder.ConstructRoot<EntitySpawnerBlobData>();


            spawnerBlobData.EntityProtoType = GetEntity(authoring.protoTypePrefab, TransformUsageFlags.Dynamic);
            spawnerBlobData.type = authoring.type;

            var result = builder.CreateBlobAssetReference<EntitySpawnerBlobData>(Allocator.Persistent);
            builder.Dispose();
            return result;
        }








    }






}


public partial class BlobAssetsSceneSystem : testPrefabSceneSystemGroup
{
    protected override string AuthoringSceneName => "BlobAssetsScene";
}







[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(BlobAssetsSceneSystem))]
// [BurstCompile]
public partial struct BlobAssetsSystem : ISystem{
    // [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitySpawnerSettings>();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        // var query = state.GetEntityQuery(typeof(EntitySpawnerSettings));
        // var settings = query.ToComponentDataArray<EntitySpawnerSettings>(Allocator.Temp);
        // foreach (var setting in settings)
        // {
        //     if (setting.blobSettings.IsCreated)
        //     {
        //         var entityProtoType = setting.blobSettings.Value.EntityProtoType;
        //         // var entityProtoTypeData = state.EntityManager.GetComponentData<EntitySpawnerComponent>(entityProtoType);
        //         // Debug.Log("EntityProtoType Data: " + entityProtoTypeData.data);
        //         // // 实例化 entityProtoType
        //         state.EntityManager.Instantiate(entityProtoType);
        //     }
        // }
        // settings.Dispose();
        // foreach (var blobSetting in SystemAPI.Query<RefRO<EntitySpawnerSettings>>()){

        //     // Debug.Log(blobSetting.ValueRO.blobSettings.Value.EntityProtoType);

        //     Debug.Log("EntityProtoType Data: " + blobSetting.ValueRO.blobSettings.Value.type);
        //     Debug.Log("EntityProtoType Data: " + blobSetting.ValueRO.blobSettings.Value.EntityProtoType);

        //     state.EntityManager.Instantiate(blobSetting.ValueRO.blobSettings.Value.EntityProtoType);

        // }


    }
}

// using System.Collections;
// using System.Collections.Generic;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Transforms;
// using UnityEngine;

// // 实体生成组件，包含一些用户数据
// struct EntitySpawnerComponent : IComponentData
// {
//     public float data;
// }

// // 枚举，表示不同的建筑类型
// public enum myType // 建筑类型
// {
//     DEVICE,
//     BUILDING
// }

// // Blob 数据结构，包含实体原型和建筑类型
// struct EntitySpawnerBlobData
// {
//     public Entity EntityProtoType; // 直接存储实体原型
//     public myType type; // 建筑类型
// }

// // 实体生成设置，引用BlobAsset
// struct EntitySpawnerSettings : IComponentData
// {
//     public BlobAssetReference<EntitySpawnerBlobData> blobSettings;
// }

// // 预制体的MonoBehaviour脚本，用于将GameObject转换为实体并创建BlobAsset
// public class BlobAssetsAuthoring : MonoBehaviour
// {
//     public GameObject protoTypePrefab = null; // 预制体
//     public myType type = myType.DEVICE; // 类型
//     public float data; // 一些附加数据

//     // Baker类，用于将MonoBehaviour转换为Entity的Baker
//     class Baker : Baker<BlobAssetsAuthoring>
//     {
//         public override void Bake(BlobAssetsAuthoring authoring)
//         {
//             // 获取当前实体
//             var entity = GetEntity(TransformUsageFlags.Dynamic);

//             // 添加用户自定义的组件
//             AddComponent(entity, new EntitySpawnerComponent
//             {
//                 data = authoring.data
//             });

//             // 检查预制体是否有效
//             if (authoring.protoTypePrefab == null)
//             {
//                 Debug.LogError("Prefab is not assigned!");
//                 return;
//             }

//             // 将预制体转换为实体
//             var prefabEntity = GetEntity(authoring.protoTypePrefab, TransformUsageFlags.Dynamic);

//             // 确保预制体转换为了有效的实体
//             if (prefabEntity == Entity.Null)
//             {
//                 Debug.LogError("Prefab conversion failed! Make sure ConvertToEntity or Baker is set up properly.");
//                 return;
//             }

//             // 创建BlobAsset
//             var settings = CreateSpawnerBlobSettings(authoring, prefabEntity);
//             AddBlobAsset(ref settings, out var hash);

//             // 添加BlobAsset组件
//             AddComponent(entity, new EntitySpawnerSettings
//             {
//                 blobSettings = settings
//             });
//         }

//         // 创建Blob Asset的方法
//         BlobAssetReference<EntitySpawnerBlobData> CreateSpawnerBlobSettings(BlobAssetsAuthoring authoring, Entity prefabEntity)
//         {
//             var builder = new BlobBuilder(Allocator.Temp);

//             // 构建Blob数据
//             ref EntitySpawnerBlobData spawnerBlobData = ref builder.ConstructRoot<EntitySpawnerBlobData>();

//             // 存储实体原型和类型
//             spawnerBlobData.EntityProtoType = prefabEntity;
//             spawnerBlobData.type = authoring.type;

//             // 创建并返回BlobAsset
//             var result = builder.CreateBlobAssetReference<EntitySpawnerBlobData>(Allocator.Persistent);
//             builder.Dispose();
//             return result;
//         }
//     }
// }

// // 自定义场景系统
// public partial class BlobAssetsSceneSystem : testPrefabSceneSystemGroup
// {
//     protected override string AuthoringSceneName => "BlobAssetsScene";
// }

// // 实现I系统接口的系统，用于处理Blob Asset的实体生成
// [RequireMatchingQueriesForUpdate]
// [UpdateInGroup(typeof(BlobAssetsSceneSystem))]
// [BurstCompile]
// public partial struct BlobAssetsSystem : ISystem
// {
//     [BurstCompile]
//     public void OnCreate(ref SystemState state)
//     {
//         // 系统只在EntitySpawnerSettings组件存在时更新
//         state.RequireForUpdate<EntitySpawnerSettings>();
//     }

//     [BurstCompile]
//     public void OnUpdate(ref SystemState state)
//     {
//         // 禁用系统更新，除非有匹配的查询
//         state.Enabled = false;

//         // 获取包含EntitySpawnerSettings的实体查询
//         var query = state.GetEntityQuery(typeof(EntitySpawnerSettings));
//         var settings = query.ToComponentDataArray<EntitySpawnerSettings>(Allocator.Temp);

//         // 遍历所有查询结果
//         foreach (var setting in settings)
//         {
//             if (setting.blobSettings.IsCreated)
//             {
//                 var entityProtoType = setting.blobSettings.Value.EntityProtoType;

//                 // 检查EntityProtoType是否为有效的实体
//                 if (entityProtoType == Entity.Null)
//                 {
//                     Debug.LogError("EntityProtoType is not valid!");
//                     continue;
//                 }

//                 // 实例化有效的实体
//                 state.EntityManager.Instantiate(entityProtoType);
//             }
//         }

//         // 释放临时内存
//         settings.Dispose();
//     }
// }