Shader "Flicker"
{
    Properties
    {
        _Color("Primary", Color) = (0, 0, 0, 0)
        _Secondary("Secondary", Color) = (0, 0, 0, 0)
        _FPS("FPS", Range(1, 60)) = 20
        [NoScaleOffset]_MainTex("MainTex", 2D) = "white" {}
        [HideInInspector]_BUILTIN_QueueOffset("Float", Float) = 0
        [HideInInspector]_BUILTIN_QueueControl("Float", Float) = -1
    }
        SubShader
        {
            Tags
            {
                // RenderPipeline: <None>
                "RenderType" = "Transparent"
                "BuiltInMaterialType" = "Unlit"
                "Queue" = "Transparent"
                "ShaderGraphShader" = "true"
                "ShaderGraphTargetId" = "BuiltInUnlitSubTarget"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
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
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
            ZTest Always
            ZWrite Off
            ColorMask RGB

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
            // GraphKeywords: <None>

            // Defines
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD0
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_UNLIT
            #define BUILTIN_TARGET_API 1
            #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
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
                 float4 interp0 : INTERP0;
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

            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                ZERO_INITIALIZE(PackedVaryings, output);
                output.positionCS = input.positionCS;
                output.interp0.xyzw = input.texCoord0;
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

            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp0.xyzw;
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
            float4 _Color;
            float4 _Secondary;
            float _FPS;
            float4 _MainTex_TexelSize;
            CBUFFER_END

                // Object and Global properties
                SAMPLER(SamplerState_Linear_Repeat);
                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float _UnscaledTime;

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

                void Unity_Multiply_float_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }

                void Unity_Sine_float(float In, out float Out)
                {
                    Out = sin(In);
                }

                void Unity_Comparison_GreaterOrEqual_float(float A, float B, out float Out)
                {
                    Out = A >= B ? 1 : 0;
                }

                void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                {
                    Out = Predicate ? True : False;
                }

                void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A * B;
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
                };

                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    UnityTexture2D _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
                    float4 _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0 = SAMPLE_TEXTURE2D(_Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.tex, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.samplerstate, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.GetTransformedUV(IN.uv0.xy));
                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_R_4 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.r;
                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_G_5 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.g;
                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_B_6 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.b;
                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.a;
                    float _Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0 = _UnscaledTime;
                    float _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0 = _FPS;
                    float _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2;
                    Unity_Multiply_float_float(_Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0, _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0, _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2);
                    float _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1;
                    Unity_Sine_float(_Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2, _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1);
                    float _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2;
                    Unity_Comparison_GreaterOrEqual_float(_Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1, 0.5, _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2);
                    float4 _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0 = _Color;
                    float4 _Property_79591a7a21494b91a8f4ccba5c975222_Out_0 = _Secondary;
                    float4 _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3;
                    Unity_Branch_float4(_Comparison_67a5a900cd104e74a8311391c775f10f_Out_2, _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0, _Property_79591a7a21494b91a8f4ccba5c975222_Out_0, _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3);
                    float4 _Multiply_9690581097414331a6ccbaea8cfbad37_Out_2;
                    Unity_Multiply_float4_float4(_SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0, _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3, _Multiply_9690581097414331a6ccbaea8cfbad37_Out_2);
                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_R_1 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[0];
                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_G_2 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[1];
                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_B_3 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[2];
                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[3];
                    float _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                    Unity_Multiply_float_float(_SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7, _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4, _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2);
                    surface.BaseColor = (_Multiply_9690581097414331a6ccbaea8cfbad37_Out_2.xyz);
                    surface.Alpha = _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                    return surface;
                }

                // --------------------------------------------------
                // Build Graph Inputs

                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                    output.ObjectSpaceNormal = input.normalOS;
                    output.ObjectSpaceTangent = input.tangentOS.xyz;
                    output.ObjectSpacePosition = input.positionOS;

                    return output;
                }
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







                    output.uv0 = input.texCoord0;
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
                    result.vertex = float4(attributes.positionOS, 1);
                    result.tangent = attributes.tangentOS;
                    result.normal = attributes.normalOS;
                    result.texcoord = attributes.uv0;
                    result.vertex = float4(vertexDescription.Position, 1);
                    result.normal = vertexDescription.Normal;
                    result.tangent = float4(vertexDescription.Tangent, 0);
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
                    Name "ShadowCaster"
                    Tags
                    {
                        "LightMode" = "ShadowCaster"
                    }

                    // Render State
                    Cull Back
                    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
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
                    // GraphKeywords: <None>

                    // Defines
                    #define ATTRIBUTES_NEED_NORMAL
                    #define ATTRIBUTES_NEED_TANGENT
                    #define ATTRIBUTES_NEED_TEXCOORD0
                    #define VARYINGS_NEED_TEXCOORD0
                    #define FEATURES_GRAPH_VERTEX
                    /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                    #define SHADERPASS SHADERPASS_SHADOWCASTER
                    #define BUILTIN_TARGET_API 1
                    #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
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
                         float4 interp0 : INTERP0;
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

                    PackedVaryings PackVaryings(Varyings input)
                    {
                        PackedVaryings output;
                        ZERO_INITIALIZE(PackedVaryings, output);
                        output.positionCS = input.positionCS;
                        output.interp0.xyzw = input.texCoord0;
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

                    Varyings UnpackVaryings(PackedVaryings input)
                    {
                        Varyings output;
                        output.positionCS = input.positionCS;
                        output.texCoord0 = input.interp0.xyzw;
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
                    float4 _Color;
                    float4 _Secondary;
                    float _FPS;
                    float4 _MainTex_TexelSize;
                    CBUFFER_END

                        // Object and Global properties
                        SAMPLER(SamplerState_Linear_Repeat);
                        TEXTURE2D(_MainTex);
                        SAMPLER(sampler_MainTex);
                        float _UnscaledTime;

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

                        void Unity_Multiply_float_float(float A, float B, out float Out)
                        {
                            Out = A * B;
                        }

                        void Unity_Sine_float(float In, out float Out)
                        {
                            Out = sin(In);
                        }

                        void Unity_Comparison_GreaterOrEqual_float(float A, float B, out float Out)
                        {
                            Out = A >= B ? 1 : 0;
                        }

                        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                        {
                            Out = Predicate ? True : False;
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
                        };

                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                        {
                            SurfaceDescription surface = (SurfaceDescription)0;
                            UnityTexture2D _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
                            float4 _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0 = SAMPLE_TEXTURE2D(_Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.tex, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.samplerstate, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.GetTransformedUV(IN.uv0.xy));
                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_R_4 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.r;
                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_G_5 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.g;
                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_B_6 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.b;
                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.a;
                            float _Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0 = _UnscaledTime;
                            float _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0 = _FPS;
                            float _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2;
                            Unity_Multiply_float_float(_Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0, _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0, _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2);
                            float _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1;
                            Unity_Sine_float(_Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2, _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1);
                            float _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2;
                            Unity_Comparison_GreaterOrEqual_float(_Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1, 0.5, _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2);
                            float4 _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0 = _Color;
                            float4 _Property_79591a7a21494b91a8f4ccba5c975222_Out_0 = _Secondary;
                            float4 _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3;
                            Unity_Branch_float4(_Comparison_67a5a900cd104e74a8311391c775f10f_Out_2, _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0, _Property_79591a7a21494b91a8f4ccba5c975222_Out_0, _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3);
                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_R_1 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[0];
                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_G_2 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[1];
                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_B_3 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[2];
                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[3];
                            float _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                            Unity_Multiply_float_float(_SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7, _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4, _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2);
                            surface.Alpha = _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                            return surface;
                        }

                        // --------------------------------------------------
                        // Build Graph Inputs

                        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                        {
                            VertexDescriptionInputs output;
                            ZERO_INITIALIZE(VertexDescriptionInputs, output);

                            output.ObjectSpaceNormal = input.normalOS;
                            output.ObjectSpaceTangent = input.tangentOS.xyz;
                            output.ObjectSpacePosition = input.positionOS;

                            return output;
                        }
                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                        {
                            SurfaceDescriptionInputs output;
                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







                            output.uv0 = input.texCoord0;
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
                            result.vertex = float4(attributes.positionOS, 1);
                            result.tangent = attributes.tangentOS;
                            result.normal = attributes.normalOS;
                            result.texcoord = attributes.uv0;
                            result.vertex = float4(vertexDescription.Position, 1);
                            result.normal = vertexDescription.Normal;
                            result.tangent = float4(vertexDescription.Tangent, 0);
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
                            // GraphKeywords: <None>

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
                            #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
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
                                 float4 interp0 : INTERP0;
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

                            PackedVaryings PackVaryings(Varyings input)
                            {
                                PackedVaryings output;
                                ZERO_INITIALIZE(PackedVaryings, output);
                                output.positionCS = input.positionCS;
                                output.interp0.xyzw = input.texCoord0;
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

                            Varyings UnpackVaryings(PackedVaryings input)
                            {
                                Varyings output;
                                output.positionCS = input.positionCS;
                                output.texCoord0 = input.interp0.xyzw;
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
                            float4 _Color;
                            float4 _Secondary;
                            float _FPS;
                            float4 _MainTex_TexelSize;
                            CBUFFER_END

                                // Object and Global properties
                                SAMPLER(SamplerState_Linear_Repeat);
                                TEXTURE2D(_MainTex);
                                SAMPLER(sampler_MainTex);
                                float _UnscaledTime;

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

                                void Unity_Multiply_float_float(float A, float B, out float Out)
                                {
                                    Out = A * B;
                                }

                                void Unity_Sine_float(float In, out float Out)
                                {
                                    Out = sin(In);
                                }

                                void Unity_Comparison_GreaterOrEqual_float(float A, float B, out float Out)
                                {
                                    Out = A >= B ? 1 : 0;
                                }

                                void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                                {
                                    Out = Predicate ? True : False;
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
                                };

                                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                {
                                    SurfaceDescription surface = (SurfaceDescription)0;
                                    UnityTexture2D _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
                                    float4 _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0 = SAMPLE_TEXTURE2D(_Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.tex, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.samplerstate, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.GetTransformedUV(IN.uv0.xy));
                                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_R_4 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.r;
                                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_G_5 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.g;
                                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_B_6 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.b;
                                    float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.a;
                                    float _Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0 = _UnscaledTime;
                                    float _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0 = _FPS;
                                    float _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2;
                                    Unity_Multiply_float_float(_Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0, _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0, _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2);
                                    float _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1;
                                    Unity_Sine_float(_Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2, _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1);
                                    float _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2;
                                    Unity_Comparison_GreaterOrEqual_float(_Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1, 0.5, _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2);
                                    float4 _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0 = _Color;
                                    float4 _Property_79591a7a21494b91a8f4ccba5c975222_Out_0 = _Secondary;
                                    float4 _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3;
                                    Unity_Branch_float4(_Comparison_67a5a900cd104e74a8311391c775f10f_Out_2, _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0, _Property_79591a7a21494b91a8f4ccba5c975222_Out_0, _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3);
                                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_R_1 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[0];
                                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_G_2 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[1];
                                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_B_3 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[2];
                                    float _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[3];
                                    float _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                                    Unity_Multiply_float_float(_SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7, _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4, _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2);
                                    surface.Alpha = _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                                    return surface;
                                }

                                // --------------------------------------------------
                                // Build Graph Inputs

                                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                {
                                    VertexDescriptionInputs output;
                                    ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                    output.ObjectSpaceNormal = input.normalOS;
                                    output.ObjectSpaceTangent = input.tangentOS.xyz;
                                    output.ObjectSpacePosition = input.positionOS;

                                    return output;
                                }
                                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                {
                                    SurfaceDescriptionInputs output;
                                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







                                    output.uv0 = input.texCoord0;
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
                                    result.vertex = float4(attributes.positionOS, 1);
                                    result.tangent = attributes.tangentOS;
                                    result.normal = attributes.normalOS;
                                    result.texcoord = attributes.uv0;
                                    result.vertex = float4(vertexDescription.Position, 1);
                                    result.normal = vertexDescription.Normal;
                                    result.tangent = float4(vertexDescription.Tangent, 0);
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
                                    // GraphKeywords: <None>

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
                                    #define _BUILTIN_SURFACE_TYPE_TRANSPARENT 1
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
                                         float4 interp0 : INTERP0;
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

                                    PackedVaryings PackVaryings(Varyings input)
                                    {
                                        PackedVaryings output;
                                        ZERO_INITIALIZE(PackedVaryings, output);
                                        output.positionCS = input.positionCS;
                                        output.interp0.xyzw = input.texCoord0;
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

                                    Varyings UnpackVaryings(PackedVaryings input)
                                    {
                                        Varyings output;
                                        output.positionCS = input.positionCS;
                                        output.texCoord0 = input.interp0.xyzw;
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
                                    float4 _Color;
                                    float4 _Secondary;
                                    float _FPS;
                                    float4 _MainTex_TexelSize;
                                    CBUFFER_END

                                        // Object and Global properties
                                        SAMPLER(SamplerState_Linear_Repeat);
                                        TEXTURE2D(_MainTex);
                                        SAMPLER(sampler_MainTex);
                                        float _UnscaledTime;

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

                                        void Unity_Multiply_float_float(float A, float B, out float Out)
                                        {
                                            Out = A * B;
                                        }

                                        void Unity_Sine_float(float In, out float Out)
                                        {
                                            Out = sin(In);
                                        }

                                        void Unity_Comparison_GreaterOrEqual_float(float A, float B, out float Out)
                                        {
                                            Out = A >= B ? 1 : 0;
                                        }

                                        void Unity_Branch_float4(float Predicate, float4 True, float4 False, out float4 Out)
                                        {
                                            Out = Predicate ? True : False;
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
                                        };

                                        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                                        {
                                            SurfaceDescription surface = (SurfaceDescription)0;
                                            UnityTexture2D _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
                                            float4 _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0 = SAMPLE_TEXTURE2D(_Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.tex, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.samplerstate, _Property_76b52b88c8ec4ff592c16af6ace71071_Out_0.GetTransformedUV(IN.uv0.xy));
                                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_R_4 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.r;
                                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_G_5 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.g;
                                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_B_6 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.b;
                                            float _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7 = _SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_RGBA_0.a;
                                            float _Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0 = _UnscaledTime;
                                            float _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0 = _FPS;
                                            float _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2;
                                            Unity_Multiply_float_float(_Property_f2b6f2d64ffc47359b4de228d9f18d7b_Out_0, _Property_a72eebac33a24f489d6d1aa1b2dcf7c9_Out_0, _Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2);
                                            float _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1;
                                            Unity_Sine_float(_Multiply_a15fa031a54041bb98dad0dd22dbe87b_Out_2, _Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1);
                                            float _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2;
                                            Unity_Comparison_GreaterOrEqual_float(_Sine_f6bc3a222c5c4ef698a21093988e5a85_Out_1, 0.5, _Comparison_67a5a900cd104e74a8311391c775f10f_Out_2);
                                            float4 _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0 = _Color;
                                            float4 _Property_79591a7a21494b91a8f4ccba5c975222_Out_0 = _Secondary;
                                            float4 _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3;
                                            Unity_Branch_float4(_Comparison_67a5a900cd104e74a8311391c775f10f_Out_2, _Property_d624e5afbaf94f6082176c46d1a036d9_Out_0, _Property_79591a7a21494b91a8f4ccba5c975222_Out_0, _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3);
                                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_R_1 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[0];
                                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_G_2 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[1];
                                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_B_3 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[2];
                                            float _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4 = _Branch_6c172683bb524e7e91f2833a3ef03e08_Out_3[3];
                                            float _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                                            Unity_Multiply_float_float(_SampleTexture2D_de0c8aa172354de9ac9f5bed03aaa426_A_7, _Split_7bf45e6734ce4ac79f058c120c1d7e08_A_4, _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2);
                                            surface.Alpha = _Multiply_add3cad5e0d84e7a9daaaa82f34fbda3_Out_2;
                                            return surface;
                                        }

                                        // --------------------------------------------------
                                        // Build Graph Inputs

                                        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                                        {
                                            VertexDescriptionInputs output;
                                            ZERO_INITIALIZE(VertexDescriptionInputs, output);

                                            output.ObjectSpaceNormal = input.normalOS;
                                            output.ObjectSpaceTangent = input.tangentOS.xyz;
                                            output.ObjectSpacePosition = input.positionOS;

                                            return output;
                                        }
                                        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                                        {
                                            SurfaceDescriptionInputs output;
                                            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







                                            output.uv0 = input.texCoord0;
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
                                            result.vertex = float4(attributes.positionOS, 1);
                                            result.tangent = attributes.tangentOS;
                                            result.normal = attributes.normalOS;
                                            result.texcoord = attributes.uv0;
                                            result.vertex = float4(vertexDescription.Position, 1);
                                            result.normal = vertexDescription.Normal;
                                            result.tangent = float4(vertexDescription.Tangent, 0);
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