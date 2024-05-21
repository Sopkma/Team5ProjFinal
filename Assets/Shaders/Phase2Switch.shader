Shader "MyShaderCategory/Phase2Switch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlashColor("Flash Color", Color) = (1,0,0,1)
        _FlashSpeed("Flash Speed", Float) = 1.0
        _FlashCount("Flash Count", Float) = 3
        _TimeElapsed("Time Elapsed", Float) = 0.0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _FlashColor;
            float _FlashSpeed;
            float _FlashCount;
            float _TimeElapsed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float flashPhase = fmod(_TimeElapsed * _FlashSpeed, 1.0);
                float flashFactor = abs(sin(flashPhase * 3.14159 * 2.0 * _FlashCount));
                half4 texColor = tex2D(_MainTex, i.uv);
                half4 color = lerp(texColor, _FlashColor, flashFactor * 0.5);
                return color;
            }
            ENDCG
        }
    }
}
