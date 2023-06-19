# Unity编辑器-Unity编辑器面板

<https://blog.csdn.net/l100142548/article/details/79849713>

## Unity3D Attribute特性（属性特性）

### RangeAttribute:[Range(最小值,最大值)]

```javascript {.line-numbers}
//属性面板中，限制最大生命值取值范围（0,100）
[Range(0,100)]
public int maxHealth=100;
```

### TooltipAttribute:[Tooltip(“这里显示描述的信息”)]

```javascript {.line-numbers}
//在属性面板中，鼠标放在属性上，显示当前属性的描述就是了参数里的话
[Tooltip("这里显示描述的信息")]
public new  Camera camera;
```

### AddComponentMenu:[AddComponentMenu(层级/结构)]

```javascript {.line-numbers}
//追加到组件栏目录
[AddComponentMenu("TestMenu/TestComponet")]
public class BoardBase : UIBase {
}
```

### RequireComponent:[RequireComponent(typeof(需要的对象))]

```javascript {.line-numbers}
//当前组件作用对象必须含有Transform组件，没有自动追加
[RequireComponent(typeof(Transform))]
public class BoardBase : UIBase {
```

### ContextMenu:[ContextMenu(“自定义的操作名”)]

```javascript {.line-numbers}
//可以在属性面板的ContextMenu（帮助右边的小齿轮那里）中增加选项。
[ContextMenu("Clear Data")]
public void ClearData(){
}
```

### ContextMenuItemAttribute:[ContextMenuItemAttribute(“操作名”, “方法名”)]

```javascript {.line-numbers}
//可以在属性面板当前对变量追加一个右键菜单，并执行指定的函数。
[ContextMenuItemAttribute("Reset", "ResetPath")]
[ContextMenuItemAttribute("Load", "LoadPath")]
public string webPath = "http://dp.katagames.cn";
private void ResetPath(){
    webPath = "http://dp.katagames.cn";
}
private void LoadPath(){
    Application.OpenURL(webPath);
}
```

### DisallowMultipleComponent：[DisallowMultipleComponent]

```javascript {.line-numbers}
//在一个Gameobject最多只能添加一个该Class的实例。
[DisallowMultipleComponent]
public class BoardBase : UIBase {
}
```

### ExecuteInEditMode:[ExecuteInEditMode]

```javascript {.line-numbers}
//这个属性让Class在Editor模式（非Play模式）下也能执行（Start，Update，OnGUI等方法）。Update方法只在Scene编辑器中有物体产生变化时，才会被调用；OnGUI方法只在GameView接收到事件时，才会被调用。
[ExecuteInEditMode]
public class BoardBase : UIBase {
}
```

### HeaderAttribute:[Header(“Header显示的内容”)]

```javascript {.line-numbers}
//这个属性可以在Inspector中变量的上面增加Header。
[Header("当前生命值")]
public int currentHP = 0;
```

### ImageEffectOpaque:[ImageEffectOpaque]

```javascript {.line-numbers}
//在OnRenderImage上使用，可以让渲染顺序在非透明物体之后，透明物体之前。
[ImageEffectOpaque]
void OnRenderImage(RenderTexture source, RenderTexture destination){
}
```

### MultilineAttribute:[Multiline];[Multiline(行数)]

```javascript {.line-numbers}
//在属性面板中，用于在多行文本区域中显示字符串值的属性
[MultilineAttribute]
public string peopleInfo;
[MultilineAttribute(5)]
public string peopleDes;
```

### RuntimeInitializeOnLoadMethodAttribute:[RuntimeInitializeOnLoadMethod];[RuntimeInitializeOnLoadMethod(枚举参数场景加载前/加载后)]

```javascript {.line-numbers}
    /// 
    /// 加载场景时调用
    /// 
    [RuntimeInitializeOnLoadMethod]
     public void Main()
     {
         print("启动了-Main");
     }
     /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
     public void Main()
     {
         print("启动前");
     }*/
    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public void Main()
    {
        print("启动后");
    }*/
```

