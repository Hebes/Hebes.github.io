# Unity功能-AStart地图数据生成

**[UnityCollect](<https://github.com/Hebes/UnityCollect>)**

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ACTool
{
    public class MapEditorMgr : MonoBehaviour
    {
        public int mapX;
        public int mapZ;
        public int blockSize;

    }
}
```

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ACTool
{
    public class BlockData : MonoBehaviour
    {
        public int mapX;
        public int mapZ;
        public int isGo; // 是否能行走 = 0, 1 可以行走;
    }
}
```

```c#
using System;
using UnityEditor;
using UnityEngine;

namespace ACTool
{

    public class MapGenEditor : EditorWindow
    {
        private int mapX = 64;
        private int mapZ = 64;
        private int blockSize = 8;
        int blockHeight = 8;
        private string blockPath = "Assets/UnityEditorTool/Child/07.AStart地图数据/AStarMapEdit/Res/Cube.prefab";

        [MenuItem("Assets/暗沉EditorTool/AStart/地图数据生成MapEidtorGen")]
        static void run()
        {
            EditorWindow.GetWindow<MapGenEditor>();
        }

        public void OnGUI()
        {
            GUILayout.Label("地图X方向块数");
            this.mapX = Convert.ToInt32(GUILayout.TextField(this.mapX.ToString()));

            GUILayout.Label("地图z方向块数");
            this.mapZ = Convert.ToInt32(GUILayout.TextField(this.mapZ.ToString()));

            GUILayout.Label("地图块大小");
            this.blockSize = Convert.ToInt32(GUILayout.TextField(this.blockSize.ToString()));
            GUILayout.Label("地图块高度");
            this.blockHeight = Convert.ToInt32(GUILayout.TextField(this.blockHeight.ToString()));

            GUILayout.Label("选择地图原点");
            if (Selection.activeGameObject != null)
            {
                GUILayout.Label(Selection.activeGameObject.name);
            }
            else
            {
                GUILayout.Label("没有选中地图原点!!!!");
            }

            if (GUILayout.Button("在原点下生成地图块"))
            {
                if (Selection.activeGameObject != null)
                {
                    Debug.Log("开始生成...");
                    this.CreateBlocksAt(Selection.activeGameObject);
                    Debug.Log("生成结束");
                }
            }

            if (GUILayout.Button("重置地图块"))
            {
                if (Selection.activeGameObject != null)
                {
                    this.ResetBlocks(Selection.activeGameObject);
                }
            }

            if (GUILayout.Button("清理地图块"))
            {
                if (Selection.activeGameObject != null)
                {
                    this.ClearBlocksAt(Selection.activeGameObject);
                }
            }
        }
        
        /// <summary>
        /// 重置地图
        /// </summary>
        /// <param name="org"></param>
        private void ResetBlocks(GameObject org)
        {
            int count = org.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cube = org.transform.GetChild(i).gameObject;
                cube.GetComponent<BlockData>().isGo = 0;
                cube.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        /// <summary>
        /// 清理地图
        /// </summary>
        /// <param name="org"></param>
        private void ClearBlocksAt(GameObject org)
        {
            int count = org.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cube = org.transform.GetChild(0).gameObject;
                GameObject.DestroyImmediate(cube);
            }
        }
        private void CreateBlocksAt(GameObject org)
        {
            MapEditorMgr mgr = org.GetComponent<MapEditorMgr>();
            if (!mgr)
            {
                mgr = org.AddComponent<MapEditorMgr>();
            }
            mgr.mapX = this.mapX;
            mgr.mapZ = this.mapZ;
            mgr.blockSize = this.blockSize;

            this.ClearBlocksAt(org);

            Vector3 startPos = new Vector3(this.blockSize * 0.5f, 0, this.blockSize * 0.5f);
            GameObject cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(blockPath);
            for (int i = 0; i < this.mapZ; i++)
            {
                Vector3 pos = startPos;
                for (int j = 0; j < this.mapX; j++)
                {
                    GameObject cube = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;
                    cube.name = "block";
                    cube.transform.SetParent(org.transform, false);
                    cube.transform.localPosition = pos;
                    cube.transform.localScale = new Vector3(this.blockSize, this.blockHeight, this.blockSize);
                    BlockData block = cube.AddComponent<BlockData>();
                    block.mapX = j;
                    block.mapZ = i;
                    block.isGo = 0;

                    pos.x += this.blockSize;
                }
                startPos.z += this.blockSize;
            }
        }

        void OnSelectionChange()
        {
            this.Repaint();
        }
    }
}
```

```c#
using System;
using UnityEditor;
using UnityEngine;

namespace ACTool
{

    public class MapGenEditor : EditorWindow
    {
        private int mapX = 64;
        private int mapZ = 64;
        private int blockSize = 8;
        int blockHeight = 8;
        private string blockPath = "Assets/UnityEditorTool/Child/07.AStart地图数据/AStarMapEdit/Res/Cube.prefab";

        [MenuItem("Assets/暗沉EditorTool/AStart/地图数据生成MapEidtorGen")]
        static void run()
        {
            EditorWindow.GetWindow<MapGenEditor>();
        }

        public void OnGUI()
        {
            GUILayout.Label("地图X方向块数");
            this.mapX = Convert.ToInt32(GUILayout.TextField(this.mapX.ToString()));

            GUILayout.Label("地图z方向块数");
            this.mapZ = Convert.ToInt32(GUILayout.TextField(this.mapZ.ToString()));

            GUILayout.Label("地图块大小");
            this.blockSize = Convert.ToInt32(GUILayout.TextField(this.blockSize.ToString()));
            GUILayout.Label("地图块高度");
            this.blockHeight = Convert.ToInt32(GUILayout.TextField(this.blockHeight.ToString()));

            GUILayout.Label("选择地图原点");
            if (Selection.activeGameObject != null)
            {
                GUILayout.Label(Selection.activeGameObject.name);
            }
            else
            {
                GUILayout.Label("没有选中地图原点!!!!");
            }

            if (GUILayout.Button("在原点下生成地图块"))
            {
                if (Selection.activeGameObject != null)
                {
                    Debug.Log("开始生成...");
                    this.CreateBlocksAt(Selection.activeGameObject);
                    Debug.Log("生成结束");
                }
            }

            if (GUILayout.Button("重置地图块"))
            {
                if (Selection.activeGameObject != null)
                {
                    this.ResetBlocks(Selection.activeGameObject);
                }
            }

            if (GUILayout.Button("清理地图块"))
            {
                if (Selection.activeGameObject != null)
                {
                    this.ClearBlocksAt(Selection.activeGameObject);
                }
            }
        }
        
        /// <summary>
        /// 重置地图
        /// </summary>
        /// <param name="org"></param>
        private void ResetBlocks(GameObject org)
        {
            int count = org.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cube = org.transform.GetChild(i).gameObject;
                cube.GetComponent<BlockData>().isGo = 0;
                cube.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        /// <summary>
        /// 清理地图
        /// </summary>
        /// <param name="org"></param>
        private void ClearBlocksAt(GameObject org)
        {
            int count = org.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject cube = org.transform.GetChild(0).gameObject;
                GameObject.DestroyImmediate(cube);
            }
        }
        private void CreateBlocksAt(GameObject org)
        {
            MapEditorMgr mgr = org.GetComponent<MapEditorMgr>();
            if (!mgr)
            {
                mgr = org.AddComponent<MapEditorMgr>();
            }
            mgr.mapX = this.mapX;
            mgr.mapZ = this.mapZ;
            mgr.blockSize = this.blockSize;

            this.ClearBlocksAt(org);

            Vector3 startPos = new Vector3(this.blockSize * 0.5f, 0, this.blockSize * 0.5f);
            GameObject cubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(blockPath);
            for (int i = 0; i < this.mapZ; i++)
            {
                Vector3 pos = startPos;
                for (int j = 0; j < this.mapX; j++)
                {
                    GameObject cube = PrefabUtility.InstantiatePrefab(cubePrefab) as GameObject;
                    cube.name = "block";
                    cube.transform.SetParent(org.transform, false);
                    cube.transform.localPosition = pos;
                    cube.transform.localScale = new Vector3(this.blockSize, this.blockHeight, this.blockSize);
                    BlockData block = cube.AddComponent<BlockData>();
                    block.mapX = j;
                    block.mapZ = i;
                    block.isGo = 0;

                    pos.x += this.blockSize;
                }
                startPos.z += this.blockSize;
            }
        }

        void OnSelectionChange()
        {
            this.Repaint();
        }
    }
}
```
