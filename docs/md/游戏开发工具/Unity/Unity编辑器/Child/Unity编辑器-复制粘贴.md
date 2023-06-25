# Unity编辑器-复制粘贴

```C#
//复制方法1 可以不在编辑器的情况下使用
public void CopyFunction1(string value)
{
    TextEditor text = new TextEditor();
    text.text = value;
    text.OnFocus();
    text.Copy();
}   
 //复制方法2
public void CopyFunction2(string value)
{
    GUIUtility.systemCopyBuffer = value;
}   
 //粘贴方法
public string PasteFuction()
{
    return GUIUtility.systemCopyBuffer;
}
```

```C#
/// <summary>
/// Copy到剪切板 https://blog.csdn.net/LLLLL__/article/details/114463650
/// </summary>
public static void ACCopyWord(this StringBuilder str)
{
    TextEditor te = new TextEditor();
    te.text = str.ToString();
    te.SelectAll();
    te.Copy();
}
```
