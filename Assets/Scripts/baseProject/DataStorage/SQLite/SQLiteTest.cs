using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SQLiteTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Save/SQLiteData.db");
        // OpenSQLiteFile(filePath);
        // SQLiteDBConnect sQLiteDBConnect = new SQLiteDBConnect(filePath);


        SQLiteDBCommand SQLiteDBCommand = new SQLiteDBCommand(filePath);
        // 创建表
        // SQLiteDBCommand.CreateTable("testOne");

        if(SQLiteDBCommand.IsTabelCreated<deviceInfoClass>()){
            Debug.Log("表存在");
        }else{
            Debug.Log("表不存在");
            // 创建 class 表
            SQLiteDBCommand.CreateTable<deviceInfoClass>();
        }
        
        

        // 单个新增
        deviceInfoClass aa = new deviceInfoClass();
        // aa.id = 1;
        aa.deviceName = "testThree";
        aa.deviceId = "123456789";
        aa.deviceStatus = "5";
        aa.deviceType = "deviceType83_40";
        aa.imei = "123321123321123$1$1$1";
        aa.orgCode = "001001";
        aa.orgName = "E_floor_18";
        aa.position = new Vector3(85, 78, 55);
        aa.x = 85;
        aa.y = 78;
        aa.z = 55;


        SQLiteDBCommand.Insert(aa);





        // 批量新增
        List<deviceInfoClass> list = new List<deviceInfoClass>();
        for (int i = 0; i < 20; i++)
        {
            deviceInfoClass b = new deviceInfoClass();
            // b.id = i;
            b.deviceName ="test_" + i;
            b.deviceId = "0" + i;
            b.deviceStatus = "5";
            b.deviceType = "deviceType83_40";
            b.imei = "123321123321123$1$1$" + i;
            b.orgCode = "001001";
            b.orgName = "E_floor_19";
            // b.position = new Vector3(85, 78, 55);
            b.x = i;
            b.y = 78;
            b.z = 55;

            list.Add(b);
        }
        SQLiteDBCommand.Insert(list);

        // 单个删除
        SQLiteDBCommand.DeleteById<deviceInfoClass>(2);

        SQLiteDBCommand.DeleteById<deviceInfoClass>(new List<int>(){2,3});
        // sql 语句删除
        // id 大于90 的  
        SQLiteDBCommand.DeleteBySql<deviceInfoClass>("id > 90");
        // 删除 imei 为 123321123321123$1$1$10 
        SQLiteDBCommand.DeleteBySql<deviceInfoClass>("imei='123321123321123$1$1$10'");
        // 删除 所有 deviceType83_40  类
        // SQLiteDBCommand.DeleteBySql<deviceInfoClass>("deviceType=' deviceType83_40 '");



        // 更新数据 UpDateById
        deviceInfoClass cc = new deviceInfoClass();
        cc.id = 20;
        cc.deviceName ="test_20";
        cc.deviceId = "0123456";
        cc.deviceStatus = "5";
        cc.deviceType = "deviceType83_40";
        cc.imei = "123321123321123$1$1$20";
        cc.orgCode = "001001";
        cc.orgName = "E_floor_19";
        // b.position = new Vector3(85, 78, 55);
        cc.x = 123;
        cc.y = 78;
        cc.z = 55;
        SQLiteDBCommand.UpDateById<deviceInfoClass>(cc);

        // UpdateBySql

        deviceInfoClass dd = new deviceInfoClass();
        // dd.id = 30;
        dd.deviceName ="test_30_aaa";
        dd.deviceId = "030";
        dd.deviceStatus = "5";
        dd.deviceType = "deviceType83_40";
        dd.imei = "123321123321123$1$1$7";
        dd.orgCode = "001001";
        dd.orgName = "E_floor_19";
        // b.position = new Vector3(85, 78, 55);
        dd.x = 300;
        dd.y = 200;
        dd.z = 11;
        SQLiteDBCommand.UpDate(dd, "imei='123321123321123$1$1$7'");




        //  查询数据
        deviceInfoClass deviceInfoData = SQLiteDBCommand.SelectById<deviceInfoClass>(20);
        Debug.Log("查询 id = 20 的数据" + deviceInfoData.deviceName);


        // 查询 id 大于 50 的数据
        List<deviceInfoClass> list2 = SQLiteDBCommand.SelectBySql<deviceInfoClass>("id > 15");
        Debug.Log("查询 id 大于 15 的数据的 Count = " + list2.Count);


        // 查询 imei 为 123321123321123$1$1$30 的数据
        List<deviceInfoClass> list3 = SQLiteDBCommand.SelectBySql<deviceInfoClass>("imei='123321123321123$1$1$17'");
        Debug.Log("查询 imei 为 123321123321123$1$1$17 的数据" + list3.Count);
        list3.ToList().ForEach((item) =>
            Debug.Log("查询 imei 为 123321123321123$1$1$17 的数据" + item.deviceName + "item.imei  =" + item.imei)
        );

        // 查询 E_floor_19 
        List<deviceInfoClass> list4 = SQLiteDBCommand.SelectBySql<deviceInfoClass>("orgName='E_floor_18'");
        Debug.Log("查询 orgName 为 E_floor_18 的数据" + list4.Count);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}





/// 基本思路

// 1、 通过接口获取数据存入 SQList 数据库
// 2、 本地数据 拖拽的 设备 信息 先储存到 本地数据库
// 3、 每次打开程序时，先检查本地数据库是否有数据，有则读取，没有则从接口获取数据
// 4、 链接 不同的 ip 地址，获取不同的数据，创建不同的数据库 db
