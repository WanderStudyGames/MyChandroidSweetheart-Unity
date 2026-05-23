Shader "Unlit/ChandroidEye"
{
    Properties
    {
        [NoScaleOffset]_Tex1("Tex1", 2D) = "white" {}
        _Color1("Color1", Color) = (1, 1, 1, 1)
        _HueShift1("HueShift1", Range(0, 1)) = 0
        [NoScaleOffset]_Tex2("Tex2", 2D) = "black" {}
        _Color2("Color2", Color) = (1, 1, 1, 1)
        _HueShift2("HueShift2", Range(0, 1)) = 0
        [NonModifiableTextureData][NoScaleOffset]_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865("Texture2D", 2D) = "white" {}
        [HideInInspector]_BUILTIN_QueueOffset("Float", Float) = 0
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1
    }
    SubShader
    {
        Tags
        {
            // RenderPipeline: <None>
            "RenderType"="Opaque"
            "BuiltInMaterialType" = "Unlit"
            "Queue"="AlphaTest"
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="BuiltInUnlitSubTarget"
        }
        Pass
        {
            Name "Pass"
            Tags
            {
                "LightMode" = "ForwardBase"
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On

            Stencil
        {
            Ref 1
            Comp Always
            Pass Replace
            Fail Keep
        }
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma multi_compile_fog
        #pragma multi_compile_fwdbase
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _LINEAXIS_VERTICAL _LINEAXIS_HORIZONTAL
        
        
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_UNLIT
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_AlphaClip 1
        #define _BUILTIN_ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865_TexelSize;
        float4 _Tex2_TexelSize;
        float4 _Tex1_TexelSize;
        float4 _Color2;
        float4 _Color1;
        float _HueShift1;
        float _HueShift2;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        SAMPLER(sampler_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        TEXTURE2D(_Tex2);
        SAMPLER(sampler_Tex2);
        TEXTURE2D(_Tex1);
        SAMPLER(sampler_Tex1);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        struct Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(float2 _Direction, float _Speed, float4 _UV, bool _UV_64c897cf98a949c9927cd64455f24906_IsConnected, float _TimeInput, bool _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected, Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float IN, out float4 Out_Vector4_1)
        {
        float4 _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 = _UV;
        bool _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected = _UV_64c897cf98a949c9927cd64455f24906_IsConnected;
        float4 _UV_0173adef6bf247fabb15a720b8644096_Out_0 = IN.uv0;
        float4 _BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3 = _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected ? _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 : _UV_0173adef6bf247fabb15a720b8644096_Out_0;
        float _Property_33a0a9f1077b47678210fb9952286561_Out_0 = _TimeInput;
        bool _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected = _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected;
        float _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3 = _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected ? _Property_33a0a9f1077b47678210fb9952286561_Out_0 : IN.TimeParameters.x;
        float2 _Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0 = float2(_BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3, _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3);
        float2 _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0 = _Direction;
        float2 _Multiply_e67992068fa44c91ba125735b34a4622_Out_2;
        Unity_Multiply_float2_float2(_Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0, _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0, _Multiply_e67992068fa44c91ba125735b34a4622_Out_2);
        float _Property_6f5d8a474bda493f8243c77249f83711_Out_0 = _Speed;
        float2 _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2;
        Unity_Multiply_float2_float2(_Multiply_e67992068fa44c91ba125735b34a4622_Out_2, (_Property_6f5d8a474bda493f8243c77249f83711_Out_0.xx), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2);
        float2 _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2;
        Unity_Add_float2((_BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3.xy), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2, _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2);
        Out_Vector4_1 = (float4(_Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2, 0.0, 1.0));
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        struct Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(float4 _InputColor, UnityTexture2D _MainTex, float2 _Scroll_Direction, float _Flicker_Speed, float _Flicker_Strength, float4 _Color, float _UseTextureForBackground, float4 _Background_Color, float _LineWidth, float _LineScale, float _LineStrength, Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float IN, out float4 Color_1, out float Alpha_2)
        {
        float _Property_65b2b8f6dbde4b54b39f7696550740df_Out_0 = _UseTextureForBackground;
        float4 _Property_a80087fbfb164298860f1e44ea199883_Out_0 = _Background_Color;
        float4 _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0 = _InputColor;
        float4 _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0 = _Color;
        float4 _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2;
        Unity_Multiply_float4_float4(_Property_74cfe5ea1308421daf51de4c77b8f924_Out_0, _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0, _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2);
        float _Split_0be4aef52f1f4793ad49df2680323816_R_1 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[0];
        float _Split_0be4aef52f1f4793ad49df2680323816_G_2 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[1];
        float _Split_0be4aef52f1f4793ad49df2680323816_B_3 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[2];
        float _Split_0be4aef52f1f4793ad49df2680323816_A_4 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[3];
        float _Split_0c085148d60e453baafce67a016ac860_R_1 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[0];
        float _Split_0c085148d60e453baafce67a016ac860_G_2 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[1];
        float _Split_0c085148d60e453baafce67a016ac860_B_3 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[2];
        float _Split_0c085148d60e453baafce67a016ac860_A_4 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[3];
        float4 _Vector4_50529047fe484555872e83056b5f6193_Out_0 = float4(_Split_0be4aef52f1f4793ad49df2680323816_R_1, _Split_0be4aef52f1f4793ad49df2680323816_G_2, _Split_0be4aef52f1f4793ad49df2680323816_B_3, _Split_0c085148d60e453baafce67a016ac860_A_4);
        float4 _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2;
        Unity_Multiply_float4_float4(_Property_a80087fbfb164298860f1e44ea199883_Out_0, _Vector4_50529047fe484555872e83056b5f6193_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2);
        float4 _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3;
        Unity_Branch_float4(_Property_65b2b8f6dbde4b54b39f7696550740df_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2, _Property_a80087fbfb164298860f1e44ea199883_Out_0, _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3);
        float _Split_ac54c6d9941745a2811f997c273cd93d_R_1 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[0];
        float _Split_ac54c6d9941745a2811f997c273cd93d_G_2 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[1];
        float _Split_ac54c6d9941745a2811f997c273cd93d_B_3 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[2];
        float _Split_ac54c6d9941745a2811f997c273cd93d_A_4 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[3];
        float _Property_960fa59466aa478296a3a6018cce0817_Out_0 = _Flicker_Speed;
        float _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_960fa59466aa478296a3a6018cce0817_Out_0, _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2);
        float _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1;
        Unity_Sine_float(_Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2, _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1);
        float _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1, _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3);
        float _Property_118264f782634e6a9ecb393ed44a4858_Out_0 = _Flicker_Strength;
        float _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2;
        Unity_Multiply_float_float(_Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3, _Property_118264f782634e6a9ecb393ed44a4858_Out_0, _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2);
        float _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1;
        Unity_OneMinus_float(_Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2, _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1);
        float2 _Property_e834c146a0434f51ab8db95f23d8b175_Out_0 = _Scroll_Direction;
        Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.uv0 = IN.uv0;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.TimeParameters = IN.TimeParameters;
        half4 _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1;
        SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(_Property_e834c146a0434f51ab8db95f23d8b175_Out_0, half(1), half4 (0, 0, 0, 0), false, half(0), false, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1);
        float _Property_0a947325d8b04b6ba499f20e953e3281_Out_0 = _LineScale;
        float2 _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3;
        Unity_TilingAndOffset_float((_ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1.xy), (_Property_0a947325d8b04b6ba499f20e953e3281_Out_0.xx), float2 (0, 0), _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3);
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_R_1 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[0];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_G_2 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[1];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_B_3 = 0;
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_A_4 = 0;
        float _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1;
        TriangleWave_float(_Split_f00e33bb6f03423dbddec083fd1c34cf_G_2, _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1);
        float _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1, _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3);
        float _Property_5e272a6466b348778d5ae159374ce824_Out_0 = _LineWidth;
        float _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2;
        Unity_Comparison_Greater_float(_Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3, _Property_5e272a6466b348778d5ae159374ce824_Out_0, _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2);
        float _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3;
        Unity_Branch_float(_Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2, float(1), float(0), _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3);
        float _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1;
        Unity_OneMinus_float(_Split_0c085148d60e453baafce67a016ac860_A_4, _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1);
        float _Multiply_b071f89d702142d4818427b50f62515b_Out_2;
        Unity_Multiply_float_float(_OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1, 2, _Multiply_b071f89d702142d4818427b50f62515b_Out_2);
        float _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2;
        Unity_Subtract_float(_Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3, _Multiply_b071f89d702142d4818427b50f62515b_Out_2, _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2);
        float _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1;
        Unity_OneMinus_float(_Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2, _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1);
        float _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0 = _LineStrength;
        float _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2;
        Unity_Multiply_float_float(_OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1, _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0, _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2);
        float _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1;
        Unity_OneMinus_float(_Multiply_887138d924ca464a95762f81c14a0bd8_Out_2, _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1);
        float _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1;
        Unity_Saturate_float(_OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1);
        float _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2;
        Unity_Multiply_float_float(_OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2);
        float _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2;
        Unity_Multiply_float_float(_Split_ac54c6d9941745a2811f997c273cd93d_A_4, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2, _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2);
        float4 _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Unity_Lerp_float4(_Branch_f99a5bfa769c4478b743c922f6795a33_Out_3, _Vector4_50529047fe484555872e83056b5f6193_Out_0, (_Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2.xxxx), _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3);
        float _Split_320e9328194e45608c30482e760fe577_R_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[0];
        float _Split_320e9328194e45608c30482e760fe577_G_2 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[1];
        float _Split_320e9328194e45608c30482e760fe577_B_3 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[2];
        float _Split_320e9328194e45608c30482e760fe577_A_4 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[3];
        Color_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Alpha_2 = _Split_320e9328194e45608c30482e760fe577_A_4;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_5d72a470fca746dd91928edad30f1efe_Out_0 = UnityBuildTexture2DStructNoScale(_Tex1);
            float4 _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5d72a470fca746dd91928edad30f1efe_Out_0.tex, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.samplerstate, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_R_4 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.r;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_G_5 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.g;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_B_6 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.b;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_A_7 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.a;
            float4 _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0 = _Color1;
            float4 _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0, _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0, _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2);
            float _Property_2880ccfc20224e1586c97f7d25889035_Out_0 = _HueShift1;
            float3 _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2;
            Unity_Hue_Normalized_float((_Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2.xyz), _Property_2880ccfc20224e1586c97f7d25889035_Out_0, _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2);
            float _Split_4191fe5768c04a0485ef7fdb05206c01_R_1 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[0];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_G_2 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[1];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_B_3 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[2];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_A_4 = 0;
            float _Split_b37e6b7460ec4f119273bae8f28c6377_R_1 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[0];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_G_2 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[1];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_B_3 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[2];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_A_4 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[3];
            float4 _Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0 = float4(_Split_4191fe5768c04a0485ef7fdb05206c01_R_1, _Split_4191fe5768c04a0485ef7fdb05206c01_G_2, _Split_4191fe5768c04a0485ef7fdb05206c01_B_3, _Split_b37e6b7460ec4f119273bae8f28c6377_A_4);
            UnityTexture2D _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0 = UnityBuildTexture2DStructNoScale(_Tex2);
            float4 _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.tex, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.samplerstate, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_R_4 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.r;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_G_5 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.g;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_B_6 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.b;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_A_7 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.a;
            float4 _Property_db6dfede820f41709370167cf6a8febd_Out_0 = _Color2;
            float4 _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0, _Property_db6dfede820f41709370167cf6a8febd_Out_0, _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2);
            float _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0 = _HueShift2;
            float3 _Hue_4c466a288837449cbfc61c06006e9350_Out_2;
            Unity_Hue_Normalized_float((_Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2.xyz), _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0, _Hue_4c466a288837449cbfc61c06006e9350_Out_2);
            float _Split_8c3721651c794447882328db6a8f0248_R_1 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[0];
            float _Split_8c3721651c794447882328db6a8f0248_G_2 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[1];
            float _Split_8c3721651c794447882328db6a8f0248_B_3 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[2];
            float _Split_8c3721651c794447882328db6a8f0248_A_4 = 0;
            float _Split_e3ce3329aa0241749340bad267461e3d_R_1 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[0];
            float _Split_e3ce3329aa0241749340bad267461e3d_G_2 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[1];
            float _Split_e3ce3329aa0241749340bad267461e3d_B_3 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[2];
            float _Split_e3ce3329aa0241749340bad267461e3d_A_4 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[3];
            float4 _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0 = float4(_Split_8c3721651c794447882328db6a8f0248_R_1, _Split_8c3721651c794447882328db6a8f0248_G_2, _Split_8c3721651c794447882328db6a8f0248_B_3, _Split_e3ce3329aa0241749340bad267461e3d_A_4);
            float4 _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3;
            Unity_Lerp_float4(_Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0, _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0, (_Split_e3ce3329aa0241749340bad267461e3d_A_4.xxxx), _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3);
            float4 Color_a6a852ac92dd45c9a9b8634c5a16ebb1 = IsGammaSpace() ? LinearToSRGB(float4(1.414214, 1.414214, 1.414214, 1)) : float4(1.414214, 1.414214, 1.414214, 1);
            float4 Color_a85e0b27fa824d8b9ea479e2d7b36710 = IsGammaSpace() ? LinearToSRGB(float4(0.8352941, 0.8352941, 0.8352941, 1)) : float4(0.8352941, 0.8352941, 0.8352941, 1);
            Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.uv0 = IN.uv0;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.TimeParameters = IN.TimeParameters;
            float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1;
            float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(_Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3, UnityBuildTexture2DStructNoScale(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865), float2 (0, -0.01), float(16), float(0.281), Color_a6a852ac92dd45c9a9b8634c5a16ebb1, 1, Color_a85e0b27fa824d8b9ea479e2d7b36710, float(0.5), float(100), float(1), _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2);
            surface.BaseColor = (_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1.xyz);
            surface.Alpha = _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "DepthOnly"
            Tags
            {
                "LightMode" = "DepthOnly"
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest Never
        ZWrite Off
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _LINEAXIS_VERTICAL _LINEAXIS_HORIZONTAL
        
        
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_AlphaClip 1
        #define _BUILTIN_ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865_TexelSize;
        float4 _Tex2_TexelSize;
        float4 _Tex1_TexelSize;
        float4 _Color2;
        float4 _Color1;
        float _HueShift1;
        float _HueShift2;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        SAMPLER(sampler_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        TEXTURE2D(_Tex2);
        SAMPLER(sampler_Tex2);
        TEXTURE2D(_Tex1);
        SAMPLER(sampler_Tex1);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        struct Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(float2 _Direction, float _Speed, float4 _UV, bool _UV_64c897cf98a949c9927cd64455f24906_IsConnected, float _TimeInput, bool _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected, Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float IN, out float4 Out_Vector4_1)
        {
        float4 _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 = _UV;
        bool _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected = _UV_64c897cf98a949c9927cd64455f24906_IsConnected;
        float4 _UV_0173adef6bf247fabb15a720b8644096_Out_0 = IN.uv0;
        float4 _BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3 = _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected ? _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 : _UV_0173adef6bf247fabb15a720b8644096_Out_0;
        float _Property_33a0a9f1077b47678210fb9952286561_Out_0 = _TimeInput;
        bool _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected = _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected;
        float _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3 = _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected ? _Property_33a0a9f1077b47678210fb9952286561_Out_0 : IN.TimeParameters.x;
        float2 _Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0 = float2(_BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3, _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3);
        float2 _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0 = _Direction;
        float2 _Multiply_e67992068fa44c91ba125735b34a4622_Out_2;
        Unity_Multiply_float2_float2(_Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0, _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0, _Multiply_e67992068fa44c91ba125735b34a4622_Out_2);
        float _Property_6f5d8a474bda493f8243c77249f83711_Out_0 = _Speed;
        float2 _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2;
        Unity_Multiply_float2_float2(_Multiply_e67992068fa44c91ba125735b34a4622_Out_2, (_Property_6f5d8a474bda493f8243c77249f83711_Out_0.xx), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2);
        float2 _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2;
        Unity_Add_float2((_BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3.xy), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2, _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2);
        Out_Vector4_1 = (float4(_Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2, 0.0, 1.0));
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        struct Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(float4 _InputColor, UnityTexture2D _MainTex, float2 _Scroll_Direction, float _Flicker_Speed, float _Flicker_Strength, float4 _Color, float _UseTextureForBackground, float4 _Background_Color, float _LineWidth, float _LineScale, float _LineStrength, Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float IN, out float4 Color_1, out float Alpha_2)
        {
        float _Property_65b2b8f6dbde4b54b39f7696550740df_Out_0 = _UseTextureForBackground;
        float4 _Property_a80087fbfb164298860f1e44ea199883_Out_0 = _Background_Color;
        float4 _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0 = _InputColor;
        float4 _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0 = _Color;
        float4 _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2;
        Unity_Multiply_float4_float4(_Property_74cfe5ea1308421daf51de4c77b8f924_Out_0, _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0, _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2);
        float _Split_0be4aef52f1f4793ad49df2680323816_R_1 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[0];
        float _Split_0be4aef52f1f4793ad49df2680323816_G_2 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[1];
        float _Split_0be4aef52f1f4793ad49df2680323816_B_3 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[2];
        float _Split_0be4aef52f1f4793ad49df2680323816_A_4 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[3];
        float _Split_0c085148d60e453baafce67a016ac860_R_1 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[0];
        float _Split_0c085148d60e453baafce67a016ac860_G_2 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[1];
        float _Split_0c085148d60e453baafce67a016ac860_B_3 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[2];
        float _Split_0c085148d60e453baafce67a016ac860_A_4 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[3];
        float4 _Vector4_50529047fe484555872e83056b5f6193_Out_0 = float4(_Split_0be4aef52f1f4793ad49df2680323816_R_1, _Split_0be4aef52f1f4793ad49df2680323816_G_2, _Split_0be4aef52f1f4793ad49df2680323816_B_3, _Split_0c085148d60e453baafce67a016ac860_A_4);
        float4 _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2;
        Unity_Multiply_float4_float4(_Property_a80087fbfb164298860f1e44ea199883_Out_0, _Vector4_50529047fe484555872e83056b5f6193_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2);
        float4 _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3;
        Unity_Branch_float4(_Property_65b2b8f6dbde4b54b39f7696550740df_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2, _Property_a80087fbfb164298860f1e44ea199883_Out_0, _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3);
        float _Split_ac54c6d9941745a2811f997c273cd93d_R_1 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[0];
        float _Split_ac54c6d9941745a2811f997c273cd93d_G_2 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[1];
        float _Split_ac54c6d9941745a2811f997c273cd93d_B_3 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[2];
        float _Split_ac54c6d9941745a2811f997c273cd93d_A_4 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[3];
        float _Property_960fa59466aa478296a3a6018cce0817_Out_0 = _Flicker_Speed;
        float _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_960fa59466aa478296a3a6018cce0817_Out_0, _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2);
        float _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1;
        Unity_Sine_float(_Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2, _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1);
        float _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1, _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3);
        float _Property_118264f782634e6a9ecb393ed44a4858_Out_0 = _Flicker_Strength;
        float _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2;
        Unity_Multiply_float_float(_Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3, _Property_118264f782634e6a9ecb393ed44a4858_Out_0, _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2);
        float _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1;
        Unity_OneMinus_float(_Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2, _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1);
        float2 _Property_e834c146a0434f51ab8db95f23d8b175_Out_0 = _Scroll_Direction;
        Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.uv0 = IN.uv0;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.TimeParameters = IN.TimeParameters;
        half4 _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1;
        SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(_Property_e834c146a0434f51ab8db95f23d8b175_Out_0, half(1), half4 (0, 0, 0, 0), false, half(0), false, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1);
        float _Property_0a947325d8b04b6ba499f20e953e3281_Out_0 = _LineScale;
        float2 _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3;
        Unity_TilingAndOffset_float((_ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1.xy), (_Property_0a947325d8b04b6ba499f20e953e3281_Out_0.xx), float2 (0, 0), _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3);
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_R_1 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[0];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_G_2 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[1];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_B_3 = 0;
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_A_4 = 0;
        float _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1;
        TriangleWave_float(_Split_f00e33bb6f03423dbddec083fd1c34cf_G_2, _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1);
        float _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1, _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3);
        float _Property_5e272a6466b348778d5ae159374ce824_Out_0 = _LineWidth;
        float _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2;
        Unity_Comparison_Greater_float(_Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3, _Property_5e272a6466b348778d5ae159374ce824_Out_0, _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2);
        float _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3;
        Unity_Branch_float(_Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2, float(1), float(0), _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3);
        float _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1;
        Unity_OneMinus_float(_Split_0c085148d60e453baafce67a016ac860_A_4, _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1);
        float _Multiply_b071f89d702142d4818427b50f62515b_Out_2;
        Unity_Multiply_float_float(_OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1, 2, _Multiply_b071f89d702142d4818427b50f62515b_Out_2);
        float _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2;
        Unity_Subtract_float(_Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3, _Multiply_b071f89d702142d4818427b50f62515b_Out_2, _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2);
        float _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1;
        Unity_OneMinus_float(_Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2, _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1);
        float _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0 = _LineStrength;
        float _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2;
        Unity_Multiply_float_float(_OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1, _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0, _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2);
        float _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1;
        Unity_OneMinus_float(_Multiply_887138d924ca464a95762f81c14a0bd8_Out_2, _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1);
        float _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1;
        Unity_Saturate_float(_OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1);
        float _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2;
        Unity_Multiply_float_float(_OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2);
        float _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2;
        Unity_Multiply_float_float(_Split_ac54c6d9941745a2811f997c273cd93d_A_4, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2, _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2);
        float4 _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Unity_Lerp_float4(_Branch_f99a5bfa769c4478b743c922f6795a33_Out_3, _Vector4_50529047fe484555872e83056b5f6193_Out_0, (_Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2.xxxx), _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3);
        float _Split_320e9328194e45608c30482e760fe577_R_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[0];
        float _Split_320e9328194e45608c30482e760fe577_G_2 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[1];
        float _Split_320e9328194e45608c30482e760fe577_B_3 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[2];
        float _Split_320e9328194e45608c30482e760fe577_A_4 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[3];
        Color_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Alpha_2 = _Split_320e9328194e45608c30482e760fe577_A_4;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_5d72a470fca746dd91928edad30f1efe_Out_0 = UnityBuildTexture2DStructNoScale(_Tex1);
            float4 _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5d72a470fca746dd91928edad30f1efe_Out_0.tex, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.samplerstate, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_R_4 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.r;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_G_5 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.g;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_B_6 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.b;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_A_7 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.a;
            float4 _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0 = _Color1;
            float4 _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0, _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0, _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2);
            float _Property_2880ccfc20224e1586c97f7d25889035_Out_0 = _HueShift1;
            float3 _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2;
            Unity_Hue_Normalized_float((_Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2.xyz), _Property_2880ccfc20224e1586c97f7d25889035_Out_0, _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2);
            float _Split_4191fe5768c04a0485ef7fdb05206c01_R_1 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[0];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_G_2 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[1];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_B_3 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[2];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_A_4 = 0;
            float _Split_b37e6b7460ec4f119273bae8f28c6377_R_1 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[0];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_G_2 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[1];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_B_3 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[2];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_A_4 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[3];
            float4 _Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0 = float4(_Split_4191fe5768c04a0485ef7fdb05206c01_R_1, _Split_4191fe5768c04a0485ef7fdb05206c01_G_2, _Split_4191fe5768c04a0485ef7fdb05206c01_B_3, _Split_b37e6b7460ec4f119273bae8f28c6377_A_4);
            UnityTexture2D _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0 = UnityBuildTexture2DStructNoScale(_Tex2);
            float4 _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.tex, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.samplerstate, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_R_4 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.r;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_G_5 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.g;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_B_6 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.b;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_A_7 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.a;
            float4 _Property_db6dfede820f41709370167cf6a8febd_Out_0 = _Color2;
            float4 _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0, _Property_db6dfede820f41709370167cf6a8febd_Out_0, _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2);
            float _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0 = _HueShift2;
            float3 _Hue_4c466a288837449cbfc61c06006e9350_Out_2;
            Unity_Hue_Normalized_float((_Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2.xyz), _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0, _Hue_4c466a288837449cbfc61c06006e9350_Out_2);
            float _Split_8c3721651c794447882328db6a8f0248_R_1 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[0];
            float _Split_8c3721651c794447882328db6a8f0248_G_2 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[1];
            float _Split_8c3721651c794447882328db6a8f0248_B_3 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[2];
            float _Split_8c3721651c794447882328db6a8f0248_A_4 = 0;
            float _Split_e3ce3329aa0241749340bad267461e3d_R_1 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[0];
            float _Split_e3ce3329aa0241749340bad267461e3d_G_2 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[1];
            float _Split_e3ce3329aa0241749340bad267461e3d_B_3 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[2];
            float _Split_e3ce3329aa0241749340bad267461e3d_A_4 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[3];
            float4 _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0 = float4(_Split_8c3721651c794447882328db6a8f0248_R_1, _Split_8c3721651c794447882328db6a8f0248_G_2, _Split_8c3721651c794447882328db6a8f0248_B_3, _Split_e3ce3329aa0241749340bad267461e3d_A_4);
            float4 _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3;
            Unity_Lerp_float4(_Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0, _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0, (_Split_e3ce3329aa0241749340bad267461e3d_A_4.xxxx), _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3);
            float4 Color_a6a852ac92dd45c9a9b8634c5a16ebb1 = IsGammaSpace() ? LinearToSRGB(float4(1.414214, 1.414214, 1.414214, 1)) : float4(1.414214, 1.414214, 1.414214, 1);
            float4 Color_a85e0b27fa824d8b9ea479e2d7b36710 = IsGammaSpace() ? LinearToSRGB(float4(0.8352941, 0.8352941, 0.8352941, 1)) : float4(0.8352941, 0.8352941, 0.8352941, 1);
            Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.uv0 = IN.uv0;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.TimeParameters = IN.TimeParameters;
            float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1;
            float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(_Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3, UnityBuildTexture2DStructNoScale(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865), float2 (0, -0.01), float(16), float(0.281), Color_a6a852ac92dd45c9a9b8634c5a16ebb1, 1, Color_a85e0b27fa824d8b9ea479e2d7b36710, float(0.5), float(100), float(1), _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2);
            surface.Alpha = _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
        
        // Render State
        Cull Back
        Blend One Zero
        ZTest LEqual
        ZWrite On
        ColorMask 0
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_shadowcaster
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        #pragma multi_compile _ _CASTING_PUNCTUAL_LIGHT_SHADOW
        #pragma shader_feature_local _LINEAXIS_VERTICAL _LINEAXIS_HORIZONTAL
        
        
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SHADOWCASTER
        #define BUILTIN_TARGET_API 1
        #define _BUILTIN_AlphaClip 1
        #define _BUILTIN_ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865_TexelSize;
        float4 _Tex2_TexelSize;
        float4 _Tex1_TexelSize;
        float4 _Color2;
        float4 _Color1;
        float _HueShift1;
        float _HueShift2;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        SAMPLER(sampler_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        TEXTURE2D(_Tex2);
        SAMPLER(sampler_Tex2);
        TEXTURE2D(_Tex1);
        SAMPLER(sampler_Tex1);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        struct Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(float2 _Direction, float _Speed, float4 _UV, bool _UV_64c897cf98a949c9927cd64455f24906_IsConnected, float _TimeInput, bool _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected, Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float IN, out float4 Out_Vector4_1)
        {
        float4 _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 = _UV;
        bool _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected = _UV_64c897cf98a949c9927cd64455f24906_IsConnected;
        float4 _UV_0173adef6bf247fabb15a720b8644096_Out_0 = IN.uv0;
        float4 _BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3 = _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected ? _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 : _UV_0173adef6bf247fabb15a720b8644096_Out_0;
        float _Property_33a0a9f1077b47678210fb9952286561_Out_0 = _TimeInput;
        bool _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected = _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected;
        float _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3 = _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected ? _Property_33a0a9f1077b47678210fb9952286561_Out_0 : IN.TimeParameters.x;
        float2 _Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0 = float2(_BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3, _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3);
        float2 _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0 = _Direction;
        float2 _Multiply_e67992068fa44c91ba125735b34a4622_Out_2;
        Unity_Multiply_float2_float2(_Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0, _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0, _Multiply_e67992068fa44c91ba125735b34a4622_Out_2);
        float _Property_6f5d8a474bda493f8243c77249f83711_Out_0 = _Speed;
        float2 _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2;
        Unity_Multiply_float2_float2(_Multiply_e67992068fa44c91ba125735b34a4622_Out_2, (_Property_6f5d8a474bda493f8243c77249f83711_Out_0.xx), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2);
        float2 _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2;
        Unity_Add_float2((_BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3.xy), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2, _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2);
        Out_Vector4_1 = (float4(_Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2, 0.0, 1.0));
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        struct Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(float4 _InputColor, UnityTexture2D _MainTex, float2 _Scroll_Direction, float _Flicker_Speed, float _Flicker_Strength, float4 _Color, float _UseTextureForBackground, float4 _Background_Color, float _LineWidth, float _LineScale, float _LineStrength, Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float IN, out float4 Color_1, out float Alpha_2)
        {
        float _Property_65b2b8f6dbde4b54b39f7696550740df_Out_0 = _UseTextureForBackground;
        float4 _Property_a80087fbfb164298860f1e44ea199883_Out_0 = _Background_Color;
        float4 _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0 = _InputColor;
        float4 _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0 = _Color;
        float4 _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2;
        Unity_Multiply_float4_float4(_Property_74cfe5ea1308421daf51de4c77b8f924_Out_0, _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0, _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2);
        float _Split_0be4aef52f1f4793ad49df2680323816_R_1 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[0];
        float _Split_0be4aef52f1f4793ad49df2680323816_G_2 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[1];
        float _Split_0be4aef52f1f4793ad49df2680323816_B_3 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[2];
        float _Split_0be4aef52f1f4793ad49df2680323816_A_4 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[3];
        float _Split_0c085148d60e453baafce67a016ac860_R_1 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[0];
        float _Split_0c085148d60e453baafce67a016ac860_G_2 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[1];
        float _Split_0c085148d60e453baafce67a016ac860_B_3 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[2];
        float _Split_0c085148d60e453baafce67a016ac860_A_4 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[3];
        float4 _Vector4_50529047fe484555872e83056b5f6193_Out_0 = float4(_Split_0be4aef52f1f4793ad49df2680323816_R_1, _Split_0be4aef52f1f4793ad49df2680323816_G_2, _Split_0be4aef52f1f4793ad49df2680323816_B_3, _Split_0c085148d60e453baafce67a016ac860_A_4);
        float4 _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2;
        Unity_Multiply_float4_float4(_Property_a80087fbfb164298860f1e44ea199883_Out_0, _Vector4_50529047fe484555872e83056b5f6193_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2);
        float4 _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3;
        Unity_Branch_float4(_Property_65b2b8f6dbde4b54b39f7696550740df_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2, _Property_a80087fbfb164298860f1e44ea199883_Out_0, _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3);
        float _Split_ac54c6d9941745a2811f997c273cd93d_R_1 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[0];
        float _Split_ac54c6d9941745a2811f997c273cd93d_G_2 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[1];
        float _Split_ac54c6d9941745a2811f997c273cd93d_B_3 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[2];
        float _Split_ac54c6d9941745a2811f997c273cd93d_A_4 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[3];
        float _Property_960fa59466aa478296a3a6018cce0817_Out_0 = _Flicker_Speed;
        float _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_960fa59466aa478296a3a6018cce0817_Out_0, _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2);
        float _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1;
        Unity_Sine_float(_Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2, _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1);
        float _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1, _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3);
        float _Property_118264f782634e6a9ecb393ed44a4858_Out_0 = _Flicker_Strength;
        float _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2;
        Unity_Multiply_float_float(_Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3, _Property_118264f782634e6a9ecb393ed44a4858_Out_0, _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2);
        float _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1;
        Unity_OneMinus_float(_Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2, _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1);
        float2 _Property_e834c146a0434f51ab8db95f23d8b175_Out_0 = _Scroll_Direction;
        Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.uv0 = IN.uv0;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.TimeParameters = IN.TimeParameters;
        half4 _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1;
        SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(_Property_e834c146a0434f51ab8db95f23d8b175_Out_0, half(1), half4 (0, 0, 0, 0), false, half(0), false, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1);
        float _Property_0a947325d8b04b6ba499f20e953e3281_Out_0 = _LineScale;
        float2 _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3;
        Unity_TilingAndOffset_float((_ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1.xy), (_Property_0a947325d8b04b6ba499f20e953e3281_Out_0.xx), float2 (0, 0), _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3);
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_R_1 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[0];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_G_2 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[1];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_B_3 = 0;
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_A_4 = 0;
        float _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1;
        TriangleWave_float(_Split_f00e33bb6f03423dbddec083fd1c34cf_G_2, _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1);
        float _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1, _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3);
        float _Property_5e272a6466b348778d5ae159374ce824_Out_0 = _LineWidth;
        float _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2;
        Unity_Comparison_Greater_float(_Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3, _Property_5e272a6466b348778d5ae159374ce824_Out_0, _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2);
        float _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3;
        Unity_Branch_float(_Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2, float(1), float(0), _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3);
        float _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1;
        Unity_OneMinus_float(_Split_0c085148d60e453baafce67a016ac860_A_4, _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1);
        float _Multiply_b071f89d702142d4818427b50f62515b_Out_2;
        Unity_Multiply_float_float(_OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1, 2, _Multiply_b071f89d702142d4818427b50f62515b_Out_2);
        float _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2;
        Unity_Subtract_float(_Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3, _Multiply_b071f89d702142d4818427b50f62515b_Out_2, _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2);
        float _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1;
        Unity_OneMinus_float(_Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2, _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1);
        float _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0 = _LineStrength;
        float _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2;
        Unity_Multiply_float_float(_OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1, _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0, _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2);
        float _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1;
        Unity_OneMinus_float(_Multiply_887138d924ca464a95762f81c14a0bd8_Out_2, _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1);
        float _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1;
        Unity_Saturate_float(_OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1);
        float _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2;
        Unity_Multiply_float_float(_OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2);
        float _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2;
        Unity_Multiply_float_float(_Split_ac54c6d9941745a2811f997c273cd93d_A_4, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2, _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2);
        float4 _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Unity_Lerp_float4(_Branch_f99a5bfa769c4478b743c922f6795a33_Out_3, _Vector4_50529047fe484555872e83056b5f6193_Out_0, (_Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2.xxxx), _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3);
        float _Split_320e9328194e45608c30482e760fe577_R_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[0];
        float _Split_320e9328194e45608c30482e760fe577_G_2 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[1];
        float _Split_320e9328194e45608c30482e760fe577_B_3 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[2];
        float _Split_320e9328194e45608c30482e760fe577_A_4 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[3];
        Color_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Alpha_2 = _Split_320e9328194e45608c30482e760fe577_A_4;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_5d72a470fca746dd91928edad30f1efe_Out_0 = UnityBuildTexture2DStructNoScale(_Tex1);
            float4 _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5d72a470fca746dd91928edad30f1efe_Out_0.tex, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.samplerstate, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_R_4 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.r;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_G_5 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.g;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_B_6 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.b;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_A_7 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.a;
            float4 _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0 = _Color1;
            float4 _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0, _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0, _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2);
            float _Property_2880ccfc20224e1586c97f7d25889035_Out_0 = _HueShift1;
            float3 _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2;
            Unity_Hue_Normalized_float((_Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2.xyz), _Property_2880ccfc20224e1586c97f7d25889035_Out_0, _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2);
            float _Split_4191fe5768c04a0485ef7fdb05206c01_R_1 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[0];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_G_2 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[1];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_B_3 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[2];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_A_4 = 0;
            float _Split_b37e6b7460ec4f119273bae8f28c6377_R_1 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[0];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_G_2 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[1];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_B_3 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[2];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_A_4 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[3];
            float4 _Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0 = float4(_Split_4191fe5768c04a0485ef7fdb05206c01_R_1, _Split_4191fe5768c04a0485ef7fdb05206c01_G_2, _Split_4191fe5768c04a0485ef7fdb05206c01_B_3, _Split_b37e6b7460ec4f119273bae8f28c6377_A_4);
            UnityTexture2D _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0 = UnityBuildTexture2DStructNoScale(_Tex2);
            float4 _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.tex, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.samplerstate, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_R_4 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.r;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_G_5 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.g;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_B_6 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.b;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_A_7 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.a;
            float4 _Property_db6dfede820f41709370167cf6a8febd_Out_0 = _Color2;
            float4 _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0, _Property_db6dfede820f41709370167cf6a8febd_Out_0, _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2);
            float _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0 = _HueShift2;
            float3 _Hue_4c466a288837449cbfc61c06006e9350_Out_2;
            Unity_Hue_Normalized_float((_Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2.xyz), _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0, _Hue_4c466a288837449cbfc61c06006e9350_Out_2);
            float _Split_8c3721651c794447882328db6a8f0248_R_1 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[0];
            float _Split_8c3721651c794447882328db6a8f0248_G_2 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[1];
            float _Split_8c3721651c794447882328db6a8f0248_B_3 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[2];
            float _Split_8c3721651c794447882328db6a8f0248_A_4 = 0;
            float _Split_e3ce3329aa0241749340bad267461e3d_R_1 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[0];
            float _Split_e3ce3329aa0241749340bad267461e3d_G_2 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[1];
            float _Split_e3ce3329aa0241749340bad267461e3d_B_3 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[2];
            float _Split_e3ce3329aa0241749340bad267461e3d_A_4 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[3];
            float4 _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0 = float4(_Split_8c3721651c794447882328db6a8f0248_R_1, _Split_8c3721651c794447882328db6a8f0248_G_2, _Split_8c3721651c794447882328db6a8f0248_B_3, _Split_e3ce3329aa0241749340bad267461e3d_A_4);
            float4 _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3;
            Unity_Lerp_float4(_Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0, _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0, (_Split_e3ce3329aa0241749340bad267461e3d_A_4.xxxx), _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3);
            float4 Color_a6a852ac92dd45c9a9b8634c5a16ebb1 = IsGammaSpace() ? LinearToSRGB(float4(1.414214, 1.414214, 1.414214, 1)) : float4(1.414214, 1.414214, 1.414214, 1);
            float4 Color_a85e0b27fa824d8b9ea479e2d7b36710 = IsGammaSpace() ? LinearToSRGB(float4(0.8352941, 0.8352941, 0.8352941, 1)) : float4(0.8352941, 0.8352941, 0.8352941, 1);
            Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.uv0 = IN.uv0;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.TimeParameters = IN.TimeParameters;
            float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1;
            float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(_Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3, UnityBuildTexture2DStructNoScale(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865), float2 (0, -0.01), float(16), float(0.281), Color_a6a852ac92dd45c9a9b8634c5a16ebb1, 1, Color_a85e0b27fa824d8b9ea479e2d7b36710, float(0.5), float(100), float(1), _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2);
            surface.Alpha = _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _LINEAXIS_VERTICAL _LINEAXIS_HORIZONTAL
        
        
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SceneSelectionPass
        #define BUILTIN_TARGET_API 1
        #define SCENESELECTIONPASS 1
        #define _BUILTIN_AlphaClip 1
        #define _BUILTIN_ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865_TexelSize;
        float4 _Tex2_TexelSize;
        float4 _Tex1_TexelSize;
        float4 _Color2;
        float4 _Color1;
        float _HueShift1;
        float _HueShift2;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        SAMPLER(sampler_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        TEXTURE2D(_Tex2);
        SAMPLER(sampler_Tex2);
        TEXTURE2D(_Tex1);
        SAMPLER(sampler_Tex1);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        struct Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(float2 _Direction, float _Speed, float4 _UV, bool _UV_64c897cf98a949c9927cd64455f24906_IsConnected, float _TimeInput, bool _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected, Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float IN, out float4 Out_Vector4_1)
        {
        float4 _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 = _UV;
        bool _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected = _UV_64c897cf98a949c9927cd64455f24906_IsConnected;
        float4 _UV_0173adef6bf247fabb15a720b8644096_Out_0 = IN.uv0;
        float4 _BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3 = _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected ? _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 : _UV_0173adef6bf247fabb15a720b8644096_Out_0;
        float _Property_33a0a9f1077b47678210fb9952286561_Out_0 = _TimeInput;
        bool _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected = _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected;
        float _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3 = _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected ? _Property_33a0a9f1077b47678210fb9952286561_Out_0 : IN.TimeParameters.x;
        float2 _Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0 = float2(_BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3, _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3);
        float2 _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0 = _Direction;
        float2 _Multiply_e67992068fa44c91ba125735b34a4622_Out_2;
        Unity_Multiply_float2_float2(_Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0, _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0, _Multiply_e67992068fa44c91ba125735b34a4622_Out_2);
        float _Property_6f5d8a474bda493f8243c77249f83711_Out_0 = _Speed;
        float2 _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2;
        Unity_Multiply_float2_float2(_Multiply_e67992068fa44c91ba125735b34a4622_Out_2, (_Property_6f5d8a474bda493f8243c77249f83711_Out_0.xx), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2);
        float2 _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2;
        Unity_Add_float2((_BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3.xy), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2, _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2);
        Out_Vector4_1 = (float4(_Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2, 0.0, 1.0));
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        struct Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(float4 _InputColor, UnityTexture2D _MainTex, float2 _Scroll_Direction, float _Flicker_Speed, float _Flicker_Strength, float4 _Color, float _UseTextureForBackground, float4 _Background_Color, float _LineWidth, float _LineScale, float _LineStrength, Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float IN, out float4 Color_1, out float Alpha_2)
        {
        float _Property_65b2b8f6dbde4b54b39f7696550740df_Out_0 = _UseTextureForBackground;
        float4 _Property_a80087fbfb164298860f1e44ea199883_Out_0 = _Background_Color;
        float4 _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0 = _InputColor;
        float4 _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0 = _Color;
        float4 _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2;
        Unity_Multiply_float4_float4(_Property_74cfe5ea1308421daf51de4c77b8f924_Out_0, _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0, _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2);
        float _Split_0be4aef52f1f4793ad49df2680323816_R_1 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[0];
        float _Split_0be4aef52f1f4793ad49df2680323816_G_2 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[1];
        float _Split_0be4aef52f1f4793ad49df2680323816_B_3 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[2];
        float _Split_0be4aef52f1f4793ad49df2680323816_A_4 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[3];
        float _Split_0c085148d60e453baafce67a016ac860_R_1 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[0];
        float _Split_0c085148d60e453baafce67a016ac860_G_2 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[1];
        float _Split_0c085148d60e453baafce67a016ac860_B_3 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[2];
        float _Split_0c085148d60e453baafce67a016ac860_A_4 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[3];
        float4 _Vector4_50529047fe484555872e83056b5f6193_Out_0 = float4(_Split_0be4aef52f1f4793ad49df2680323816_R_1, _Split_0be4aef52f1f4793ad49df2680323816_G_2, _Split_0be4aef52f1f4793ad49df2680323816_B_3, _Split_0c085148d60e453baafce67a016ac860_A_4);
        float4 _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2;
        Unity_Multiply_float4_float4(_Property_a80087fbfb164298860f1e44ea199883_Out_0, _Vector4_50529047fe484555872e83056b5f6193_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2);
        float4 _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3;
        Unity_Branch_float4(_Property_65b2b8f6dbde4b54b39f7696550740df_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2, _Property_a80087fbfb164298860f1e44ea199883_Out_0, _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3);
        float _Split_ac54c6d9941745a2811f997c273cd93d_R_1 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[0];
        float _Split_ac54c6d9941745a2811f997c273cd93d_G_2 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[1];
        float _Split_ac54c6d9941745a2811f997c273cd93d_B_3 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[2];
        float _Split_ac54c6d9941745a2811f997c273cd93d_A_4 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[3];
        float _Property_960fa59466aa478296a3a6018cce0817_Out_0 = _Flicker_Speed;
        float _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_960fa59466aa478296a3a6018cce0817_Out_0, _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2);
        float _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1;
        Unity_Sine_float(_Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2, _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1);
        float _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1, _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3);
        float _Property_118264f782634e6a9ecb393ed44a4858_Out_0 = _Flicker_Strength;
        float _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2;
        Unity_Multiply_float_float(_Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3, _Property_118264f782634e6a9ecb393ed44a4858_Out_0, _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2);
        float _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1;
        Unity_OneMinus_float(_Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2, _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1);
        float2 _Property_e834c146a0434f51ab8db95f23d8b175_Out_0 = _Scroll_Direction;
        Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.uv0 = IN.uv0;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.TimeParameters = IN.TimeParameters;
        half4 _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1;
        SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(_Property_e834c146a0434f51ab8db95f23d8b175_Out_0, half(1), half4 (0, 0, 0, 0), false, half(0), false, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1);
        float _Property_0a947325d8b04b6ba499f20e953e3281_Out_0 = _LineScale;
        float2 _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3;
        Unity_TilingAndOffset_float((_ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1.xy), (_Property_0a947325d8b04b6ba499f20e953e3281_Out_0.xx), float2 (0, 0), _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3);
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_R_1 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[0];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_G_2 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[1];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_B_3 = 0;
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_A_4 = 0;
        float _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1;
        TriangleWave_float(_Split_f00e33bb6f03423dbddec083fd1c34cf_G_2, _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1);
        float _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1, _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3);
        float _Property_5e272a6466b348778d5ae159374ce824_Out_0 = _LineWidth;
        float _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2;
        Unity_Comparison_Greater_float(_Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3, _Property_5e272a6466b348778d5ae159374ce824_Out_0, _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2);
        float _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3;
        Unity_Branch_float(_Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2, float(1), float(0), _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3);
        float _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1;
        Unity_OneMinus_float(_Split_0c085148d60e453baafce67a016ac860_A_4, _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1);
        float _Multiply_b071f89d702142d4818427b50f62515b_Out_2;
        Unity_Multiply_float_float(_OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1, 2, _Multiply_b071f89d702142d4818427b50f62515b_Out_2);
        float _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2;
        Unity_Subtract_float(_Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3, _Multiply_b071f89d702142d4818427b50f62515b_Out_2, _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2);
        float _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1;
        Unity_OneMinus_float(_Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2, _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1);
        float _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0 = _LineStrength;
        float _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2;
        Unity_Multiply_float_float(_OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1, _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0, _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2);
        float _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1;
        Unity_OneMinus_float(_Multiply_887138d924ca464a95762f81c14a0bd8_Out_2, _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1);
        float _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1;
        Unity_Saturate_float(_OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1);
        float _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2;
        Unity_Multiply_float_float(_OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2);
        float _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2;
        Unity_Multiply_float_float(_Split_ac54c6d9941745a2811f997c273cd93d_A_4, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2, _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2);
        float4 _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Unity_Lerp_float4(_Branch_f99a5bfa769c4478b743c922f6795a33_Out_3, _Vector4_50529047fe484555872e83056b5f6193_Out_0, (_Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2.xxxx), _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3);
        float _Split_320e9328194e45608c30482e760fe577_R_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[0];
        float _Split_320e9328194e45608c30482e760fe577_G_2 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[1];
        float _Split_320e9328194e45608c30482e760fe577_B_3 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[2];
        float _Split_320e9328194e45608c30482e760fe577_A_4 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[3];
        Color_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Alpha_2 = _Split_320e9328194e45608c30482e760fe577_A_4;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_5d72a470fca746dd91928edad30f1efe_Out_0 = UnityBuildTexture2DStructNoScale(_Tex1);
            float4 _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5d72a470fca746dd91928edad30f1efe_Out_0.tex, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.samplerstate, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_R_4 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.r;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_G_5 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.g;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_B_6 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.b;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_A_7 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.a;
            float4 _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0 = _Color1;
            float4 _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0, _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0, _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2);
            float _Property_2880ccfc20224e1586c97f7d25889035_Out_0 = _HueShift1;
            float3 _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2;
            Unity_Hue_Normalized_float((_Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2.xyz), _Property_2880ccfc20224e1586c97f7d25889035_Out_0, _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2);
            float _Split_4191fe5768c04a0485ef7fdb05206c01_R_1 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[0];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_G_2 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[1];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_B_3 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[2];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_A_4 = 0;
            float _Split_b37e6b7460ec4f119273bae8f28c6377_R_1 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[0];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_G_2 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[1];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_B_3 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[2];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_A_4 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[3];
            float4 _Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0 = float4(_Split_4191fe5768c04a0485ef7fdb05206c01_R_1, _Split_4191fe5768c04a0485ef7fdb05206c01_G_2, _Split_4191fe5768c04a0485ef7fdb05206c01_B_3, _Split_b37e6b7460ec4f119273bae8f28c6377_A_4);
            UnityTexture2D _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0 = UnityBuildTexture2DStructNoScale(_Tex2);
            float4 _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.tex, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.samplerstate, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_R_4 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.r;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_G_5 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.g;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_B_6 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.b;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_A_7 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.a;
            float4 _Property_db6dfede820f41709370167cf6a8febd_Out_0 = _Color2;
            float4 _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0, _Property_db6dfede820f41709370167cf6a8febd_Out_0, _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2);
            float _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0 = _HueShift2;
            float3 _Hue_4c466a288837449cbfc61c06006e9350_Out_2;
            Unity_Hue_Normalized_float((_Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2.xyz), _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0, _Hue_4c466a288837449cbfc61c06006e9350_Out_2);
            float _Split_8c3721651c794447882328db6a8f0248_R_1 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[0];
            float _Split_8c3721651c794447882328db6a8f0248_G_2 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[1];
            float _Split_8c3721651c794447882328db6a8f0248_B_3 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[2];
            float _Split_8c3721651c794447882328db6a8f0248_A_4 = 0;
            float _Split_e3ce3329aa0241749340bad267461e3d_R_1 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[0];
            float _Split_e3ce3329aa0241749340bad267461e3d_G_2 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[1];
            float _Split_e3ce3329aa0241749340bad267461e3d_B_3 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[2];
            float _Split_e3ce3329aa0241749340bad267461e3d_A_4 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[3];
            float4 _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0 = float4(_Split_8c3721651c794447882328db6a8f0248_R_1, _Split_8c3721651c794447882328db6a8f0248_G_2, _Split_8c3721651c794447882328db6a8f0248_B_3, _Split_e3ce3329aa0241749340bad267461e3d_A_4);
            float4 _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3;
            Unity_Lerp_float4(_Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0, _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0, (_Split_e3ce3329aa0241749340bad267461e3d_A_4.xxxx), _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3);
            float4 Color_a6a852ac92dd45c9a9b8634c5a16ebb1 = IsGammaSpace() ? LinearToSRGB(float4(1.414214, 1.414214, 1.414214, 1)) : float4(1.414214, 1.414214, 1.414214, 1);
            float4 Color_a85e0b27fa824d8b9ea479e2d7b36710 = IsGammaSpace() ? LinearToSRGB(float4(0.8352941, 0.8352941, 0.8352941, 1)) : float4(0.8352941, 0.8352941, 0.8352941, 1);
            Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.uv0 = IN.uv0;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.TimeParameters = IN.TimeParameters;
            float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1;
            float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(_Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3, UnityBuildTexture2DStructNoScale(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865), float2 (0, -0.01), float(16), float(0.281), Color_a6a852ac92dd45c9a9b8634c5a16ebb1, 1, Color_a85e0b27fa824d8b9ea479e2d7b36710, float(0.5), float(100), float(1), _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2);
            surface.Alpha = _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 3.0
        #pragma multi_compile_instancing
        #pragma vertex vert
        #pragma fragment frag
        
        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>
        
        // Keywords
        // PassKeywords: <None>
        #pragma shader_feature_local _LINEAXIS_VERTICAL _LINEAXIS_HORIZONTAL
        
        
        
        // Defines
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS ScenePickingPass
        #define BUILTIN_TARGET_API 1
        #define SCENEPICKINGPASS 1
        #define _BUILTIN_AlphaClip 1
        #define _BUILTIN_ALPHATEST_ON 1
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
        #ifdef _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #define _SURFACE_TYPE_TRANSPARENT _BUILTIN_SURFACE_TYPE_TRANSPARENT
        #endif
        #ifdef _BUILTIN_ALPHATEST_ON
        #define _ALPHATEST_ON _BUILTIN_ALPHATEST_ON
        #endif
        #ifdef _BUILTIN_AlphaClip
        #define _AlphaClip _BUILTIN_AlphaClip
        #endif
        #ifdef _BUILTIN_ALPHAPREMULTIPLY_ON
        #define _ALPHAPREMULTIPLY_ON _BUILTIN_ALPHAPREMULTIPLY_ON
        #endif
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Shim/Shims.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/LegacySurfaceVertex.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865_TexelSize;
        float4 _Tex2_TexelSize;
        float4 _Tex1_TexelSize;
        float4 _Color2;
        float4 _Color1;
        float _HueShift1;
        float _HueShift2;
        CBUFFER_END
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        SAMPLER(sampler_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865);
        TEXTURE2D(_Tex2);
        SAMPLER(sampler_Tex2);
        TEXTURE2D(_Tex1);
        SAMPLER(sampler_Tex1);
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // Graph Functions
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Hue_Normalized_float(float3 In, float Offset, out float3 Out)
        {
            // RGB to HSV
            float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
            float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
            float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
            float D = Q.x - min(Q.w, Q.y);
            float E = 1e-10;
            float V = (D == 0) ? Q.x : (Q.x + E);
            float3 hsv = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), V);
        
            float hue = hsv.x + Offset;
            hsv.x = (hue < 0)
                    ? hue + 1
                    : (hue > 1)
                        ? hue - 1
                        : hue;
        
            // HSV to RGB
            float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
            Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);
        }
        
        void Unity_Lerp_float4(float4 A, float4 B, float4 T, out float4 Out)
        {
            Out = lerp(A, B, T);
        }
        
        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
        Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Smoothstep_float(float Edge1, float Edge2, float In, out float Out)
        {
            Out = smoothstep(Edge1, Edge2, In);
        }
        
        void Unity_OneMinus_float(float In, out float Out)
        {
            Out = 1 - In;
        }
        
        void Unity_Multiply_float2_float2(float2 A, float2 B, out float2 Out)
        {
        Out = A * B;
        }
        
        void Unity_Add_float2(float2 A, float2 B, out float2 Out)
        {
            Out = A + B;
        }
        
        struct Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(float2 _Direction, float _Speed, float4 _UV, bool _UV_64c897cf98a949c9927cd64455f24906_IsConnected, float _TimeInput, bool _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected, Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float IN, out float4 Out_Vector4_1)
        {
        float4 _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 = _UV;
        bool _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected = _UV_64c897cf98a949c9927cd64455f24906_IsConnected;
        float4 _UV_0173adef6bf247fabb15a720b8644096_Out_0 = IN.uv0;
        float4 _BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3 = _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0_IsConnected ? _Property_5b664f2a60a14ec18aeb92805a21259e_Out_0 : _UV_0173adef6bf247fabb15a720b8644096_Out_0;
        float _Property_33a0a9f1077b47678210fb9952286561_Out_0 = _TimeInput;
        bool _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected = _TimeInput_9a5fa11886364297b72dc1227b338501_IsConnected;
        float _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3 = _Property_33a0a9f1077b47678210fb9952286561_Out_0_IsConnected ? _Property_33a0a9f1077b47678210fb9952286561_Out_0 : IN.TimeParameters.x;
        float2 _Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0 = float2(_BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3, _BranchOnInputConnection_d032c6ae3c5f4012837bef49188d4f77_Out_3);
        float2 _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0 = _Direction;
        float2 _Multiply_e67992068fa44c91ba125735b34a4622_Out_2;
        Unity_Multiply_float2_float2(_Vector2_eba155cddd0641d09b0102cd4a8ae5e1_Out_0, _Property_a2f0d59fb8e149929cc5ed365c11a20d_Out_0, _Multiply_e67992068fa44c91ba125735b34a4622_Out_2);
        float _Property_6f5d8a474bda493f8243c77249f83711_Out_0 = _Speed;
        float2 _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2;
        Unity_Multiply_float2_float2(_Multiply_e67992068fa44c91ba125735b34a4622_Out_2, (_Property_6f5d8a474bda493f8243c77249f83711_Out_0.xx), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2);
        float2 _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2;
        Unity_Add_float2((_BranchOnInputConnection_c4a87b6082584d058f151186297e75d3_Out_3.xy), _Multiply_68510e94443c468baf3f04c4b3747b36_Out_2, _Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2);
        Out_Vector4_1 = (float4(_Add_80510b7ea7f94ad39c0c128fa15e5445_Out_2, 0.0, 1.0));
        }
        
        void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
        {
            Out = UV * Tiling + Offset;
        }
        
        void TriangleWave_float(float In, out float Out)
        {
            Out = 2.0 * abs( 2 * (In - floor(0.5 + In)) ) - 1.0;
        }
        
        void Unity_Comparison_Greater_float(float A, float B, out float Out)
        {
            Out = A > B ? 1 : 0;
        }
        
        void Unity_Branch_float(float Predicate, float True, float False, out float Out)
        {
            Out = Predicate ? True : False;
        }
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Saturate_float(float In, out float Out)
        {
            Out = saturate(In);
        }
        
        struct Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float
        {
        half4 uv0;
        float3 TimeParameters;
        };
        
        void SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(float4 _InputColor, UnityTexture2D _MainTex, float2 _Scroll_Direction, float _Flicker_Speed, float _Flicker_Strength, float4 _Color, float _UseTextureForBackground, float4 _Background_Color, float _LineWidth, float _LineScale, float _LineStrength, Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float IN, out float4 Color_1, out float Alpha_2)
        {
        float _Property_65b2b8f6dbde4b54b39f7696550740df_Out_0 = _UseTextureForBackground;
        float4 _Property_a80087fbfb164298860f1e44ea199883_Out_0 = _Background_Color;
        float4 _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0 = _InputColor;
        float4 _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0 = _Color;
        float4 _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2;
        Unity_Multiply_float4_float4(_Property_74cfe5ea1308421daf51de4c77b8f924_Out_0, _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0, _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2);
        float _Split_0be4aef52f1f4793ad49df2680323816_R_1 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[0];
        float _Split_0be4aef52f1f4793ad49df2680323816_G_2 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[1];
        float _Split_0be4aef52f1f4793ad49df2680323816_B_3 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[2];
        float _Split_0be4aef52f1f4793ad49df2680323816_A_4 = _Multiply_655d1aa00611415ca5f7a6babccb1603_Out_2[3];
        float _Split_0c085148d60e453baafce67a016ac860_R_1 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[0];
        float _Split_0c085148d60e453baafce67a016ac860_G_2 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[1];
        float _Split_0c085148d60e453baafce67a016ac860_B_3 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[2];
        float _Split_0c085148d60e453baafce67a016ac860_A_4 = _Property_74cfe5ea1308421daf51de4c77b8f924_Out_0[3];
        float4 _Vector4_50529047fe484555872e83056b5f6193_Out_0 = float4(_Split_0be4aef52f1f4793ad49df2680323816_R_1, _Split_0be4aef52f1f4793ad49df2680323816_G_2, _Split_0be4aef52f1f4793ad49df2680323816_B_3, _Split_0c085148d60e453baafce67a016ac860_A_4);
        float4 _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2;
        Unity_Multiply_float4_float4(_Property_a80087fbfb164298860f1e44ea199883_Out_0, _Vector4_50529047fe484555872e83056b5f6193_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2);
        float4 _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3;
        Unity_Branch_float4(_Property_65b2b8f6dbde4b54b39f7696550740df_Out_0, _Multiply_89b2341474a147c3aaae5dd80a863c1b_Out_2, _Property_a80087fbfb164298860f1e44ea199883_Out_0, _Branch_f99a5bfa769c4478b743c922f6795a33_Out_3);
        float _Split_ac54c6d9941745a2811f997c273cd93d_R_1 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[0];
        float _Split_ac54c6d9941745a2811f997c273cd93d_G_2 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[1];
        float _Split_ac54c6d9941745a2811f997c273cd93d_B_3 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[2];
        float _Split_ac54c6d9941745a2811f997c273cd93d_A_4 = _Property_3ba8edbe658e4978bedd46c47c3ef810_Out_0[3];
        float _Property_960fa59466aa478296a3a6018cce0817_Out_0 = _Flicker_Speed;
        float _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2;
        Unity_Multiply_float_float(IN.TimeParameters.x, _Property_960fa59466aa478296a3a6018cce0817_Out_0, _Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2);
        float _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1;
        Unity_Sine_float(_Multiply_64eacf3c6b734e3ab3be0184a0a23290_Out_2, _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1);
        float _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _Sine_73b2ca7dce0f4762855694b3836c3562_Out_1, _Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3);
        float _Property_118264f782634e6a9ecb393ed44a4858_Out_0 = _Flicker_Strength;
        float _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2;
        Unity_Multiply_float_float(_Smoothstep_b12ac2efc2ff409294878de2c78984a5_Out_3, _Property_118264f782634e6a9ecb393ed44a4858_Out_0, _Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2);
        float _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1;
        Unity_OneMinus_float(_Multiply_562c4ba668fc41fd9bbc4bd7a1f92b01_Out_2, _OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1);
        float2 _Property_e834c146a0434f51ab8db95f23d8b175_Out_0 = _Scroll_Direction;
        Bindings_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.uv0 = IN.uv0;
        _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813.TimeParameters = IN.TimeParameters;
        half4 _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1;
        SG_ScrollingUV_311ab9d079efdb74199b0f76c93b2f50_float(_Property_e834c146a0434f51ab8db95f23d8b175_Out_0, half(1), half4 (0, 0, 0, 0), false, half(0), false, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813, _ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1);
        float _Property_0a947325d8b04b6ba499f20e953e3281_Out_0 = _LineScale;
        float2 _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3;
        Unity_TilingAndOffset_float((_ScrollingUV_7b0c91dab4424251ad23fb6726dcc813_OutVector4_1.xy), (_Property_0a947325d8b04b6ba499f20e953e3281_Out_0.xx), float2 (0, 0), _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3);
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_R_1 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[0];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_G_2 = _TilingAndOffset_8efa999be9b946279ca927ff5577aa67_Out_3[1];
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_B_3 = 0;
        float _Split_f00e33bb6f03423dbddec083fd1c34cf_A_4 = 0;
        float _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1;
        TriangleWave_float(_Split_f00e33bb6f03423dbddec083fd1c34cf_G_2, _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1);
        float _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3;
        Unity_Smoothstep_float(float(-1), float(1), _TriangleWave_1e0022f9750f49618ff0e7a42a11d0a2_Out_1, _Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3);
        float _Property_5e272a6466b348778d5ae159374ce824_Out_0 = _LineWidth;
        float _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2;
        Unity_Comparison_Greater_float(_Smoothstep_f3dab1c6fd85487db609dca7db2e8586_Out_3, _Property_5e272a6466b348778d5ae159374ce824_Out_0, _Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2);
        float _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3;
        Unity_Branch_float(_Comparison_6249b9ac479041bab60b457bbdc124e7_Out_2, float(1), float(0), _Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3);
        float _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1;
        Unity_OneMinus_float(_Split_0c085148d60e453baafce67a016ac860_A_4, _OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1);
        float _Multiply_b071f89d702142d4818427b50f62515b_Out_2;
        Unity_Multiply_float_float(_OneMinus_b5d5a2399da24173b44bf6231b3b16ea_Out_1, 2, _Multiply_b071f89d702142d4818427b50f62515b_Out_2);
        float _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2;
        Unity_Subtract_float(_Branch_b5c41ec13f7f4e0582b0fdfdc0479998_Out_3, _Multiply_b071f89d702142d4818427b50f62515b_Out_2, _Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2);
        float _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1;
        Unity_OneMinus_float(_Subtract_bba01dac0dc54b5aa33f8486291b48b9_Out_2, _OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1);
        float _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0 = _LineStrength;
        float _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2;
        Unity_Multiply_float_float(_OneMinus_39c229b5940f4c7c9c8409ecef7019b1_Out_1, _Property_7182d93ae7064ef6910d9220f73bcd7a_Out_0, _Multiply_887138d924ca464a95762f81c14a0bd8_Out_2);
        float _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1;
        Unity_OneMinus_float(_Multiply_887138d924ca464a95762f81c14a0bd8_Out_2, _OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1);
        float _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1;
        Unity_Saturate_float(_OneMinus_a57a903e110d48879c4c906b49cae18b_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1);
        float _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2;
        Unity_Multiply_float_float(_OneMinus_bc62417254854c8e87b85fb11e79ac19_Out_1, _Saturate_338dadc1cd944593a5d2af2ec4e96085_Out_1, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2);
        float _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2;
        Unity_Multiply_float_float(_Split_ac54c6d9941745a2811f997c273cd93d_A_4, _Multiply_aee85e03c517406c94c75503cfae08e7_Out_2, _Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2);
        float4 _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Unity_Lerp_float4(_Branch_f99a5bfa769c4478b743c922f6795a33_Out_3, _Vector4_50529047fe484555872e83056b5f6193_Out_0, (_Multiply_9dc7446332a2467b817f34cff6e7035a_Out_2.xxxx), _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3);
        float _Split_320e9328194e45608c30482e760fe577_R_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[0];
        float _Split_320e9328194e45608c30482e760fe577_G_2 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[1];
        float _Split_320e9328194e45608c30482e760fe577_B_3 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[2];
        float _Split_320e9328194e45608c30482e760fe577_A_4 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3[3];
        Color_1 = _Lerp_6e711a7c29e24d61975f90ca33921f73_Out_3;
        Alpha_2 = _Split_320e9328194e45608c30482e760fe577_A_4;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
            float AlphaClipThreshold;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_5d72a470fca746dd91928edad30f1efe_Out_0 = UnityBuildTexture2DStructNoScale(_Tex1);
            float4 _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0 = SAMPLE_TEXTURE2D(_Property_5d72a470fca746dd91928edad30f1efe_Out_0.tex, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.samplerstate, _Property_5d72a470fca746dd91928edad30f1efe_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_R_4 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.r;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_G_5 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.g;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_B_6 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.b;
            float _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_A_7 = _SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0.a;
            float4 _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0 = _Color1;
            float4 _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_e9dd8b8ff9504efe97cd8fd4fc1d6f64_RGBA_0, _Property_c7fe4db9b31e4058a509455b4c3c26d3_Out_0, _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2);
            float _Property_2880ccfc20224e1586c97f7d25889035_Out_0 = _HueShift1;
            float3 _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2;
            Unity_Hue_Normalized_float((_Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2.xyz), _Property_2880ccfc20224e1586c97f7d25889035_Out_0, _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2);
            float _Split_4191fe5768c04a0485ef7fdb05206c01_R_1 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[0];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_G_2 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[1];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_B_3 = _Hue_1ab71e8ead5745f3811d15d8ed36770d_Out_2[2];
            float _Split_4191fe5768c04a0485ef7fdb05206c01_A_4 = 0;
            float _Split_b37e6b7460ec4f119273bae8f28c6377_R_1 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[0];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_G_2 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[1];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_B_3 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[2];
            float _Split_b37e6b7460ec4f119273bae8f28c6377_A_4 = _Multiply_990ee9b9fa6642a597c9b07128c18aa5_Out_2[3];
            float4 _Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0 = float4(_Split_4191fe5768c04a0485ef7fdb05206c01_R_1, _Split_4191fe5768c04a0485ef7fdb05206c01_G_2, _Split_4191fe5768c04a0485ef7fdb05206c01_B_3, _Split_b37e6b7460ec4f119273bae8f28c6377_A_4);
            UnityTexture2D _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0 = UnityBuildTexture2DStructNoScale(_Tex2);
            float4 _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0 = SAMPLE_TEXTURE2D(_Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.tex, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.samplerstate, _Property_a6f0e2a0c3d74679914cd62571516d55_Out_0.GetTransformedUV(IN.uv0.xy));
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_R_4 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.r;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_G_5 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.g;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_B_6 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.b;
            float _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_A_7 = _SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0.a;
            float4 _Property_db6dfede820f41709370167cf6a8febd_Out_0 = _Color2;
            float4 _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2;
            Unity_Multiply_float4_float4(_SampleTexture2D_52113c3ab62c4b92969a8802eb0fa2a2_RGBA_0, _Property_db6dfede820f41709370167cf6a8febd_Out_0, _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2);
            float _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0 = _HueShift2;
            float3 _Hue_4c466a288837449cbfc61c06006e9350_Out_2;
            Unity_Hue_Normalized_float((_Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2.xyz), _Property_d351b9f59bda4b3d9186587b7e1df42a_Out_0, _Hue_4c466a288837449cbfc61c06006e9350_Out_2);
            float _Split_8c3721651c794447882328db6a8f0248_R_1 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[0];
            float _Split_8c3721651c794447882328db6a8f0248_G_2 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[1];
            float _Split_8c3721651c794447882328db6a8f0248_B_3 = _Hue_4c466a288837449cbfc61c06006e9350_Out_2[2];
            float _Split_8c3721651c794447882328db6a8f0248_A_4 = 0;
            float _Split_e3ce3329aa0241749340bad267461e3d_R_1 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[0];
            float _Split_e3ce3329aa0241749340bad267461e3d_G_2 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[1];
            float _Split_e3ce3329aa0241749340bad267461e3d_B_3 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[2];
            float _Split_e3ce3329aa0241749340bad267461e3d_A_4 = _Multiply_e52118466aa449889211e7a3a4c7eb94_Out_2[3];
            float4 _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0 = float4(_Split_8c3721651c794447882328db6a8f0248_R_1, _Split_8c3721651c794447882328db6a8f0248_G_2, _Split_8c3721651c794447882328db6a8f0248_B_3, _Split_e3ce3329aa0241749340bad267461e3d_A_4);
            float4 _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3;
            Unity_Lerp_float4(_Vector4_02cee519b93e48ca80d3003431f12ab8_Out_0, _Vector4_22e14b4e900f4af8b821e3469751f33e_Out_0, (_Split_e3ce3329aa0241749340bad267461e3d_A_4.xxxx), _Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3);
            float4 Color_a6a852ac92dd45c9a9b8634c5a16ebb1 = IsGammaSpace() ? LinearToSRGB(float4(1.414214, 1.414214, 1.414214, 1)) : float4(1.414214, 1.414214, 1.414214, 1);
            float4 Color_a85e0b27fa824d8b9ea479e2d7b36710 = IsGammaSpace() ? LinearToSRGB(float4(0.8352941, 0.8352941, 0.8352941, 1)) : float4(0.8352941, 0.8352941, 0.8352941, 1);
            Bindings_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.uv0 = IN.uv0;
            _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded.TimeParameters = IN.TimeParameters;
            float4 _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1;
            float _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            SG_CathodeDisplay_b4bc5cea2d4171643b6c92ef29dae456_float(_Lerp_17b12023d38548bc889c6e6140e6ef8f_Out_3, UnityBuildTexture2DStructNoScale(_CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_MainTex_4176285865), float2 (0, -0.01), float(16), float(0.281), Color_a6a852ac92dd45c9a9b8634c5a16ebb1, 1, Color_a85e0b27fa824d8b9ea479e2d7b36710, float(0.5), float(100), float(1), _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Color_1, _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2);
            surface.Alpha = _CathodeDisplay_bf31ae567cd842dba6138ba8c94aaded_Alpha_2;
            surface.AlphaClipThreshold = float(0.5);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
            
        
        
        
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        void BuildAppDataFull(Attributes attributes, VertexDescription vertexDescription, inout appdata_full result)
        {
            result.vertex     = float4(attributes.positionOS, 1);
            result.tangent    = attributes.tangentOS;
            result.normal     = attributes.normalOS;
            result.texcoord   = attributes.uv0;
            result.vertex     = float4(vertexDescription.Position, 1);
            result.normal     = vertexDescription.Normal;
            result.tangent    = float4(vertexDescription.Tangent, 0);
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
        }
        
        void VaryingsToSurfaceVertex(Varyings varyings, inout v2f_surf result)
        {
            result.pos = varyings.positionCS;
            // World Tangent isn't an available input on v2f_surf
        
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogCoord = varyings.fogFactorAndVertexLight.x;
                COPY_TO_LIGHT_COORDS(result, varyings.fogFactorAndVertexLight.yzw);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(varyings, result);
        }
        
        void SurfaceVertexToVaryings(v2f_surf surfVertex, inout Varyings result)
        {
            result.positionCS = surfVertex.pos;
            // viewDirectionWS is never filled out in the legacy pass' function. Always use the value computed by SRP
            // World Tangent isn't an available input on v2f_surf
        
            #if UNITY_ANY_INSTANCING_ENABLED
            #endif
            #if !defined(LIGHTMAP_ON)
            #if UNITY_SHOULD_SAMPLE_SH
            #endif
            #endif
            #if defined(LIGHTMAP_ON)
            #endif
            #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                result.fogFactorAndVertexLight.x = surfVertex.fogCoord;
                COPY_FROM_LIGHT_COORDS(result.fogFactorAndVertexLight.yzw, surfVertex);
            #endif
        
            DEFAULT_UNITY_TRANSFER_VERTEX_OUTPUT_STEREO(surfVertex, result);
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/BuiltIn/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
        ENDHLSL
        }
    }
    CustomEditorForRenderPipeline "UnityEditor.Rendering.BuiltIn.ShaderGraph.BuiltInUnlitGUI" ""
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}