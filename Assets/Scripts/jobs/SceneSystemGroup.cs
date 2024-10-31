using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract partial class SceneSystemGroup : ComponentSystemGroup
{
    private bool initialized;
    protected abstract string SceneName { get; }
    protected override void OnCreate()
    {
        base.OnCreate();
        initialized = false;
        // Enabled = false;


        if (SceneManager.GetActiveScene().isLoaded)
        {

            // Debug.Log("SceneManager.GetActiveScene().name == " + SceneManager.GetActiveScene().name );
            // var subscenes = Object.FindAnyObjectByType<SubScene>();
            // 获取所有的 subScene

            // Debug.Log("SceneName: " + subscenes.gameObject.name );

            // if(subscenes != null){
            //     Enabled = SceneName == subscenes.gameObject.name;
            // } else {
            //     Enabled = false;
            // }

            // 获取所有的 subScene 

            Object[] subscenes = Object.FindObjectsOfType<SubScene>();
            // Debug.Log("Found subscene objects: " + subscenes);
            foreach (SubScene item in subscenes)
            {
                // Debug.Log("subscene.gameObject.name: " +item.gameObject.name + "== SceneName==="+ SceneName );
                if(item.gameObject.name == SceneName){
                    Enabled = true;
                    break;
                } else {
                    Enabled = false;
                }
            };

            // if (subscenes.Length > 0){
            // }

            initialized = true;

        }



        
    }

    protected override void OnUpdate()
    {
        if (!initialized)
        {
            if (SceneManager.GetActiveScene().isLoaded)
            {

                // Debug.Log("SceneName: " + SceneName );
                // Debug.Log("SceneManager.GetActiveScene().name: " + SceneManager.GetActiveScene().name);
                
                // Enabled = SceneName == SceneManager.GetActiveScene().name;
                // initialized = true;


                var subscenes = Object.FindAnyObjectByType<SubScene>();
                // Debug.Log("SceneName: " + subscenes.gameObject.name );
                // Enabled = false;
                if(subscenes != null){
                    Enabled = SceneName == subscenes.gameObject.name;
                } 
                // else {
                //     Enabled = false;
                // }
                initialized = true;

            }

            // var subscenes = Object.FindAnyObjectByType<SubScene>();

            // Debug.Log("Found subscene object: " + subscene);



        }
        base.OnUpdate();
    }

    
}



