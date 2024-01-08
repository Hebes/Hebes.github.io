# Unity编辑器-Unity编辑器窗口

**[Unity 拓展编辑器 - 文章仍在不断更新中](<https://zhuanlan.zhihu.com/p/570387531>)**

**[Unity拓展编辑器 —— EditorWindow（一）](<https://blog.csdn.net/qq_35030499/article/details/88350521>)**

**[Unity Editor拓展编辑器大全](<https://blog.csdn.net/linxinfa/article/details/105387179>)**

**[Unity编辑器开发](<https://blog.csdn.net/qq_42316280/category_11487414.html>)**

**[unity编辑器扩展#2 GUILayout、EditorGUILayout 控件整理](<https://blog.csdn.net/qq_38275140/article/details/84778344>)**

**[使用unity自带API EditorUtility打开本地文件夹或者文件，以及打包后发布打开本地文件夹](<https://blog.csdn.net/qq_30300405/article/details/89002083>)**

https://zhuanlan.zhihu.com/p/503154643

https://blog.csdn.net/WenHuiJun_/article/details/108975211

在EditorWindow子类的OnGUI()函数中使用GUILayout.Button，可以绘制一个按钮。但默认按钮的大小是占据所用可用空间的。我想让按钮的大小只是刚好包裹文字，实现方法如下:

``` C#
GUI.skin.button.wordWrap = true; // 这行不能少
if (GUILayout.Button("按钮", GUILayout.Width(0)))
{
    // do something
}
```

另外，想要绘制一个数字输入框用:

``` C#
float num=0;
num = EditorGUILayout.FloatField("请输入一个数字:", num);
```

想要绘制一个进度条:

``` C#
float ratio = 0.1f;
ratio = GUILayout.HorizontalSlider(ratio, 0.0f, 1.0f);
```

打开文件夹目录:

``` C#
EditorUtility.RevealInFinder(“路径”);
```

C#中将string转换为float:

``` C#
string str = "1.01";
float num = Convert.ToSingle(str);
// 或者
float tmp;
if (float.TryParse(str, out tmp))
{
    num = tmp;
}
// 不建议使用,因为若str无法转换成float会抛异常造成程序崩溃。
num = float.Parse(str);
```

消息提示

``` C#
if (EditorUtility.DisplayDialog("消息提示", "确定要删除吗", "确定", "取消"))
{
    DeleteGameObject();
}
else
{

} 
```

开启滚动视图

``` C#
private Vector2 scrollRoot;

scrollRoot = EditorGUILayout.BeginScrollView(scrollRoot); //开启滚动视图
{

}
EditorGUILayout.EndScrollView(); //结束滚动视图
```