### SelectionBaseAttribute:[SelectionBase]

```javascript {.line-numbers}
///当一个GameObject1含有使用了该属性的Component的时候，在SceneView中选择该GameObject1整体时或者子物体时，会优先选择GameObject1。
[SelectionBase]
public class Abb : MonoBehaviour {   
}
```

### SerializeField:[SerializeField]

```javascript {.line-numbers}
//在变量上使用该属性，可以强制该变量进行序列化。即可以在属性上对变量的值进行编辑。
[SerializeField]
private int attack;
```

### SpaceAttribute:[Space];[Space(像素)]

```javascript {.line-numbers}
    //使用该属性可以在属性面板上增加一些空位。 
    [Space]
    public int attack;
    [Space(15)]
    public int currentHealth;
```

### TextAreaAttribute:[TextArea];[TextArea]

```javascript {.line-numbers}
    //该属性可以把string在Inspector上的编辑区变成一个TextArea。 
    [TextArea]
    public string goodsInfo;
    [TextArea(3,15)]
    public string peopleInfo;
```

### SharedBetweenAnimatorsAttribute:[SharedBetweenAnimators]

```javascript {.line-numbers}
//用于StateMachineBehaviour上，不同的Animator将共享这一个StateMachineBehaviour的实例，可以减少内存占用。
[SharedBetweenAnimators]
public class AttackBehaviour : StateMachineBehaviour
{
    public new void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("OnStateEnter");
    }
}
```

### UnityAPICompatibilityVersionAttribute:[UnityAPICompatibilityVersionAttribute(版本)]

```javascript {.line-numbers}
//用来声明API的版本兼容性
```

### FormerlySerializedAsAttribute:[FormerlySerializedAs(“老的属性名”)]

```javascript {.line-numbers}
    //防止我们在属性视图中设计好的数值因为更改属性名而被修改
    //比如我之前m_MyValue变量叫做m_MyValueB，在属性中设置数值了，现在变量名叫m_MyValue
    //可以使用FormerlySerializedAs来防止数值变化
    [SerializeField]
    [FormerlySerializedAs("m_MyValueB")]
    private string m_MyValue;
```

### HideInInspector:[HideInInspector]

```javascript {.line-numbers}
    //在属性面板隐藏该属性
    [HideInInspector]
    public int Health = 100;
```

### MonoPInvokeCallbackAttribute:[MonoPInvokeCallback(接收的代理类型)]DllImportAttribute:[DllImport(“DLL名称”)]

参考网站:<http://www.cnblogs.com/kingBook/p/6723620.html>

```javascript {.line-numbers}
    internal delegate void TestCallBack(string eventName);
    //C++/c（非托管代码） 的方法可从c#调用。
    [DllImport("__Internal")]
    private static extern void _TestInit(TestCallBack callback);
    //C#（托管代码）中注册方法可以从C++/c（非托管代码）调用。
    [MonoPInvokeCallback(typeof(TestCallBack))]
    public static void onCallBack(string eventName)
    {
        if (eventName == "complete")
        {
            //do something            
        }
    }
```

### SerializableAttribute:[System.Serializable] NonSerializedAttribute:[System.NonSerialized]

```javascript {.line-numbers}
//序列化类,在属性面板显示对象成员变量
[System.Serializable]
public class Role
{
    public string name;
    public int maxHealth;
    //禁止序列化，如果类进行序列化是，属性不序列化，在属性面板不显示对象成员变量
    [System.NonSerialized]
    public int health = 100;
}
```

### CustomEditor:[CustomEditor(目标类型)];[CustomEditor(目标类型,子类是否可以继承使用这个自定义界面)]

