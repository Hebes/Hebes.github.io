# Shader-圆角矩形圆形

```C#
Shader "UI/RoundShape"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
 
       
 
        _RoundRadius("Round Radius", Range(0,0.5)) = 0.5
        _Width("Width", Float) = 100
        _Height("Height", Float) = 100
    }
 
    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
 
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp] 
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
 
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
 
        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
 
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
 
            #pragma multi_compile __ UNITY_UI_ALPHACLIP
 
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
 
            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };
 
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _MainTex_ST;
            float4 _ClipRect;
            float _RoundRadius;
            float _Width;
            float _Height;
            
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = IN.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
 
                //OUT.texcoord = IN.texcoord;
                OUT.texcoord = TRANSFORM_TEX(IN.texcoord, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }
 
            sampler2D _MainTex;
 
            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
 
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
 
                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
 
                float aspect = _Height/_Width;
 
                float2 center = float2(abs(round(IN.texcoord.x) - _RoundRadius*aspect),abs(round(IN.texcoord.y) - _RoundRadius));
                float dis = distance(fixed2(IN.texcoord.x * _Width,IN.texcoord.y * _Height),fixed2(center.x * _Width,center.y * _Height));
                
                float pwidth = fwidth(dis);
                float alpha = saturate((_RoundRadius * _Height - dis) / pwidth);
                
                //float alpha = step(dis,_RoundRadius * _Height);
                float a = color.a* alpha;
                
 
                float oy = max(step(IN.texcoord.y,_RoundRadius),step((1-_RoundRadius),IN.texcoord.y));
                float ox = max(step(IN.texcoord.x,_RoundRadius*aspect),step((1-_RoundRadius*aspect),IN.texcoord.x));
                color.a = ox * (oy * a + (1-oy) * color.a) + (1-ox) * color.a;
 
                return color;
                
            }
        ENDCG
        }
    }
}
```
