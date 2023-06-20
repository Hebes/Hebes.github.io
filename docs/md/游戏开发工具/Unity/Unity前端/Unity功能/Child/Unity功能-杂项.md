# Unity功能-杂项

## unity事件接口

```javascript {.line-numbers}
using UnityEngine;
using UnityEngine.EventSystems;

public class test : MonoBehaviour, IPointerClickHandler,IDragHandler
{
    /// <summary>
    /// 拖拽的方法
    /// </summary>
    /// <param name="eventData"></param>
    Vector3 v3Pos;
    public void OnDrag(PointerEventData eventData)
    {
        //仅仅适用于覆盖模式
        //eventData.position 光标的位置（屏幕坐标）
        //transform.position = eventData.position;
        //通用
        //将屏幕坐标转换为世界坐标
        RectTransform parentRTF = transform.parent as RectTransform;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRTF, eventData.position, eventData.pressEventCamera,out v3Pos);
        transform.position = v3Pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount==2)
        {
            print("点击了:"+ eventData.clickCount);
        }
    }

    void Update()
    {
        print(v3Pos);
    }
}
```

## 物体吸附

建造游戏中隐藏墙体可将对应墙体的MeshRenderer->Lighting->CastShadows中选择Shadows Only选项

复制运行时的物体,然后粘贴至未运行时(例如运行时让物体下落,等物体静止后再复制物体,然后结束运行,将下落后的物体粘贴到未运行状态)

按住V移动物体可让两个3D物体对齐,也同样可以用于吸附

按住Ctrl+Shift移动物体会让物体根据碰撞体和原点位置吸附在其他面上

物体移动穿模原因:transform.position与Rigidbody.position不一致,后者比前者慢一帧

```c#
//解决方法1:用Rigidbody.MovePosition方法代替transform移动
//将移动方法添加至FixedUpdate使移动更稳定
//此方法不受重力影响
Rigidbody.MovePosition(Rigidbody.position + Vector3.Up * Time.deltaTime * speed);
```

引擎崩溃:项目文件->Temp->_Backupscenes

## 物体倾斜情况下向前移动

```c#
//根据Y轴旋转角修改水平前进的方向
Vector3 dir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
//世界空间下移动
//(默认Space.Self情况下,Vector3.forward = transform.forward)
transform.Translate(dir.normalized * Time.deltaTime * Speed, Space.World);
```

## Shader Graph

alt + 左键 拖拽

space 创建

F 移动到选中项

右键节点 可一次性断开所有连接

## 模型缩放

对模型按T切换到Rect Tool模式下也可进行缩放

## 保存预制体显示的图片

```c#
//using UnityEditor;
//using System.IO;
GameObject prefab = Resources.Load<GameObject>("ObjectPrefab");
Texture2D Tex = AssetPreview.GetAssetPreview(prefab);
// Encode texture into PNG
byte[] bytes = Tex.EncodeToPNG();
File.WriteAllBytes(Application.dataPath + "/../Assets/SavedScreen.png", bytes);

```

## 旋转

```c#
transform.Rotate(transform.up * RotateSpeed);
transform.rotation *= Quaternion.Euler(0,10,0);
rigidbody.angularVelocity = transform.up * speed;
//围绕着另外的一个物体的某个轴进行旋转
transform.RotateAround(transform.parent.transform.position, transform.up, speed * Time.deltaTime);
//看向某个物体
transform.LookAt(transform);
```

## 发送数据SendMessage

```c#
//接收者gameObject arg1:需要执行的函数名称;arg2:值;arg3:是否需要返回值
gameObject.SendMessage(string methodName, value, SendMessageOptions);
```

## Assert(断言)

用于判断物体

UnityEngine.Assertions;

## UI

启用/禁用按钮

```c#
//启用
Button _btnImg;
_btnImg.enabled = true;
Button.interactable = true;
```

点击事件

```c#
using UnityEngine.EventSystems;
public class test:MonoBehaviour,IPointerClickHandler,IPointer,IDragHandler
{
    
}

```



## Text过长显示省略号

