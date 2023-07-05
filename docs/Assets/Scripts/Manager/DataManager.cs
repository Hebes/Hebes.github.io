using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System;
using System.ComponentModel;
using System.Text;
/// <summary>
/// 数据管理器
/// </summary>
public class DataManager
{
    /// <summary>
    /// 保存数据
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    public static void SaveData<T>(string fileName,T data) where T:new()
    {
        string dataStr = JsonConvert.SerializeObject(data);
        string filePath = PathUtil.playerDataPath + fileName + ".json";
        FileInfo file = new FileInfo(filePath);
        if(file.Exists )
        {
            file.Delete();
        }
        using(FileStream fs = new FileStream(filePath,FileMode.OpenOrCreate))
        {
            byte[] bytes = Encoding.UTF8.GetBytes(dataStr);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="filePath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T LoadData<T>(string filePath) where T:new()
    {
        if(!File.Exists(filePath))
        {
            Debug.LogFormat($"{filePath} is not exist!");
            return default(T);
        }
        string fileStr = string.Empty;
        using(FileStream fs = new FileStream(filePath,FileMode.Open))
        {
            StreamReader sr =new StreamReader(fs);
            fileStr = sr.ReadToEnd();
            sr.Close();
            fs.Close ();
        }
        if(!string.IsNullOrEmpty(fileStr))
        {
            T t = JsonConvert.DeserializeObject<T>(fileStr);
            return t;
        }
        return default(T);
    }
}
