# Shader-模板

```Shader
Shader "ACShader/NewUnlitShader"
{
    //子着色器
    SubShader
    {
        pass
        {
            // 插入Cg代码开始
            CGPROGRAM

            //定义顶点着色器的方法vert
            # pragma vertex vert
            //定义片段着色器的方法frag
            # pragma fragment frag

            //POSITION SV_POSITION SV_TARGET 语义
            float4 vert(float4 pos:POSITION):SV_POSITION 
            {
                float n = 1;//定义变量
                
                float2 point = float2(10,5.6);//定义二维向量,也就是一个点的坐标
                
                float3 pos = float3(10,5.6,3);//定义三维向量

                float4 pos = 1;//定义4维向量=等于float4(1,1,1,1);
                float4 pos = float4(pos,4);//定义4维向量

                float4 pos = float4(10,5.6,3,5);//定义4维向量
                pos.r;//分量:(r,g,b,a)或者(x,y,z,w)只能用其中一种,不能混用
                pos.agrb;//可以打乱排序=float4(5,5.6,10,3)
            }

            float4 frag():SV_TARGET
            {

            }

            // 插入Cg代码结束
            ENDCG
        }
    }
}
```
