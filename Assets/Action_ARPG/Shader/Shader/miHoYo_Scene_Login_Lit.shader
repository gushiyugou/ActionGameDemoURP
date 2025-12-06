Shader "miHoYo/Scene/Login_Lit" {
	Properties {
		[Enum(Concrete,0,Grass,1,Sand,2,Water,3,Wood,4,Metal,5,Carpet,6)] _MaterialAudioType ("MaterialAudioType", Float) = 0
		[HideInInspector] _WorkflowMode ("WorkflowMode", Float) = 1
		_BaseColor ("Color", Vector) = (1,1,1,1)
		_BaseMap ("Albedo", 2D) = "white" {}
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
		_Smoothness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
		_MetallicGlossMap ("Mask map", 2D) = "red" {}
		_SpecColor ("Specular", Vector) = (0.2,0.2,0.2,1)
		_SpecGlossMap ("Specular", 2D) = "white" {}
		_SpecularHighlights ("Specular Highlights", Float) = 1
		_EnvironmentReflections ("Environment Reflections", Float) = 1
		_StringMode ("绳索模式", Float) = 0
		_BumpScale ("Scale", Float) = 1
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Parallax ("Scale", Range(0.005, 0.08)) = 0.005
		_ParallaxMap ("Height Map", 2D) = "black" {}
		_POMRCSMap ("POM RSCMap", 2D) = "white" {}
		_BaseColorContribution ("BaseColor贡献系数，只对烘焙有效", Range(0, 1)) = 1
		_OcclusionStrength ("Strength", Range(0, 1)) = 1
		_OcclusionMap ("Occlusion", 2D) = "white" {}
		_WetnessNormalAffection ("控制在潮湿环境下法线受影响的程度", Range(0, 1)) = 1
		_FlareId ("Flare ID", Range(0, 63)) = 0
		[HideInInspector] _BaseColorArr ("BaseColorTexArr", 2DArray) = "" {}
		[HideInInspector] _MRCArr ("MRCTexArr", 2DArray) = "" {}
		[HideInInspector] _NormalArr ("NormalTexArr", 2DArray) = "" {}
		[Toggle(_AFFECT_BY_GLOBAL_EMISSION_COLOR)] _AffectByGlobalEmissionColor ("受全局Config影响", Float) = 0
		[HDR] _EmissionColor ("Color", Vector) = (0,0,0,1)
		_EmissionMap ("Emission", 2D) = "white" {}
		[ToggleUI] _UseUV3ForEmission ("使用第三套UV采样Mask图的自发光通道", Float) = 0
		[ToggleUI] _UseUV3ForDetail ("使用第三套UV", Float) = 0
		[Enum(Ether, 0,  Common, 1, CoverTop, 2, GlobalCoverTop, 3)] _DetailLayerType ("Detail层类型", Float) = 0
		[Enum(UV0, 0,  UV3, 2)] _DetailUVType ("使用第三套UV采样Mask图的自发光通道", Float) = 0
		_DetailLayerMask ("Detail Layer Mask", 2D) = "white" {}
		_DetailAlbedoMap ("Detail Albedo", 2D) = "black" {}
		_DetailIntensity ("Detail Intensity", Range(0, 2)) = 1
		_DetailMREMap ("Detail Mask Map", 2D) = "black" {}
		[HDR] _DetailEmissionColor ("Detail Emission Color", Vector) = (0,0,0,1)
		[Normal] _DetailNormalMap ("Detail Normal Map", 2D) = "bump" {}
		_DetailNormalMapScale ("Detail Normal Map Scale", Range(0, 2)) = 1
		[Enum(Overlay, 0, Blend, 1)] _DetailBlendMode ("Detail Blend Mode", Float) = 0
		_DetailNormalClipThreshold ("Detail Coverage", Range(0, 1)) = 0.5
		_DetailNormalClipFade ("Detail Edge Fade", Range(0, 1)) = 0.5
		_DetailGlobalSettingIntensity ("Detail Global Intensity", Range(0, 1)) = 0
		_DiffuseTwo ("DetailAlbedo", 2D) = "black" {}
		_LayerMask ("DetailMask", 2D) = "black" {}
		_LayerNormalClipThreshold ("Detail Coverage", Range(0, 1)) = 0.5
		_LayerNormalClipFade ("Detail Edge Fade", Range(0, 1)) = 0.5
		_DiffuseTwoScale ("DetailAlbedo Scale", Range(0, 2)) = 1
		[Normal] _NormalMapTwo ("Detail Normal Map", 2D) = "bump" {}
		_NormalMapTwoScale ("Detail Normal Map Scale", Range(0, 2)) = 1
		[ToggleUI] _RainTexIgnoreRTS ("雨天贴图使用模型坐标采样", Float) = 0
		_Wind ("wind", Float) = 0
		_WindParam1 ("WindParam", Vector) = (1.57,0,-1,1.4)
		_WindParam2 ("WindParam2", Vector) = (3,1,0,1)
		_WindParam3 ("WindParam3", Vector) = (4,0.03,0,0)
		_WindParam4 ("WindParam4", Vector) = (0,1,0,0)
		[HDR] _ScreenBaseColor ("ScreenBaseColor", Vector) = (1,1,1,1)
		[SingleLine(false)] _ScreenMap ("ScreenBaseMap", 2D) = "black" {}
		[SingleLine(false)] _ScreenMaskTex ("ScreenMaskMap", 2D) = "white" {}
		[SingleLine(false)] _ScreenBumpMap ("ScreenBumpMap", 2D) = "bump" {}
		[Vector4] _CustomScreenMap_ST ("Tiling Offset 1", Vector) = (1,1,0,0)
		[HideInInspector] _Surface ("__surface", Float) = 0
		[HideInInspector] _Blend ("__blend", Float) = 0
		[HideInInspector] _AlphaClip ("__clip", Float) = 0
		[HideInInspector] _SrcBlend ("__src", Float) = 1
		[HideInInspector] _DstBlend ("__dst", Float) = 0
		[HideInInspector] _ZWrite ("__zw", Float) = 1
		[HideInInspector] _Cull ("__cull", Float) = 2
		[HideInInspector] _Stencil ("__Stencil", Float) = 8
		[HideInInspector] _QueueOffset ("Queue offset", Float) = 0
		_GlossMapScale ("Smoothness Scale", Range(0, 1)) = 1
		_SmoothnessTextureChannel ("Smoothness texture channel", Float) = 0
		[HideInInspector] _MainTex ("BaseMap", 2D) = "white" {}
		[HideInInspector] _Color ("Base Color", Vector) = (1,1,1,1)
		[HideInInspector] _GlossMapScale ("Smoothness", Float) = 0
		[HideInInspector] _Glossiness ("Smoothness", Float) = 0
		[HideInInspector] _GlossyReflections ("EnvironmentReflections", Float) = 0
		[HideInInspector] _DitherAlpha ("DitherAlpha", Float) = 1
		[HideInInspector] _DitherAlpha2 ("DitherAlpha2", Float) = 1
		[HideInInspector] _ForceDoubleSideGI ("烘焙时强制启用双面渲染", Float) = 0
		_Glitch ("Glitch", Float) = 0
		_BlockMaskTex ("Color Blocks Mask", 2D) = "black" {}
		[HDR] _BlockColorA ("Color A", Vector) = (1,0,0,1)
		[HDR] _BlockColorB ("Color B", Vector) = (0.5,1,0,1)
		[HDR] _BlockColorC ("Color C", Vector) = (0,1,1,1)
		[HDR] _BlockColorD ("Color D", Vector) = (0.5,0,1,1)
		[ToggleUI] _SecondaryEmission ("次级发光", Float) = 0
		_SecondaryEmissionTex ("次级发光贴图", 2D) = "white" {}
		[Vector2] _SecondaryEmissionTexSpeed ("流动速度", Vector) = (0,0,0,0)
		_SecondaryEmissionTexRotation ("Rotation", Range(0, 1)) = 0
		[HDR] _SecondaryEmissionColor ("Emission Color", Vector) = (1,1,1,1)
		[ToggleUI] _SecondaryEmissionUseUV2 ("使用UV2", Float) = 0
		[Enum(R,0,RGB,1)] _SecondaryEmissionChannel ("发光通道", Float) = 0
		[ToggleUI] _MultiplyAlbedo ("使用主贴图颜色", Float) = 1
		_SecondaryEmissionMaskTex ("遮罩贴图", 2D) = "white" {}
		[Enum(R,0,G,1,B,2)] _SecondaryEmissionMaskChannel ("遮罩通道", Float) = 0
		[ToggleUI(_RIGID_ANIMATION_ON)] _RigidAnimation ("Rigid Animation", Float) = 0
		[NoScaleOffset] _PositionTexture ("Position Texture", 2D) = "linearGray" {}
		[NoScaleOffset] _RotationTexture ("Rotation Texture", 2D) = "black" {}
		[Vector3] _BoundaryMin ("Boundary Min", Vector) = (-1,-1,-1,1)
		[Vector3] _BoundaryMax ("Boundary Max", Vector) = (1,1,1,1)
		_Frame ("Frame", Range(0, 256)) = 0
		_LastFrame ("Last Frame", Range(0, 256)) = 0
		[Enum(Clamp,0, Repeat,1)] _WrapMode ("Wrap Mode", Float) = 0
		[ToggleUI(_VERTEX_ANIMATION_ON)] _VertexAnimation ("Vertex Animation", Float) = 0
		[NoScaleOffset] _VertexPositionTex ("Vertex Position", 2D) = "black" {}
		[NoScaleOffset] _VertexNormalTex ("Vertex Normal", 2D) = "bump" {}
		_CurrentFrame ("Current Frame", Range(0, 255)) = 0
		_VAT_LastFrame ("Last Frame", Range(0, 255)) = 0
		_Frames ("Frames", Range(1, 256)) = 24
		_Transition ("Transition", Float) = 0
		_TransitionCompletion ("Transition Completion", Range(0, 1)) = 0
		_TransitionTex ("Transition Tex", 2D) = "white" {}
		_TransitionWidth ("Transition Width", Range(0, 1)) = 0
		[HDR] _TransitionColor ("Transition Color", Vector) = (1,1,1,1)
		_LocalGIIntensity ("Local GI Intensity", Float) = 1
		_LayerIDColor ("LayerIDColor", Vector) = (0,0,0,1)
		[IntRange] _SceneStencil ("Stencil", Range(0, 255)) = 32
		_SoftEdgeParams ("_SoftEdgeParams", Vector) = (0.25,1,0,0)
		[HideInInspector] _WorldTwistSpecificIntensity ("WorldTwistSpecificIntensity", Range(0, 1)) = 1
		[HideInInspector] _WorldTwistNoiseWeight ("WorldTwistNoiseWeight", Range(0, 1)) = 0
		_EnableSpecularAA ("EnableSpecularAA", Float) = 0
		_SpecularAAScreenSpaceVariance ("SpecularAAScreenSpaceVariance", Range(0, 1)) = 1
		_SpecularAAThreshold ("SpecularAAThreshold", Range(0, 1)) = 1
		_EnableBuildingBottomLight ("EnableBuildingBottomLight", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;
			float4 _Color;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy) * _Color;
			}

			ENDHLSL
		}
	}
	Fallback "Hidden/Universal Render Pipeline/FallbackError"
	//CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.LitShader"
}