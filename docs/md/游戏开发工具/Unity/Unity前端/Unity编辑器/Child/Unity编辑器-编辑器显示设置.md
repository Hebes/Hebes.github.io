# Unity编辑器-编辑器显示设置

编辑器显示在指定位置

```C#
window = (MyTools)GetWindow(typeof(MyTools));
window.titleContent.text = "字体设置";
window.position = new Rect(PlayerSettings.defaultScreenWidth / 2,PlayerSettingsdefaultScreenHeight / 2, 400, 160);
window.Show();

window.Close();
```
