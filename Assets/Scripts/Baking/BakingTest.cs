using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakingTest : MonoBehaviour
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

// Baking is a process that transforms GameObject data in the Unity Editor (authoring data) into entities written to entity scenes (runtime data).
// Bake 是一个过程 将 transforms GameObject data 转换成 entities  
// 把 scene 作为 subset of entities 进行 烘焙



// 在 bake 里可以定义依赖关系


// 为了保证 增量 baking  的 正常工作，需要保证 依赖关系 的正确性，  使用  DependsOn 但是我现在 不知道 这个 方法是什么意思



// ================ ==========================================================================================================

//  基于 ESC的baking Processing 整体架构
// 为了 创作方便  所以 baking  Authiring  Scene 的内容转曾 ecs 数据



// 编译器本省处理这个模式i，也是基于 ECS 架构 也是可以 job system 与 Burs 编译器来提升性能

// Authoring Scene 
// Authoring GameObject ->  Entity
// Authoring Component -> ECS Data


// Baker 处理   输出  -------------------------------------------------------
// Entity World ， 在编辑器里就可以 看到  Bake 出来的 结果， 会创建一个 mainWorld 在 Entity World 里，会把 bake 出来的 （Entity Component）  结果 写入到 mainWorld 里
// 输出 Entity + Component， 这些内容都会被放到 mainWorld 里 
// 转换的过程中  ，我们搞一个 转换的 Convert World ,在这个里面 负责把  转换的完最后  同步到  Mian world 里面
// 转换的 过程中 还有 一个 shardow world (影子 world) ，这个 记录的是上一次 转换的结果；
// 就是 比较 shadow world 和 Convert World 的差异，然后 把差异 写入到 mainWorld 里
// 如果 是 full baking  我们能会输出 一系列 的文件到 我们的磁盘，方便 运行的时候直接使用
// incremental baking  我们会 把变化的   写入到 mainWorld 里 ，  把变化的 直接转到 内存里面

// 基于 ECS模式的  =======
// 1.  把 Authoring GameObject 转换成 Entity， 只有一些 辅助数据信息 matedata ，         //? 比如名字，tag，layer，parent，child，sibling，transform，material，mesh，skinned mesh，animation，等等
// 2.  运行一些特定 system 来做一些 特定 的 处理，                                      //? 比如 生成 mesh，生成 skinned mesh，生成 animation，等等
// 3. 对需要 baking 的物体的 来调用它的 baker ，  把结果写入到 mainWorld 里
// 4.  定义 一些特定的 system 来统一 处理好 所有生成的 entity 与 component  ；  
//      4-1： 比如 ： BoxCollider  -> bake -> 的数据 -> system 全部来处理 这些数据 直接 运行 可以用
// 5.还给一个时机  system group 来处理 最后一些 相关的 工作


// 执行的时机  ---
// 1、Pre baker system
// 2、Baker
// 3、baker system 来批量处理 与 迭代对应的 数据
// 4、最后结尾 的 system group 来处理 最后一些 相关的 工作

// Entity ---> convert world + shardow world ---> main world


// Baking System 详解 ----------------------------------
// Baking 基于 ECS 模式下特定处理算法  -> baking system
// baking system 是批量处理 整个世界world 里面 特定的  entity 组件 与 实体
// 因为 本身 baking system 是一个 system ，所以 它可以 使用 job system 和 burst机制 来提升性能（多线程）
// Baking system  与 bake 不一样的 点是， baking system 是一个 system ，它处理的是整个 world 里面的 entity 和 component （ecs componentData） ，而 bake 只处理一个 entity 和 component
// 当有些操作时成批的 处理已经 转换好 的数据的时候， 使用 bake system ，可以 提升性能
// 
// 所有 的 Baking syste 都发生在 创建我们的 Entities 以后才会调用，但是  特例  PreBakingSystemGroup 这个分组的 system 是 执行在所 Entities 创建之前
// 其他分组的 baking  system 里面的 system 可以访问全部 Baker 转好 的 entity 与component  ----> 定义 特定的处理

