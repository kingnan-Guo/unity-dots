using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;



/// <summary>
/// 标记 这个类是一个 IComponentData 类型；
/// 这是一个   非托管（unmanaged data） 数据类型 有限 
/// </summary>
struct rootComponent : IComponentData{
    public float rootSpeed;
    public rootComponent(float speed){
        rootSpeed = speed;
    }
}



public class rootComponentAuthoring : MonoBehaviour
{
    public float rootSpeed = 3;
    // 自定义一个  Baker  方便场景 在 bake 的时候  来转 Entity


}
// 把普通 的 MonoBehaviour 的组件数据 转成 ECS Component 的 ComponentData
// 编写一个 baker 把普通 的 MonoBehaviour 的组件数据  传递到  entity 里，相关 的 ComponentData

public class Baker : Baker<rootComponentAuthoring>
{

    public override void Bake(rootComponentAuthoring authoring)
    {
        Debug.Log("rootComponentAuthoring 发生在 编译模式下");

        // 往  Entity 里面添加 Component Data 
        var entity = this.GetEntity(TransformUsageFlags.Dynamic);//获取 当前 bake 的entity, bake 当前节点的组件 可以通过 GetEntity 来获取 entity

        // 创建一个组件;  这里的参数  并不是 Entity 里面 通过 ArchType 分配 出来的内存； 真是  的
        var data = new rootComponent{
            rootSpeed = math.radians(authoring.rootSpeed)// math.radians 将角度 转换为弧度
        };

        AddComponent(entity, data);

        // throw new System.NotImplementedException(); //抛出异常
    }
}