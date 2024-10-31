using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;



public class AssetBundleMainMapInfo {
    public static string platform = "StandaloneWindows"; /// web 
    public static string AssetBundleName = "StandaloneWindows";
    // ip + /scf3DStatic/uploads/StreamingAssets/StandaloneWindows/StandaloneWindows
    public static string pathUrl = "/scf3DStatic/uploads/StreamingAssets/";

    public static string buildingInfoJsonName = "buildingInfo.json";
    // public static string[] nameArr = new string[]{"mainmap", "treemap"};
}



public class networkLoadABManager : baseManager<networkLoadABManager>
{

    private AssetBundle _mianAB = null;
    private AssetBundleManifest _manifest = null;
    // 字典存储 加载的 AB 包
    private Dictionary<string, AssetBundle> ABDictionary = new Dictionary<string, AssetBundle>();

    private string AssetBundleMainMapInfoPathUrl = AssetBundleMainMapInfo.pathUrl;

    private string AssetBundleMainMapInfoPlatform = AssetBundleMainMapInfo.platform;
    

    // string pathUrl = "/opt/evo/evo-subsystem/uploads/StreamingAssets/StandaloneWindows/";

    public networkLoadABManager(){
    }

    public string assembleUrl(string url){
        StringBuilder stringBuilder= new StringBuilder();
        stringBuilder.Append(gloabNetWorkConfig.networkProtocol);
        stringBuilder.Append(gloabNetWorkConfig.ip);
        if(gloabNetWorkConfig.port != null){
            stringBuilder.Append($":{gloabNetWorkConfig.port}");
        }
        stringBuilder.Append(url);//请求地址
        return stringBuilder.ToString();
    }


    public void getModels(string pathUrl, UnityAction<UnityWebRequest> action){

        string url = assembleUrl(pathUrl);
        // string jsonParams = "{\"pageNum\": 1,\"pageSize\": 0, \"isLabeled\": 1}";
        networkManager.getInstance().Factory(url, "GET", "", (webRequest) => {
            Debug.Log("getModels  Factory 请求成功");
            // Debug.Log("newWorkDeviceList =="+newWorkDeviceList.Count);
            action(webRequest);
        });
    }

    public Task<UnityWebRequest> getModelsTask(string pathUrl){
        string url = assembleUrl(pathUrl);
        return networkManager.getInstance().aysncFactory(url, "GET", "");
    }



    public async Task loadDepend(AssetBundleManifest manifest, string mainModelname, string source){
            // Debug.Log("loadDepend 2");
            List<Task<UnityWebRequest>> allTask = new List<Task<UnityWebRequest>>();
            string[] dependABNameArr = manifest.GetAllDependencies(mainModelname);
            // AssetBundle dependAB = null;           
            for(int i = 0; i < dependABNameArr.Length; i++){
                string dependABPath = AssetBundleMainMapInfoPathUrl + AssetBundleMainMapInfoPlatform + "/" + dependABNameArr[i];
                // Debug.Log("dependABPath =="+dependABPath + "  = dependABNameArr[i] =="+ dependABNameArr[i]);

                if(!ABDictionary.ContainsKey(dependABNameArr[i])){
                    // 加载 依赖包
                    // dependAB = AssetBundle.LoadFromFile(dependABPath);
                    // getModels(dependABPath , (dependABPathwebRequest)=>{
                    //     AssetBundle dependAB = AssetBundle.LoadFromMemory(dependABPathwebRequest.downloadHandler.data);
                    //     Debug.Log("dependABPath ="+ dependAB);
                    // });
                    Task<UnityWebRequest> aa = getModelsTask(dependABPath);

                    allTask.Add(aa);
                    // mainModel(mainModelname, source);
                }
            }




            UnityWebRequest[] UnityWebRequestArr = await Task.WhenAll(allTask.ToArray());

            for (int i = 0; i < UnityWebRequestArr.Count(); i++)
            {
                if(!ABDictionary.ContainsKey(dependABNameArr[i])){
                    AssetBundle dependAB = AssetBundle.LoadFromMemory(UnityWebRequestArr[i].downloadHandler.data);
                    ABDictionary.Add(dependABNameArr[i], dependAB);
                }
            }

            // foreach (UnityWebRequest item in UnityWebRequestArr)
            // {
            //     AssetBundle dependAB = AssetBundle.LoadFromMemory(item.downloadHandler.data);
            //     ABDictionary.Add(dependABNameArr[i], dependAB);
            // }

            // Task<int> task1 = Task.Delay(2000).ContinueWith(ite => 1);
            // Task.WhenAll(allTask.ToArray()).ContinueWith(task =>{
            //     Debug.Log("task =="+ task);
            // });
            //     Task<int> rask1 = Task.Run(()=>{
            //         return 1;
            //     });
            //    Task<int[]> result = Task.WhenAll(rask1);




    }

    public async Task<GameObject> mainModel(string ABName, string modelprefabName = "Assets/Prefabs/mainMap.prefab"){
        GameObject gameObject = null;
        if(!ABDictionary.ContainsKey(ABName)){
            UnityWebRequest mainModelwebRequest =await getModelsTask(AssetBundleMainMapInfoPathUrl+ AssetBundleMainMapInfoPlatform +  "/" +ABName);
            AssetBundle ABD = AssetBundle.LoadFromMemory(mainModelwebRequest.downloadHandler.data);
            ABDictionary.Add(ABName, ABD);          
            return  ABD.LoadAsset<GameObject>(modelprefabName);
        }
        return gameObject;
    }

    // "mainmap", "Assets/Prefabs/mainMap.prefab"
    public async Task<GameObject> loadABAsync(string url, string ABName, string resName){
        if(_mianAB == null){
            UnityWebRequest webRequest = await networkLoadABManager.getInstance().getModelsTask(url);
            AssetBundle mianAB = AssetBundle.LoadFromMemory(webRequest.downloadHandler.data);
            _mianAB = mianAB;

            AssetBundleManifest manifest = mianAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            _manifest = manifest;
        }

        await networkLoadABManager.getInstance().loadDepend(_manifest, ABName, resName);
        // Debug.Log("loadDepend 1");
        GameObject go = await networkLoadABManager.getInstance().mainModel(ABName, resName);
        // GameObject goi = GameObject.Instantiate(go);
        return  go;
    }


    public async Task<object> LoadResAsync(string url,string ABName, string resName)
    {

        await loadABAsync(url, ABName, resName);
        object obj = ABDictionary[ABName].LoadAsset(resName);
        if(obj is GameObject){
            return GameObject.Instantiate(obj as GameObject);
        } else {
            return obj;
        }
   
        
    }





}