```c#
using UnityEngine;
using UnityEngine.UI;
public static class TextExtension
{
    /// <summary>
    /// 文本过长省略号显示
    /// </summary>
    /// <param name="textComponent">目标Text</param>
    /// <param name="value">需要填的值</param>
    public static void SetTextWithEllipsis(this Text textcomponent, string value)
    {
        if(value.Length <= 0) return;
        var generator = new TextGenerator();
        var rectTransform = textComponent.GetComponent<RectTransform>();
        var settings = textComponent.GetGenerationSettings(rectTransform.rect.size);
        generator.Populate(value, settings);

        // truncate visible value and add ellipsis
        var characterCountVisible = generator.characterCountVisible;
        if (characterCountVisible <= 0) return;
        var updatedText = value;
        if (value.Length > characterCountVisible)
        {
            updatedText = value.Substring(0, characterCountVisible - 1);
            updatedText += "…";
        }

        // update text
        textComponent.text = updatedText;
    }
    
    /// <summary>
    /// 文本过长省略号显示
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    public static void SetTextWithEllipsis(this Text textComponent, string value, int characterVisibleCount)
    {

        var updatedText = value;

        // 判断是否需要过长显示省略号
        if (value.Length > characterVisibleCount)
        {
            updatedText = value.Substring(0, characterVisibleCount - 1);
            updatedText += "…";
        }

        // update text
        textComponent.text = updatedText;
    }


    #region 非通用方法
    /// <summary>
    /// 显示信息与日期
    /// </summary>
    /// <param name="txtComponent">Text组件</param>
    /// <param name="msg">信息</param>
    /// <param name="dateTime">日期</param>
    public static void ShowMsgDate(this Text txtComponent, string msg, string dateTime)
    {
        int size = txtComponent.fontSize;
        txtComponent.text = $"{msg}\n" +
            $"Date <size={size-2}>{dateTime}</size> ";
    }
    #endregion
}
```

## 场景加载

```c#
using UnityEngine.SceneManagement;
//单例
public class Singleton:MonoBehaviour
{      
    private static Singleton _instance;
    public static Singleton Instance{get{return _instance;}}
    public string SceneName;
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        //切换场景时不删除自己
        DontDestoryOnLoad(gameObject);
        SceneManager.LoadScene(string newsceneName);
    }
}

public class StartGame:MonoBehaviour
{
    public void LoadScene()
    {
        //同步加载
        SceneManager.LoadScene(string loadSceneName);
        //加载场景需要加载的场景名称
        Singleton.Instance.SceneName = string;
    }
}

public class TransitionPanel:MonoBehaviour
{
    public UISprite LoadSystem;
    //public Slider LoadSystem;//UGUI
    bool _isUpdate = false;
    AsyncOperation _async;
    private void Start()
    {
        LoadSceneAsync(Singleton.Instance.SceneName);
    }
    private void Update()
    {
        if(isUpdate)
            LoadSystem.fillAmount = _async.progress;
            //LoadSystem.value = _async.progress;//UGUI
    }
    public void LoadSceneAsync(string sceneName)
    {
        isUpdate = true;
        _async = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(OnLoadSceneAsync());        
    }
    private IEnumerator OnLoadSceneAsync()
    {
        //等待异步完成
        yield return _async;
    }
}
```

## 存档

序列化类不可使用索引

### JsonUtility存档

```c#
//[Serializable] 添加在需要序列化的类前
public void SaveJson<T>(T saveClass,string path)
{
    if(!File.Exists(path))
        File.Create(path);
    string json = JsonUtility.ToJson(saveClass);
    File.WriteAllText(path, json);
    Debug.Log("保存成功");
}
public void ReadJson<T>(string path)
{
    if(!File.Exists(path))
    {
        Debug.Log("目录不存在");
        return;
    }
    string json = FileReadAllText(path);
    T typeClass = JsonUtility.FromJson<T>(json);
}
//序列化列表
[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> _target;
    public List<T> ToList(){return _target;}
    public Serialization(List<T> target)
    {
        _target = target;
    }
}
//序列化字典
[Serializable]
public class Serialization<TKey,TValue>:ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> _keys;
    List<TValue> _values;
    
    Dictionary<TKey,TValue> _target;
    public Dictionary<TKey,TValue> ToDictionary(){return _target;}
    public Serialization(Dictionary<TKey,TValue> target)
    {
        _target = target;
    }
    public void OnBeforeSerialize()
    {
        _keys = new List<TKey>(_target.keys);
        _values = new List<TValue>(_target.values);
    }
    public void OnAfterDeserialize()
    {
        var count = Math.min(_keys.Count, _values.Count);
        target = new Dictionary<TKey, Tvalue>(count);
        for(var i = 0; i < count;i++)
        {
            _target.Add(_keys[i],_values[i]);
        }
    }
}
/*
//示例
//List<T> -> Json
string str = JsonUtility.ToJson(new Serialization<Enemy>(enemies));
//Json -> List<T>
List<Enemy> enemies = JsonUtility.FromJson<Serialization<Enemy>>(str).ToList();

//Dictionary<TKey,TValue> -> Json
string str = JsonUtility.ToJson(new Serialization<int, Enemy>(enemies));
Dictionary<int, Enemy> enemies = JsonUtility.FromJson<Serialization<int, Enemy>>(str).ToDictionary();
*/
```

