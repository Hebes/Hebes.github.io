# Unity框架-摇杆模块

<a href="\md\Unity前端\Resources\Unity框架-摇杆模块\摇杆模块.unitypackage" target="_blank">摇杆模块</a>

[使用Unity框架-点击模块的](/md/Unity前端/Child/Unity框架-点击模块.md)

PEListener.cs

WindowRoot.cs

```C#
public static class ClientConfig {
    public const int ScreenStandardWidth = 2160;
    public const int ScreenStandardHeight = 1080;

    public const int ScreenOPDis = 135;
}
```

```C#
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayWnd : WindowRoot {
    public Transform testObjTrans;
    public float speedMultipler;

    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;
    public Transform ArrowRoot;

    float pointDis = 135;
    Vector2 startPos = Vector2.zero;
    Vector2 defaultPos = Vector2.zero;
    void Start() {
        pointDis = Screen.height * 1.0f / ClientConfig.ScreenStandardHeight * ClientConfig.ScreenOPDis;
        SetActive(ArrowRoot, false);
        defaultPos = imgDirBg.transform.position;

        RegisterMoveEvts();
    }

    void RegisterMoveEvts() {
        SetActive(ArrowRoot, false);

        OnClickDown(imgTouch.gameObject, (PointerEventData evt, object[] args) => {
            startPos = evt.position;
            Debug.Log($"evt.pos:{evt.position}");
            imgDirPoint.color = new Color(1, 1, 1, 1f);
            imgDirBg.transform.position = evt.position;
        });
        OnClickUp(imgTouch.gameObject, (PointerEventData evt, object[] args) => {
            imgDirBg.transform.position = defaultPos;
            imgDirPoint.color = new Color(1, 1, 1, 0.5f);
            imgDirPoint.transform.localPosition = Vector2.zero;
            SetActive(ArrowRoot, false);

            InputMoveKey(Vector2.zero);
        });
        OnDrag(imgTouch.gameObject, (PointerEventData evt, object[] args) => {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if(len > pointDis) {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else {
                imgDirPoint.transform.position = evt.position;
            }

            if(dir != Vector2.zero) {
                SetActive(ArrowRoot);
                float angle = Vector2.SignedAngle(new Vector2(1, 0), dir);
                ArrowRoot.localEulerAngles = new Vector3(0, 0, angle);
            }

            InputMoveKey(dir.normalized);
        });
    }

    void InputMoveKey(Vector2 dir) {
        //Debug.Log($"Input Dir:{dir}");
        this.dir = dir;
    }


    Vector3 dir;
    void Update() {
        if(dir != Vector3.zero) {
            Vector3 pos = dir * Time.deltaTime * speedMultipler;
            Debug.Log(pos);
            testObjTrans.position += pos;
        }
    }
}
```
