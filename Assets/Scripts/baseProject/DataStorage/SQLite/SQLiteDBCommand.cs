using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Text;
using System.Reflection;
using System;
using System.Data;

public class SQLiteDBCommand : SQLiteDBConnect
{

    private SqliteCommand _SQLCommand;
    public SQLiteDBCommand(string DBPath) : base(DBPath)
    {
        // Connect(DBPath);
        _SQLCommand = new SqliteCommand(_SQLConnection);
    }

    #region 表管理

    // 创建表
    public int CreateTable(string tableName, string[] columns = null)
    {
        string sql = "create table " + tableName + "(id int, name string)";
        _SQLCommand.CommandText = sql;
        return _SQLCommand.ExecuteNonQuery();
    }

    public int CreateTable<T>() where T : BaseDataClass
    {
        System.Type type = typeof(T);
        string tableName = type.Name;
        // StringBuilder 
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("create table " + tableName + "(");
        var publicProperties = type.GetProperties();
        foreach (var property in publicProperties)
        {
            // Debug.Log("property.Name = " + property);
            // 获取 当前属性 的 特性 来判断是否 添加到数据库中
            var attribute = property.GetCustomAttribute<ModelHelp>();

            // Debug.Log("property ===" + property +" = attribute = " + attribute);
            if(attribute?.IsCreated ?? false){
                stringBuilder.Append($"{attribute.FieldName} {attribute.Type} ");
                
                // 判断是否为 主键
                if(attribute.IsPrimaryKey){
                    stringBuilder.Append(" PRIMARY KEY AUTOINCREMENT");
                }
                // 是否可以为空
                if(attribute.IsCanBeNUll){
                    stringBuilder.Append(" null ");
                } else {
                    stringBuilder.Append(" not null ");
                }
                if(attribute.IsUnique){
                    stringBuilder.Append(" UNIQUE ");
                }
                stringBuilder.Append(",");

            }

            // stringBuilder.Append($"{property.Name} {property.PropertyType.Name},");
        }
        //去掉最后一个 逗号
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        stringBuilder.Append(")");

        // Debug.Log("CreateTable T sql = " + stringBuilder.ToString());
        
        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }


    public int DeleteTable<T>() where T : BaseDataClass
    {
        string sql = $"drop table { typeof(T).Name}";
        _SQLCommand.CommandText = sql;
        return _SQLCommand.ExecuteNonQuery();
    }


    public bool IsTabelCreated<T>() where T : BaseDataClass
    {
        string sql = $"select count(*) from sqlite_master where type='table' and name='{typeof(T).Name}'";
        _SQLCommand.CommandText = sql;
        int count = Convert.ToInt32(_SQLCommand.ExecuteScalar());
        return count > 0;


        // 使用 上面 两行 替换 这部分
        // SqliteDataReader dr = _SQLCommand.ExecuteReader();
        // //dr.Read() ; dr 读取 一行的数据
        // if (dr != null && dr.Read()){
        //     var temp = Convert.ToInt32(dr[dr.GetName(0)]);
        //     if(temp == 1){
        //         return true;
        //     } else {
        //         return false;
        //     }
        // }

        // return false;

    }

    public bool IsTabelCreated(string Name)
    {
        string sql = $"select count(*) from sqlite_master where type='table' and name='{Name}'";
        _SQLCommand.CommandText = sql;
        int count = Convert.ToInt32(_SQLCommand.ExecuteScalar());
        return count > 0;
    }


    #endregion



    #region 数据管理 新增