### 参考存档方式

```c#
//Interface
public interface IId
{
    uint Id{get;set;}
}
public interface ISavable:IId
{
    Type DataType{get;}
    Type DataContainerType{get;}
    void Read(object data);
    void Write(object data);
}
public interface ISavableContainer
{
    public IEnumerable<ISavable> Savables{get;set;}
}

//Class
//这个类型存储了实际的数据,相当于是一个数据库
[Serializable]
public class SaveDataContainer
{
    //存储实际物体的数据,需要将这个字典转换成数组并序列化
    private DIctionary<uint,IId> _data;
    //全数据
    public Dictionary<uint, IId> Data{get{return _data;}}
    //获取数据
    public IId GetData(uint id)
    {
        return _data[id];
    }
    //保存数据
    public void SetData(IId data)
    {
        _data[data.Id] = data;
    }
}

//Mono
//挂载在需要存档的物体上
public class SaveEntity:MonoBehaviour
{
    public void Save(SaveDataContainer container)
    {
        foreach(ISavable savable in GetSavables())
        {
            if(savable.DataContainerType == container.GetType())
            {
                IId newData = Activator.CreateInstance(savable.DataType) as IId;
                newData.Id = savable.Id;
                savable.Write(newData);
                container.SetData(newData);
            }
        }
    }
    //加载数据
    public void Load(SaveDataContainer container)
    {
        foreach(ISavable savable in GetSavables())
        {
            if(savable.DataContainerType == container.GetType())
            {
                IId data = container.GetData(savable.Id);
                savable.Read(data);
            }
        }
    }
    //获取全存档数据
    public IEnumerable<ISavable> GetSavables()
    {
        foreach(ISavable savable in GetComponents<ISavable>())
        {
            yield return savable;
        }
        foreach(ISavableContainer savableContainer in GetComponents<ISavableContainer>())
        {
            foreach(ISavable savable in savableContainer.Savables)
            {
                yield return savable;
            }
        }
    }
}

//Example
[Serializable]
public class ExampleSaveDataContainer:SaveDataContainer, ISerializationCallbackReceiver
{
    public List<UnitSave> Units;
    public List<ItemSave> Items;
    public List<InventorySave> Inventories;
    //序列化之后
    public void OnAfterDeserialize()
    {
        //将列表的数据赋值给字典
        for(int i = 0; i < Units.Count;i++)
        {
            Data.Add(Units[i].Id, Units[i]);
        }
        for(int i = 0; i < Inventories.Count;i++)
        {
            Data.Add(Inventories[i].Id, Units[i]);
        }
    }
    //序列化之前
    public void OnBeforeSerialize()
    {
        //将字典的数据赋值给列表
        Units = new List<UnitSave>(Data.Values.OfType<UnitSave>());
        Items = new List<ItemSave>(Data.Values.OfType<ItemSave>());
        Inventories = new List<InventorySave>(Data.Values.OfType<InventorySave>());
    }
}
```

## 异步

```c#
//Monobehavour.Invoke,不会自动销毁,执行时不会影响挂载脚本的物体
//多久之后执行MethodName,只执行一次
Invoke(string:MethodName, float:DelayTime);
//延迟执行MethodName,每隔一段时间执行一次
InvokeRepeat(string:MethodName, float:DelayTime, float:RepeatTime);
//销毁指定函数,若为空则销毁当前monobehavour的全部异步
CancelInvoke([string:MethodName]);

```

## 特性

```C#
//不允许场景中有多个挂载该脚本的物体
[DisallowMultipleComponent]

//当物体添加有该特性的类时会同时添加typeof内指定的类
[RequireComponent(typeof(BoxCollider2D))]

//代码在editor模式下也会调用
[ExecuteInEditorMode]

//修改变量名称后不丢失
[FormerlySerializedAs(“修改前的变量名”)]
```

