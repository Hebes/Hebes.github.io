# Unity编辑器-UnityEditor选择电脑上的文件

**[Unity Editor选择电脑上的文件](<https://blog.csdn.net/w0100746363/article/details/109464362>)**

```C#
public class TestT : EditorWindow
{
    static TestT window;
    static string filePath;
    [MenuItem("Assets/选择文件")]
    static void ExcelToXmlToLevel()
    {
        window = (TestT)GetWindow(typeof(TestT));
        window.titleContent.text = "选择文件";
        window.position = new Rect(PlayerSettings.defaultScreenWidth / 2, PlayerSettings.defaultScreenHeight / 2, 400, 120);
        window.Show();
    }
 
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class OpenFileName
    {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public string filter = null;
        public string customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public string file = null;
        public int maxFile = 0;
        public string fileTitle = null;
        public int maxFileTitle = 0;
        public string initialDir = null;
        public string title = null;
        public int flags = 0;
        public short fileOffset = 0;
        public short fileExtension = 0;
        public string defExt = null;
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public string templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }
 
    public class WindowDll
    {
        [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
        public static bool GetOpenFileName1([In, Out] OpenFileName ofn)
        {
            return GetOpenFileName(ofn);
        }
    }
 
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 400, 140));
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);
        GUILayout.Label("文档路径:", GUILayout.MinWidth(15));
        filePath = EditorGUILayout.TextField(filePath, GUILayout.ExpandWidth(true));
        if (GUILayout.Button("选择"))
        {
            OpenFile();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("确认"))
        {
            if (!string.IsNullOrEmpty(filePath))
            {
            }
            else
            {
                Debug.LogError("请选择文件！");
            }
        }
        GUILayout.Space(20);
        if (GUILayout.Button("取消"))
        {
            window.Close();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
 
    private static void OpenFile()
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "All Files\0*.*\0\0";
        //ofn.filter = "Image Files(*.jpg;*.png)\0*.jpg;*.png\0";
        //ofn.filter = "Txt Files(*.txt)\0*.txt\0";
 
        //ofn.filter = "Word Files(*.docx)\0*.docx\0";
        //ofn.filter = "Word Files(*.doc)\0*.doc\0";
        //ofn.filter = "Word Files(*.doc:*.docx)\0*.doc:*.docx\0";
 
        //ofn.filter = "Excel Files(*.xls)\0*.xls\0";
        //ofn.filter = "Excel Files(*.xlsx;*.xlsm;*.xltx;*.xltm)\0*.xlsx;*.xlsm;*.xltx;*.xltm\0";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = Application.streamingAssetsPath;
        path = path.Replace('/', '\\');
        //默认路径  
        ofn.initialDir = path;
        ofn.title = "Open Project";
 
        ofn.defExt = "";//显示文件的类型  
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
 
        if (WindowDll.GetOpenFileName(ofn))
        {
            filePath = EditorGUILayout.TextField(ofn.file, GUILayout.ExpandWidth(true));
        }
    }
}
```
