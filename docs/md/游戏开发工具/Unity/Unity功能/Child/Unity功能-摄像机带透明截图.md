# Unity功能-摄像机带透明截图

**[Unity3D 摄像机带透明截图](<https://www.cnblogs.com/shamoyuu/p/CropCamera.html>)**

```c#
using System;
using UnityEngine;
using System.IO;

public class CropPicture : MonoBehaviour
{
    public Camera cropCamera; //待截图的目标摄像机
    RenderTexture renderTexture;
    Texture2D texture2D;

    void Start()
    {
        renderTexture \= new RenderTexture(800, 600, 32);
        texture2D \= new Texture2D(800, 600, TextureFormat.ARGB32, false);
        cropCamera.targetTexture \= renderTexture;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RenderTexture.active \= renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            RenderTexture.active \= null;
            
            byte\[\] bytes = texture2D.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath \+ "//pic//" + (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds + ".png", bytes);
        }
    }
}
```

然后把这个脚本拖到主摄像机上

新建一个需要截图的摄像机，为什么需要新建呢？因为它不能有天空盒。

然后把这个摄像机物体拖到主摄像机CropPicture脚本上的CropCamera变量上

然后设置这个需要截图的摄像机的属性如下

![1](https://images2015.cnblogs.com/blog/669149/201706/669149-20170611211859215-155846366.png)

颜色这里只要A是0就可以了，其他3个随意

在工程目录下新建一个pic文件夹，然后运行，按空格就截图了

![2](https://images2015.cnblogs.com/blog/669149/201706/669149-20170611213006387-1511188797.jpg)
