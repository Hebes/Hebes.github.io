# Unity框架-UI获取

## 方案一

<a href="md\Unity前端\Resources\Unity框架-UI获取\ACUIManagerEditor.zip" target="_blank">ACUIManagerEditor.zip</a>

![1](\../../Image/Unity框架-获取UI代码/1.png)

ACManagerEditor.cs

请复制后折叠起来看,会比较清楚!

```C#
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

/// <summary>
/// 修改ACManager样式
/// </summary>
[CustomEditor(typeof(ACUIManager), true)]//自定义ReferenceCollector类在界面中的显示与功能
public class ACManagerEditor : Editor
{
    private string ACUIData_key = "key";
    private string ACUIData_gameObject = "gameObject";
    private string ACUIManager_data = "data";

    private string Prefix = "T_";
    private string _searchKey = "";
    private Object heroPrefab;
    //输入在textfield中的字符串
    private string searchKey
    {
        get
        {
            return _searchKey;
        }
        set
        {
            if (_searchKey != value)
            {
                _searchKey = value;
                heroPrefab = aCManager.Get<Object>(searchKey);
            }
        }
    }
    private ACUIManager aCManager { get; set; }

    private void OnEnable()
    {
        Debug.Log("面板启动了");
        aCManager = (ACUIManager)target;
    }
    private void OnDestroy()
    {
        Debug.Log("面板关闭了");
    }
    //https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedProperty.html
    //https://docs.unity.cn/cn/2021.1/ScriptReference/EditorGUILayout.html
    //https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedObject.html
    public override void OnInspectorGUI()
    {
        SerializedProperty dataProperty = OnUIGetSerializedProperty();
        EditorGUILayout.LabelField("前缀按钮", EditorStyles.boldLabel);
        OnUIPrefixButton(dataProperty);
        EditorGUILayout.LabelField("获取物体", EditorStyles.boldLabel);
        OnUIGetGameObject(dataProperty);
        EditorGUILayout.LabelField("获取脚本", EditorStyles.boldLabel);
        OnUIGetScript(dataProperty);
        EditorGUILayout.LabelField("内容", EditorStyles.boldLabel);
        OnUIContent1(dataProperty);
        EditorGUILayout.Space(10);
        OnUIContent2(dataProperty);
        EditorGUILayout.Space(10);
        OnUIDrog(dataProperty);
        OnUIUpdata();
    }

    private SerializedProperty OnUIGetSerializedProperty()
    {
        //使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
        Undo.RecordObject(aCManager, "Changed Settings");
        var dataProperty = serializedObject.FindProperty(ACUIManager_data);//按名称查找序列化属性。
        return dataProperty;
    }//获取属性
    private void OnUIPrefixButton(SerializedProperty dataProperty)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button($"添加{Prefix}"))
            OnAddPrefix(dataProperty, Prefix);
        if (GUILayout.Button($"移除{Prefix}"))
            OnRemovePrefix(dataProperty, Prefix);
        if (GUILayout.Button("去除空格"))
            OnDelTrim(dataProperty);
        EditorGUILayout.EndHorizontal();
    }//前缀按钮
    private void OnUIGetGameObject(SerializedProperty dataProperty)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加物体"))
            AddReference(dataProperty, Guid.NewGuid().GetHashCode().ToString(), null);
        if (GUILayout.Button("删除全部"))
            aCManager.Clear();
        if (GUILayout.Button($"获取{Prefix}物体"))
            OoGetKeyGos(dataProperty);
        if (GUILayout.Button("删除空引用"))
            OnDelNullReference();
        if (GUILayout.Button("排序"))
            aCManager.Sort();
        EditorGUILayout.EndHorizontal();
    }//获取物体
    private void OnUIContent1(SerializedProperty dataProperty)
    {
        EditorGUILayout.BeginHorizontal();
        //可以在编辑器中对searchKey进行赋值，只要输入对应的Key值，就可以点后面的删除按钮删除相对应的元素
        searchKey = EditorGUILayout.TextField(searchKey);
        //添加的可以用于选中Object的框，这里的object也是(UnityEngine.Object
        //第三个参数为是否只能引用scene中的Object
        EditorGUILayout.ObjectField(heroPrefab, typeof(Object), false);
        if (GUILayout.Button("删除"))
        {
            aCManager.Remove(searchKey);
            heroPrefab = null;
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
    }//内容
    private void OnUIContent2(SerializedProperty dataProperty)//内容二
    {
        if (dataProperty.arraySize == 0) return;
        SerializedProperty property;
        for (int i = aCManager.data.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_key);
            property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(200));
            property = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);
            property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);
            if (GUILayout.Button("删除"))
                dataProperty.DeleteArrayElementAtIndex(i);
            if (GUILayout.Button("复制名称"))
                Copy(aCManager.data[i].key);
            EditorGUILayout.EndHorizontal();
        }
    }
    private void OnUIDrog(SerializedProperty dataProperty)
    {
        var eventType = Event.current.type;
        //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
        if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
        {
            // Show a copy icon on the drag
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (eventType == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();//接受拖动操作。
                foreach (var o in DragAndDrop.objectReferences)
                    AddReference(dataProperty, o.name, o);
            }
            Event.current.Use();
        }
    }//拖拽
    private void OnUIUpdata()
    {
        serializedObject.ApplyModifiedProperties();//应用属性修改。
        serializedObject.UpdateIfRequiredOrScript();//更新序列化对象的表示形式
    }//更新
    public string ETUITool_ClassName { get; set; }
    public int toolbarid { get; set; }
    public List<string> components = new List<string>() { "Button", "Text", "Image" };
    private void OnUIGetScript(SerializedProperty dataProperty)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("获取脚本必要代码"))
            OnGetEssentialCode();
        if (GUILayout.Button("获取脚本"))
            OnGetCode();
        EditorGUILayout.EndHorizontal();
        //***********************************************************************************
        toolbarid = GUILayout.SelectionGrid(toolbarid, components.ToArray(), 4);
        EditorGUILayout.BeginHorizontal();
        ETUITool_ClassName = EditorGUILayout.TextField("组件类型", components[toolbarid]);
        if (GUILayout.Button("变量获取"))
            OnGetValue(dataProperty, ETUITool_ClassName);
        if (GUILayout.Button("组件获取"))
            OnGetButtonCode(dataProperty, ETUITool_ClassName);
        EditorGUILayout.EndHorizontal();
    }//获取脚本按钮组


    private void AddReference(SerializedProperty dataProperty, string key, Object obj)
    {
        int index = dataProperty.arraySize;
        dataProperty.InsertArrayElementAtIndex(index);
        var element = dataProperty.GetArrayElementAtIndex(index);
        element.FindPropertyRelative(ACUIData_key).stringValue = key;
        element.FindPropertyRelative(ACUIData_gameObject).objectReferenceValue = obj;
    }//添加元素
    private void ReplaceReferenceName(SerializedProperty dataProperty, string key)
    {
        Debug.Log("走的序列化的修改并设置");
        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            SerializedProperty serializedProperty = dataProperty.GetArrayElementAtIndex(i);
            var keyProperty = serializedProperty.FindPropertyRelative(ACUIData_key);//物体名称
            var gameObjectProperty = serializedProperty.FindPropertyRelative(ACUIData_gameObject);//查找物体
            if (gameObjectProperty.objectReferenceValue != null)
            {
                switch (key)
                {
                    case "Add"://设置名称
                        gameObjectProperty.objectReferenceValue.name = keyProperty.stringValue =
                            $"{Prefix}{gameObjectProperty.objectReferenceValue.name}";
                        break;
                    case "Remove"://设置名称
                        gameObjectProperty.objectReferenceValue.name = keyProperty.stringValue =
                            gameObjectProperty.objectReferenceValue.name.Replace(Prefix, "");
                        break;
                    case "Trim"://设置名称
                        gameObjectProperty.objectReferenceValue.name = keyProperty.stringValue =
                            gameObjectProperty.objectReferenceValue.name.Trim().Replace(" ", "");
                        break;
                }
            }
        }
        //https://docs.unity.cn/cn/2019.4/ScriptReference/EditorUtility.SetDirty.html
        EditorUtility.SetDirty(aCManager);
        OnUIUpdata();
    }//修改引用名称
    public void ACLoopGetKeywordGO(Transform transform, string keyValue, ref List<GameObject> goList)
    {
        if (transform.name.StartsWith(keyValue))
            goList.Add(transform.gameObject);
        for (int i = 0; i < transform?.childCount; i++)
            ACLoopGetKeywordGO(transform.GetChild(i), keyValue, ref goList);
    }//查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)


    private void OnDelTrim(SerializedProperty dataProperty)
    {
        if (dataProperty.arraySize == 0)
            Array.ForEach(Selection.gameObjects, (go) => { go.name = go?.name.Trim().Replace(" ", ""); });
        else
            ReplaceReferenceName(dataProperty, "Trim");
    }//去除空白
    private void OnAddPrefix(SerializedProperty dataProperty, string Prefix)
    {
        if (dataProperty.arraySize == 0)
            Array.ForEach(Selection.gameObjects, (go) => { go.name = $"{Prefix}{go.name}"; });
        else
            ReplaceReferenceName(dataProperty, "Add");
    }//添加前缀
    private void OnRemovePrefix(SerializedProperty dataProperty, string Prefix)
    {
        if (dataProperty.arraySize == 0)
            Array.ForEach(Selection.gameObjects, (go) => { go.name = go.name.Replace(Prefix, ""); });
        else
            ReplaceReferenceName(dataProperty, "Remove");
    }//移除前缀
    private Type ACReflectClass(string className, string namespaceName = "UnityEngine.UI")
    {
        Assembly assem = Assembly.Load(namespaceName);
        Type type = assem.GetType($"{namespaceName}.{className}");
        return type;
    }//反射命名空间
    private void Copy(string content)
    {
        TextEditor te = new TextEditor();
        te.text = content.ToString();
        te.SelectAll();
        te.Copy();
    }//复制

    private void OnDelNullReference()
    {
        var dataProperty = serializedObject.FindProperty(ACUIManager_data);
        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
            if (gameObjectProperty.objectReferenceValue == null)
            {
                dataProperty.DeleteArrayElementAtIndex(i);
                EditorUtility.SetDirty(aCManager);
                OnUIUpdata();
            }
        }
    }//删除空引用
    private void OoGetKeyGos(SerializedProperty dataProperty)
    {
        if (dataProperty.arraySize >= 0)
        {
            Debug.Log("有数据先清理");
            //根据PropertyPath读取prefab文件中的数据
            //如果不知道具体的格式，可以直接右键用文本编辑器打开，搜索data就能找到
            dataProperty.ClearArray();
            UnityEditor.EditorUtility.SetDirty(this);
            OnUIUpdata();
        }

        //获取物体
        List<GameObject> gameObjects = new List<GameObject>();
        ACLoopGetKeywordGO(aCManager.gameObject.transform, Prefix, ref gameObjects);
        for (int i = 0; i < gameObjects?.Count; i++)
            AddReference(dataProperty, gameObjects[i].name, gameObjects[i]);
    }//获取关键字开头的物体


    private void OnGetEssentialCode()
    {
        StringBuilder sb = new StringBuilder();
        //自己填写需要的代码
        sb.AppendLine($"aCUIManager = GetComponent<ACUIManager>();");
        Debug.Log(sb.ToString());
        Copy(sb.ToString());

    }//获取脚本必要代码 请自行填写
    private void OnGetCode()
    {
        Debug.Log("暂时没写");
    }//获取脚本 请自行填写
    private void OnGetValue(SerializedProperty dataProperty, string eTUITool_ClassName)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
            Type type = ACReflectClass(eTUITool_ClassName);
            Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
            if (component != null)
                sb.AppendLine($"public {eTUITool_ClassName} {gameObjectProperty.objectReferenceValue.name}{eTUITool_ClassName};");
        }

        Debug.Log(sb.ToString());
        Copy(sb.ToString());
    }//获取变量 请自行填写
    private void OnGetButtonCode(SerializedProperty dataProperty, string eTUITool_ClassName)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = dataProperty.arraySize - 1; i >= 0; i--)
        {
            var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative(ACUIData_gameObject);//查找物体
            Type type = ACReflectClass(eTUITool_ClassName);
            Component component = (gameObjectProperty.objectReferenceValue as GameObject).GetComponent(type);
            if (component != null)
                sb.AppendLine($"{gameObjectProperty.objectReferenceValue.name}{eTUITool_ClassName} = aCUIManager.GetComponent<{eTUITool_ClassName}>(\"{gameObjectProperty.objectReferenceValue.name}\");");
        }

        Debug.Log(sb.ToString());
        Copy(sb.ToString());
    }//获取Button脚本
}
```

