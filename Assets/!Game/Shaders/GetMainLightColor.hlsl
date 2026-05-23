#ifndef GETLIGHT_INCLUDED
#define GETLIGHT_INCLUDED

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
//#include "UnityLightingCommon.cginc"

void GetSun_float(out float3 lightDir, out float3 color)
{
#if SHADERGRAPH_PREVIEW
    lightDir = float3(0.707, 0.707, 0);
    color = 1;
#else
    
        lightDir = _WorldSpaceLightPos0;
        color = _LightColor0;
        
#endif
}

#endif