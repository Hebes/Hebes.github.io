# Unity功能-射线检测

```c#
Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
RaycastHit hit;
if(Physics.Raycast(ray, out hit))
{
    Debug.Log("当前鼠标碰到的物体名字是"+ hit.collider.name);
}
```

## 射线可视化

```C#
 Debug.DrawLine(RayStartPoint.position, hit.point, Color.red,100);
```

## 射线进入移出等

CSRayCasterInput.cs

```C#
using UnityEngine;
using UnityEngine.Events;
namespace AnRay
{
    public class CSRayCasterInput
    {
        /// <summary>
        /// 射线进入时执行
        /// </summary>
        public UnityEvent<Collider> OnRayEnter;
        /// <summary>
        /// 射线停留时执行
        /// </summary>
        public UnityEvent<Collider> OnRayStay;
        /// <summary>
        /// 射线离开时执行
        /// </summary>
        public UnityEvent<Collider> OnRayExit;
        /// <summary>
        /// 射线启用时执行
        /// </summary>
        public UnityEvent<Collider> OnRayEnable;
        /// <summary>
        /// 射线禁用时执行
        /// </summary>
        public UnityEvent<Collider> OnRayDisable;
        /// <summary>
        /// ?打开射线
        /// </summary>
        public bool IsOpen { get => _isOpen; }
        /// <summary>
        /// 用来保存上一个射线扫到的物体
        /// </summary>
        private Collider _previousCollider;
        /// <summary>
        /// 射线打开
        /// </summary>
        private bool _isOpen = true;
        public CSRayCasterInput()
        {
            OnRayEnter = new UnityEvent<Collider>();
            OnRayStay = new UnityEvent<Collider>();
            OnRayExit = new UnityEvent<Collider>();
            OnRayEnable = new UnityEvent<Collider>();
            OnRayDisable = new UnityEvent<Collider>();
        }
        /// <summary>
        /// 打开/关闭射线
        /// </summary>
        /// <param name="isOn">打开/关闭</param>
        public void TurnRay(bool isOn)
        {
            if (_isOpen == isOn) return;
            _isOpen = isOn;
            //if (previousCollider == null) return;
            switch (isOn)
            {
                default:
                    OnRayEnable?.Invoke(_previousCollider);
                    break;
                case false:
                    //OnRayExit?.Invoke(previousCollider);
                    OnRayDisable?.Invoke(_previousCollider);
                    break;
            }
        }
        /// <summary>
        /// 根据鼠标位置发射射线
        /// </summary>
        /// <param name="viewCam">视角摄像机</param>
        public void CastMouseRay(Camera viewCam)
        {
            if (!_isOpen) return;
            // 从摄像机向鼠标位置发射一束射线，并返回相关信息
            Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            // 处理返回的信息
            CollisionProcess(hitInfo.collider);
        }
        /// <summary>
        /// 根据鼠标位置发射射线
        /// </summary>
        /// <param name="viewCam">视角摄像机</param>
        /// <param name="mask">能被接收的层级</param>
        public void CastMouseRay(Camera viewCam, float range, LayerMask mask)
        {
            if (!_isOpen) return;
            Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, range, mask);
            CollisionProcess(hitInfo.collider);
        }
        private void CollisionProcess(Collider current)
        {
            if (!_isOpen) return;
            // 执行射线离开物体触发的事件
            // 如果在当前帧中，射线没有碰撞到任何物体，并且在记录中保存到碰撞体不为空，就触发该物体的结束射线碰撞事件
            if (current == null)
            {
                if (_previousCollider != null)
                {
                    OnRayExit?.Invoke(_previousCollider);
                }
            }

            // 射线停留在某个物体时触发的事件
            // 如果当前射线碰撞的物体和上一个射线碰撞到物体相同，则触发射线停留事件
            else if (_previousCollider == current)
            {
                OnRayStay?.Invoke(current);
            }

            // 射线更新事件
            // 如果当前射线和上一个射线碰撞的物体不是同一个，则上一个物体触发射线离开事件，当前物体触发射线碰撞事件
            else if (_previousCollider != null)
            {
                OnRayEnter?.Invoke(current);
                OnRayExit?.Invoke(_previousCollider);
            }
            // 如果之前帧中没有射线碰撞的物体，当前帧有射线碰撞到物体，则触发此事件。
            else
            {
                // no collider on last frame
                OnRayEnter?.Invoke(current);
            }

            // 将当前射线碰撞的物体保存为 上一个碰撞的物体
            _previousCollider = current;
        }
    }
}
```

Awake中使用

```C#
//射线检测
CSray = new CSRayCasterInput();
//射线进入的时候
CSray.OnRayEnter.AddListener(self.OnRayEnter);
//射线移出的时候
CSray.OnRayExit.AddListener(self.OnRayExit);
```

```C#
public static void OnRayEnter(this PlayerToGhostRoomSceneInteractionComponent self, Collider collider)
{
    Debug.Log("射线进入");
}
public static void OnRayExit(this PlayerToGhostRoomSceneInteractionComponent self, Collider collider)
{
    Debug.Log("射线离开");
}
```

Update中

```C#
CSray.CastMouseRay(Camera.main, 5f, LayerMask.GetMask(ClientConfig.InteractGo));//发射射线
```
