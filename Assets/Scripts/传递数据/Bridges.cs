using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class UIEventBridge : IComponentData
{
    public UIEventHandler handler;
}

// public class InputEventBridge : IComponentData
// {
//     public InputEventHandler handler;
// }