# Unity功能-游戏FPS

## 方案一

```C#
using UnityEngine;

public class FPSSample : MonoBehaviour
{
    //更新速度
    public float updateInterval = 1.0f;
    private float _timer;
    private float averageFramerate;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        float smoothDeltaTime = Time.smoothDeltaTime;
        _timer = _timer <= 0 ? updateInterval : _timer -= smoothDeltaTime;
        if (_timer <= 0)
        {
            averageFramerate = 1 / smoothDeltaTime;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle { fontSize = 27 };
        style.normal.textColor = new Color(1, 1, 1);
        GUI.Label(new Rect(100, 100, 200, 200), averageFramerate.ToString("F2"), style);
    }
}
```

## 方案二

```C#
using LogUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 显示FPS模块
/// https://blog.csdn.net/f_957995490/article/details/104470689
/// </summary>
public class FPSModule : MonoBehaviour
{

    float _updateInterval = 1f;//设定更新帧率的时间间隔为1秒  
    float _accum = .0f;//累积时间  
    int _frames = 0;//在_updateInterval时间内运行了多少帧  
    float _timeLeft;
    string fpsFormat;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _timeLeft = _updateInterval;
    }

    void OnGUI()
    {
        //定义一个GUIStyle的对象  
        GUIStyle labelFont = new GUIStyle();
        //设置文本颜色 
        labelFont.normal.textColor = new Color(0, 0, 0);
        //设置字体大小
        labelFont.fontSize = 40;
        GUI.Label(new Rect(100, 100, 200, 200), fpsFormat, labelFont);
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        //Time.timeScale可以控制Update 和LateUpdate 的执行速度,  
        //Time.deltaTime是以秒计算，完成最后一帧的时间  
        //相除即可得到相应的一帧所用的时间  
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;//帧数  

        if (_timeLeft <= 0)
        {
            float fps = _accum / _frames;
            //Debug.Log(_accum + "__" + _frames);  
            fpsFormat = System.String.Format("{0:F2}FPS", fps);//保留两位小数  
            //Debug.LogError(fpsFormat);

            _timeLeft = _updateInterval;
            _accum = .0f;
            _frames = 0;
        }
    }
}
```

## 方案三

**[Unity FPS 计算](<https://blog.51cto.com/u_15127702/4443571>)**

```C#
using UnityEngine;
using System.Collections;

public class FPSDisplay : MonoBehaviour{

 private float m_time = 0.0f;

 void Update(){
  m_time += (Time.unscaledDeltaTime - m_time) * 0.1f;
  
  float ms = m_time * 1000.0f;
  float fps = 1.0f / m_time;
  
  Debug.Log($"{fps} FPS ({ms}ms)");
 }

}
```
