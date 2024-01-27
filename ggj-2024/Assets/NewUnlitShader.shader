Shader"Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FadeNoise ("FadeNoise", Range(0.0, 1.0)) = 1
        _Dist ("Dist", Float) = 256
        _PixelScaling ("PixelScaling", Float) = 0.25
        _OverallFade ("OverallFade", Range(0.0, 1.0)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define iResolution _ScreenParams
            #define fragCoord ((i.screenPos.xy/i.screenPos.w) * _ScreenParams.xy)


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
                float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float _FadeNoise;
            float _Dist;
            float _PixelScaling;
            float _OverallFade;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);   
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);    
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample main tex using wrapping frag coord x and y based on size of image
                float4 col = tex2D(_MainTex, ((i.worldPos.xy / _MainTex_TexelSize.zw)) * _PixelScaling) * _FadeNoise;
                float dist = distance(fragCoord.xy, iResolution.xy / 2);
                float coverage = smoothstep(0.0f, _Dist, dist);
                float noiseAmt = (1.0f - coverage) * 2.0f;
                float4 colourOut = float4(col.rgb * max(noiseAmt, 0.02), min(coverage + col.a * 0.2f, 0.98f));
                colourOut.a *= _OverallFade;
                return colourOut;
            }
            ENDCG
        }
    }
}