ACUIManager.cs

```C#
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

//使其能在Inspector面板显示，并且可以被赋予相应值
[Serializable]
public class ACUIData
{
    public string key;
    public Object gameObject;
    //public Component DataComponent;
}

//继承IComparer对比器，Ordinal会使用序号排序规则比较字符串，因为是byte级别的比较，所以准确性和性能都不错
public class ACUIDataComparer : IComparer<ACUIData>
{
    public int Compare(ACUIData x, ACUIData y)
    {
        return string.Compare(x.key, y.key, StringComparison.Ordinal);
    }
}

public class ACUIManager : MonoBehaviour, ISerializationCallbackReceiver
{
    public List<ACUIData> data = new List<ACUIData>();
    private readonly Dictionary<string, Object> dict = new Dictionary<string, Object>();//Object并非C#基础中的Object，而是 UnityEngine.Object

#if UNITY_EDITOR
    public void Add(string key, Object obj)
    {
        UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        //根据PropertyPath读取数据
        //如果不知道具体的格式，可以右键用文本编辑器打开一个prefab文件（如Bundles/UI目录中的几个）
        //因为这几个prefab挂载了ReferenceCollector，所以搜索data就能找到存储的数据
        UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        //遍历data，看添加的数据是否存在相同key
        for (i = 0; i < data.Count; i++)
        {
            if (data[i].key == key)
            {
                break;
            }
        }
        //不等于data.Count意为已经存在于data List中，直接赋值即可
        if (i != data.Count)
        {
            //根据i的值获取dataProperty，也就是data中的对应ReferenceCollectorData，不过在这里，是对Property进行的读取，有点类似json或者xml的节点
            UnityEditor.SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            //对对应节点进行赋值，值为gameobject相对应的fileID
            //fileID独一无二，单对单关系，其他挂载在这个gameobject上的script或组件会保存相对应的fileID
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        else
        {
            //等于则说明key在data中无对应元素，所以得向其插入新的元素
            dataProperty.InsertArrayElementAtIndex(i);
            UnityEditor.SerializedProperty element = dataProperty.GetArrayElementAtIndex(i);
            element.FindPropertyRelative("key").stringValue = key;
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }
        //应用与更新
        UnityEditor.EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }//添加新的元素
    public void Remove(string key)
    {
        UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("data");
        int i;
        for (i = 0; i < data.Count; i++)
        {
            if (data[i].key == key)
            {
                break;
            }
        }
        if (i != data.Count)
        {
            dataProperty.DeleteArrayElementAtIndex(i);
        }
        UnityEditor.EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }//删除元素，知识点与上面的添加相似
    public void Clear()
    {
        UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        //根据PropertyPath读取prefab文件中的数据
        //如果不知道具体的格式，可以直接右键用文本编辑器打开，搜索data就能找到
        var dataProperty = serializedObject.FindProperty("data");
        dataProperty.ClearArray();
        UnityEditor.EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }
    public void Sort()
    {
        UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject(this);
        data.Sort(new ACUIDataComparer());
        UnityEditor.EditorUtility.SetDirty(this);
        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();
    }
#endif
    public T Get<T>(string key) where T : class
    {
        Object dictGo;
        if (!dict.TryGetValue(key, out dictGo)) return null;
        return dictGo as T;
    }
    public T GetComponent<T>(string key) where T : Component
    {
        Object dictGo;
        if (!dict.TryGetValue(key, out dictGo)) return null;
        return (dictGo as GameObject).GetComponent<T>();
    }
    public Object GetObject(string key)
    {
        Object dictGo;
        if (!dict.TryGetValue(key, out dictGo)) return null;
        return dictGo;
    }

    public void OnBeforeSerialize()
    {

    }
    public void OnAfterDeserialize()
    {
        Debug.Log("执行了反序列化");
        dict.Clear();
        foreach (ACUIData referenceCollectorData in data)
        {
            if (!dict.ContainsKey(referenceCollectorData.key))
            {
                dict.Add(referenceCollectorData.key, referenceCollectorData.gameObject);
            }
        }
    }//在反序列化后运行
}
```

## 方案二

[Unity：UI工具-新UI代入时避免频繁的修改程序代码](<https://blog.csdn.net/qq_35207836/article/details/100182268>)
