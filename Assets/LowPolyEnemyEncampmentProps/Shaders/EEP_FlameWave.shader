// EEP_FlameWave.shader - Low Poly Enemy Encampment Props: animated low-poly flame driven by vertex displacement
// (URP 17 / Unity 6, SRP Batcher compatible). Used by MAT_EEP_Flame and MAT_EEP_FlameCore.
//
// Fragment = the same path URP/Lit uses for these materials (metallic from _MetallicGlossMap,
// emission from _EmissionMap x _EmissionColor, opaque, no normal map): with _Amplitude = _RiseScale = 0 the pixel is
// identical to URP/Lit with the same maps.
// The mesh NEEDS the flame vertex color (every flame mesh in this pack has it); on a mesh without it the whole object sways.
// Vertex = side-to-side wave + vertical stretch in WORLD SPACE, weighted by the mesh vertex color
// (written into every flame mesh of this pack): R = relative height within the tongue (0 base -> 1 tip) - G = per-tongue phase (0-1)
//                                     B = tongue height in meters (scale: candle and campfire share the same material)
// The base (R = 0) never moves; the weight grows with R^_BaseLock. The waves travel up the tongue (phase R*freq - time).
// All temporal frequencies are multiples of _Speed/3: the motion repeats every 3/_Speed s.
// _EEPTime (global, not a material property): when > 0 it replaces _Time.y - for frame capture only.
Shader "EEP/FlameWave"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        _MetallicGlossMap("Metallic (R) Smoothness (A)", 2D) = "white" {}
        _Metallic("Metallic (no map)", Range(0,1)) = 0
        _Smoothness("Smoothness (x map alpha)", Range(0,1)) = 1
        _EmissionMap("Emission Map", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (2,2,2,1)

        [Header(Flame motion)]
        _Amplitude("Amplitude (x tongue height)", Range(0, 0.4)) = 0.09
        _Speed("Speed (loop = 3 / Speed s)", Range(0, 4)) = 1
        _Frequency("Frequency (waves per tongue)", Range(0, 4)) = 0.9
        _RiseScale("Rise Scale (stretch x tongue height)", Range(0, 0.4)) = 0.10
        _BaseLock("Base Lock (weight exponent)", Range(0.5, 4)) = 1.6
        _InstancePhase("Instance Phase (per-copy phase; 0 = all in sync)", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "UniversalMaterialType" = "Lit" "IgnoreProjector" = "True" "DisableBatching" = "False" }
        LOD 300

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _BaseColor;
            half4 _EmissionColor;
            half _Metallic;
            half _Smoothness;
            float _Amplitude;
            float _Speed;
            float _Frequency;
            float _RiseScale;
            float _BaseLock;
            float _InstancePhase;
        CBUFFER_END

        TEXTURE2D(_BaseMap);          SAMPLER(sampler_BaseMap);
        TEXTURE2D(_MetallicGlossMap); SAMPLER(sampler_MetallicGlossMap);
        TEXTURE2D(_EmissionMap);      SAMPLER(sampler_EmissionMap);

        float _EEPTime;   // capture-only global (Shader.SetGlobalFloat); 0 = normal time

        #define EEP_TAU 6.28318530718

        // 3D hash -> [0,1) using only mul/add/frac ("hash without sine", D. Hoskins; constants chosen by testing): gives the
        // same value on every GPU (sin() of a large argument differs between vendors) and is not linear in position, so
        // copies placed in a row do not fall into phase at any regular spacing.
        float EEPHash13(float3 p)
        {
            p = frac(p * float3(0.0997, 0.1081, 0.1309));
            p += dot(p, p.zyx + 28.75);
            return frac((p.x + p.y) * p.z);
        }

        // World-space displacement. positionWS: vertex already in world space; c: flame vertex color.
        float3 FlameDisplace(float3 positionWS, float4 c)
        {
            float t = _EEPTime > 0 ? _EEPTime : _Time.y;
            float R = saturate(c.r);
            float H = max(c.b, 0.01);
            float3 org = GetObjectToWorldMatrix()._m03_m13_m23;              // object origin: per-copy variation
            float ph = c.g * EEP_TAU + _InstancePhase * EEP_TAU * EEPHash13(org);                       // phase
            float rot = _InstancePhase * EEP_TAU * EEPHash13(org + float3(17.31, 5.13, 9.71));          // sway axis rotation
            float s = t * _Speed / 3.0;
            float w = pow(max(R, 1e-4), _BaseLock);
            float f = _Frequency;
            float a1 = EEP_TAU * (R * f - 2 * s) + ph;
            float a2 = EEP_TAU * (R * f * 1.7 - 5 * s) + ph * 2.0 + 1.3;
            float a3 = EEP_TAU * (R * f * 0.8 - 3 * s) + ph * 1.5 + 0.4;
            float a4 = EEP_TAU * (R * f * 1.3 - 4 * s) + ph * 3.0 + 2.1;
            float sx = 0.65 * sin(a1) + 0.35 * sin(a2);
            float sz = 0.65 * sin(a3) + 0.35 * sin(a4);
            float cr = cos(rot), sr = sin(rot);
            float2 sw = float2(cr * sx - sr * sz, sr * sx + cr * sz);
            float ry = 0.6 * sin(EEP_TAU * 3 * s + ph * 1.7) + 0.4 * sin(EEP_TAU * 7 * s + ph * 2.9);
            float3 o = float3(sw.x * _Amplitude * H * w, ry * _RiseScale * H * R, sw.y * _Amplitude * H * w);
            return positionWS + o;
        }
        ENDHLSL

        // ------------------------------------------------------------------ forward (same math as URP/Lit)
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForwardOnly" }
            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex FlameVert
            #pragma fragment FlameFrag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ EVALUATE_SH_MIXED EVALUATE_SH_VERTEX
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BLENDING
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_BOX_PROJECTION
            #pragma multi_compile_fragment _ _REFLECTION_PROBE_ATLAS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile_fragment _ _SCREEN_SPACE_IRRADIANCE
            #pragma multi_compile_fragment _ _LIGHT_COOKIES
            #pragma multi_compile _ _LIGHT_LAYERS
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fragment _ REFLECTION_PROBE_ROTATION
            #pragma multi_compile _ USE_LEGACY_LIGHTMAPS
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Fog.hlsl"
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ProbeVolumeVariants.hlsl"
            #pragma multi_compile_instancing
            #pragma instancing_options renderinglayer

            #define _METALLICSPECGLOSSMAP 1
            #define _EMISSION 1
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float4 color      : COLOR;
                float2 texcoord   : TEXCOORD0;
                float2 staticLightmapUV : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv         : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 normalWS   : TEXCOORD2;
            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                half4 fogFactorAndVertexLight : TEXCOORD5;
            #else
                half fogFactor    : TEXCOORD5;
            #endif
                DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
            #ifdef USE_APV_PROBE_OCCLUSION
                float4 probeOcclusion : TEXCOORD10;
            #endif
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings FlameVert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                float3 positionWS = FlameDisplace(TransformObjectToWorld(input.positionOS.xyz), input.color);
                float4 positionCS = TransformWorldToHClip(positionWS);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                half3 vertexLight = VertexLighting(positionWS, normalWS);
                half fogFactor = 0;
            #if !defined(_FOG_FRAGMENT)
                fogFactor = ComputeFogFactor(positionCS.z);
            #endif
                output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
                output.normalWS = normalWS;
                OUTPUT_LIGHTMAP_UV(input.staticLightmapUV, unity_LightmapST, output.staticLightmapUV);
                OUTPUT_SH4(positionWS, output.normalWS.xyz, GetWorldSpaceNormalizeViewDir(positionWS), output.vertexSH, output.probeOcclusion);
            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                output.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
            #else
                output.fogFactor = fogFactor;
            #endif
                output.positionWS = positionWS;
                output.positionCS = positionCS;
                return output;
            }

            void FlameFrag(Varyings input, out half4 outColor : SV_Target0
            #ifdef _WRITE_RENDERING_LAYERS
                , out uint outRenderingLayers : SV_Target1
            #endif
            )
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                // SurfaceData = InitializeStandardLitSurfaceData (metallic map, no normal/occlusion map, opaque)
                half4 albedoAlpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                half4 specGloss = SAMPLE_TEXTURE2D(_MetallicGlossMap, sampler_MetallicGlossMap, input.uv);
                specGloss.a *= _Smoothness;
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb;
                surfaceData.alpha = 1.0;
                surfaceData.metallic = specGloss.r;
                surfaceData.specular = half3(0, 0, 0);
                surfaceData.smoothness = specGloss.a;
                surfaceData.normalTS = half3(0, 0, 1);
                surfaceData.occlusion = 1.0;
                surfaceData.emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, input.uv).rgb * _EmissionColor.rgb;

                InputData inputData = (InputData)0;
                inputData.positionWS = input.positionWS;
                inputData.normalWS = NormalizeNormalPerPixel(input.normalWS);
                inputData.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
            #if defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
            #else
                inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif
            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactorAndVertexLight.x);
                inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
            #else
                inputData.fogCoord = InitializeInputDataFog(float4(input.positionWS, 1.0), input.fogFactor);
            #endif
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);

            #if defined(_SCREEN_SPACE_IRRADIANCE)
                inputData.bakedGI = SAMPLE_GI(_ScreenSpaceIrradiance, input.positionCS.xy);
            #elif !defined(LIGHTMAP_ON) && (defined(PROBE_VOLUMES_L1) || defined(PROBE_VOLUMES_L2))
                inputData.bakedGI = SAMPLE_GI(input.vertexSH, GetAbsolutePositionWS(inputData.positionWS), inputData.normalWS,
                    inputData.viewDirectionWS, input.positionCS.xy, input.probeOcclusion, inputData.shadowMask);
            #else
                inputData.bakedGI = SAMPLE_GI(input.staticLightmapUV, input.vertexSH, inputData.normalWS);
                inputData.shadowMask = SAMPLE_SHADOWMASK(input.staticLightmapUV);
            #endif

                half4 color = UniversalFragmentPBR(inputData, surfaceData);
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                color.a = 1.0;
                outColor = color;
            #ifdef _WRITE_RENDERING_LAYERS
                outRenderingLayers = EncodeMeshRenderingLayer();
            #endif
            }
            ENDHLSL
        }

        // ------------------------------------------------------------------ depth (same displacement)
        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }
            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex DepthVert
            #pragma fragment DepthFrag
            #pragma multi_compile_instancing

            struct Attributes { float4 positionOS : POSITION; float4 color : COLOR; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; UNITY_VERTEX_INPUT_INSTANCE_ID UNITY_VERTEX_OUTPUT_STEREO };

            Varyings DepthVert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformWorldToHClip(FlameDisplace(TransformObjectToWorld(input.positionOS.xyz), input.color));
                return output;
            }
            half DepthFrag(Varyings input) : SV_TARGET { return input.positionCS.z; }
            ENDHLSL
        }

        Pass
        {
            Name "DepthNormalsOnly"
            Tags { "LightMode" = "DepthNormalsOnly" }
            ZWrite On
            Cull Back

            HLSLPROGRAM
            #pragma target 2.0
            #pragma vertex DNVert
            #pragma fragment DNFrag
            #pragma multi_compile_instancing
            #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/RenderingLayers.hlsl"

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; float4 color : COLOR; UNITY_VERTEX_INPUT_INSTANCE_ID };
            struct Varyings { float4 positionCS : SV_POSITION; float3 normalWS : TEXCOORD0; UNITY_VERTEX_INPUT_INSTANCE_ID UNITY_VERTEX_OUTPUT_STEREO };

            Varyings DNVert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformWorldToHClip(FlameDisplace(TransformObjectToWorld(input.positionOS.xyz), input.color));
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }
            void DNFrag(Varyings input, out half4 outNormalWS : SV_Target0
            #ifdef _WRITE_RENDERING_LAYERS
                , out uint outRenderingLayers : SV_Target1
            #endif
            )
            {
                outNormalWS = half4(NormalizeNormalPerPixel(input.normalWS), 0.0);
            #ifdef _WRITE_RENDERING_LAYERS
                outRenderingLayers = EncodeMeshRenderingLayer();
            #endif
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
