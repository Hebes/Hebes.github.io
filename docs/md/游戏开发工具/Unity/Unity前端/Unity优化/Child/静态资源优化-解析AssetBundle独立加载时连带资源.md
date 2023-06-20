# 静态资源优化-解析AssetBundle独立加载时连带资源

优化美术资源

查看每个ab独立加载时需要连带加载多少东西

通过该脚本可以在指定目录下生成几个解析文件，分别按名字、加载大小、加载时依赖数量来排序从而优化Assetbundle的资源分配

```C#
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Plugins;
using UnityEditor;
using UnityEngine;
 
namespace Editor
{
    public class AssetBundleProfiler : EditorWindow
    {
 
        public class Status
        {
            public string Item;
            public long Size = 0;
            public int DependCount = 0;
        }
        
        private static string AssetsFolder;
        private static AssetBundleManifest Manifest;
        
        private static readonly Dictionary<string, Status> ItemSizes = new Dictionary<string, Status>(30000);
        private static Status TempStatus;
        private static readonly HashSet<string> TempChecked = new HashSet<string>();
 
        [MenuItem("Tools/OutputAbData")]
        public static void Profile()
        {
            if (Manifest == null)
            {
                AssetsFolder = "Assets/StreamingAssets/" + PlatformHelper.PlatformName.ToLower() + "/gameresources/";
                string path = AssetsFolder + "gameresources";
                AssetBundle.UnloadAllAssetBundles(true);
                var ab = AssetBundle.LoadFromFile(path);
                if (ab != null)
                {
                    Manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }
 
            if (Manifest == null)
            {
                return;
            }
 
            var items = Manifest.GetAllAssetBundles();
            ItemSizes.Clear();
 
            foreach (var item in items)
            {
                TempStatus = new Status();
                TempStatus.Item = item;
                TempChecked.Clear();
                GetSize(item);
                ItemSizes.Add(item, TempStatus);
            }
            
            AssetBundle.UnloadAllAssetBundles(true);
            Manifest = null;
 
            if (!Directory.Exists("AbLog"))
            {
                Directory.CreateDirectory("AbLog");
            }
            
            var list = ItemSizes.Values.ToList();
            list.Sort(SizeComparer);
            StringBuilder sb = new StringBuilder();
            foreach (var s in list)
            {
                sb.Append(string.Format("{0}  ==>[{1}]  ==>[{2}] \n", s.Item, s.Size, s.DependCount));
            }
 
            SaveTextToFile("AbLog/AbSizeData.txt", sb.ToString());
            
            list.Sort(DependCountComparer);
            StringBuilder sb2 = new StringBuilder();
            foreach (var s in list)
            {
                sb2.Append(string.Format("{0}  ==>[{1}]  ==>[{2}] \n", s.Item, s.Size, s.DependCount));
            }
            
            SaveTextToFile("AbLog/AbDependData.txt", sb2.ToString());
            
            list.Sort(ItemComparer);
            StringBuilder sb3 = new StringBuilder();
            foreach (var s in list)
            {
                sb3.Append(string.Format("{0}  ==>[{1}]  ==>[{2}] \n", s.Item, s.Size, s.DependCount));
            }
            
            SaveTextToFile("AbLog/AbData.txt", sb3.ToString());
            
        }
 
        private static int ItemComparer(Status s1, Status s2)
        {
            return String.Compare(s1.Item, s2.Item, StringComparison.Ordinal);
        }
        
        private static int SizeComparer(Status s1, Status s2)
        {
            return -s1.Size.CompareTo(s2.Size);
        }
        
        private static int DependCountComparer(Status s1, Status s2)
        {
            return -s1.DependCount.CompareTo(s2.DependCount);
        }
 
        private static void GetSize(string item)
        {
            if (TempChecked.Contains(item))
            {
                return;
            }
            
            var path = GetPath(item);
            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                TempStatus.Size += fi.Length;
                TempStatus.DependCount++;
            }
 
            TempChecked.Add(item);
 
            var depends = Manifest.GetAllDependencies(item);
            foreach (var depend in depends)
            {
                GetSize(depend);
            }
        }
        
        private static void SaveTextToFile(string file, string text)
        {
            byte[] buff = Encoding.Default.GetBytes(text);
            SaveFile(file, buff);
        }
 
        private static void SaveFile(string file, byte[] datas)
        {
            string path = file;
            using (FileStream fs = File.Create(path))
            {
                fs.Write(datas, 0, datas.Length);
            }
        }
 
        private static string GetPath(string file)
        {
            return AssetsFolder + file;
        }
        
        
        
    }
}
```

