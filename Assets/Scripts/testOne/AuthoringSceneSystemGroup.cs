using Unity.Entities;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.SceneManagement;

// namespace DOTS.DOD
// {

// }



    public abstract partial class AuthoringSceneSystemGroup : ComponentSystemGroup
    {
        private bool initialized;
        protected override void OnCreate()
        {
            base.OnCreate();
            initialized = false;

            UnityEngine.Debug.Log("AuthoringSceneSystemGroup OnCreate");
            
        }

        protected override void OnUpdate()
        {
            // UnityEngine.Debug.Log("AuthoringSceneSystemGroup OnUpdate");
            if (!initialized)
            {
                if (SceneManager.GetActiveScene().isLoaded)
                {
                    var subScene = Object.FindObjectOfType<SubScene>();
                    // Debug.Log("AuthoringSceneSystemGroup subScene: " + subScene);
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

        protected abstract string AuthoringSceneName { get; }
    }

