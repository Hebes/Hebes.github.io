# Unity功能-Rotation

```C#
transform.localPosition和transform.localScale都是直接赋值三元数，给旋转赋值需要用
方法一： 
xxx.transform.localEulerAngles = new Vector3 (0.0f,0.0f,0.0f); 
方法二： 
xxx.transform.rotation=Quaternion.Euler(0.0f,0.0f,0.0f);
```
