using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

// public class Bridges : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

public class UIEventBridge : IComponentData
{
    public mainEventHandler handler;
}

// receiveDataFromEntities
public class InputEventBridge : IComponentData
{
    public receiveDataFromEntities handler;
}