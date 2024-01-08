# Unity功能-异步过渡场景

## Unity 3D开发--SceneManager场景管理(异步使用同一个过渡场景)

在U3D开发过程中经常使用到多场景的切换，有同步SceneManager.LoadScene()和异步SceneManager.LoadSceneAsync()两种方法，同步的话一般就会卡住界面直到加载完成，使用异步的话一般都做一个加载的进度条，每次切换的时候都需要一个加载动画，所以需要建一个专门的过渡加载场景来进行统一加载，也可以避免场景直接切换出现的黑屏。

一、建立一个单例进行切换，在项目代码的任何位置都可以调用

```C#
 
public class LoadSceneManager 
{
    private static LoadSceneManager instance;
    public static LoadSceneManager Instance
    {
        get {
            if (instance == null)
            {
                instance = new LoadSceneManager();
            }
            return instance;
        }
    }
 
    public string nextSceneName;
 
    //异步加载
    public void LoadSceneAsync(string sceneName)
    {
        Debug.Log("LoadSceneAsync:" + sceneName);
        nextSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log("LoadScene:" + sceneName);
        SceneManager.LoadScene(sceneName);
    }
 
}
```

使用的时候直接使用LoadSceneManager.Instance.LoadSceneAsync("SecondScene");

二、异步加载的过程

要做个进度条然后进行管理，代码很简单就不说了

```C#
 
public class LoadingController : MonoBehaviour
{
    public Slider loadingSlider;
    public TMP_Text loadingText;
    private AsyncOperation asyncOperation;
    private float operationProgress;
 
    // Start is called before the first frame update
    void Start()
    {
        loadingSlider.value = 0.0f;
        if (SceneManager.GetActiveScene().name == "LoadingScene")
        {
            StartCoroutine("LoadingScene");
        }
    }
 
    IEnumerator LoadingScene()
    {
        asyncOperation = SceneManager.LoadSceneAsync(LoadSceneManager.Instance.nextSceneName);
        asyncOperation.allowSceneActivation = false;
        yield return asyncOperation;
    }
 
    // Update is called once per frame
    void Update()
    {
        operationProgress = asyncOperation.progress;
        //最大值只到0.9，后面有进行插值运算更新
        if (Mathf.Approximately(operationProgress, 0.9f))
        {
            operationProgress = 1;
        }
        if (Mathf.Approximately(operationProgress, 0.6f))
        {
            System.Threading.Thread.Sleep(5000);
        }
        UpdateLoadingUI(operationProgress);
    }
    private void UpdateLoadingUI(float value)
    {
        if (operationProgress != loadingSlider.value)
        {
            loadingSlider.value = Mathf.Lerp(loadingSlider.value, operationProgress, Time.deltaTime * 2);
            if (Mathf.Approximately(operationProgress, operationProgress))
            {
                loadingSlider.value = operationProgress;
            }
        }
        Debug.Log("Progress:" + loadingSlider.value);
        if (loadingText != null)
        {
            loadingText.text = Mathf.Round(loadingSlider.value * 100) + "%";
        }
 
        //自动换场景
        if (Mathf.Approximately(loadingSlider.value, 1.0f))
        {
            asyncOperation.allowSceneActivation = true;
        }
    }
}
```
