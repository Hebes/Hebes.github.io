# Unity功能-分辨率设置

```c#
//ExclusiveFullScreen独占全屏,FullScreenWindow全屏窗口,MaximizedWindow最大化窗口,Windowed窗口
Screen.SetResolution(1920,1080,FullScreenMode.ExclusiveFullScreen);

//pr1:宽度,pr2:高度,pr3:是否为全屏(此全屏为无边框窗口全屏)
Screen.SetResolution(1920,1080,true);
```
