# 生成Byts文件

<https://blog.csdn.net/qq_38139402/article/details/114232785>

这里需要注意的是.xls使用CreateBinaryReader方法而.xlsx使用CreateOpenXmlReader方法

```CSharp
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using Excel;
 
/// <summary>
/// 将Excel文件转换为脚本
/// </summary>
public class Excel2Script : MonoBehaviour
{
    /// <summary>
    /// 每行类型
    /// </summary>
    private enum RowType : byte
    {
        FIELD_NAME = 4,
        FIELD_TYPE = 5,
        DATA_START_ROW = 6
    }
 
    /// <summary>
    /// 放置Excel文件的路径
    /// </summary>
    private static readonly string ExcelPath = $"{Application.dataPath}/Excel2Script/Excel";
 
    /// <summary>
    /// 放置要生成的脚本文件的路径
    /// </summary>
    private static readonly string ScriptPath = $"{Application.dataPath}/Excel2Script/Script";
 
    /// <summary>
    /// 放置要生成的二进制文件的路径
    /// </summary>
    private static readonly string BytePath = $"{Application.dataPath}/Excel2Script/Byte";
 
    [MenuItem("Tools/Excel To Script")]
    private static void _Excel2Script()
    {
        foreach (string filePath in Directory.EnumerateFiles(ExcelPath, "*.xls"))
        {
            string[][] data = LoadExcel(filePath);
            CreateScript(filePath, data);
        }
 
        AssetDatabase.Refresh();
        Debug.Log("Excel转换成CS文件完成");
    }
 
    [MenuItem("Tools/Excel To Byte")]
    private static void _Excel2Byte()
    {
        foreach (string filePath in Directory.EnumerateFiles(ExcelPath, "*.xls"))
        {
            string[][] data = LoadExcel(filePath);
            CreateByte(filePath, data);
        }
 
        AssetDatabase.Refresh();
        Debug.Log("Excel转换成二进制文件完成");
    }
 
    /// <summary>
    /// 读取Excel数据并保存为字符串锯齿数组
    // </summary>
    private static string[][] LoadExcel(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
 
        DataSet dataSet = fileInfo.Extension == ".xls"
            ? ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet()
            : ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet();
 
        DataRowCollection rows = dataSet.Tables[0].Rows;
        string[][] data = new string[rows.Count][];
        for (int i = 0; i < rows.Count; ++i)
        {
            int columnCount = rows[i].ItemArray.Length;
            string[] columnArray = new string[columnCount];
            for (int j = 0; j < columnArray.Length; ++j)
            {
                columnArray[j] = rows[i].ItemArray[j].ToString();
            }
 
            data[i] = columnArray;
        }
 
        return data;
    }
 
    /// <summary>
    /// 通过Excel数据生成脚本文件
    /// </summary>
    private static void CreateScript(string filePath, string[][] data)
    {
        StringBuilder scriptStr = new StringBuilder();
        string className = new FileInfo(filePath).Name.Split('.')[0];
        scriptStr.AppendLine("using System.Collections.Generic;\n");
        scriptStr.AppendLine($"public class {className}");
        scriptStr.AppendLine("{");
        string[] filedTypeArray = data[(int) RowType.FIELD_TYPE];
        string[] filedNameArray = data[(int) RowType.FIELD_NAME];
        for (int i = 1; i < filedTypeArray.Length; ++i)
        {
            scriptStr.AppendLine($"\tpublic {filedTypeArray[i].PadRight(10, ' ')}\t{filedNameArray[i]};");
        }
 
        scriptStr.AppendLine("}");
        string path = $"{ScriptPath}/{className}.cs";
        File.Delete(path);
        File.WriteAllText(path, scriptStr.ToString());
    }
 
    /// <summary>
    /// 创建二进制文件
    /// </summary>
    private static void CreateByte(string filePath, string[][] data)
    {
        string className = new FileInfo(filePath).Name.Split('.')[0];
        string path = $"{BytePath}/{className}";
        File.Delete(path);
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            List<Type> types = GetTypeByFieldType(data);
 
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
            {
                for (int i = (int) RowType.DATA_START_ROW; i < data.Length; ++i)
                {
                    for (int j = 0; j < types.Count; ++j)
                    {
                        byte[] bytes = GetField(types[j], data[i][j + 1]);
 
                        binaryWriter.Write(bytes);
                    }
                }
            }
        }
    }
 
    private static byte[] GetField(Type type, string data)
    {
        if (IsListType(type))
        {
            string[] dataArray = data.Split('|');
            List<byte> byteList = BitConverter.GetBytes(dataArray.Length).ToList();
            for (int i = 0; i < dataArray.Length; ++i)
            {
                byteList.AddRange(GetBasicField(type.GenericTypeArguments[0], dataArray[i]).ToList());
            }
            
            return byteList.ToArray();
        }
 
        return GetBasicField(type, data);
    }
 
    private static byte[] GetBasicField(Type type, string data)
    {
        byte[] bytes = null;
        if (type == typeof(int))
        {
            bytes = BitConverter.GetBytes(int.Parse(data));
        }
        else if (type == typeof(float))
        {
            bytes = BitConverter.GetBytes(float.Parse(data));
        }
        else if (type == typeof(string))
        {
            List<byte> dataBytes = Encoding.Default.GetBytes(data).ToList();
            List<byte> lengthBytes = BitConverter.GetBytes(dataBytes.Count).ToList();
            lengthBytes.AddRange(dataBytes);
            bytes = lengthBytes.ToArray();
        }
 
        if (bytes == null) throw new Exception($"{nameof(name)}.GetBasicField: 其类型未配置或不是基础类型 Type:{type} Data:{data}");
        return bytes;
    }
 
    private static List<Type> GetTypeByFieldType(string[][] data)
    {
        List<Type> types = new List<Type>();
        string[] temp = data[(int) RowType.FIELD_TYPE];
        for (int i = 1; i < temp.Length; ++i)
        {
            if (temp[i] == "int") types.Add(typeof(int));
            else if (temp[i] == "float") types.Add(typeof(float));
            else if (temp[i] == "string") types.Add(typeof(string));
            else if (temp[i] == "List<int>") types.Add(typeof(List<int>));
            else if (temp[i] == "List<float>") types.Add(typeof(List<float>));
        }
 
        return types;
    }
 
    private static bool IsListType(Type type)
    {
        if (type == typeof(List<int>)) return true;
        if (type == typeof(List<float>)) return true;
        if (type == typeof(List<string>)) return true;
        return false;
    }
}
```
