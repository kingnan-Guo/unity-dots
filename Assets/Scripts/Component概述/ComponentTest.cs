using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
// 1、Component 时存放  Entity 数据的载体
// 2、Component 里面的这个数据是给System 算法使用的
// 3、Component 是一个抽象类，不能实例化
// 4、定义一个 ComponentData/Component  struct Component : IComponentData, 什么作用都没有 只是用来标记 这个 组件类型 是一个  Component
// 5、如果 组件 ComponentData 继承 IComponentData 他里面的数据成员 只能是 非托管（unmanaged data） 数据类型，不能被垃圾回收； 如 int float bool； 可以带一些方法，但是不能有引用类型
// 6、如果要创建 可以托管的 ComponentData ，就把这个 ComponentData 定义成 class 就可以


// ========
// 拥有不同类型组件 的组合 把他设置成一个 ArchType
// EntityA ： ComponentA ComponentB ComponentC 特定组合 变为  ArchType；
// EntityB ： ComponentD ComponentE ComponentF 特定组合 变为  ArchTypeB；
// 每个 ComponentData 是 chunk 里面的一个数据成员，每个 chunk 大小是 16kb

// ====

// chunk [
//     ArteType_size , // 根据 ArteType_size 大小 分配内存
// 
// ]

// 假设 EntityA 有 10 个  类型是  ArchTypeA； EntityB 有 20 个 类型是  ArchTypeB ；
// 那么就从 chunk 里面分配 10 个 EntityA 的内存， 每个 内存的大小是 ArteType_size ，然后每个 EntityA 的内存里面存放 ComponentA ComponentB ComponentC，直到 chunk 的 16kb 分配完
// 存储 EntityB 是要 new  chunk ，然后每个 chunk 里面存放 EntityB 的数据，直到 chunk 分配完

// 每一个 Entity 都是存放 特定组合的内存

// 如果 Entity 调用 RemoveComponent ，那么这个 Entity 的内存 就会被回收，然后 chunk 里面的内存也会被回收 =》 系统 会 改变 Entity 的 结构 ，重新分配一个 新的 ArchType ，然后 拷贝数据 到新的内存里面去
// 但是 如果 是 Enable Component ， ArchType 不会改变 ，不会引发 内u才能分配


//  ===========
// 往 subscene 中 的 gameObject添加 自定义的 ComponentData
// subscene 的 作用 是， Unity 会自动启动 我们一个 bake 的流程，把 subscene 里面的数据，导出成二进制文件，然后存放到一个文件里面，这个文件是只读的，不能修改，只能读取
// bake ===》启动 Bake  会变成  == 》Entity  里面包含  ComponentData;
// 这些 ComponentData  都是 自带组件 ；  转过来？？？
// 如果 有自定义的 组件 ComponentData ，加到 转完 之后  的 Entity 里
// 当我们 bake 一个  gameObject 的时候， 会把自定义的数据 组件  生成出来





// ===============

// 先把普通法的额物体 转为  Entity 的代码  ===== 》这个过程 称为  Authoring  （负责 bake 普通数据  到 Entity  里面） 

// 编译的时候 会把 subscene 里面 普通 的物体 bake 成 Entites
//  这时  要把 自定义的 Component 加到 gameobject 里 ，就要借助 Authoring  自定义 一个 bake 出来

// 第一步 新建一个  Authoring 这个组件类 
// 第二步 要给 这个组件类  ===》实例化后 ==》挂到节点上， 当场景 整个 subscene 的时候 Bake ， 如何转换这个 组件 实例 ， 要求 这个组件类定义一个 bake ，系统按照 这个 bake 的方法，把 实例 转换为 Entity
// 第三步  定义 ComponentData 类型