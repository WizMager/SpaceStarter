Shader "GLS/Toon/Lit"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowThreshold ("Shadow Threshold", Range(0, 1)) = 0.5 
        _ShadowStrength ("Shadow Strength", Range(0, 3)) = 1. 

        _Brightness ("Brightness", Range(-1, 1)) = 0.
        _Contrast("Contrast", Range(0, 2)) = 1.
        _Saturation("Saturation", Range(0, 2)) = 1.
    }
    SubShader
    {
        Tags 
        { 
            "LightMode" = "ForwardBase"
            "RenderType" = "Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"            

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : NORMAL;
                float4 pos : SV_POSITION;
                SHADOW_COORDS(2)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize(mul((float3x3)UNITY_MATRIX_M, v.normal));
                TRANSFER_SHADOW(o)
                return o;
            }

            float4 _Color;
            float _ShadowThreshold;
            float _ShadowStrength;
            float _Brightness;
            float _Contrast;
            float _Saturation;

             inline float4 applyHSBEffect(float4 startColor)
            {
                float4 outputColor = startColor;
                outputColor.rgb = (outputColor.rgb - 0.5f) * (_Contrast)+0.5f;
                outputColor.rgb = outputColor.rgb + _Brightness;
                float3 intensity = dot(outputColor.rgb, float3(0.299, 0.587, 0.114));
                outputColor.rgb = lerp(intensity, outputColor.rgb, _Saturation);
                return outputColor;
            }

            float4 frag (v2f i) : SV_Target
            {
                float NdotL = dot(i.worldNormal, _WorldSpaceLightPos0);

                float shadow = SHADOW_ATTENUATION(i);
                float lightIntensity = smoothstep(0, _ShadowThreshold, NdotL * shadow);	

                // float light = saturate(floor(NdotL * 3) / (2 - 0.5)) * _LightColor0 * lightIntensity;
                float light = _LightColor0 * lightIntensity;

                float4 col = tex2D(_MainTex, i.uv);
                col = applyHSBEffect(col);

                return (col * _Color) * (light + unity_AmbientSky);
                // return col;
            }
            ENDCG
        }

        // Shadow casting support.
        UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
    }
}