// baking system 不会自动 判断依赖，必须要手动定义 依赖关系，
// baking system  在任何时候都可以访问 world ，包含创建的新的 entity； 可以以 任意的 方式 操作 它的  world，包括创建 新的 entity，修改 entity 的 component，删除 entity，等等, 但是 在 system 创建 的 entity 不会在 场景Bake结束后跟着 销毁；
// 也就是在 system 阶段 创建 的 entity 不会是可以留在我们的世界里面的 **** 
// 也可以 创建 entity 在对各 baking system 之间进行数据传递。如果希望 entity 在场景 Bake结束后跟着 销毁，需要 在 baker 里创建， 在 baker 里面常见的entity 就会在 baker 结束后 销毁 ，CreateAddtitionalEntity






// 如何编译自己的 baking system -------------------------------
// 给 system 做一个特定的注解 [WorldSystemFilter(WorldSystemFilterFlags.Baking)] ，这个注解可以指定 system 的执行时机，比如 PreBakingSystemGroup，BakingSystemGroup，PostBakingSystemGroup，等等





// baking 的主要阶段 baking phases

// 1. Enity creation Enity 创建 ： 在 baking 之前， Unity 会常见出来场景里面的每个 GameObject  对应的 entity， 这个时候 entity 是空的 容器 没有ComponentData , 只有 部分的 internal metadata 辅助数据 
// 2. Baker阶段 就是  Authoring GameObject 单个 转换成 Entity + component
//  2-1: run baker 执行每个数据 的 bakes 来完成转化， 每个baker 处理自己的类型， 同一种类型的 component 使用同一种 baker； eg: EntityA EntityB 都有 Authroing Components ， 那么就会使用同一个 baker 来处理
//  2-2： 到多时候 都是使用 系统默认的 Baker 
//  2-3： 运行 baker 的顺序 是不固定的 ，所以在这个过程中 不要 交叉依赖，不能从 entity conponet 里面 去度取其他 entity 的数据，转 A 的时候  不能去读  B 的数据，这时  B 可能还没转好
//  2-4： 只能往 entiy 里添加新的组件


// 3. Baking System 阶段 就是 所有的 entity + component 做 批量的数据处理， 提升 性能




// baking system phases
// 1. Pre baking system group  在 baker 之前来调用 它分组下面的 所有的 system   
// 2、TransformBajingSystemGroup 就在 Baker 之后这个分组 金额以用来让我们做一些特定的数据转换
// 3、 BakingSystemGroup 在 baker 之后，用来 批量处理 所有的 entity + component
// 4、PostBakingSystemGroup 在 baking system 之后 做一些出路

// Baking  World 详解  ---------- Baking worlds overview 
// 每个 Authroing Scene 在 bake的时候 都是独立的分开来处理 ，每一个都i有一个 world
// 后台每次 一个一个场景来进行bake 处理
// 打开多个  sub scene 的时候，每个 sub scene 都有两个  独立的 world，一个  Conversion world  Shadow world

//  比较两次的 bake结果，可以对比  Conversion world 和 Shadow world 的变化， 存到 main world 里


// 猜测 *********** 在后台编辑器模式下  mian world 可能是 一个， 每个 subscene 都会有一个  Conversion world 和 Shadow world  ************************





// Filter baking output ===
//  [BakingOnlyEntity ]



// prefabs in baking 
// ESC 里面 我们也可以包 entity 做成 prefab 
// Entity prefab 
// 第一步 。 把GameObject Prefab Baking 成 enity prefab
// 第二部 才是使用我们的  entity Prefab 来进行复制 我们的 entity 
// 
// entity prefab 是一种特殊的  entity： 
// prefab tag 标记 却别entity prefab与 普通的  却别entity，默认查询 的 规则就是根据  tag 排除掉 prefab entity
// （2）linkedEntityGroup ： 理解成一种数据结构保存了所有的孩子的 entity 的引用； 利用 （2）linkedEntityGroup 可以方便的找到 子 entity ， GameObject ==》 entity ，就可以把 entity 与 孩子 entity 都一起创建出来
// （3）在entity scene中 能找得到

// 注意： 如果 Authoring Scene 有一个预制体关联 这个节点，那么 baking 处理的时候，不会把这个场景 预制体关联 的 节点当作 普通节点，不会当作预制体 去 bake

// ===
// 如何把 首先把GameObject Prefab Baking 成 entity Prefab  ==》 Baker 里面来处理
// 这样 就以为 有一个依赖： bake 的时候 依赖于 GameObject
// 当baking 出来以后， baking 出来就会 存放到 subscene， 
