## Animator

```c#
//判断动画
public void CheckAnime(int layer, string animeName)
{
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);
    if(stateInfo.IsName(animeName)&& stateInfo.normalizedTime >= 1.0f)//动画已经播放完毕
    {
        //执行完毕事件
    }
}
```

## 网络

### Netcode

```c#
NetworkManager.Singleton.   ;

```

### UnityWebRequest

```c#
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class CSAPIRequest
{
    public IEnumerator GetRequest(IWebAPI webAPI, string url, Action<string> callback)
    {
        string jsonData = JsonUtility.ToJson(webAPI);
        //url = System.Uri.EscapeUriString(url);//防止带中文
        byte[] bytes = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.uploadHandler = new UploadHandlerRaw(bytes);//上传参数
            //request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long)System.Net.HttpStatusCode.OK)//如果返回参数不对
            {
                Debug.LogError("response error: " + request.responseCode);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                callback(request.downloadHandler.text);
            }
            request.Dispose();
        }
    }
    public IEnumerator GetRequest(string url, Action<byte[]> callback)
    {
        using(UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long)System.Net.HttpStatusCode.OK)//如果返回参数不对
            {
                Debug.LogError("response error: " + request.responseCode);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                callback(request.downloadHandler.data);
            }
            request.Dispose();
        }
    }

    public IEnumerator PostRequest(IWebAPI webAPI, string url, Action<string> callback)
    {
        string jsonData = JsonUtility.ToJson(webAPI);
        Debug.Log(webAPI.URL);
        Debug.Log(jsonData);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonData);

        using(UnityWebRequest request = new UnityWebRequest(url, "POST"))//UnityWebRequest.Post(url,UnityWebRequest.kHttpVerbPOST))
        {
            request.uploadHandler = new UploadHandlerRaw(bytes);
            //request.uploadHandler.contentType = "application/json";
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if(request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long)System.Net.HttpStatusCode.OK)//如果返回参数不对
            {
                Debug.LogError("response error: " + request.responseCode);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                callback(request.downloadHandler.text);
            }
            request.Dispose();
        }
    }

    public IEnumerator PostRequest(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("network problem: " + request.error);
            }
            else if (request.responseCode != (long)System.Net.HttpStatusCode.OK)//如果返回参数不对
            {
                Debug.LogError("response error: " + request.responseCode);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                callback(request.downloadHandler.text);
            }
            request.Dispose();
        }
    }
}

public interface IWebAPI
{
    public string URL { get;}
}
public interface IWebData
{
    /// <summary>
    /// 返回编码(10000表示成功)
    /// </summary>
    public int Code { get; }
    /// <summary>
    /// 返回描述(SUCCESS表示成功)
    /// </summary>
    public string Msg { get; }
}

#region 获取图形验证码
[Serializable]
public class GetCaptcha : IWebAPI
{
    public string URL =>$"http://island.test.umxverse.com/api/login/captcha?uuid={uuid}";
    public string uuid;
}
[Serializable]
public class CaptchaData:IWebData
{
    [SerializeField]
    private int code;
    [SerializeField]
    private string msg;
    public string singleData;

    public int Code => code;

    public string Msg => msg;
}
#endregion

#region 获取短信验证码
[Serializable]
public class GetPhoneMailData:IWebAPI
{
    public string uuid;
    public string phone;
    public string verifyCode;

    public string URL => "http://island.test.umxverse.com/api/login/sms";
}
public class PhoneMailData : IWebData
{
    [SerializeField]
    private int code;
    [SerializeField]
    private string msg;
    public int Code => code;

    public string Msg => msg;
}
#endregion

#region 手机验证码登录
[Serializable]
public class GetPhoneData:IWebAPI
{
    public string phone;
    public string smsCode;

    public string URL => "http://island.test.umxverse.com/api/login";
}
[Serializable]
public class PhoneData : IWebData
{
    [SerializeField]
    private int code;
    [SerializeField]
    private string msg;

    public CSTokenData singleData;
    public int Code => code;

    public string Msg => msg;
}
[Serializable]
public class CSTokenData
{
    public string token;
    public string uid;
    public string userId;
}

#endregion

#region 获取用户信息
public class GetUserMsg : IWebAPI
{
    public string URL => "http://island.test.umxverse.com/api/purchased/holding";
}
public class WebUserMsg : IWebData
{
    private int code;
    private string msg;
    public UserAsstes assets;
    public UserCommunities communities;
    public UserIslands islands;
    public User user;
    public int Code => code;

    public string Msg => msg;
}
/// <summary>
/// 用户资产
/// </summary>
[Serializable]
public class UserAsstes
{
    /// <summary>
    /// 资产id
    /// </summary>
    public string id;
    /// <summary>
    /// 产品id
    /// </summary>
    public string productId;
    /// <summary>
    /// 产品名称
    /// </summary>
    public string productName;
    /// <summary>
    /// 产品类型
    /// </summary>
    public string productType;
    /// <summary>
    /// 产品图片
    /// </summary>
    public string url;
}
/// <summary>
/// 用户社区
/// </summary>
[Serializable]
public class UserCommunities
{
    /// <summary>
    /// 子社区id
    /// </summary>
    public int id;
    /// <summary>
    /// 占领岛屿数
    /// </summary>
    public int leftIslands;
    /// <summary>
    /// 社区名称
    /// </summary>
    public string name;
    /// <summary>
    /// 总岛屿数
    /// </summary>
    public int totalIslands;
}
/// <summary>
/// 用户岛屿
/// </summary>
[Serializable]
public class UserIslands
{

}
/// <summary>
/// 用户
/// </summary>
[Serializable]
public class User
{
    public string id;
    public string name;
}
#endregion
```