    public int Insert<T>(T t) where T : class
    {
        if(t == default(T)){
            Debug.LogError("Insert Error : t is null 参数错误");
            return -1;
        }

        StringBuilder stringBuilder = new StringBuilder();


        // INSERT INTO table_name (column1,column2,column3,...)


        System.Type type = typeof(T);
        string tableName = type.Name;


        
        stringBuilder.Append($"INSERT INTO {tableName}  (");


        var publicProperties = type.GetProperties();
        
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                stringBuilder.Append(attribute.FieldName);
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append(") VALUES (");


        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){

                if(attribute.Type == "string"){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                }
                else if(string.Equals(attribute.Type, "TEXT")){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                }
                else {
                    stringBuilder.Append(property.GetValue(t));
                }


                // stringBuilder.Append(property.GetValue(t));
                stringBuilder.Append(",");
            }
        }


        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append(")");
      // Debug.Log("Insert sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }



    public int Insert<T>(List<T> ListT) where T : class
    {
        if(ListT == null || ListT.Count == 0){
            Debug.LogError("Insert Error : t is null 参数错误");
            return -1;
        }

        StringBuilder stringBuilder = new StringBuilder();


        // INSERT INTO table_name (column1,column2,column3,...)


        System.Type type = typeof(T);
        string tableName = type.Name;


        
        stringBuilder.Append($"INSERT INTO {tableName}  (");


        var publicProperties = type.GetProperties();
        
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                stringBuilder.Append(attribute.FieldName);
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append(") VALUES ");


        foreach (var t in ListT){
            stringBuilder.Append("( ");
            foreach (var property in publicProperties)
            {
                var attribute = property.GetCustomAttribute<ModelHelp>();
                if(attribute.IsCreated && !attribute.IsPrimaryKey){


                    if(attribute.Type == "string"){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    }
                    else if(string.Equals(attribute.Type, "TEXT")){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    }
                    else {
                        stringBuilder.Append(property.GetValue(t));
                    }
                    // stringBuilder.Append(property.GetValue(t));
                    stringBuilder.Append(",");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
            stringBuilder.Append("),");


        }





        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        // stringBuilder.Append(")");
      // Debug.Log("Insert sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }


    // 没有数据就新增 有数据就更新
    public int InsertOrUpdate<T>(T t, string name = "imei") where T : class
    {
  
        if(t == default(T)){
            Debug.LogError("Insert Error : t is null 参数错误");
            return -1;
        }

        StringBuilder stringBuilder = new StringBuilder();


        // INSERT INTO table_name (column1,column2,column3,...)


        System.Type type = typeof(T);
        string tableName = type.Name;


        
        stringBuilder.Append($"INSERT INTO {tableName}  (");


        var publicProperties = type.GetProperties();
        
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                stringBuilder.Append(attribute.FieldName);
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append(") VALUES (");


        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){

                if(attribute.Type == "string"){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                }
                else if(string.Equals(attribute.Type, "TEXT")){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                }
                else {
                    stringBuilder.Append(property.GetValue(t));
                }


                // stringBuilder.Append(property.GetValue(t));
                stringBuilder.Append(",");
            }
        }


        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append(")");

        if(name == null){
            name = "imei";
        }
        // ON CONFLICT(id) DO UPDATE SET field1 = EXCLUDED.field1, field2 = EXCLUDED.field2;
        stringBuilder.Append($" ON CONFLICT({name}) DO UPDATE SET ");

        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                // stringBuilder.Append(attribute.FieldName);
                // stringBuilder.Append(",");

                // if(property.GetValue(t) == null || property.GetValue(t).GetType() == typeof(System.DBNull)){
                //   // Debug.Log("空数据 == " +  property.Name);
                // } else{

                //     stringBuilder.Append($"{attribute.FieldName} = ");

                //     if(attribute.Type == "string"){
                //         stringBuilder.Append($"'{property.GetValue(t)}'");
                //     } 
                //     else if(string.Equals(attribute.Type, "TEXT")){
                //         stringBuilder.Append($"'{property.GetValue(t)}'");
                //     }
                //     else {
                //         stringBuilder.Append(property.GetValue(t));
                //     }
                //     stringBuilder.Append(",");
                // }




                    stringBuilder.Append($"{attribute.FieldName} = ");

                    if(attribute.Type == "string"){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    } 
                    else if(string.Equals(attribute.Type, "TEXT")){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    }
                    else {
                        stringBuilder.Append(property.GetValue(t));
                    }
                    stringBuilder.Append(",");



            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号

      // Debug.Log("Insert sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }


    // insert or replace
    /// <summary>
    /// 插入或者替换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ListT"></param>
    /// <returns></returns>
    public int InsertOrReplace<T>(List<T> ListT) where T : class
    {
        if(ListT == null || ListT.Count == 0){
            Debug.LogError("Insert Error : t is null 参数错误");
            return -1;
        }

        StringBuilder stringBuilder = new StringBuilder();


        // INSERT INTO table_name (column1,column2,column3,...)


        System.Type type = typeof(T);
        string tableName = type.Name;


        
        stringBuilder.Append($"INSERT OR REPLACE INTO {tableName}  (");


        var publicProperties = type.GetProperties();
        
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                stringBuilder.Append(attribute.FieldName);
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append(") VALUES ");


        foreach (var t in ListT){
            stringBuilder.Append("( ");
            foreach (var property in publicProperties)
            {
                var attribute = property.GetCustomAttribute<ModelHelp>();
                if(attribute.IsCreated && !attribute.IsPrimaryKey){


                    if(attribute.Type == "string"){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    }
                    else if(string.Equals(attribute.Type, "TEXT")){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    }
                    else {
                        stringBuilder.Append(property.GetValue(t));
                    }
                    // stringBuilder.Append(property.GetValue(t));
                    stringBuilder.Append(",");
                }
            }

            stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
            stringBuilder.Append("),");


        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        // stringBuilder.Append(")");
      // Debug.Log("Insert sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }


    // insert or ignore
    // 待 开发

    #endregion

    #region 数据管理 删除

    public int DeleteById<T>(int id) where T : class{
        if(id == null){
            return -1;
        }

        System.Type type = typeof(T);
        string tableName = type.Name;

        string sql = $"DELETE FROM {tableName} WHERE id = {id}";


        _SQLCommand.CommandText = sql.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }

    public int DeleteById<T>(List<int> idList) where T : class{
        if(idList == null || idList.Count == 0){
            return -1;
        }
        int Count = 0;
        foreach (var id in idList)
        {
            Count += DeleteById<T>(id);
        }
        return Count;
    }


    public int DeleteByIds<T>(List<int> idList) where T : class{
        if(idList == null || idList.Count == 0){
            return -1;
        }
        int Count = 0;
        foreach (var id in idList)
        {
            Count += DeleteById<T>(id);
        }
        return Count;
    }


    public int DeleteBySql<T>(string sql_where) where T : class{
        System.Type type = typeof(T);
        string tableName = type.Name;
        string sql = $"DELETE FROM {tableName} WHERE {sql_where}";
        _SQLCommand.CommandText = sql.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }

    #endregion



    #region 数据管理 修改


    // public int UpDateById<T>(int id) where T : class{
    //     if(id == null){
    //         return -1;
    //     }

    //     System.Type type = typeof(T);
    //     string tableName = type.Name;

    //     // string sql = $"DELETE FROM {tableName} WHERE id = {id}";

    //     _SQLCommand.CommandText = sql.ToString();
    //     return _SQLCommand.ExecuteNonQuery();
    // }



    // UPDATE table_name
    // SET column1 = value1, column2 = value2, ...
    // WHERE condition;
    /// <summary>
    /// 根据 id 修改数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public int UpDateById<T>(T t) where T : BaseDataClass
    {
        if(t == default(T)){
            Debug.LogError("upDate 参数错误");
            return -1;
        }
        System.Type type = typeof(T);
        string tableName = type.Name;

        StringBuilder stringBuilder = new StringBuilder();

        
        stringBuilder.Append($"UPDATE {tableName} SET ");

        var publicProperties = type.GetProperties();
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                // stringBuilder.Append(attribute.FieldName);
                // stringBuilder.Append(",");
                stringBuilder.Append($"{attribute.FieldName} = ");

                if(attribute.Type == "string"){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                } 
                else if(string.Equals(attribute.Type, "TEXT")){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                }
                else {
                    stringBuilder.Append(property.GetValue(t));
                }
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号

        stringBuilder.Append($" where id = {t.id}");

      // Debug.Log("UpDateById sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }



    public int UpDateByIds<T>(List<T> idList) where T : BaseDataClass
    {
        if(idList == null || idList.Count == 0){
            Debug.LogError("upDate List<T> 参数错误");
            return -1;
        }
        int Count = 0;
        foreach (var id in idList)
        {
            Count += UpDateById<T>(id);
        }
        return Count;
    }


    /// <summary>
    /// 根据条件修改
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="sql">条件</param>
    /// <returns></returns>
    public int UpDate<T>(T t, string sql) where T : BaseDataClass
    {
        if(t == default(T)){
            Debug.LogError("upDate 参数错误");
            return -1;
        }
        System.Type type = typeof(T);
        string tableName = type.Name;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"UPDATE {tableName} SET ");
        var publicProperties = type.GetProperties();
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){
                stringBuilder.Append($"{attribute.FieldName} = ");
                if(attribute.Type == "string"){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                } 
                else if(string.Equals(attribute.Type, "TEXT")){
                    stringBuilder.Append($"'{property.GetValue(t)}'");
                }
                else {
                    stringBuilder.Append(property.GetValue(t));
                }
                stringBuilder.Append(",");
            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号
        stringBuilder.Append($" where {sql}");
      // Debug.Log("UpDate sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }

    public int UpDates<T>(List<T> TList, List<string> sqlList) where T : BaseDataClass
    {
        if(TList == null || TList.Count == 0){
            Debug.LogError("upDate List<T> 参数错误");
            return -1;
        }
        int Count = 0;
        // foreach (var item in TList)
        // {
        //     Count += UpDate<T>(item, sql);
        // }

        for (int i = 0; i < TList.Count; i++){
            Count += UpDate<T>(TList[i], sqlList[i]);
        }
        return Count;
    }




    public int UpDateByIMEI<T>(T t) where T : BaseDataClass
    {
        if(t == default(T)){
            Debug.LogError("upDate 参数错误");
            return -1;
        }
        System.Type type = typeof(T);
        string tableName = type.Name;

        StringBuilder stringBuilder = new StringBuilder();

        
        stringBuilder.Append($"UPDATE {tableName} SET ");
        string imei = "";
        var publicProperties = type.GetProperties();
        foreach (var property in publicProperties)
        {
            var attribute = property.GetCustomAttribute<ModelHelp>();
            if(attribute.IsCreated && !attribute.IsPrimaryKey){

                if(property.GetValue(t) == null || property.GetValue(t).GetType() == typeof(System.DBNull)){
                    // Debug.Log("空数据");
                } else{
                    // stringBuilder.Append(attribute.FieldName);
                    // stringBuilder.Append(",");
                    stringBuilder.Append($"{attribute.FieldName} = ");

                    if(attribute.Type == "string"){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    } 
                    else if(string.Equals(attribute.Type, "TEXT")){
                        stringBuilder.Append($"'{property.GetValue(t)}'");
                    }
                    else {
                        stringBuilder.Append(property.GetValue(t));
                    }

                    if(attribute.FieldName == "imei"){
                        imei = property.GetValue(t).ToString();
                    }
                    stringBuilder.Append(",");
                }

            }
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);// 去掉最后一个 逗号

        stringBuilder.Append($" where imei = '{imei}'");

      // Debug.Log("UpDateById sql = " + stringBuilder.ToString());

        _SQLCommand.CommandText = stringBuilder.ToString();
        return _SQLCommand.ExecuteNonQuery();
    }




    #endregion





    #region 查询 
    // 查询需要把 表数据 转换成 实体类型

    /// <summary>
    /// 单个数据查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <returns></returns>
    public T SelectById<T>(int id) where T : BaseDataClass
    {
        string sql = $"SELECT * FROM {typeof(T).Name} where id = {id}";
        // return default(T);
        _SQLCommand.CommandText = sql;
        var dr = _SQLCommand.ExecuteReader();// 使用SqlCommand对象的ExecuteReader()方法执行查询，返回一个SqlDataReader对象
        // Debug.Log("SelectById SqliteDataReader = " + dr);

        //dr.Read() ; dr 读取 一行的数据
        if (dr != null && dr.Read()){

            T data = DateReaderToData<T>(dr);
            dr.Close(); 
            return data;
        }
        return default(T);
       
    }


    private T DateReaderToData<T>(SqliteDataReader reader) where T : BaseDataClass{
        try {
            // 获取 所有字段
            List<string> fieldNameList = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                // Debug.Log("DateReaderToData fieldNameList = " + reader.GetName(i));
                fieldNameList.Add(reader.GetName(i));
            }
            System.Type type = typeof(T);
            // 创建一个 T 类型的 实体
            // T t = (T)Activator.CreateInstance(type);
            T data = (T)Activator.CreateInstance<T>();
            // 获取所有属性
            var publicProperties = type.GetProperties();
            foreach (var property in publicProperties)
            {
                
                if(!property.CanWrite){
                    continue;
                }
                var attribute = property.GetCustomAttribute<ModelHelp>();
                if(attribute.IsCreated && fieldNameList.Contains(attribute.FieldName)){
                    var reader_item_value = reader[attribute.FieldName];
                    // Debug.Log("DateReaderToData reader_item_value ="+ attribute.FieldName +" =" + reader[attribute.FieldName]);
                    if(string.Equals(attribute.Type, "string", StringComparison.OrdinalIgnoreCase)){
                        // string 类型
                        property.SetValue(data, reader_item_value.ToString());
                    } 
                    else if(string.Equals(attribute.Type, "text", StringComparison.OrdinalIgnoreCase)){
                        property.SetValue(data, reader_item_value.ToString());
                    }
                    else if(string.Equals(attribute.Type, "integer", StringComparison.OrdinalIgnoreCase)){
                        if(reader_item_value.GetType() == typeof(System.DBNull)){
                            property.SetValue(data, null);
                        } else {
                            // Debug.Log("reader[attribute.FieldName] =GetType="+ (reader[attribute.FieldName]).GetType());
                            property.SetValue(data, reader[attribute.FieldName]);
                        }
                        // property.SetValue(data, reader[attribute.FieldName]);
                    }
                    else if(string.Equals(attribute.Type, "real", StringComparison.OrdinalIgnoreCase)){

                        // Debug.Log("reader[attribute.FieldName] =="+ attribute.FieldName + "==="+ reader_item_value );
                        // property.SetValue(data, (float)reader[attribute.FieldName]);
                        if(reader_item_value == null){
                            property.SetValue(data, null);
                        } else if(reader_item_value.GetType() == typeof(System.DBNull)){
                            // Debug.Log("reader[attribute.FieldName] =="+ aaa + "===" );
                            property.SetValue(data, null);
                        } else {
                            // Debug.Log("reader[attribute.FieldName] =22="+ reader[attribute.FieldName]);
                            property.SetValue(data, reader_item_value);
                        }
                        // else {
                        //     property.SetValue(data, null);
                        // }
                    } else {
                        // Debug.Log("reader[attribute.FieldName] =33="+ reader[attribute.FieldName]);

                        if(reader_item_value.GetType() == typeof(System.DBNull)){
                            // Debug.Log("reader[attribute.FieldName] =="+ aaa + "===" );
                            property.SetValue(data, null);
                        } else {
                            property.SetValue(data, reader[attribute.FieldName]);
                        }

                        
                    }
                }
            }
            return data;
        } catch (Exception e){
            Debug.LogError($"DateReaderToData<T> 转换错误 类型 {typeof(T).Name} 错误信息 {e.Message}");
            return default(T);
        }
    }




    public List<T> SelectAll<T>() where T : BaseDataClass
    {
        List<T> list = new List<T>();
        string sql = $"SELECT * FROM {typeof(T).Name}";
        // return default(T);
        _SQLCommand.CommandText = sql;
        SqliteDataReader dr = _SQLCommand.ExecuteReader();

        //dr.Read() ; dr 读取 一行的数据
        if (dr != null){
            while (dr.Read())
            {
                list.Add(DateReaderToData<T>(dr));
            }
            
        }
        dr.Close(); 
        return list;
    }




    public List<T> SelectBySql<T>(string fliterSql = "", long offest = 0, int pageSize = 0) where T : BaseDataClass
    {

        string sql = "";
        if(string.IsNullOrEmpty(fliterSql)){
            Debug.LogError("SelectBySql fliterSql 参数错误");
            // return SelectAll<T>();
            sql = $"SELECT * FROM {typeof(T).Name}";
        } else {
            if(fliterSql.Contains("ORDER")){
                sql = $"SELECT * FROM {typeof(T).Name}  {fliterSql}";
            } else {
                sql = $"SELECT * FROM {typeof(T).Name} where {fliterSql}";
            }
            

            if(offest> 0 && pageSize > 0){
                sql = sql + $" limit {pageSize} offset {offest}";
            }
        }
      // Debug.Log("SelectBySql sql ="+ sql);
        List<T> list = new List<T>();
        
        // return default(T);
        _SQLCommand.CommandText = sql;
        SqliteDataReader dr = _SQLCommand.ExecuteReader();
        //dr.Read() ; dr 读取 一行的数据
        if (dr != null){
            while (dr.Read())
            {
                list.Add(DateReaderToData<T>(dr));
            }
        }
        dr.Close(); 
        return list;
    }


    // public List<T> SelectPage<T>(long offest, int pageSize, string sqlaa) where T : BaseDataClass
    // {

    //     string sql = "";
    //     if(string.IsNullOrEmpty(sqlaa)){
    //         Debug.LogError("SelectBySql fliterSql 参数错误");
    //         // return SelectAll<T>();
    //         sql = $"SELECT * FROM {typeof(T).Name}";
    //     } else {
    //         sql = $"SELECT * FROM {typeof(T).Name} ";
    //         if(sqlaa!=null){
    //            sql = sql + $"where {sqlaa}";
    //         }
    //         sql = sql + $" limit {pageSize} offset {offest}";

    //     }
    //     List<T> list = new List<T>();
        
    //     // return default(T);
    //     _SQLCommand.CommandText = sql;
    //     SqliteDataReader dr = _SQLCommand.ExecuteReader();
    //     //dr.Read() ; dr 读取 一行的数据
    //     if (dr != null){
    //         while (dr.Read())
    //         {
    //             list.Add(DateReaderToData<T>(dr));
    //         }
    //     }
    //     dr.Close(); 
    //     // return default(List<T>);
    //     return list;
    // }

    #endregion




    #region 

    // 删除 数据库 DeleteDatabase
    public void DeleteDatabase(string _dbName){

        // 断开链接 
        // _SQLConnection.Close();
        // return;
        // if(_SQLCommand != null){
        //     _SQLCommand.CommandText = "DROP DATABASE IF EXISTS " + _dbName;
        //     _SQLCommand.ExecuteNonQuery();
        // }
        // DataTable tableNames = connection.GetSchema("Tables");
        Debug.Log("DeleteDatabase == " + _SQLConnection.GetSchema("Tables"));
                // 删除所有表
        foreach (DataRow  row in _SQLConnection.GetSchema("Tables").Rows)
        {
            // Debug.Log("DeleteDatabase == " + row["TABLE_NAME"]);
            string tableName = row["TABLE_NAME"].ToString();
            // string sql = string.Format("DROP TABLE IF EXISTS {0};", tableName);
            Debug.Log("DeleteDatabase == " + tableName + "_dbName =="+ _dbName);
            if(_dbName == tableName){
                string sql = $"DROP TABLE IF EXISTS {tableName};";
                // string sql = $"DELETE FROM deviceInfoData WHERE id > 0";
                // Debug.Log("sql  sql=="+ sql);
                _SQLCommand.CommandText = sql.ToString();
                _SQLCommand.ExecuteNonQuery();
            }
        }

    }
    #endregion




}
