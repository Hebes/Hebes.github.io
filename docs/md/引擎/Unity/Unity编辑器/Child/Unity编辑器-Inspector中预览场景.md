# Unity编辑器-Inspector中预览场景

## 代码

![1](\../Image/Unity编辑器-Inspector中预览场景/1.gif)

```C#
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;

[CustomEditor(typeof(SceneAsset))]
[CanEditMultipleObjects]
public class ScenePreview : Editor
{
    const string PreviewFolders = "_scenes"; //你可以修改为你自己的路径，用来存放场景缩略图
    static bool _shouldRefreshDatabase;

    [RuntimeInitializeOnLoadMethod]
    public static void CaptureScreenshot()
    {
        var previewPath = GetPreviewPath(SceneManager.GetActiveScene().name);
        var dir = Path.GetDirectoryName(previewPath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        Debug.LogFormat("Saving scene preview at {0}", previewPath);
        ScreenCapture.CaptureScreenshot(previewPath);
        Debug.LogFormat("Scene preview saved at {0}", previewPath);

        _shouldRefreshDatabase = true;
    }

    public override void OnInspectorGUI()
    {        
        if (_shouldRefreshDatabase)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            _shouldRefreshDatabase = false;
        }

        var sceneNames = targets.Select(t => ((SceneAsset)t).name).OrderBy(n => n).ToArray();

        var previewWidth = 200;
        var previewHeight = 200;

        for (int i = 0; i < sceneNames.Length; i++)
        {
            DrawPreview(i, sceneNames[i], previewWidth, previewHeight);
        }
    }

    void DrawPreview(int index, string sceneName, float width, float height)
    {
        var previewPath = GetPreviewPath(sceneName);
        var preview = Resources.Load(sceneName) as Texture;

        if (preview == null)
        {
            EditorGUILayout.HelpBox(string.Format(
                "还没有场景{0}的预览图{1}. 请切换到这个场景然后点击播放，会自动生成该场景的缩略图",
                sceneName,
                previewPath),
                MessageType.Info);
        }
        else
        {
            GUILayout.Button(preview,GUILayout.Width(width),GUILayout.Height(height));
        }
    }

    static string GetPreviewPath(string sceneName)
    {
        return string.Format("{0}/{1}/Resources/{2}.png", Application.dataPath, PreviewFolders, sceneName);
    }
}
```

## 参考

![Unity3D 在 Inspector 中预览场景](<https://blog.csdn.net/piai9568/article/details/99486887>)
