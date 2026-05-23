Shader "Krivodeling/UI/UI Blur Mobile"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        [Toggle] _FlipX("Flip X", float) = 0
        [Toggle] _FlipY("Flip Y", float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Int) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Int) = 0
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
            
            Blend DstColor Zero
            
            Pass
            {
                    CGPROGRAM
                    #include "UnityCG.cginc"
                    #pragma debug
                    #pragma vertex vert
                    #pragma fragment frag 
                    #pragma multi_compile _ DISABLE_BLUR
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
                        float2 uv : TEXCOORD0;
                        float4 vcol : COLOR0;
                    };

                    struct v2f
                    {
                        float4 position : POSITION;
                        float2 uv : TEXCOORD0;
                        float4 vcol : COLOR0;
                    };

                    v2f vert(data i)
                    {
                        v2f o;
                        o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                        o.position = UnityObjectToClipPos(i.vertex);
                        o.vcol = i.vcol;

                        return o;
                    }

                    float4 frag(v2f i) : SV_Target
                    {
                        
                            float4 c = lerp(1, i.vcol * _Color, tex2D(_MainTex, i.uv).a * i.vcol.a * _Color.a);
                        return c;
                    }

                ENDCG
                }
        }
            Fallback Off
}