## 音频可视化

```c#
//音乐频率范围约为20hz--20khz
/*
采样方式
Rectangular W[n] = 1.0;
Trangle W[n] = TRI(2n/N);
Hamming W[n] = 0.54 - (0.46 * COS(n/N))
Hanning W[n] = 0.5 * (1.0 - COs(n/N))
Blackman W[n] = 0.42 - (0.5 * COS(n/n)) + (0.08 * COS(2.0 * n/N))
BlackmanHarris W[n] = 
    0.35875 - (0.48829 * COS(1.0 * n/N)) + (0.14128 * COS(2.0 * n/N) - (0.01168 * COS(3.0 * n/N)))
*/
/// <summary>
/// AudioSource的方法
/// <param name="samples">采样率(64-8192)</param>
/// <param name="channel">通道,默认0</param>
/// <param name="window">采样方式</param>
/// </summary>
public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
```

## Spine动画

UI

```c#
protected SkeletonGraphic _graphic;
protected TrackEntryDelegate _act=null;
//播放速度
public void SetTime(float time)
{
    _graphic.AnimationState.TimeScale = time;
}
//按顺序播放动画
public void PlayTrackAnimation(List<string> liAniNames, bool isLoop = false, Action action = null)
{
    if(liAniNames == null || liAniNames <= 0) return;
    Spine.AnimationState state = _graphic.AnimationState;
    state.ClearTracks();
    foreach(var item int liAniNames)
    {
        state.AddAnimation(0, item, isLoop, 0f);
    }
    if(action == null) return;
    _act = delegate
    {
        action();
        state.Complete-=_act;
        _act =null;
    }
    state.Complete += _act;
}
bool _isPlaying = false;
public void LimitPlay(int weight, string spineName, boo isLoop, Action callback = null)
{
    if(_isPlaying) return;
    _isPlaying = true;
    Spine.AnimationState state = _graphic.AnimationState;
    state.ClearTracks();
    state.SetAnimation(weight, spineName, isLoop);
    if(callback ==null)
    {
        _isPlaying = false;
        return;
    }
    _act = delegate{
        callback();
        state.Complete -= _act;  
        _act = null;
    }
    state.Complete += _act;
}
public void Stop(SkeletonGraphic sg, int trackIndex, float mixDuration)
{
    sg.AnimationState.SetEmptyAnimation(trackIndex, mixDuration);
}
public void Play(int weight,)
```


## 鼠标在世界中的位置

```C#
public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) 
{
    Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
    return worldPosition;
}
```

```C#
/// <summary>
/// Get Mouse Position in World with Z = 0f
/// 获得鼠标在世界中的位置 Z = 0f
/// </summary>
/// <returns></returns
public static Vector3 GetMouseWorldPosition() 
{
    Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    vec.z = 0f;
    return vec;
}
```
