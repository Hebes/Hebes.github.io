# Text文字颜色渐变和描边

**[UGUI 之 Text文字颜色渐变和描边](<https://czhenya.blog.csdn.net/article/details/83033738>)**

对文字渐变色做设置，同时还要加上描边的效果，，，在项目中发现一个比较好用的代码，可以直接拿来使用，用作任何的设置，分享个大家，也方便以后的使用，，，（描边效果是Unity自带的，添加Outline组件进行设置就好了，）

注意：一定是字体渐变色脚本在，描边组件的上边…向下图这样会报错的

![1](Image/Text%E6%96%87%E5%AD%97%E9%A2%9C%E8%89%B2%E6%B8%90%E5%8F%98%E5%92%8C%E6%8F%8F%E8%BE%B9/1.png)

```CSharp {.line-numbers}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
/// <summary>
/// 渐变字体
/// </summary>
[AddComponentMenu("UI/Effects/Gradient")]
public  class FontGradient : BaseMeshEffect
{
    public Color32 topColor = Color.white;
    public Color32 bottomColor = Color.black;
    public bool useGraphicAlpha = true;
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var count = vh.currentVertCount;
        if (count == 0)
            return;

        var vertexs = new List<UIVertex>();
        for (var i = 0; i < count; i++)
        {
            var vertex = new UIVertex();
            vh.PopulateUIVertex(ref vertex, i);
            vertexs.Add(vertex);
        }

        var topY = vertexs[0].position.y;
        var bottomY = vertexs[0].position.y;


        for (var i = 1; i < count; i++)
        {
            var y = vertexs[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }

        }

        var height = topY - bottomY;
        for (var i = 0; i < count; i++)
        {
            var vertex = vertexs[i];

            var color = Color32.Lerp(bottomColor, topColor, (vertex.position.y - bottomY) / height);

            vertex.color = color;

            vh.SetUIVertex(vertex, i);
        }
    }
}
```
