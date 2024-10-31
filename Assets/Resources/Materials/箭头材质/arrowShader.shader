Shader "Unlit/arrowShader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        // 透明度
        _Alpha("Alpha", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
            // "CanUseSpriteAtlas"="True"
            // "RenderType"="Opaque" 
        }
        LOD 100

        Cull Off

        Pass
        {
            Tags { "LightMode"="ForwardBase" }
            //Open Alpha Blend
            ZWrite Off // 关闭深度写入
            Blend SrcAlpha OneMinusSrcAlpha // 开启透明度混合
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Alpha;
            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {

                return float4(_Color.rgb, _Alpha);
            }
            ENDCG
        }
    }
}
