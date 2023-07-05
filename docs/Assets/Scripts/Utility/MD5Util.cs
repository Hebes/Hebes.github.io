using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
/// <summary>
/// 加密解密工具类
/// </summary>
public class MD5Util
{
    /// <summary>
    /// 文件加密
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <returns></returns>
    public static string CreateMD5(string filePath)
    {
        StringBuilder sb = new StringBuilder();
        if(!File.Exists(filePath))
        {
            Debug.Log("文件不存在:" + filePath);
            return string.Empty;
        }
        using (FileStream fs = new FileStream(filePath, FileMode.Open))
        {
            byte[] data = new byte[1024];
            int count = fs.Read(data, 0, data.Length);
            if(count>0)
            {
                MD5 md = MD5.Create();
                byte[] result = md.ComputeHash(data);
                for(int i=0;i<result.Length;i++)
                {
                    sb.Append(result[i].ToString("x2"));
                }
            }
        }
        return sb.ToString();
    }
}
