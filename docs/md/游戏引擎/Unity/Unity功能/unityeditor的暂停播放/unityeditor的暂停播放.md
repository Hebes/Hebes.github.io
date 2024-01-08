# unityeditor的暂停播放

```CSharp
public static void Play()
{
    //编辑器播放
    EditorApplication.isPlaying = true;
}

public static void Pause()
{
    //编辑器暂停
    EditorApplication.isPaused = true;
}

public static void Stop()
{
    //编辑器停止播放
    EditorApplication.isPlaying = false;
}

EditorApplication.playmodeStateChanged = OnUnityPlayModeChanged;
```
