using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ABManager : SingletonAutoMono<ABManager>
{


    // 字典存储 加载的 AB 包
    private Dictionary<string, AssetBundle> ABDic = new Dictionary<string, AssetBundle>();


    // 主包
    private AssetBundle mianAB = null;
    // 依赖包的 获取用的配置文件 
    private AssetBundleManifest manifest = null;
    // 依赖包
    private List<AssetBundle> dependABList = new List<AssetBundle>();
    // 目标包
    private AssetBundle targetAB = null;
    // 目标包中的 资源
    private Object targetRes = null;


    /// <summary>
    /// 主包名
    /// 不同平台 包名 不一样
    /// </summary>
    private string mianABName{
        get{
#if UNITY_IOS
            return "IOS";
#elif UNITY_ANDROID
            return "Android";
#elif UNITY_STANDALONE_WIN
            return "StandaloneWindows";
            // return "StandaloneWindows_pref";
#elif UNITY_STANDALONE_OSX
            return "StandaloneOSXUniversal";
#elif UNITY_STANDALONE_LINUX
            return "Linux";
// #elif UNITY_WEBGL
//             return "WebGL";
// #elif UNITY_WSA
//             return "WSA";
// #elif UNITY_EDITOR
//             return "Editor";
#else
            return "StandaloneWindows";
#endif
        }
    }

    // AB 包方便修改  路径
    private string pathUrl{
        get{
            // return Application.streamingAssetsPath + "/StandaloneWindows64/";
            // return Application.streamingAssetsPath + "/" + mianABName + "/";

            // Debug.Log("mianABName =="+ mianABName);

            return Application.streamingAssetsPath + "/" + mianABName + "/";
            // return "http://127.0.0.1:5500/AssetsBundles/StandaloneWindows64/";
        }
    }




    // 递归加载 Ab包的依赖
    

    ///<summary>
    /// 同步 加载AB包
    /// </summary>
    /// <param name="ABName">AB包名</param>
    ///<summary>
    private void loadAB(string ABName){
        /// 加载 AB 包
        if(mianAB == null){
            // 加载 主包
            mianAB = AssetBundle.LoadFromFile(pathUrl + mianABName);
            //加载 关键依赖 文件
            manifest = mianAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        /// 获取 依赖包的 关系
        AssetBundle dependAB = null;
        AssetBundleManifest dependABmanifest = null;
        string[] dependABNameArr = manifest.GetAllDependencies(ABName);
        // 加载 依赖包
        for(int i = 0; i < dependABNameArr.Length; i++){
            string dependABPath = pathUrl + dependABNameArr[i];
            
            // 判断 包是否被加载过
            if(!ABDic.ContainsKey(dependABNameArr[i])){
                // 加载 依赖包
                dependAB = AssetBundle.LoadFromFile(dependABPath);

                // dependABmanifest = dependAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                // Debug.Log("dependABmanifest =="+ dependABmanifest);
                // // 加载 依赖包 中的 资源
                // string[] dependResNameArr = dependABmanifest.GetAllAssetBundles();
                // for(int j = 0; j < dependResNameArr.Length; j++){
                //     Debug.Log("dependResNameArr =="+ dependResNameArr[j]);
                //     // string dependResPath = pathUrl + dependResNameArr[j];
                //     // 加载 依赖包 中的 资源
                //     // dependAB = AssetBundle.LoadFromFile(dependResPath);
                // }
                // 存入 依赖包
                // 存入 字典
                ABDic.Add(dependABNameArr[i], dependAB);
            }
        }

        if(!ABDic.ContainsKey(ABName)){
            /// 加载 资源 来源包;目标包
            AssetBundle ab = AssetBundle.LoadFromFile(pathUrl + ABName);
            // Debug.Log("ab =="+ ab);
            // GameObject.Instantiate(ab);
            ABDic.Add(ABName, ab);
        }
    }



    //// <summary>
    /// 同步 加载AB包
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    public object LoadRes(string ABName, string resName)
    {

        loadAB(ABName);
        object obj = ABDic[ABName].LoadAsset(resName);
        if(obj is GameObject){
            return GameObject.Instantiate(obj as GameObject);
        } else {
            return obj;
        }
   
        
    }



    // 同步 加载 AB 包 ； 根据类型
    public object LoadRes(string ABName, string resName, System.Type type){
        loadAB(ABName);
        // 指定类型 
        object obj = ABDic[ABName].LoadAsset(resName, type);
        if(obj is GameObject){
            return GameObject.Instantiate(obj as GameObject);
        } else {
            return obj;
        }
    }


    // 同步 加载  泛型
    public T LoadRes<T>(string ABName, string resName) where T : Object
    {
        loadAB(ABName);
        // 指定类型 
        T obj = ABDic[ABName].LoadAsset<T>(resName);
        if(obj is GameObject){
            return GameObject.Instantiate(obj);
        } else {
            return obj;
        }
    }


    /// <summary>
    /// 单个 AB 包卸载
    /// </summary>
    public void unLoad(string ABName){
        if( ABDic.ContainsKey(ABName) ){
            ABDic[ABName].Unload(false);
            ABDic.Remove(ABName);
        }
    }


    /// <summary>
    /// 卸载所有AB包
    /// </summary>
    public void unLoadAll(){
        AssetBundle.UnloadAllAssetBundles(false);
        ABDic.Clear();
        mianAB = null;
        manifest = null;
    }


    /// <summary>
    /// 卸载 AB包
    /// </summary>
    /// <param name="ABName"></param>
    public void unLoadAB(string ABName){
        if( ABDic.ContainsKey(ABName) ){
            ABDic[ABName].Unload(false);
            ABDic.Remove(ABName);
        }
    }

    /// <summary>
    /// 异步 加载AB包
    /// </summary>
    /// <param name="path"></param>
    /// <param name="callback"></param>
    public void LoadResAsync(string ABName, string resName, UnityAction<Object> callback){
        // 启动协程
        StartCoroutine(ReallyLoadResAsync(ABName, resName, callback));
    }




    private IEnumerator ReallyLoadResAsync(string ABName, string resName, UnityAction<Object> callback){
        loadAB(ABName);
        // 指定类型 
        AssetBundleRequest assetBundleRequest = ABDic[ABName].LoadAssetAsync(resName);
        yield return assetBundleRequest;

        // if(assetBundleRequest.asset == null){
        // }

        if(assetBundleRequest.asset is GameObject){
            callback(GameObject.Instantiate(assetBundleRequest.asset));
        } else {
            callback(assetBundleRequest.asset);
        }
    }




    public void LoadResAsync(string ABName, string resName, System.Type type, UnityAction<Object> callback) 
    {
        // 启动协程
        StartCoroutine(ReallyLoadResAsync(ABName, resName, type, callback));
    }

    private IEnumerator ReallyLoadResAsync(string ABName, string resName, System.Type type, UnityAction<Object> callback)
    {
        loadAB(ABName);
        // 指定类型 
        AssetBundleRequest assetBundleRequest = ABDic[ABName].LoadAssetAsync(resName, type);
        yield return assetBundleRequest;
        // if(assetBundleRequest.asset == null){
        // }
        if(assetBundleRequest.asset is GameObject){
            callback(GameObject.Instantiate(assetBundleRequest.asset) );
        } else {
            callback(assetBundleRequest.asset);
        }
    }




    public void LoadResAsync<T>(string ABName, string resName, UnityAction<T> callback) where T: Object
    {
        // 启动协程
        StartCoroutine(ReallyLoadResAsync<T>(ABName, resName, callback));
    }

    private IEnumerator ReallyLoadResAsync<T>(string ABName, string resName, UnityAction<T> callback) where T : Object{
        loadAB(ABName);
        // 指定类型 
        AssetBundleRequest assetBundleRequest = ABDic[ABName].LoadAssetAsync<T>(resName);
        yield return assetBundleRequest;

        if(assetBundleRequest.asset is GameObject){
            callback(GameObject.Instantiate(assetBundleRequest.asset as T));
        } else {
            callback(assetBundleRequest.asset as T);
        }
    }

}
