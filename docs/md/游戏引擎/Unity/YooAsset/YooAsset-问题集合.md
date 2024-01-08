# YooAsset-问题集合

## 问题一

Unity 出现error CS0103: The name ‘AssetDatabase‘ does not exist in the current context

### 问题描述

在Unity场景中，在进行build操作时出现这种报错，导致资源bundle无法正常生成，出现以下问题：

```C#
error CS0103: The name 'AssetDatabase' does not exist in the current context

error CS0234: The type or namespace name 'AssetDatabase' does not exist in the namespace 'UnityEditor' (are you missing an assembly reference?)
```

ps:上面两种错误都是同一种问题造成的，报错不一样的原因是由于UnityEditor在代码中的位置不同造成的：
前者是在开头声明了using UnityEditor，方法中使用AssetDatabase.LoadAssetAtPath；
后者是未声明using UnityEditor，而是在方法中直接使用了UnityEditor.AssetDatabase.LoadAssetAtPath

### 原因分析

在非Editor文件夹下的脚本中，存在着有关UnityEditor方法的使用

方法中第一行使用了UnityEditor中的AssetDatabase.LoadAssetAtPath方法，并且该方法所在的文件并非是在Editor文件夹下，导致build操作时出现报错

### 解决方案

```C#
添加
#if UNITY_EDITOR
…
#endif
```

在方法外部，加上#if UNITY_EDITOR #endif，保证该方法只有在Unity编辑器中运行

### 注意

注：需要把调用该方法的地方也要用#if #endif包括起来
因为该方法时需要被调用的，然后测试的时候出现了以下问题
error CS0103: The name 'EditorLoadAsset' does not exist in the current context
出现问题的原因是调用此方法的地方未用#if #endif包含进去，在正式运行状态下，他会认为该方法不存在，找不到该方法导出出现报错。所以要将调用该方法的地方也要用#if #endif包括进来，让正式运行状态下也不用执行调用该方法的语句

## Cannot load asset bundle file using LoadRawFileAsync method

修改yooaseet PackRule 为 PackRawFile 目录下的资源文件会被处理为原生资源包