```javascript {.line-numbers}
using UnityEditor;
using UnityEngine;
//为某个组件制作个性化属性视图
[CustomEditor(typeof(MyComponent))]
public class MyComponentEditor : Editor
{
    MyComponent script;//所对应的脚本对象
    GameObject rootObject;//脚本的GameObject
    SerializedObject seriObject;//所对应的序列化对象
    //初始化
    public void OnEnable()
    {
        seriObject = base.serializedObject;
        var tscript = (MyComponent)(base.serializedObject.targetObject);
        if (tscript != null)
        {
            script = tscript;
            rootObject = script.gameObject;
        }
    }
    //清理
    public void OnDisable()
    {
        seriObject = null;
        script = null;
        rootObject = null;
    }
    //界面绘制
    public override void OnInspectorGUI()
    {
    }
}
```

### CallbackOrderAttribute

```javascript {.line-numbers}
public class Tip: UIBase{
    [UICall()]
    public void UIListHandle()
    {

    }
}
/// 
/// CallbackOrderAttribute 需要回调索引的属性的基类。
/// 
public class UICall : CallbackOrderAttribute
{

}
```

### CanEditMultipleObjects:[CanEditMultipleObjects()]

```javascript {.line-numbers}
//自定义编辑器支持多对象编辑。
[CanEditMultipleObjects()]
//为某个组件制作个性化属性视图
[CustomEditor(typeof(MyComponent))]
public class MyComponentEditor : Editor
{
}
```

### CustomPreviewAttribute:[CustomPreview(对象类型)]

```javascript {.line-numbers}
using UnityEditor;
using UnityEngine;
//将一个class标记为指定类型的自定义预览
//预览窗口在属性窗口下 窗口名称：Preview （预览）
//默认一般是隐藏，头部是对象名称
[CustomPreview(typeof(MyComponent))]
public class MyPreview : ObjectPreview
{
    public override bool HasPreviewGUI()
    {
        return true;
    }
    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        GUI.skin.label.normal.textColor = Color.white;
        GUI.Label(r, target.GetType() + " is being previewed");
    }
}

```

### DrawGizmo: [DrawGizmo(Gizmo类型)];[DrawGizmo(Gizmo类型,绘制的类型)]

```javascript {.line-numbers}
    //可以在Scene视图中显示自定义的Gizmo
    [DrawGizmo(GizmoType.Selected | GizmoType.Active,typeof(MyComponent))]
    static void DrawGizmoForMyScript(MyComponent scr, GizmoType gizmoType)
    {
        Vector3 position = scr.transform.position;
        //距离大于10显示logo
        if (Vector3.Distance(position, Camera.current.transform.position) > 10f)
            Gizmos.DrawIcon(position, "logo.png");
    }
```

### InitializeOnLoadAttribute:[InitializeOnLoad] InitializeOnLoadMethodAttribute: [InitializeOnLoadMethod()]

```javascript {.line-numbers}
using UnityEditor;
//允许一个编辑器类在Unity加载时初始化，而不需要用户操作。
//运行时也会被调用一次
[InitializeOnLoad]
public class EditorMain {
    static EditorMain()
    {
        UnityEngine.Debug.Log("Main");
    }
}
public class EditorMainTwo
{
    //允许一个编辑器类方法在Unity加载时初始化，而不需要用户操作。
    //运行时也会被调用一次
    [InitializeOnLoadMethod()]
    static void EditorMain()
    {
        UnityEngine.Debug.Log("MainTwo");
    }
}

```

### PreferenceItem:[PreferenceItem(“标签名”)]

```javascript {.line-numbers}
public class MyPreferences
{
    //定制Unity的Preference(偏好)界面。Edit->Preferences...
    [PreferenceItem("My Preferences")]
    public static void PreferencesGUI()
    {
        GUILayout.Label("更多信息:dp.katagames.cn", EditorStyles.boldLabel);
        if (GUILayout.Button("前往卡塔独立游戏"))
        {
            Application.OpenURL("http://dp.katagames.cn");
        }
    }
}
```

### OnOpenAssetAttribute:[OnOpenAsset()];[OnOpenAsset(执行顺序)]

