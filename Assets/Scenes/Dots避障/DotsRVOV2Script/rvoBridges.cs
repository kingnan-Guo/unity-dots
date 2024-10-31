using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class rvoInputEventBridge : IComponentData
{
    public rvoSingleMethod handler;
}

public class rvoEventBridge : IComponentData
{
    // public mainEventHandler handler;
}
