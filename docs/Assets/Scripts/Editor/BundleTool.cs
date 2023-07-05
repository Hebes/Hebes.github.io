using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
/// <summary>
/// Bundle打包工具
/// </summary>
public class BundleTool
{
   
    [MenuItem("Tools/BundleTools/BundleAndroidTool")]
    public static void GenerateAndroidBundle()
    {
        GenerateBundles(BuildTarget.Android);
    }

    [MenuItem("Tools/BundleTools/BundleIOSTool")]
    public static void GenerateIOSBundle()
    {
        GenerateBundles(BuildTarget.iOS);
    }

    [MenuItem("Tools/BundleTools/BundlePCTool")]
    public static void GeneratePCBundle()
    {
        GenerateBundles(BuildTarget.StandaloneWindows);
    }

    /// <summary>
    /// 构建Bundle
    /// </summary>
    /// <param name="target">目标平台</param>
    static void GenerateBundles(BuildTarget target)
    {
        //需要打包的资源列表
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
        //文件信息列表
        List<string> bundleInfos = new List<string>();
        //搜索所有的文件名 按文件打包
        string[] files = Directory.GetFiles(PathUtil.buildResourcesPath,"*",SearchOption.AllDirectories);

        for(int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }
            //获取文件标准路径
            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log($"fileName:{fileName}");
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            //assetbundle资源名
            string assetName = PathUtil.GetUnityPath(fileName);
            assetBundle.assetNames=new string[] {assetName};
            //assetbundle名
            string bundleName = fileName.Replace(PathUtil.buildResourcesPath,"").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            //添加到资源列表
            assetBundleBuilds.Add(assetBundle);

            //添加文件的依赖信息
            List<string> dependenceInfo = GetDependeces(assetName);
            string bundleInfo = assetName + "|" + bundleName + ".ab";

            if(dependenceInfo.Count>0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);
            }

            bundleInfos.Add(bundleInfo);
        }

        //判断Bundle输出路径是否存在
        if(Directory.Exists(PathUtil.bundlePath))
        {
            Directory.Delete(PathUtil.bundlePath,true);
        }
        Directory.CreateDirectory(PathUtil.bundlePath);
        //             打包AssetBundles           打包路径                    build列表               压缩格式（默认）          Bundle打包平台
        BuildPipeline.BuildAssetBundles(PathUtil.bundlePath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        //将bundle信息列表 保存
        File.WriteAllLines(PathUtil.bundlePath + "/" + GameConfig.FileListName, bundleInfos);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取文件的依赖信息
    /// </summary>
    /// <param name="curFile">文件名</param>
    /// <returns></returns>
    static List<string> GetDependeces(string curFile)
    {
        List<string> dependences = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        //剔除脚本文件和他本身
        dependences = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList<string>();
        return dependences;
    }
}
