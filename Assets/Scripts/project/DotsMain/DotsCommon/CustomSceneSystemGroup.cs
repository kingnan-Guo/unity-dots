using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract partial class CustomSceneSystemGroup : ComponentSystemGroup
{
    protected abstract string AuthoringSceneName { get; }
    private bool initialized;
    protected override void OnCreate()
    {
        base.OnCreate();
        initialized = false;
    }

    protected override void OnUpdate()
    {
        if (!initialized)
        {
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

