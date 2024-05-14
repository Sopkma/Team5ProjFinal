Shader "MyShaderCategory/MyShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("BaseColor", Color) = (1,1,1,0)
        _LightDir("Light Direction", Vector) = (1, -1, 1, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Cull Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                half3 worldNormal : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _LightDir;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // col *= _Color;
                col -= max(0,dot(i.worldNormal, _LightDir));
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                // return col / _Color;
                return col;
                // return col / _Color * fixed4(1, 0.5, 0.5, 1);
            }
            ENDCG
        }
    }
}
