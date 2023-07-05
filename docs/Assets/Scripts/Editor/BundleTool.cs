using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
/// <summary>
/// Bundle�������
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
    /// ����Bundle
    /// </summary>
    /// <param name="target">Ŀ��ƽ̨</param>
    static void GenerateBundles(BuildTarget target)
    {
        //��Ҫ�������Դ�б�
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();
        //�ļ���Ϣ�б�
        List<string> bundleInfos = new List<string>();
        //�������е��ļ��� ���ļ����
        string[] files = Directory.GetFiles(PathUtil.buildResourcesPath,"*",SearchOption.AllDirectories);

        for(int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta"))
            {
                continue;
            }
            //��ȡ�ļ���׼·��
            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log($"fileName:{fileName}");
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            //assetbundle��Դ��
            string assetName = PathUtil.GetUnityPath(fileName);
            assetBundle.assetNames=new string[] {assetName};
            //assetbundle��
            string bundleName = fileName.Replace(PathUtil.buildResourcesPath,"").ToLower();
            assetBundle.assetBundleName = bundleName + ".ab";
            //��ӵ���Դ�б�
            assetBundleBuilds.Add(assetBundle);

            //����ļ���������Ϣ
            List<string> dependenceInfo = GetDependeces(assetName);
            string bundleInfo = assetName + "|" + bundleName + ".ab";

            if(dependenceInfo.Count>0)
            {
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);
            }

            bundleInfos.Add(bundleInfo);
        }

        //�ж�Bundle���·���Ƿ����
        if(Directory.Exists(PathUtil.bundlePath))
        {
            Directory.Delete(PathUtil.bundlePath,true);
        }
        Directory.CreateDirectory(PathUtil.bundlePath);
        //             ���AssetBundles           ���·��                    build�б�               ѹ����ʽ��Ĭ�ϣ�          Bundle���ƽ̨
        BuildPipeline.BuildAssetBundles(PathUtil.bundlePath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.None, target);
        //��bundle��Ϣ�б� ����
        File.WriteAllLines(PathUtil.bundlePath + "/" + GameConfig.FileListName, bundleInfos);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// ��ȡ�ļ���������Ϣ
    /// </summary>
    /// <param name="curFile">�ļ���</param>
    /// <returns></returns>
    static List<string> GetDependeces(string curFile)
    {
        List<string> dependences = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        //�޳��ű��ļ���������
        dependences = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList<string>();
        return dependences;
    }
}
