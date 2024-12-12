Shader "UI/OutlineImage"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0, 0, 0, 1)
        _OutlineThickness ("Outline Thickness", Range(0, 100)) = 1
        _OutlineSoftness ("Outline Softness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;
            float _OutlineSoftness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 texelSize = float2(_OutlineThickness / _ScreenParams.x, _OutlineThickness / _ScreenParams.y);

                float4 mainColor = tex2D(_MainTex, i.uv);
                if (mainColor.a > 0.0)
                    return mainColor;

                float alpha = 0.0;

                float2 offsets[16];
                int idx = 0;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        offsets[idx++] = float2(x, y) * texelSize;
                    }
                }

                for (int j = 0; j < 9; j++)
                {
                    float2 sampleUV = i.uv + offsets[j];

                    // Проверка выхода за границы текстуры
                    if (sampleUV.x < 0.0 || sampleUV.x > 1.0 || sampleUV.y < 0.0 || sampleUV.y > 1.0)
                        continue;

                    float4 sampleColor = tex2D(_MainTex, sampleUV);
                    float distance = length(offsets[j]);

                    if (sampleColor.a > 0.0)
                    {
                        alpha = max(alpha, 1.0 - (distance / _OutlineThickness) * _OutlineSoftness);
                    }
                }

                return float4(_OutlineColor.rgb, _OutlineColor.a * alpha);
            }
            ENDHLSL
        }
    }
}
