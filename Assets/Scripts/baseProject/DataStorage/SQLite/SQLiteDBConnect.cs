using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.IO;

/// <summary>
/// SQLite数据库连接
/// </summary>
public class SQLiteDBConnect
{
    protected SqliteConnection _SQLConnection;

    public SQLiteDBConnect(string DBPath){
        // _SQLConnection = new SqliteConnection(
        //     //SqliteConnectionStringBuilder 用来 初始化
        //     new SqliteConnectionStringBuilder(){
        //         DataSource = DBPath
        //     }.ToString()
        // );

        // _SQLConnection.Open();// 打开连接
        // 判断是否存在 文件
        if(!File.Exists(DBPath)){
            CreateDBSqlite(DBPath);
        } 
        ConnectDBSqlite(DBPath);
    }

    public bool CreateDBSqlite(string DBPath){
        Debug.Log($"创建数据库：{DBPath}");
        try
        {
            string dirName = new FileInfo(DBPath).Directory.FullName;
            // 判断 父级 文件夹 是否存在 
            if(!Directory.Exists(dirName)){
                //如果不存在  创建文件夹
                Directory.CreateDirectory(dirName);
            }
            SqliteConnection.CreateFile(DBPath);
            return true;
        }
        catch(System.Exception e)
        {
            Debug.LogError($"创建数据库失败：{e.Message}");
            return false;
            throw;
        }
        
    }


    public bool ConnectDBSqlite(string DBPath){
        Debug.Log($"连接数据库：{DBPath}");
        try
        {
            _SQLConnection = new SqliteConnection(
                //SqliteConnectionStringBuilder 用来 初始化
                new SqliteConnectionStringBuilder(){
                    DataSource = DBPath
                }.ToString()
            );

            _SQLConnection.Open();// 打开连接
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"链接数据库失败：{e.Message}");
            return false;
        }
    }



    public void Dispose(){
        // _SQLConnection.Close();
        _SQLConnection.Dispose();
    }
}
