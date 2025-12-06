Shader "miHoYo/Scene/Decal" {
	Properties {
		[Enum(Concrete,0,Grass,1,Sand,2,Water,3,Wood,4,Metal,5,Carpet,6)] _MaterialAudioType ("MaterialAudioType", Float) = 0
		_BaseColor ("_BaseColor", Vector) = (1,1,1,1)
		_BaseColorMap ("BaseColorMap", 2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_MaskMap ("MaskMap", 2D) = "red" {}
		_DecalBlend ("_DecalBlend", Range(0, 1)) = 0.5
		_Parallax ("Scale", Range(0.005, 0.08)) = 0.005
		_ParallaxMap ("Height Map", 2D) = "black" {}
		[Toggle(_EMISSIVE_FLOW)] _EnableEmissiveFlow ("Emissive flow@启用自发光Mask", Float) = 0
		_EmissiveFlowTex ("Emissive Flow@世界空间Flow效果", 2D) = "black" {}
		_EmissiveFlowSpeed ("Emissive Flow@速度", Vector) = (0,0,1,1)
		_POMRCSMap ("POM RSCMap", 2D) = "white" {}
		[Toggle(_ENABLE_TRACK)] _EnableTrack ("启用沙地雪地轨迹", Float) = 0
		_TrackParams ("轨迹(强度、范围)、闪光(光泽度、强度", Vector) = (1,0,1,1)
		_SparkleParams ("闪光(大小 密度)、闪光噪声 (强度、密度）", Vector) = (1,1,1,1)
		_SparkleMaskTex ("闪光噪声", 3D) = "" {}
		[ToggleUI] _AlbedoMode ("_AlbedoMode", Range(0, 1)) = 1
		[Toggle(_CLIP_WITH_NORMAL)] _ClipWithNormal ("切掉侧面的贴花", Float) = 0
		[Toggle(_REMOVE_MOIRE)] _RemoveMoire ("移除摩尔纹", Float) = 0
		_LODDistanceThreshold ("LOD距离阈值", Range(0, 5)) = 0.5
		_LODScale ("LOD缩放", Range(0.1, 2)) = 0.5
		[HideInInspector] _NormalBlendSrc ("_NormalBlendSrc", Float) = 1
		[HideInInspector] _MaskBlendSrc ("_MaskBlendSrc", Float) = 1
		[HideInInspector] _MaskBlendMode ("_MaskBlendMode", Float) = 4
		[ToggleUI] _MaskmapMetal ("_MaskmapMetal", Range(0, 1)) = 0
		[ToggleUI] _MaskmapAO ("_MaskmapAO", Range(0, 1)) = 0
		[ToggleUI] _MaskmapSmoothness ("_MaskmapSmoothness", Range(0, 1)) = 1
		[HideInInspector] _DecalMeshDepthBias ("_DecalMeshDepthBias", Float) = 0
		[Enum(Bottom0,0,Default10,10,Up20_Crack,20,UP25,25,UP_30_RodeMarking,30,UP50_WaterOrDust,50,UP60_DecalWithHighMap,60)] _NewDrawOrder ("_NewDrawOrder", Float) = 10
		[ToggleUI] _Emissive ("_Emissive", Range(0, 1)) = 0
		_EmissiveIntensity ("Emissive Intensity", Float) = 1
		[HideInInspector] _DecalFadeFactor ("_DecalFadeFactor", Float) = 0
		[HideInInspector] _DecalEmissionIntensity ("_DecalEmissionIntensity", Float) = 0
		[HideInInspector] _DecalNormalIntensity ("_DecalNormalIntensity", Float) = 0
		[HideInInspector] _DecalUVOffset ("_DecalUVOffset", Vector) = (0,0,0,0)
		[HideInInspector] _DecalStencilRef ("_DecalStencilRef", Float) = 16
		[HideInInspector] _DecalStencilWriteMask ("_DecalStencilWriteMask", Float) = 16
		[HideInInspector] _SmoothnessRemapMin ("SmoothnessRemapMin", Float) = 0
		[HideInInspector] _SmoothnessRemapMax ("SmoothnessRemapMax", Float) = 1
		[HideInInspector] _AORemapMin ("AORemapMin", Float) = 0
		[HideInInspector] _AORemapMax ("AORemapMax", Float) = 1
		[HideInInspector] _MetallicScale ("_MetallicScale", Range(0, 1)) = 1
		[HideInInspector] _DecalMaskMapBlueScale ("_DecalMaskMapBlueScale", Range(0, 1)) = 1
		_WetnessScale ("材质吸水性", Range(0, 1)) = 1
		[HideInInspector] _DitherAlpha ("DitherAlpha", Float) = 1
		[HideInInspector] _DitherAlpha2 ("DitherAlpha2", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	//CustomEditor "Rendering.Universal.DecalUI"
}