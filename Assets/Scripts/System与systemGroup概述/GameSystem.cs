using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class GameSystem : SystemBase 
{
    protected override void OnCreate(){

    }

    protected override void OnDestroy(){

    }

    protected override void OnUpdate()
    {

        // 查找 rootComponent 和 localTransform
        foreach (var (r, l) in SystemAPI.Query<RefRW<rootComponent>, RefRO<LocalTransform>>())
        {
            Debug.Log("rootComponent : " + r.ValueRO.rootSpeed + "  localTransform : " + l.ValueRO.Position);
        }
        // throw new System.NotImplementedException();
    }
}


//一、 ==========
// system 是一种服务 代码逻辑，改变组件的数据状态 ， 以一个状态 到另一个状态 ，可以理解为 ComponentData 的数据变化，可以只读  也可以修改
// eg ： System :  Move Entity : 上一次的位置 prevPosition + （速度 * 时间 ）  = 下一次的状态 NextPosition


// 二、 ====================
// system 算法的程序 是 运行在 main thread 上的，所以不能有耗时的操作，否则会卡住主线程，导致整个游戏卡住， 每一帧执行一次 ，准确来说  是 system.Update () 每一帧执行一次
//  system 根据 system Group 的顺序 执行，所以要注意 system Group 的顺序，不能有相互依赖的 system ，否则会导致死循环， 根据 system Group 的体系 来决定 调用顺序

// 三 、 ==========
// 可以创建 一个 基于 托管的 或 一个 非托管的 system 
//  托管 system 是在 main thread 上运行的，非托管 system 是在 worker thread 上运行的，所以非托管 system 可以执行耗时的操作，不会卡住主线程
//  创建一个 托管的  system  ，定义一个  class  继承  Systembase
//  创建一个 非托管的  system  ，定义一个 结构体 然后继承自 ISystem；                  （ 定义一个  class  继承  NonUnityEngineSystembase）？？ 这个是啥
// Systembase ISystem 都有 OnUpdate 、OnCreate、 OnDestroy 方法，OnUpdate 是每一帧执行一次，OnCreate 是在 system 创建时执行一次，OnDestroy 是在 system 销毁时执行一次



// 四、 ==========
//  一个 system 只能 处理一个世界 world Entity , 通过 system.World 来获取 属性， 可以获得当前这个 system 对象所在 World世界；


// 五、 ==========
// 定义了 一个 system 但是没有定义 实例 但是依然 可以，默认情况下 有一个处理程序 ，在启动过程 会为  system 与 systemGroup  自动创建一个实例

// 默认启动的时候 会自动创建一个 世界 ，同时 默认创建  三个 systemGroup ：InitializationSystemGroup（初始化） 、SimulationSystemGroup（模拟迭代）、 PresentationSystemGroup（呈现迭代）
// 默认情况下 一个 system 的实例 会被添加到  SimulationSystemGroup（模拟迭代） 这个分组下，
// 可以使用 [UpdateInGroup] 来指定 system 的实例 添加到哪个 systemGroup 下，默认情况下 添加到 SimulationSystemGroup（模拟迭代） 这个分组下


// 六、 ==========
// 关闭 这个 默认的 自动的脚本操作 ， 使用 宏定义 #UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP 






// ============================================================================================================



// System types
// 可以使用的 system 类型
// 1. 托管的 system  Systembase 
// 2. 非托管的 system  ISystem   : interface
// 3. EntyityCommandBufferSystem : 命令  和 buffer 改变结构 提升性能
// 4、 ComponentSystemGroup     ： 提供 组织关系 维护 system group 的关系



// ========================================================================================================================

// SystemGroup
// 一个 systemGroup 里面 可以 有很多个 system 也可以 有子 systemGroup, systemGroup 是一个树状结构，可以嵌套
// systemGroup 的作用 是 组织 system 的执行顺序，可以控制 system 的执行顺序
// systemGroup 也有 Update  可以在 重写 Update ，来调用  子 systemGroup 相关 的Update 来 定制 调用的 顺序
//          ？？？？？？？？？systemGroup 也有 OnCreate OnDestroy  可以在 重写 OnCreate OnDestroy ，来调用  子 systemGroup 相关 的OnCreate OnDestroy 来 定制 调用的 顺序
// 



// ========================================================================================================================
// Inspecting systems
// 检查 system
// 可以 可以使用 system.window 来查看 每个世界 的 system 的信息 更新的顺序, 看全部的层级的


// ========================================================================================================================
// System in Editor
// 在编辑器中 system
// 根据 icon 查看 每个 system 的类型