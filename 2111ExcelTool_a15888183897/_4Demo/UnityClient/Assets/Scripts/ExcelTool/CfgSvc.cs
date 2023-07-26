/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 配置服务

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using ProtoBuf;
using System.IO;
using UnityEngine;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TblDic = System.Collections.Generic.Dictionary<int, ProtoBuf.IExtensible>;

public class CfgSvc {
    private static CfgSvc instance = null;
    public static CfgSvc Instance {
        get {
            if(instance == null) {
                instance = new CfgSvc();
            }
            return instance;
        }
    }

    private string xlsPath = null;
    private Dictionary<Type, TblDic> cfgDic = null;

    public void Init() {
        xlsPath = Application.streamingAssetsPath + "/xls/";
        cfgDic = new Dictionary<Type, TblDic>();
    }

    public IExtensible GetTblItem<T>(int ID) where T : IExtensible {
        TblDic dic = LoadTbl<T>();
        if(dic.ContainsKey(ID))
            return dic[ID];
        return null;
    }

    public TblDic LoadTbl<T>() where T : IExtensible {
        if(cfgDic.ContainsKey(typeof(T)))
            return cfgDic[typeof(T)];
        T table = ParsePBData<T>();
        TblDic dic = ParseTbl(table);
        cfgDic[typeof(T)] = dic;
        return dic;
    }

    private T ParsePBData<T>() where T : IExtensible {
        Type t = typeof(T);
        string path = xlsPath + t.Name + ".bytes";
        byte[] tbData = ExcelTool.LoadData(path);
        byte[] data = ExcelTool.Decompress(tbData);
        MemoryStream ms = new MemoryStream(data);
        T res = Serializer.Deserialize<T>(ms);
        return res;
    }

    private static TblDic ParseTbl(IExtensible table) {
        TblDic dic = new TblDic();
        Type t = table.GetType();
        PropertyInfo p = t.GetProperty("Lsts");
        IList lst = (IList)p.GetValue(table, null);
        foreach(IExtensible item in lst) {
            Type tp = item.GetType();
            PropertyInfo prop = tp.GetProperty("Id");
            int key = (int)prop.GetValue(item, null);
            dic[key] = item;
        }
        return dic;
    }
}