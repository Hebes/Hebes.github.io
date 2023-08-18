# Unity编辑器-MenuItem及相关操作

使用**MenuItem**在编辑模式下添加选项按钮，实现不同的功能以满足开发需求。

注意：  
1.使用**MenuItem**需要引入**UnityEditor**命名空间  
2.添加的选项按钮执行的函数不需要管私有或者公有的，但需要是**静态**的，确保能够直接访问。

## MenuItem参数简单介绍

```csharp
   //MenuItem(string itemName, bool isValidateFunction, int priority)
   //itemName：菜单名称路径
   //isValidateFunction：默认false，true则表示点击菜单前就会调用，用来添加验证方法
   //priority：菜单项显示排序，越小就会越显示在上方，当相邻的两个优先级差大于11时，会自动被划分成两个组，不同的组中间会有一条分割线
```

## MenuItem参数功能验证

1.MenuItem 第二个参数 isValidateFunction 验证

```csharp
    //该参数的功能是给选项按钮添加一个验证方法，在未点击选项按钮之前执行，通过验证方法的返回值来判断添加的选项按钮是否可以点击
    [MenuItem("Tools/DelateAll", true, 10)]//添加一个DelateAll选项按钮的验证方法，路径需与添加的按钮保持一致
    static bool CheckDelate()//需要有一个bool返回值来验证
    {
        if (Selection.objects.Length > 0)//验证的一些条件
        {
            return true;//返回true 则DelateAll按钮是高亮可点击状态
        }
        return false;//false 则DelateAll按钮是灰色不可点击状态
    }

    [MenuItem("Tools/DelateAll", false, 10)]//添加一个DelateAll选项按钮
    static void Delate()
    {}
```

2.MenuItem 第三个参数priority验证

```csharp
    //显示顺序应该为：test2 test1 | test3 （|表示分组分割线）
    [MenuItem("Tools/test1", false, 1)]
    static void Test1()
    {}
    [MenuItem("Tools/test2", false, 0)]
    static void Test2()
    {}
    [MenuItem("Tools/tes3", false, 30)]
    static void Test3()
    {}
```

效果如下：  
![1](https://img-blog.csdnimg.cn/20200525221321955.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQxNDY4MjE5,size_16,color_FFFFFF,t_70)
<!-- ![1](\../Image/Unity编辑器-MenuItem及相关操作/1.png) -->

## 记录一下在不同位置添加选项按钮的方法

1.Unity上方菜单栏自定义按钮

```csharp
    [MenuItem("Tools/DoSomethingTest")]//自定义路径,支持（"Tools/Test/DoSomethingTest"）
    static void DoSomethingTest_1()
    {}
```

2.在Unity系统功能栏里添加选项按钮，只需把路径改到系统选项路径

```csharp
    [MenuItem("Window/DoSomethingTest")]//在Window栏里添加选项按钮
    static void DoSomethingTest_2()
    {}
```

3.在Hierarchy窗口右键单击弹出的菜单栏添加选项按钮

```csharp
    //发现Hierarchy窗口的右键菜单栏其实就是GamemObject下的部分选项按钮，可以通过如下路径添加
    [MenuItem("GameObject/DoSomethingTest", false, 10)]
    static void DoSomethingTest_3()
    {}
```

4.在Project窗口右键单击弹出的菜单栏添加选项按钮

```csharp
    //发现Project窗口的右键菜单栏其实就是Assets下的选项按钮，可以通过添加到Assets下实现
    [MenuItem("Assets/DoSomethingTest")]
    static void DoSomethingTest_4()
    {}
```

5.当选中Hierarchy窗口里的物体时，在右键单击Inspector面板里组件时弹出的菜单栏里添加选项按钮，有两种方法，一种是使用**MenuItem**，一种是在需要添加的脚本里使用**ContextMenu**

```csharp
    [MenuItem("CONTEXT/PlayerMove/Change PlayerMove Something")]//固定用法（"CONTEXT/组件名/选项按钮名"）
    static void Test4(MenuCommand command)//一般用于一键初始化组件数据
    {
        var com = command.context as PlayerMove;//MenuCommand.context就是右键选中的当前组件
        if (com != null)//修改当前组件的值
            com.m_Speed = 13;
    }
```

```csharp
public class PlayerMove : MonoBehaviour
{
    //两个参数
    //name 要对该变量添加的选项按钮
    //function 点击选项后执行的方法，必须存在，否则添加按钮不成功
    [ContextMenuItem("ChangeSpeed_2", "ChangeSpeed")]//对组件里该变量添加右击选项按钮ChangeSpeed_2，参数(name,function)
    public float m_Speed = 4;
    
    public bool m_CanShoot = false;
        
    //对该脚本组件右击添加选项按钮ChnageSpeed_1
    [ContextMenu("ChangeSpeed_1")]
    void ChangeSpeed()
    {
        m_Speed = 10;
    }
}
```

## 添加的菜单选项自定义快捷键

注：**快捷键** 与 **选项路径** 需要隔开

```csharp
    //[MenuItem("Tools/Test _t")]//普通快捷键，t，  使用下划线_
    //[MenuItem("Tools/Test %t")]//组合快捷键，Ctrl+t， %代表Ctrl
    //[MenuItem("Tools/Test #t")]//组合快捷键，Shift+t，    #代表Shift
    //[MenuItem("Tools/Test &t")]//组合快捷键，Alt+t，  &代表Alt
    [MenuItem("Tools/Test %#t")]//组合快捷键，Ctrl+Shift+t
    static void Test6()
    {
        Debug.Log("Test 6");
    }
```

## 二、Selection相关

editor模式下一般根据需求来操作单个或多个物体，这时候需要按条件获取到一些选中的object，通过**UnityEditor.Selection**来获取

```csharp
    [MenuItem("Tools/Show Select Info")]
    static void Test5()
    {
        //列举几个常用
        //Selection 通过里面的变量和静态方法按需求获取object
        //SelectionMode 选择模式，用来过滤选择用的
        Debug.Log(Selection.objects.Length);//选中的所有物体，包括Hierarchy和Project窗口
        Debug.Log(Selection.activeObject);//返回当前点击的场景游戏物体或Project资源(包括场景、脚本、预制等任意);选择多个则返回第一个选择的；未选择相应的则返回null
        Debug.Log(Selection.GetFiltered<TextAsset>(SelectionMode.Assets).Length);//返回按类型和模式过滤的当前选择,该行是筛选在Assets文件夹下选中的所有文本文件
    }
```

Selection里面的静态变量和静态函数基本能满足需求。

官方文档介绍：[https://docs.unity3d.com/ScriptReference/Selection.html](https://docs.unity3d.com/ScriptReference/Selection.html)  
详细博客介绍：[https://blog.csdn.net/qq\_33337811/article/details/72858209](https://blog.csdn.net/qq_33337811/article/details/72858209)
