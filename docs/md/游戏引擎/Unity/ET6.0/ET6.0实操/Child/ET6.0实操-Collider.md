# ET6.0实操-Collider

在角色身上写个脚本，放个action回调，挂上这个脚本,Mono文件夹下
这个ColliderCallback脚本挂载角色身上

```C#
namespace ET
{
    /// <summary>
    /// Collider的事件 回调的脚本
    /// </summary>
    public class ColliderCallback : MonoBehaviour
    {
        public Action<Collider> OnTriggerEnterAction;
        public Action<Collider> OnTriggerExitAction;

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterAction?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitAction?.Invoke(other);
        }
    }
}
```

ET代码
AfterUnitCreate_CreateUnitView.cs中

```C#
//是个是创建角色的
GameObject bundleGameObject = (GameObject)ResourcesComponent.Instance.GetAsset("Unit.unity3d", "Unit");
GameObject prefab = bundleGameObject.Get<GameObject>("PlayerArmature");
GameObject playerGo = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
//添加
args.Unit.AddComponent<PlayerToSchoolSceneInteractionComponent, GameObject>(playerGo);
```

Unity.ModelView下

```C#
namespace ET
{
    /// <summary>
    /// 玩家与学校的交互
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class PlayerToSchoolSceneInteractionComponent : Entity, IAwake<GameObject>, IUpdate, IDestroy
    {
        /// <summary>
        /// 交互界面
        /// </summary>
        public UI eachOtherPanel;
        /// <summary>
        /// 碰撞体
        /// </summary>
        public Collider other;
        /// <summary>
        /// 是否进入
        /// </summary>
        public bool isJoin;

        /// <summary>
        /// 玩家角色
        /// </summary>
        public GameObject playerGo { get; set; }
    }
}
```

System编写
Unity.HotfixView下

```C#
namespace ET
{

    [ObjectSystem]
    public class PlayerToSchoolSceneInteractionComponentAwakeSystem : AwakeSystem<PlayerToSchoolSceneInteractionComponent, GameObject>
    {
        public override void Awake(PlayerToSchoolSceneInteractionComponent self, GameObject go)
        {
            self.playerGo = go;

            //监听触发
            ColliderCallback colliderCallback = go.GetComponent<ColliderCallback>();
            colliderCallback.OnTriggerEnterAction = self.OnTriggerEnter;
            colliderCallback.OnTriggerExitAction = self.OnTriggerExit;
        }
    }

    [ObjectSystem]
    public class PlayerToSchoolSceneInteractionComponentUpdateSystem : UpdateSystem<PlayerToSchoolSceneInteractionComponent>
    {
        public override void Update(PlayerToSchoolSceneInteractionComponent self)
        {
            //暂未完成      自行编写判断
            //other和isJoin未赋值
            //if (self.isJoin == true && self.other != null)
            //{
            //    if (Input.GetKeyDown(KeyCode.E) && self.other.transform.TryGetComponent<Door>(out Door door))
            //    {
            //        //door.JudgeDoor(self.other.transform);
            //        Debug.Log("join");
            //    }
            //}
        }
    }

    /// <summary>
    /// 玩家与学校的交互
    /// </summary>
    [FriendClass(typeof(PlayerToSchoolSceneInteractionComponent))]
    public static class PlayerToSchoolSceneInteractionComponentSystem
    {
        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="self"></param>
        public static async void OnCreatUI(this PlayerToSchoolSceneInteractionComponent self)
        {
            self.eachOtherPanel = await UIHelper.Create(self.ZoneScene(), UIType.UIMateSchoolScene_EachOtherPanel, UILayer.High);
            self.eachOtherPanel.GameObject.SetActive(false);
        }

        /// <summary>
        /// 打开或关闭
        /// </summary>
        public static void OnOpenAndHideUI(this PlayerToSchoolSceneInteractionComponent self)
        {
            self.eachOtherPanel.GameObject.SetActive(!self.eachOtherPanel.GameObject.activeSelf);
        }

        /// <summary>
        /// 进入碰撞
        /// </summary>
        public static void OnTriggerEnter(this PlayerToSchoolSceneInteractionComponent self, Collider collider)
        {
            Log.Debug("进入了触发碰撞");
            Log.Debug("碰撞的物体是" + collider.gameObject.name);
        }

        /// <summary>
        /// 退出碰撞
        /// </summary>
        /// <param name="self"></param>
        /// <param name="collider"></param>
        public static void OnTriggerExit(this PlayerToSchoolSceneInteractionComponent self, Collider collider)
        {
            Log.Debug("退出了触发碰撞");
        }
    }
}
```
