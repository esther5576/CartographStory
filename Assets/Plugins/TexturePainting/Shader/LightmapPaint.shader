

Shader "Custom/LightMap Diffuse" {

	Properties{
		_Tint("Tint", Color) = (1, 1, 1, .5)
		
		_MainTex("Texture 1", 2D) = "white" {}
	_NoiseTex("Texture Noise", 2D) = "white" {}
	_NoiseScale("Noise Scale", Range(0.0, 2.0)) = 0.9
		_PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
		_Slice("Slice Amount Start", Range(0.0, 2.0)) = 0
			_Slice2("Slice Amount End", Range(0.0, 2.0)) = 0.2
			[Toggle(TINTO)] _TINTO("Tint Only", Float) = 0
			[Toggle(BRUSHMAIN)] _BRUSHMAIN ("Brush on Main Texture", Float) =0
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase"  }

		Pass{
		
		Lighting Off

		CGPROGRAM
	
#pragma vertex vert
#pragma fragment frag
#pragma shader_feature TINTO
#pragma shader_feature BRUSHMAIN

#include "UnityCG.cginc"


	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
		float3  worldPos : TEXCOORD2;
	
		};

	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float2 texcoord1 : TEXCOORD1;

	};

	
	float4 _Tint, _Tint2;
	sampler2D _PaintMap, _NoiseTex;
	sampler2D _MainTex;
	float4 _MainTex_ST; 
	float _Slice;
	float _Slice2, _NoiseScale;
	v2f vert(appdata v) {
		v2f o;
		
		o.pos = UnityObjectToClipPos(v.vertex);
		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);

		
		o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	
		return o;
	}

	half4 frag(v2f o) : COLOR{
		half4 main_color = tex2D(_MainTex, o.uv0); // main texture
		half4 paint = (tex2D(_PaintMap, o.uv1)); // painted on texture
		half4 noise = tex2D(_NoiseTex, o.worldPos.xz * _NoiseScale); // noise
		
		noise *= paint;
		float4 cutoff = 1 - step(_Slice, noise.r) * step(noise.r, _Slice2);
#if BRUSHMAIN
		main_color *= paint; // 
#endif
		half4 final = lerp(paint * _Tint, main_color, cutoff);
#if TINTO
		final = lerp(_Tint,main_color, cutoff );
#endif
		return final;
	}
		ENDCG
	}
	}
}