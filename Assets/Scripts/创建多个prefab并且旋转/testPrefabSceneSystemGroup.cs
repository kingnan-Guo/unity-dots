using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

// public class testPrefabSceneSystemGroup : MonoBehaviour
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



public abstract partial class testPrefabSceneSystemGroup : ComponentSystemGroup
{
    protected abstract string AuthoringSceneName { get; }
    private bool initialized;
    protected override void OnCreate()
    {
        base.OnCreate();
        initialized = false;

        // UnityEngine.Debug.Log("testPrefabSceneSystemGroup OnCreate");
        
    }

    protected override void OnUpdate()
    {
        // UnityEngine.Debug.Log("testPrefabSceneSystemGroup OnUpdate");
        if (!initialized)
        {
            // UnityEngine.Debug.Log("testPrefabSceneSystemGroup OnUpdate");
            if (SceneManager.GetActiveScene().isLoaded)
            {
                var subScene = Object.FindObjectOfType<SubScene>();
                // Debug.Log("testPrefabSceneSystemGroup subScene: " + subScene);
                if (subScene != null)
                {
                    // Debug.Log("AuthoringSceneName  " + AuthoringSceneName + " == subScene " + subScene.gameObject.scene.name );
                    Enabled = AuthoringSceneName == subScene.gameObject.scene.name;
                    
                }
                else
                {
                    Enabled = false;
                }
                initialized = true;
            }
        }
        base.OnUpdate();
    }

    
}

