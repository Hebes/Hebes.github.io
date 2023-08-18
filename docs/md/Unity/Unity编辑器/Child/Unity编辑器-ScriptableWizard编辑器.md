# Unity编辑器-ScriptableWizard编辑器

```C#
using UnityEditor;
using UnityEngine;

//继承ScriptableWizard
public class ChangePlayerDialog : ScriptableWizard
{
    //添加选项按钮，创建编辑器向导窗口
    [MenuItem("Tools/Create Player Wizard")]
    static void CreateWizard()
    {
        //创建的向导类型为ChangePlayerDialog类型
        ScriptableWizard.DisplayWizard<ChangePlayerDialog>("ChangePlayerMove", "Change By Value", "Clear");//参数分别是向导窗口标题，create按钮名称，自定义按钮名称
    }

    //显示在向导窗口里的变量
    public float speedValue = 10;
    public bool canShoot = true;

    //用户单击创建按钮进行调用，固定用法，点击后向导窗口关闭
    private void OnWizardCreate()
    {
        Debug.Log("Create : Change By Value");
        //一般在这里做最终的处理
        //比如在这里可以获取选中的全部object，再利用向导窗口里填写的变量批量改变文件数值
    }

    //用户单击自定义其他按钮时进行调用，固定用法，点击后向导窗口不会关闭
    private void OnWizardOtherButton()//在这里是一个初始化数据的功能
    {
        Debug.Log("Clear");
        speedValue = 0;
        canShoot = false;
        ShowNotification(new GUIContent("数据已经初始化完毕"));//该功能用来弹出一个小提示通知信息，几秒后自动消失
    }

    //打开向导或者更改向导里的内容时进行调用，固定用法
    private void OnWizardUpdate()
    {
        Debug.Log("Change");
        //当在向导窗口里一些操作错误或者不规范时，可以通过设置helpString和errorString来进行提示操作人员
        helpString = "文中某某变量填写规范为\"XXXX+DDDD+SS\"";
        errorString = "文中某某变量填写不规范";

        //编辑器模式下，可以使用EditorPrefs进行数据的存取，用法与游戏运行时PlayerPrefs用法一致
        EditorPrefs.SetFloat("key", speedValue);
        
    }

    //当在工程里选中操作有变化时进行调用
    private void OnSelectionChange()
    {
        OnWizardUpdate();
    }
}
```
