using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

    // public class WebRequestSkipCertificate : CertificateHandler
    // {
    //     protected override bool ValidateCertificate(byte[] certificateData)
    //     {
    //         return true;
    //     }
    // }

// #region 
// // 特性
// // 本质上 就是在调用 特性类 的构造函数
// //类 函数 变量 上一行， 表示他们具有 该提醒信息
// #endregion

// class myCustomAttribute : Attribute
// {
//     public string info;
//     public myCustomAttribute(string info)
//     {
//         this.info = info;
//     }

//     public void TestFun()
//     {

//         Debug.Log("TestFun myCustomAttribute this.info = " + this.info);
//     }
// }

public class WebRequestSkipCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}


public class responseClass 
{
    public string code;
    public object data;
    public string errMsg;
    public string success;
}

// [myCustomAttribute("这是一个自定义特性1")]
public class networkManager : baseManager<networkManager>
{

    public class keyInfo{
        public  string url;
        public string timestamp;
    }

    public class dataInfo {
        public  int status = 0;

        public IEnumerator enumerator;
    }
    private Dictionary<keyInfo, dataInfo> urlDictionary = new Dictionary<keyInfo, dataInfo>();

    /// <summary>
    /// 网络管理器
    /// 
    /// 调接 http 接口
    /// 
    /// 1、获取要连接 的 ip 地址
    /// 2、post get 请求
    /// </summary>
    /// 

    public networkManager(){

        //初始化
        MonoManager.getInstance().AddUpdateListener(Update);
        Debug.Log(" == networkManager  的 构造函数");



        
        
    }


    /// <summary>
    /// Factory 调接口的方法
    /// </summary>
    /// <param name="url">url 地址</param>
    /// <param name="method">GET POST</param>
    /// <param name="jsonParams">"{\"params1\": 1,\"params2\":\"001001\"}"</param>
    /// <param name="callback">回调</param>
    public void Factory(string url, string method, string jsonParams, Action<UnityWebRequest> callback){
        // MonoManager.getInstance().StartCoroutine(
        //     HTTPMethod(url, method, jsonParams, (webRequest) => {
        //         callback(webRequest);
        //     })
        // );
        keyInfo keyInfo = new keyInfo();
        keyInfo.url = url;
        keyInfo.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        dataInfo dataInfo = new dataInfo();
        dataInfo.enumerator =  HTTPMethod(url, method, jsonParams, (webRequest) => { 
            urlDictionary.Remove(keyInfo);
            // Debug.Log("urlDictionary =="+ urlDictionary.Count);
            callback(webRequest); 
        });


        urlDictionary.Add(keyInfo, dataInfo);
    }

    




    private void Update(){
        _run();
    }

    private void _run(){

        

        // Debug.Log("urlDictionary =="+ urlDictionary.Count);

        foreach (keyInfo key in urlDictionary.Keys)
        {
            if(urlDictionary[key].status == 0){
                urlDictionary[key].status = 1;
                IEnumerator fun = urlDictionary[key].enumerator;
                MonoManager.getInstance().StartCoroutine(fun);
            }


            // if(item.status != 0){
            //     item.status = 1;
            //     IEnumerator fun =urlDictionary[item];
            //     MonoManager.getInstance().StartCoroutine(fun);
            // }
            
        }
    }






    IEnumerator HTTPMethod(string url, string type, string jsonParams = "{}", UnityAction<UnityWebRequest> callback = null)
    {
        Debug.Log("HTTPMethod url ==" + url);
        UnityWebRequest webRequest = new UnityWebRequest(url, type);
        // if(gloabNetWorkConfig.networkProtocol == "https://"){
        // }
        webRequest.certificateHandler = new WebRequestSkipCertificate();
        // webRequest 的请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", gloabNetWorkConfig.accessToken);
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // Debug.Log("timestamp =="+ timestamp);
        webRequest.SetRequestHeader("Timestamp", (timestamp).ToString());
        webRequest.downloadHandler = new DownloadHandlerBuffer();//实例化下载存贮器
        byte[] jsonToBytes = new System.Text.UTF8Encoding().GetBytes(jsonParams);
        if(type == "POST"){
            Debug.Log("jsonToBytes ==:" + jsonToBytes);
            // webRequest 带参数
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToBytes);//实例化上传缓存器
        }
        yield return webRequest.SendWebRequest();


        if (webRequest.responseCode == 200)//检验是否成功
        {
            // Debug.Log("请求成功" + webRequest.downloadHandler.text);
            string text = webRequest.downloadHandler.text;
            responseClass resObj = JsonUtility.FromJson<responseClass>(text);
            if(resObj.code == "27001007") { // token过期
                Debug.Log("token过期" + resObj);
                // Token.getInstance().refreshToken(() =>{
                // });
            } else {
                Debug.Log("token有效" + resObj);
                callback(webRequest);
            }
        }
        else
        {
            Debug.Log("请求失败：" + webRequest);
        }


    }


    



    public async Task<UnityWebRequest> aysncFactory(string url, string method, string jsonParams){
        return await RequestMethod(url, method, jsonParams);
    }

    public async Task<UnityWebRequest> RequestMethod(string url, string type, string jsonParams = "{}"){
        Debug.Log("RequestMethod url ==" + url);
        UnityWebRequest webRequest = new UnityWebRequest(url, type);
        // if(gloabNetWorkConfig.networkProtocol == "https://"){
        // }
        webRequest.certificateHandler = new WebRequestSkipCertificate();
        // webRequest 的请求头
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.SetRequestHeader("Authorization", gloabNetWorkConfig.accessToken);
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        // Debug.Log("timestamp =="+ timestamp);
        webRequest.SetRequestHeader("Timestamp", (timestamp).ToString());
        webRequest.downloadHandler = new DownloadHandlerBuffer();//实例化下载存贮器
        if(type == "POST"){
            byte[] jsonToBytes = new System.Text.UTF8Encoding().GetBytes(jsonParams);
            // Debug.Log("jsonToBytes ==:" + jsonToBytes);
            // webRequest 带参数
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToBytes);//实例化上传缓存器
        }
        await webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)//检验是否成功
        {
            // string text = webRequest.downloadHandler.text;//打印获得值
            // Debug.Log("请求成功" + webRequest.downloadHandler.text);
            return  webRequest;
        }
        else
        {
            Debug.Log("请求失败：" + webRequest.responseCode);
            return  null;
        }
    }






}