```javascript {.line-numbers}
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
public class MyAssetHandler
{
    //在打开一个Asset后被调用。
    [OnOpenAsset(1)]
    public static bool step1(int instanceID, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instanceID).name;
        Debug.Log("Open Asset step: 1 (" + name + ")");
        return false; // 我们没有自己的处理手段，如果是true需要我们自己去做处理
    }

    // step2 因为顺序是 2, 所以在 step1 之后执行
    [OnOpenAsset(2)]
    public static bool step2(int instanceID, int line)
    {
        Debug.Log("Open Asset step: 2 (" + instanceID + ")");
        return false; // 我们没有自己的处理手段，如果是true需要我们自己去做处理
    }
}
```

### PostProcessBuildAttribute:[PostProcessBuild()];[PostProcessBuild(调用顺序)]

```javascript {.line-numbers}
    //该属性是在build完成后，被调用的callback。
    //同时具有多个的时候，可以指定先后顺序。
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(pathToBuiltProject);
    }
```

### PostProcessSceneAttribute:[PostProcessScene()];[PostProcessScene(顺序)];[PostProcessScene(顺序,版本)]

```javascript {.line-numbers}
    //此属性添加到方法中，以便在构建场景之后获得通知
    //进入playmode，当application.loadlevel或application.loadleveladditive也会执行
    [PostProcessScene(1)]
    public static void OnPostProcessScene(BuildTarget target, string pathToBuiltProject)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
    }
```

### CustomPropertyDrawer:[CustomPropertyDrawer(类型)];[CustomPropertyDrawer(类型,应用于子类)]

```javascript {.line-numbers}
using UnityEngine;
using UnityEditor;
public enum IngredientUnit { Spoon, Cup, Bowl, Piece }
// 自定义序列化类
[System.Serializable]
public class Ingredient
{
    public string name;
    public int amount = 1;
    public IngredientUnit unit;
}
public class ExampleClass : MonoBehaviour
{
    public Ingredient potionResult;
    public Ingredient[] potionIngredients;
}
//自定义序列化绘制
[CustomPropertyDrawer(typeof(Ingredient))]
public class IngredientDrawer : PropertyDrawer
{
    // 绘制属性区域
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        // 绘制Label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        // 不让子字段缩进
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        // 计算矩形
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
        // 绘制数值域
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
        // 缩进设置原来位置
        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
```

### CustomPropertyDrawer: [CustomPropertyDrawer(属性特性类型)]; [CustomPropertyDrawer(属性特性类型,是否用在子类继承)]

```javascript {.line-numbers}
using UnityEditor;
using UnityEngine;
public class RangeTest : MonoBehaviour {
    [MyRange(1,10)]
    public int num=5;    
}
// 这不是编辑器脚本不要放在Editor中。属性特性类应放置在一个常规脚本文件中。
public class MyRange : PropertyAttribute
{
    public float min;
    public float max;
    public MyRange(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}
//属性特性类建议当作一个编辑器脚本，放在位于一个名为Editor的文件夹中。
//告诉 RangeDrawer  他将为MyRange 做绘制
[CustomPropertyDrawer(typeof(MyRange))]
public class RangeDrawer : PropertyDrawer
{
    // 绘制区块
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 第一获取属性特性，因为包含范围信息
        MyRange range = attribute as MyRange;
        // 现在绘制滑块有两种类型一种Float 一种 Int
        if (property.propertyType == SerializedPropertyType.Float)
            EditorGUI.Slider(position, property, range.min, range.max, label);
        else if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);
        else
            EditorGUI.LabelField(position, label.text, "请使用类型 Int 或者 Float");
    }
}
```

### ColorUsageAttribute:[ColorUsage(是否显示Alpha)];[ColorUsage(是否显示Alpha, 是否是hdr,最小亮度,最大亮度,最小曝光度,最大曝光度)]

```javascript {.line-numbers}
    public Color color;
    //配置颜色GUI
    [ColorUsage(false)]
    public Color color2;
    [ColorUsage(true, true, 0, 8, 0.125f, 3)]
    public Color color3;
```
