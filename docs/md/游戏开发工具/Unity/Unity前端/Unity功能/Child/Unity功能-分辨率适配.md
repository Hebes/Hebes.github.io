# Unity功能-分辨率适配

## 分辨率适配方法1

Canvas->Render Mode->Screen Space - Camera

Canvas Scaler->Scale Mode->Scale With Screen Size

## 分辨率适配方法2

Canvas->Render Mode->World Space

Event Camera->映射游戏内容的Camera

Rect Transform的宽高为分辨率宽高,Scale的x,y设置为0.01

## 分辨率适配方法3

```c#
public class TestCamera:MonoBehaviour
{
    const float devHeight = 9.6f;
    const float devWidth = 6.4f;
    void start()
    {
        float screenHeight = Screen.height;//获取屏幕宽度
        Debug.Log("screenHegiht = " + screenHeight);
        //拿到相机的正交属性设置摄像机大小
        float orthographicSize = this.GetComponent<Camera>().orthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height; //宽高比
        //实际的宽高比和摄像机的orthographicSize值来计算出摄像机的宽度值
        float cameraWidth = orthographicSize * 2 * aspectRatio;
        Debug.Log("cameraWidth = " + cameraWidth);
        //如果摄像机宽度小于设计的尺寸宽度
        if(cameraWidth < devWidth)
        {
            //将尺寸宽度/2倍的宽高比 = 相机大小
            orthographicSize = devWidth / (2 * aspectRatio);
            Debug.Log("new orthographicSize = " + orthographicSize);
            //将这个摄像机大小赋值回摄像机属性
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
        }
    }
}
```
