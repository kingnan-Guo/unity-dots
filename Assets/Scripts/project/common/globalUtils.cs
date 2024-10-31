using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class globalUtils : baseManager<globalUtils>
{


    // public globalUtils(){
    //     // Debug.Log("utils");
    //     //初始化
    //     // MonoManager.getInstance().AddUpdateListener(myUpdate);
    // }


    /// <summary>
    /// 全局 全局 function 阻挡 射线
    /// 不传参数 忽略 所有   canvas 图层
    /// </summary>
    /// <param name="IgnoreRaycastTagList">阻挡射线的 标签 集合名称</param>
    /// <returns></returns>
    public bool UIElementsBlockRaycast(List<string> IgnoreRaycastTagList = null)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        // gr.Raycast(eventData, list);
        EventSystem.current.RaycastAll(eventData, list);
        // Debug.Log("UIElementsBlockRaycast ==" + list.Count + "IgnoreRaycastTagList =="+ IgnoreRaycastTagList?.Count);
        if(IgnoreRaycastTagList != null && IgnoreRaycastTagList.Count > 0){
            bool tagert = false;
            foreach (RaycastResult item in list){
                // Debug.Log("UIElementsBlockRaycast item =="+ item.gameObject.name);
                if (IgnoreRaycastTagList.Contains(item.gameObject.tag)){
                    // return true;
                    tagert = true;
                }
            }
            return tagert;

        } else {
            return list.Count > 0;
        }
    }


    /// 截流
    /// /// <summary>
    /// 
    /// </summary>
    /// <param name="time">间隔时间</param>
    /// <param name="action">执行方法</param>
    /// <returns></returns>
    public IEnumerator DelayInvoke(float time, System.Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }






    // public virtual 

    /// <summary>
    /// 过滤函数
    /// T 输出类型
    /// K 输入类型
    /// </summary>
    // /// <param name="data"></param>
    public T filterFunctionOfClass<T, K>(K data) where T : new()
    {
        T res = new T();

        System.Type TClass = typeof(T);

        // TClass.InvokeMember ("", 
        // BindingFlags.InvokeMethod | BindingFlags.Default, null, new T(),
        //     new object [] {});

        // PropertyInfo[] publicProperties = TClass.GetProperties();
        // foreach (var property in publicProperties)
        // {
        //     Debug.Log("property.Name = " + property);
        // }

        FieldInfo[] FieldInfoArr = TClass.GetFields();
        foreach (var item in FieldInfoArr)
        {
            // Debug.Log("FieldInfo.Name = " + item.Name);

            // Debug.Log("FieldInfo.GetValue(res) = " + item.GetValue(res));

            // Debug.Log("GetMethod  = " + TClass.GetMethod("get"+ item.Name)?.Name);
            
            item.SetValue(res, TClass.GetMethod("get"+ item.Name)?.Invoke(res, new object[] { data}));

        }

        // MemberInfo[] memberInfos = TClass.GetMembers(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly );
        // foreach (var item in memberInfos)
        // {
        //     Debug.Log("GetMembers = " + item);
        // }
        // BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
        //BindingFlags.Instance | BindingFlags.Public

        // BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;

        // 获取所有属性
        // PropertyInfo[] propertyInfos = TClass.GetProperties(flag);
        // foreach (var property in propertyInfos)
        // {
        //     // Console.WriteLine($"{property.Name}: {property.GetValue(people)}");

        //     Debug.Log("propertyInfos = " + property);
        // }
        // 获取所有方法
        // MethodInfo[] methodInfos = TClass.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
        // foreach (var method in methodInfos)
        // {
        //     // Console.WriteLine($"{method.Name}");
        //     Debug.Log("methodInfos = " + method.Name);

        //     Debug.Log("methodInfos = " +  method.Invoke(res, new object[] { data}));

        //     // fieldInfo_key.SetValue(di, value);

        //     // TClass.

        //     // ParameterInfo[] parameterInfos = method.GetParameters();

        //     // foreach (var parameter in parameterInfos)
        //     // {
        //     //     Debug.Log("parameterInfos = " + parameter.Name + " parameter=="+ parameter.ParameterType.Name);
        //     // }

        // }


        // 查看类构造函数
        // ConstructorInfo[] constructorInfos = TClass.GetConstructors();    
        // foreach (ConstructorInfo constructor in constructorInfos)
        // {
        //     ParameterInfo[] ps = constructor.GetParameters();   //取出每个构造函数的所有参数
        //     foreach (ParameterInfo pi in ps)                    
        //     {
        //         // Console.Write(pi.ParameterType.ToString() + " " + pi.Name + ",");

        //         Debug.Log("pi.ParameterType.ToString() = " + pi.ParameterType.ToString() + " =="+ pi.Name);
        //     }
        // }

        // Debug.Log("res = === " + (res as BuildingInfoClass).BuildingTag);

        return res;
    }


    // public T filterFunctionOfBoject<T, K>(K name) where T : System.Object
    // {
    //     // System.Type TClass = typeof(T);

    //     // T res = ;
    //     return default(T);
    // }
    public void filterFunction(string name)
    {
        // return name;
        Debug.Log("filterFunction name =="+ name);
    }




    /// <summary>
    /// filterSpecified 过滤器 
    /// 用于传入过滤方法 并返回结果
    /// T 输出类型
    /// K 输入类型
    /// </summary>
    /// <param name="generateKey"></param>
    public void filterSpecified<T, V, K>(System.Func<K, V> generateKey, K name, UnityAction<V> callback = null){

        // Func<string> aa = generateKey;
        // generateKey(name);
        // System.Func<T> a = (System.Func<T>)generateKey;

        // object aa = generateKey<T>(name);
        callback(generateKey(name));

        // GenerateKey generateKey = new GenerateKey();
        // typeof (GenerateKey).GetField("filterSpecified").SetValue(generateKey, true);
 
    }




    public TR filterFunctionOfListCustom<TR, T>(T data)
    {
        // T res = new T();
        // return res;
        return default(TR);
    }

    /// <summary>
    /// filterSpecifiedList 过滤器
    /// </summary>
    /// <typeparam name="T">System.Func<T, K, TR> 的输入参数 泛型</typeparam>
    /// <typeparam name="TR">System.Func<T, K, TR> 的输输出参数 泛型</typeparam>
    /// <typeparam name="L">filterSpecifiedList 过滤器 的 List 的泛型</typeparam>
    /// <typeparam name="K">filterSpecifiedList 过滤器 keyWorld 的泛型， 会传输到 System.Func<T, K, TR> 的 K 用于过滤条件</typeparam>
    /// <param name="generateKey">传入的过滤规则的函数</param>
    /// <param name="dataList">传入的 list 数据 用于 for 循环  使用 generateKey 去过滤</param>
    /// <param name="keyWorld">过滤条件的 参数</param>
    /// <param name="callback">返回值 参数，  泛型 为 TR</param>
    public void filterSpecifiedList<T, TR, L, K>(
        System.Func<T, K, TR> generateKey, 
        L dataList, 
        K keyWorld,
        UnityAction<List<TR>> callback = null){

        // List<B> list= new List<B>();
        // B data = new B();

        // object b = new object();
        // srr<TR> rt = new List<TR>();
        // TR[] rt = new TR[]{};

        List<TR> rt = new List<TR>();

        // typeof(L).GetMethod("ForEach").Invoke(dataList, new object[] {
            
        // });


        // Debug.Log("dataList =="+ typeof(L).IsGenericType);
        // Debug.Log("dataList =="+ typeof(L).GetGenericTypeDefinition());
        ///  适配  objetc[] 和 list<object>
        // (typeof(L).IsGenericType ?  (dataList as List<object>) :  (dataList as object[]).ToList()).ForEach((item) => {

        //     // args[0] = item;
        //     // args[1] = keyCode;
        //     // res = (TR[])typeof(L).GetMethod("filterFunctionOfListCustom").Invoke(dataList, args);
        //     // res = (TR[])generateKey(item, keyCode);
        //     // res = (TR[])generateKey(((item as GameObject).name), keyCode);

        //     TR a =  generateKey((T)(item), keyWorld);
        //     if(a != null){
        //         rt.Add(a);
        //     }

        // });

        // Debug.Log("filterSpecifiedList =dataList ="+ dataList);
        // Debug.Log("filterSpecifiedList = keyWorld ="+ keyWorld);


        // IEnumerable<object>  协方差  
        (dataList as IEnumerable<object>).ToList().ForEach((item) => {
            //  Debug.Log("filterSpecifiedList = item ="+ item);
            TR a =  generateKey((T)(item), keyWorld);
            // Debug.Log("a =="+ a + " default(TR) =="+ default(TR) + "==="+ EqualityComparer<T>.Default.Equals(a));
            // Debug.Log("IsDefault<TR>(a) ==" + IsDefault<TR>(a));
            if(a != null && !IsDefault<TR>(a)){
                // Debug.Log("IsDefault ==="+ IsDefault(a) + " ==" + default(TR) == default(TR));
                
                // Debug.Log("IsValueType ==="+ typeof(TR).IsValueType);

                // Debug.Log(" IsNullable =="+ IsDefault<TR>(a));
                
                rt.Add(a);
            }
        });
        // Debug.Log("(dataList as object[])  =="+ ((dataList as object[]) != null ? (dataList as object[]).ToList() : (dataList as List<object>)));


        // if(typeof(L).IsGenericType ){
        //     // 这个特性称为泛型协方差；要获得更多信息，请搜索C#协方差，您将看到大量解释其工作原理的文章。
        //     // 特别要注意的是，协方差只适用于引用类型；您不能将整数序列用作对象序列。你明白为什么吗？
        //     // 或者，如果您手头有一个IEnumerable，则可以将任何序列的副本复制到带有mysequence.Cast<object>().ToList()的对象列表中。但这是一个副本，而不是一个参考转换。
        //     Debug.Log("dataList =="+ typeof(L).GetGenericTypeDefinition());
        //     Debug.Log("(dataList as List<object>).Count(); =="+ (dataList as IEnumerable<object>).ToList());
        // } else {
        //     Debug.Log("(dataList as List<object>).Count(); 222222=="+ (dataList as IEnumerable<object>).ToList());
        // }



        // Debug.Log("dataList as object[]    =="+ dataList + " =="+  (dataList as List<object>));
        // Debug.Log("typeof(L).IsGenericType    ==" +  typeof(L).IsGenericType + " === "+ (typeof(L).IsGenericType ? (dataList as List<object>) :  (dataList as object[])) );



        

        // Debug.Log("dataList =="+ typeof(L));

        // Debug.Log(" rt  =="+ rt);


        callback((rt));





            // generateKey<T, R>();
            // System.Func<T, R> a = (System.Func<T, R>)generateKey;

            // (T)key;
    




 
    }




    /// <summary>
    /// 判断是否为默认值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsDefault<T>(T value)
    {

        return EqualityComparer<T>.Default.Equals(value, default(T));
        // return typeof(T).IsValueType && EqualityComparer<T>.Default.Equals(value, default(T));
    }


    public static bool IsNullable<T>()
    {
        return typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(System.Nullable<>);
        
    }


    





    /// <summary>
    /// 获取本地路径的 json 文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonFolderPath"></param>
    /// <param name="jsonFileName"></param>
    public T LoadJson<T>(string jsonFolderPath, string jsonFileName)
    {
        string filePath = Path.Combine(jsonFolderPath, jsonFileName);
 
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            T data = JsonUtility.FromJson<T>(json);
            // 使用data中的数据
            // Debug.Log($"Key: {data.key}, Value: {data.value}");

            return data;
        }
        return default(T);
    }



    public void receiveJsonDateFormResources<T>(string path = "json/test", UnityAction<T> callback = null) where T : new()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        // Debug.Log("receiveJsonDateFormResources == "+textAsset.text);
        // 调用 同级 JsonDataManager.cs 文件下 的JsonDataManager 类
        T info = JsonDataManager.getInstance().LoadData<T>(textAsset.text);

        // Dictionary<string, T> dataDic = new Dictionary<string, T>();

        // info.GetType().GetFields().ToList().ForEach((item) => {
            
        // });

        // Dictionary<string, T> dataDic = new Dictionary<string, T>();
        // Debug.Log(info.data.Length);
        // for (int i = 0; i < info.data.Length; i++)
        // {
        //     dataDic.Add(info.data[i].name, info.data[i]);
        // }
        // // deviceInfo deviceinfo = (deviceInfo)dataDic["123321123321123$1$1$1"];
        callback(info);
    }








    //// ========================= 多线程 ================
    
    /// <summary>
    /// 创建线程池
    /// </summary>
    public void creatThreadingPool(Action action){
        // ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork), null);
        ThreadPool.QueueUserWorkItem(DoWork, action);
    }

    public void DoWork(object action)
    {
        // 这里写你的耗时操作
        // Debug.Log("DoWork");
        ((Action)action)();
    }




    //// =============== 数据库 ==================
    
    // public List<T> SelectByImei<T>(string sql){

    //     List<T> dataList = new List<T>();

    //     return dataList;
    //     // return null;
    // }

    public void createClassData<T>(){

    }




    // 摄像机 视角切换
    
    public void changeCameraPosition(Transform transform,  int distance = 200){
        gloabCameraLookAtInfo gloabCameraLookAtInfo = new gloabCameraLookAtInfo();
        gloabCameraLookAtInfo.position = transform.position;
        // 如果当前 设备 的 父级 是 楼层那么 中心点的 高度是楼层 的 高度，如果 是 MAIN_MAP 那么 中心点的高度是 设备的高度； 暂不启用
        if(!transform.parent.CompareTag(gloab_TagName.MAIN_MAP) && false){
            Vector3 vector3 = new Vector3(transform.position.x, transform.parent.position.y, transform.position.z);
            gloabCameraLookAtInfo.position = vector3;
        }
        gloabCameraLookAtInfo.distance = distance;
        gloabCameraLookAtInfo.direction = 0;
        EventCenterOptimize.getInstance().EventTrigger<gloabCameraLookAtInfo>(gloab_EventCenter_Name.CAMERA_POSITION, gloabCameraLookAtInfo);
    }




    public GameObject FindGameObjectRecursive(Transform parent, string name)
    {
        if (parent == null)
            return null;
 
        if (parent.gameObject.name == name)
            return parent.gameObject;
 
        foreach (Transform child in parent)
        {
            GameObject found = FindGameObjectRecursive(child, name);
            if (found != null)
                return found;
        }
 
        return null;
    }




    // 数据库相关 ============

    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <param name="list"></param>
    /// <param name="operate"></param>
    /// <param name="className"></param>
    /// <returns></returns>
    public MyEventArgs<string, List<BaseData>> createMyEventArgs(List<BaseData> list, string operate, string className){
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>(className, list);
        return args;
    }
    public MyEventArgs<string, List<BaseData>> createMyEventArgs(List<BaseData> list, string operate, string className, string extendData = null, string MethodName = null){
        MyEventArgs<string, List<BaseData>> args = new MyEventArgs<string, List<BaseData>>(className, list, extendData:extendData, MethodName: MethodName);
        return args;
    }

    /// <summary>
    /// 类 data 转 json 返回
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public string dataToJson<T>(T data){
        if(data != null){
            var serializer = new DataContractJsonSerializer(typeof(T));
            // Debug.Log("serializer ==="+ serializer);
            var stream = new MemoryStream();
            // Debug.Log("stream 1 ==="+ stream);
            serializer.WriteObject(stream, data);
            // Debug.Log("stream 2 ==="+ stream);  
            string json = Encoding.UTF8.GetString(stream.ToArray());
            // Debug.Log("json ==="+ json);
            return json;
        }
        return null;

    }


    /// <summary>
    /// 判断是不是 空类
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsEmptyClass(Type type)
    {
        return type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Length == 0
            && type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Length == 0
            && type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).All(m => m.IsSpecialName);
    }


    // 判断类中是否 赋过值
    public static bool IsClassValue<T>(T data){
        if(data != null){
            var serializer = new DataContractJsonSerializer(typeof(T));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, data);
            string json = Encoding.UTF8.GetString(stream.ToArray());
            if(json == "{}"){
                return false;
            }
            return true;
        }
        return false;
        // PropertyInfo[] properties = data.GetType().GetProperties();
        // foreach (PropertyInfo property in properties)
        // {
        //     // 获取属性值
        //     object value = property.GetValue(data, null);
        //     // 判断属性值是否为空
        //     if (value != null)
        //     {
        //         // 添加到List
        //         SelectListBaseData.Add(new BaseData(property.Name, value.ToString()));
        //     }
        // }

    }




    public static T DeepCopy<T>(T obj)
    {
        var type = typeof(T);
        var copy = Activator.CreateInstance(type);
    
        foreach (var field in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            var value = field.GetValue(obj);
            if (value is ICloneable cloneable)
            {
                value = cloneable.Clone();
            }
            field.SetValue(copy, value);
        }
    
        return (T)copy;
    }



}

