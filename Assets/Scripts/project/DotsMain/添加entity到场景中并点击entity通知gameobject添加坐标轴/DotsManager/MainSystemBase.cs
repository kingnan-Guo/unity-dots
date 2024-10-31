using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// Base class for all systems in the game.
/// </summary>

// [DisableAutoCreation]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(PrefabSceneSystem))]
[BurstCompile]
public partial class MainSystemBase : SystemBase
{
    protected override void OnCreate()
    {
        Debug.Log("MainSystemBase OnCreate");
        // RequireForUpdate<AssetsReferences>();
    }

    protected override void OnUpdate()
    {
        Debug.Log("MainSystemBase OnUpdate");
    }
}

