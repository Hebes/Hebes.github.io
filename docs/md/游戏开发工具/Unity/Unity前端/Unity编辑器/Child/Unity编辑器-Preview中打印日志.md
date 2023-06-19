# Unity编辑器-Preview中打印日志

Preview窗口除了可以预览模型之外，我们还可以做别的操作。
今天我们来写个小工具在Preview窗口中显示调试信息。
可以看下面的图，同样是打印 health 和 power 的日志，在 Preview 中显示比在 Console 中显示舒服多了。
左边是Console中显示,右边是Preview窗口中显示。

![1](\../Image/Unity编辑器-Preview中打印日志/1.gif)

```C#
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Object), true)]
public class PreviewGUIEditor : Editor {
  /** Update every 15th frame. */
  private const int updateOnFrame = 15;

  private GUIStyle _previewLabelStyle;

  private GUIStyle previewLabelStyle {
    get {
      if (_previewLabelStyle == null) {
        _previewLabelStyle = new GUIStyle("PreOverlayLabel") {
          richText = false,
          alignment = TextAnchor.UpperLeft,
          fontStyle = FontStyle.Normal
        };
        // Try to get a fixed-width font on macOS.
        var font = Font.CreateDynamicFontFromOSFont("Monaco", 12);
        // Failing that, try to get a fixed-width font on Windows.
        if (font == null)
          font = Font.CreateDynamicFontFromOSFont("Lucida Console", 12);
        // XXX What fixed-width font should I request if we're on Linux?
        if (font != null)
          _previewLabelStyle.font = font;
        // Debug.Log("Fonts: \n" + string.Join("\n", Font.GetOSInstalledFontNames()));
      }
      return _previewLabelStyle;
    }
  }

  public override bool HasPreviewGUI() {
    return Application.isPlaying;
  }

  public override bool RequiresConstantRepaint() {
    // Only repaint on the nth frame.
    return Application.isPlaying && Time.frameCount % updateOnFrame == 0;
  }

  public override void OnPreviewGUI(Rect rect, GUIStyle background) {

    string str = target.ToString();

    GUI.Label(rect, str, previewLabelStyle);
  }
}
```

在我们需要打印日志的类里面 重载ToString()函数，返回需要在preview中输出的内容。

下面是上面截图的示例，一个Player类，在ToString()函数中返回了 health 和 power的输出内容。

```C#
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10; 
    public int power = 10;
    // Use this for initialization
    void Start () {
    
    }

    // Update is called once per frame
    void Update ()
    {
        health += 1;
        power += 2; 
        Debug.LogError("health = "+ health);
        Debug.LogError("power  = "+ power);

    }   
    public override string ToString()
    {
        return "health = " + health+"\n"+
               "power  = " + power;
    }
}
```
