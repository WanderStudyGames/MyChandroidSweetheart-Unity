Shader "Krivodeling/UI/UI Blur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        [Toggle] _FlipX("Flip X", float) = 0
        [Toggle] _FlipY("Flip Y", float) = 0
        _Intensity("Intensity", Range(0, 1)) = 0
        _Multiplier("Multiplier", Range(0, 4)) = 0.15
    }

        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
            ZWrite Off
            Cull back
            
            //unity does not allow conditionals outside of CGPROGRAM blocks, meaning the subshader's blend mode and use of GrabPass are hard-coded into the shader.
            //I'd like to dynamically enable/disable them via multi_compile, but unity does not allow that.

            //if DISABLE_BLUR
            // Blend DstColor Zero
            
            //else
            Blend SrcAlpha OneMinusSrcAlpha
            GrabPass { }
            
            //endif
            
            Pass
            {
                    CGPROGRAM
                    #include "UnityCG.cginc"
                    #pragma debug
                    #pragma vertex vert
                    #pragma fragment frag 
                    #ifndef SHADER_API_D3D11
                    #pragma target 3.0
                    #else
                    #pragma target 4.0
                    #endif

                    sampler2D _GrabTexture;
                    sampler2D _MainTex;
                    float4 _MainTex_ST;
                    half4 _Color;
                    float _FlipX;
                    float _FlipY;
                    float _Intensity;
                    float _Multiplier;

                    struct data
                    {
                        float4 vertex : POSITION;
                        float3 normal : NORMAL;
                        float2 uv : TEXCOORD0;
                        float4 vcol : COLOR0;
                    };

                    struct v2f
                    {
                        float4 position : POSITION;
                        float4 screenPos : TEXCOORD1;
                        float2 uv : TEXCOORD0;
                        float4 vcol : COLOR0;
                    };

                    v2f vert(data i)
                    {
                        v2f o;
                        o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                        o.position = UnityObjectToClipPos(i.vertex);
                        o.vcol = i.vcol;
                        o.screenPos = o.position;

                        return o;
                    }

                    float4 frag(v2f i) : SV_Target
                    {
                        float2 screenPos = i.screenPos.xy / i.screenPos.w;
                        float depth = _Intensity * tex2D(_MainTex, i.uv).a * _Multiplier / 200;

                        if (_FlipX)
                            screenPos.x = 1 - (screenPos.x + 1) * 0.5;
                        else
                            screenPos.x = (screenPos.x + 1) * 0.5;

                        if (_FlipY)
                            screenPos.y = (screenPos.y + 1) * 0.5;
                        else
                            screenPos.y = 1 - (screenPos.y + 1) * 0.5;

                        half4 sum = half4(0.0h, 0.0h, 0.0h, 0.0h);

                        sum += tex2D(_GrabTexture, float2(screenPos.x - 5.0 * depth, screenPos.y + 5.0 * depth)) * 0.025;
                        sum += tex2D(_GrabTexture, float2(screenPos.x + 5.0 * depth, screenPos.y - 5.0 * depth)) * 0.025;

                        sum += tex2D(_GrabTexture, float2(screenPos.x - 4.0 * depth, screenPos.y + 4.0 * depth)) * 0.05;
                        sum += tex2D(_GrabTexture, float2(screenPos.x + 4.0 * depth, screenPos.y - 4.0 * depth)) * 0.05;

                        sum += tex2D(_GrabTexture, float2(screenPos.x - 3.0 * depth, screenPos.y + 3.0 * depth)) * 0.09;
                        sum += tex2D(_GrabTexture, float2(screenPos.x + 3.0 * depth, screenPos.y - 3.0 * depth)) * 0.09;

                        sum += tex2D(_GrabTexture, float2(screenPos.x - 2.0 * depth, screenPos.y + 2.0 * depth)) * 0.12;
                        sum += tex2D(_GrabTexture, float2(screenPos.x + 2.0 * depth, screenPos.y - 2.0 * depth)) * 0.12;

                        sum += tex2D(_GrabTexture, float2(screenPos.x - 1.0 * depth, screenPos.y + 1.0 * depth)) * 0.15;
                        sum += tex2D(_GrabTexture, float2(screenPos.x + 1.0 * depth, screenPos.y - 1.0 * depth)) * 0.15;

                        sum += tex2D(_GrabTexture, screenPos - 5.0 * depth) * 0.025;
                        sum += tex2D(_GrabTexture, screenPos - 4.0 * depth) * 0.05;
                        sum += tex2D(_GrabTexture, screenPos - 3.0 * depth) * 0.09;
                        sum += tex2D(_GrabTexture, screenPos - 2.0 * depth) * 0.12;
                        sum += tex2D(_GrabTexture, screenPos - 1.0 * depth) * 0.15;
                        sum += tex2D(_GrabTexture, screenPos) * 0.25;
                        sum += tex2D(_GrabTexture, screenPos + 1.0 * depth) * 0.025;
                        sum += tex2D(_GrabTexture, screenPos + 2.0 * depth) * 0.05;
                        sum += tex2D(_GrabTexture, screenPos + 3.0 * depth) * 0.09;
                        sum += tex2D(_GrabTexture, screenPos + 4.0 * depth) * 0.12;
                        sum += tex2D(_GrabTexture, screenPos + 5.0 * depth) * 0.15;
                   
                        float4 c = lerp(sum / 2 * i.vcol, sum / 2 * i.vcol * _Color, tex2D(_MainTex, i.uv).a * _Color.a);
               
                        c.a = tex2D(_MainTex, i.uv).a * i.vcol.a;
                        return c;
                    }


                ENDCG
                }
        }
            Fallback Off
}
