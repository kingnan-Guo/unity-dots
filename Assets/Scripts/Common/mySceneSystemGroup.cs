using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract partial class mySceneSystemGroup : ComponentSystemGroup
{
    protected abstract string AuthoringSceneName { get; }
    private bool initialized;
    protected override void OnCreate()
    {
        base.OnCreate();
        initialized = false;

        UnityEngine.Debug.Log("createEntityFormMeshSceneSystemGroup OnCreate");
        
    }

    protected override void OnUpdate()
    {
        // UnityEngine.Debug.Log("createEntityFormMeshSceneSystemGroup OnUpdate");
        if (!initialized)
        {
            UnityEngine.Debug.Log("createEntityFormMeshSceneSystemGroup OnUpdate");
            if (SceneManager.GetActiveScene().isLoaded)
            {
                var subScene = Object.FindObjectOfType<SubScene>();
                // Debug.Log("createEntityFormMeshSceneSystemGroup subScene: " + subScene);
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

// public partial class createEntityFormMeshSceneSystemGroup : AuthoringSceneSystemGroup
// {
//     protected override string AuthoringSceneName => "createEntityFormMesh";
// }


