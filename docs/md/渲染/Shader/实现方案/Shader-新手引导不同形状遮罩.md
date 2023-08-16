# Shader-新手引导不同形状遮罩

**[[Shader] 使用Shader制作新手引导不同形状的遮罩, 圆形、矩形、圆角矩形](<https://www.kadastudio.cn/archives/89>)**

## 背景

> 在项目工作中，新手引导总是由于场景不同用到**不同形状的遮罩** ✌，如果为每个新手引导制作不同的遮罩，则会增大内存的开销，同时网上的相关资源也没有合适的解决方案 😳 ，因此写了这个 shader解决类似问题。

## 概要

> 🎉 unity3D 新手引导遮罩，支持圆形,矩形框,圆角矩形框。图形位置和大小可以根据控件的位置和大小调节，通用所有分辨率设备。黄色区域遮挡，只有白色区域可以点穿。

👍 内容如下：

+ 实现不同形状遮罩包括：圆形、双圆形、矩形、圆角矩形
+ 提供漏洞点击实现代码及使用方法
+ 给出 GuideMask.cs 源码及 GuideMask.Shader 源码

下面，进入正题~😋

## 一、基本图形

## 1、圆形

实现代码：

```auto
/// <summary> 
/// 创建圆形点击区域 
/// </summary> 
/// <param name="pos">矩形中心点坐标</param> 
/// <param name="widthAndHeight">矩形宽高</param>  
public void CreateCircleMask(Vector3 pos, float rad, Vector3 pos1, float rad1) 
{
     _materia.SetFloat("_MaskType", 0f);
     _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, rad, 0));
     _materia.SetVector("_TopOri", new Vector4(0,0, 0, 0)); 
}
```

效果如下：

![](https://b3logfile.com/file/2023/03/%E5%9C%86%E5%BD%A2-1wYjeyf.jpg?imageView2/2/interlace/1/format/webp)

## 2、双圆形

实现代码：

```auto
/// <summary> 
/// 创建双圆形点击区域 
/// </summary> 
/// <param name="pos">大圆形中心点坐标</param> 
/// <param name="rad">大圆形半径</param> 
/// <param name="pos1">小圆形中心点坐标</param> 
/// <param name="rad1">小圆形半径</param>  
public void CreateCircleMask(Vector3 pos, float rad, Vector3 pos1, float rad1) 
{   
     _materia.SetFloat("_MaskType", 0f);
     _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, rad, 0));
     _materia.SetVector("_TopOri", new Vector4(pos1.x, pos1.y, rad1, 0)); 
}
```

效果如下：

![](https://b3logfile.com/file/2023/03/%E5%8F%8C%E5%9C%86%E5%BD%A2-HjIkyKj.jpg?imageView2/2/interlace/1/format/webp)

## 3、矩形

实现代码：

```auto
/// <summary> 
/// 创建矩形点击区域 
/// </summary> 
/// <param name="pos">矩形中心点坐标</param> 
/// <param name="widthAndHeight">矩形宽高</param> 
public void CreateRectangleMask(Vector2 pos, Vector2 widthAndHeight, float raid) 
{
     _materia.SetFloat("_MaskType", 1f);
     _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, widthAndHeight.x, widthAndHeight.y));
}
```

效果如下：

![](https://b3logfile.com/file/2023/03/%E7%9F%A9%E5%BD%A2-EQwtOOP.jpg?imageView2/2/interlace/1/format/webp)

## 4、圆角矩形

实现代码：

```auto
/// <summary> 
/// 创建圆角矩形点击区域 
/// 水平圆角矩形_MaskType为2，垂直为3 
/// </summary> 
/// <param name="pos">矩形中心点坐标</param> 
/// <param name="widthAndHeight">矩形宽高</param> 
/// <param name="raid">圆角大小</param> 
/// <param name="isHorizontal">水平还是垂直</param> 
public void CreateCircleRectangleMask(Vector2 pos, Vector2 widthAndHeight,float raid,bool isHorizontal = true) 
{
     _materia.SetFloat("_MaskType", isHorizontal?2f:3f);
     _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, widthAndHeight.x, widthAndHeight.y));
     _materia.SetFloat("_Raid", raid); 
}
```

效果如下：

![](https://b3logfile.com/file/2023/03/%E5%9C%86%E8%A7%92%E7%9F%A9%E5%BD%A2-b7LrXJX.jpg?imageView2/2/interlace/1/format/webp)

通过参数设置可实现不同的圆角效果，如下：

![](https://b3logfile.com/file/2023/03/%E5%9C%86%E8%A7%92%E7%9F%A9%E5%BD%A22-ADw7mHb.jpg?imageView2/2/interlace/1/format/webp)

## 二、漏洞点击实现

## 1、实现代码

```auto
/// <summary> 
/// 设置目标不被Mask遮挡 
/// </summary> 
/// <param name="tg">目标</param> 
public void SetTargetImage(GameObject tg) 
{
     target = tg; 
} 
/// <summary> 
/// 需要继承ICanvasRaycastFilter接口 
/// </summary> 
public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) 
{
     //没有目标则捕捉事件渗透
     if (target == null)
     {
         return true;
     }
     //在目标范围内做事件渗透
     return !RectTransformUtility.RectangleContainsScreenPoint(
     target.GetComponent<RectTransform>(),sp, eventCamera); 
}
```

## 2、使用方法

✌ 将 MyGuideMask 挂载到脚本上，然后通过 GuideMask 创建材质并赋值，根据需要调用图形对应方法。

❗ 注意：如果需点击漏洞下的组件，需将其赋值给 Target。

## 三、源码

## 1、GuideMask.cs 源码

```auto
using UnityEngine;
using UnityEngine.UI;

public class MyGuideMask : MonoBehaviour, ICanvasRaycastFilter
{
    public Image _Mask; //遮罩图片
    private Material _materia;
    private GameObject target;

    private void Awake()
    {
        _materia = _Mask.material;
    }
  
    /// <summary>
    /// 创建圆角矩形区域
    /// </summary>
    /// <param name="pos">矩形的屏幕位置</param>
    /// <param name="pos1">左下角位置</param>
    /// <param name="pos2">右上角位置</param>
    public void CreateCircleRectangleMask(Vector2 pos, Vector2 widthAndHeight,float raid,bool isHorizontal = true) 
{
     _materia.SetFloat("_MaskType", isHorizontal?2f:3f);
     _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, widthAndHeight.x, widthAndHeight.y));
     _materia.SetFloat("_Raid", raid); 
}
  
    /// <summary>
    /// 创建矩形点击区域
    /// </summary>
    /// <param name="pos">矩形中心点坐标</param>
    /// <param name="widthAndHeight">矩形宽高</param>
    public void CreateRectangleMask(Vector2 pos, Vector2 widthAndHeight, float raid)
    {
        _materia.SetFloat("_MaskType", 1f);
        _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, widthAndHeight.x, widthAndHeight.y));
    }
  
    /// <summary>
    /// 创建双圆形点击区域
    /// </summary>
    /// <param name="pos">大圆形中心点坐标</param>
    /// <param name="rad">大圆形半径</param>
    /// <param name="pos1">小圆形中心点坐标</param>
    /// <param name="rad1">小圆形半径</param>
    public void CreateCircleMask(Vector3 pos, float rad, Vector3 pos1, float rad1)
    {
        _materia.SetFloat("_MaskType", 0f);
        _materia.SetVector("_Origin", new Vector4(pos.x, pos.y, rad, 0));
        _materia.SetVector("_TopOri", new Vector4(pos1.x, pos1.y, rad1, 0));
    }

    /// <summary>
    /// 设置目标不被Mask遮挡
    /// </summary>
    /// <param name="tg">目标</param>
    public void SetTargetImage(GameObject tg)
    {
        target = tg;
    }
  
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        //没有目标则捕捉事件渗透
        if (target == null)
        {
            return true;
        }

        //在目标范围内做事件渗透
        return !RectTransformUtility.RectangleContainsScreenPoint(target.GetComponent<RectTransform>(),
            sp, eventCamera);
    }
}
```

## 2、GuideMask.Shader 源码

```auto
Shader "UI/GuideMask"
{
 Properties{
     [PerRendererData] _MainTex("Sprite Texture", 2D)="white"{}
         _Color("Tint",Color)=(1,1,1,1)

        _StencilComp("Stencil Comparison", Float) = 8
  _Stencil("Stencil ID", Float) = 0
  _StencilOp("Stencil Operation", Float) = 0
  _StencilWriteMask("Stencil Write Mask", Float) = 255
  _StencilReadMask("Stencil Read Mask", Float) = 255
  _ColorMask("Color Mask", Float) = 15

  _Origin("Rect",Vector) = (0,0,0,0)
  _TopOri("TopCircle",Vector) = (0,0,0,0)
  _Raid("RectRaid",Range(0,100)) = 0
  _MaskType("Type",Float) = 0

    }
 SubShader{
  Tags{
   "Queue" = "Transparent"
   "IgnoreProjector" = "True"
   "RenderType" = "Transparent"
   "PreviewType" = "Plane"
   "CanUseSpriteAtlas" = "True"
        }
  Stencil{
   Ref[_Stencil]
   Comp[_StencilComp]
   Pass[_StencilOp]
   ReadMask[_StencilReadMask]
   WriteMask[_StencilWriteMask]
  }

  Cull Off
  Lighting Off
  ZWrite Off
  ZTest[unity_GUIZTestMode]
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask[_ColorMask]

  Pass{
   Name "Default"
   CGPROGRAM
   #pragma vertex vert
   #pragma fragment frag
   #pragma target 2.0

   #include "UnityCG.cginc"
   #include "UnityUI.cginc"

   struct appdata_t
   {
    float4 vertex : POSITION;
    float4 color : COLOR;
    float2 texcoord : TEXCOORD0;
            };

   struct v2f{
    float4 vertex:SV_POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    float4 worldPosition : TEXCOORD1;
            };

   sampler2D _MainTex;
   fixed4 _Color;
   fixed4 _TextureSampleAdd;
   float4 _ClipRect;
   float4 _Origin;
   float4 _TopOri;
   float _Raid;
   float _MaskType;
   //0 圆形 1 矩形 2 圆角矩形 

   v2f vert(appdata_t IN){
    v2f OUT;
    OUT.worldPosition = IN.vertex;
    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color;
    return OUT;
   }
   //垂直圆角矩形
   fixed checkInCircleRectVectory (float4 worldPosition) {
    float4 rec1Pos=float4(_Origin.x-_Origin.z/2,_Origin.y-_Origin.w/2-_Raid,_Origin.x+_Origin.z/2,_Origin.y+_Origin.w/2+_Raid);
    float4 rec2Pos=float4(_Origin.x-_Origin.z/2+_Raid,_Origin.y-_Origin.w/2-2*_Raid,_Origin.x+_Origin.z/2-_Raid,_Origin.y+_Origin.w/2+2*_Raid);
    fixed2 step1=step(rec1Pos.xy, worldPosition.xy) * step(worldPosition.xy, rec1Pos.zw);
    fixed2 step2=step(rec2Pos.xy, worldPosition.xy) * step(worldPosition.xy, rec2Pos.zw);
    fixed rec1=step1.x*step1.y<1?0:1;
    fixed rec2=step2.x*step2.y<1?0:1;
    fixed dis1=distance(float2(_Origin.x+_Origin.z/2-_Raid,_Origin.y+_Origin.w/2+_Raid),worldPosition.xy)<_Raid?1:0;
    fixed dis2=distance(float2(_Origin.x-_Origin.z/2+_Raid,_Origin.y-_Origin.w/2-_Raid),worldPosition.xy)<_Raid?1:0;
    fixed dis3=distance(float2(_Origin.x+_Origin.z/2-_Raid,_Origin.y-_Origin.w/2-_Raid),worldPosition.xy)<_Raid?1:0;
    fixed dis4=distance(float2(_Origin.x-_Origin.z/2+_Raid,_Origin.y+_Origin.w/2+_Raid),worldPosition.xy)<_Raid?1:0;
    return (dis1+dis2+dis3+dis4+rec1+rec2)>0?0:1;
   }

   //水平圆角矩形
   fixed checkInCircleRectHorizontal (float4 worldPosition) {

    float4 rec1Pos=float4(_Origin.x-_Origin.z/2-_Raid,_Origin.y-_Origin.w/2,_Origin.x+_Origin.z/2+_Raid,_Origin.y+_Origin.w/2);
    float4 rec2Pos=float4(_Origin.x-_Origin.z/2-2*_Raid,_Origin.y-_Origin.w/2+_Raid,_Origin.x+_Origin.z/2+2*_Raid,_Origin.y+_Origin.w/2-_Raid);
    fixed2 step1=step(rec1Pos.xy, worldPosition.xy) * step(worldPosition.xy, rec1Pos.zw);
    fixed2 step2=step(rec2Pos.xy, worldPosition.xy) * step(worldPosition.xy, rec2Pos.zw);
    fixed rec1=step1.x*step1.y<1?0:1;
    fixed rec2=step2.x*step2.y<1?0:1;
    fixed dis1=distance(float2(_Origin.x-_Origin.z/2-_Raid,_Origin.y+_Origin.w/2-_Raid),worldPosition.xy)<_Raid?1:0;
    fixed dis2=distance(float2(_Origin.x-_Origin.z/2-_Raid,_Origin.y-_Origin.w/2+_Raid),worldPosition.xy)<_Raid?1:0;
    fixed dis3=distance(float2(_Origin.x+_Origin.z/2+_Raid,_Origin.y+_Origin.w/2-_Raid),worldPosition.xy)<_Raid?1:0;
    fixed dis4=distance(float2(_Origin.x+_Origin.z/2+_Raid,_Origin.y-_Origin.w/2+_Raid),worldPosition.xy)<_Raid?1:0;
    return (dis1+dis2+dis3+dis4+rec1+rec2)>0?0:1;
   }
   //圆形/双圆形
   fixed checkInCircle (float4 worldPosition) {
    fixed dis1=distance(worldPosition, _Origin.xy)< _Origin.z?1:0;
    fixed dis2=distance(worldPosition.xy, _TopOri.xy)< _TopOri.z?1:0;
    return (dis1+dis2)>0?0:1;
   }
   //矩形
   fixed checkInRect (float4 worldPosition) {
    float4 temp=float4(_Origin.x-_Origin.z/2,_Origin.y-_Origin.w/2,_Origin.x+_Origin.z/2,_Origin.y+_Origin.w/2);
    float2 inside = step(temp.xy, worldPosition.xy) * step(worldPosition.xy, temp.zw);
    return inside.x*inside.y>0?0:1;

   }
   fixed4 frag(v2f IN) : SV_Target{
    half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
    if(_MaskType==0){
     color.a=checkInCircle(IN.worldPosition)==0?0:color.a;
                }else if(_MaskType==1){
     color.a=checkInRect(IN.worldPosition)==0?0:color.a;
                }else if(_MaskType==3){
     color.a=checkInCircleRectVectory(IN.worldPosition)==0?0:color.a;
                }
    else if(_MaskType==2){
     color.a=checkInCircleRectHorizontal(IN.worldPosition)==0?0:color.a;
                }
    return color;
   }



   ENDCG
        }
    }
}
```
