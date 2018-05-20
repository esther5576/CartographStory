// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)
// Edited by MinionsArt for hologram effect

Shader "FX/Hologram FX " {
	Properties{
		_TintColor("Tint Color", Color) = (0,0.5,1,1)
		_RimColor("Rim Color", Color) = (0,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_GlitchTime("Glitches Over Time", Range(0.01,3.0)) = 1.0
		_WorldScale("Line Amount", Range(1,200)) = 20
			_LineWidth("Line Width", Range(0.01,1)) = 0.5
			_LinesGlow("Line Brightness", Range(-1,2)) = 0.5
			_GlitchSegment("Glitch Segment Width", Range(0.01,3)) = 0.2
			[Toggle(GRID)] _GRID("Enable Grid", Float) = 0
	}

		Category{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Sphere" "DisableBatching" = "True" }
		
		SubShader{
		Pass{
			ColorMask 0
			}
		Pass{
			Blend SrcAlpha OneMinusSrcAlpha

			Cull Back
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma shader_feature GRID

#include "UnityCG.cginc"

	sampler2D _MainTex;
	fixed4 _TintColor;
	fixed4 _RimColor;


	struct appdata_t {
		float4 vertex : POSITION;
		fixed4 color : COLOR;

		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL; // vertex normal
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord : TEXCOORD0;
		float3 projPos : TEXCOORD2;
		float3 wpos : TEXCOORD1; // worldposition
		float3 normalDir : TEXCOORD3; // normal direction for rimlighting

	};

	float4 _MainTex_ST;
	float _GlitchTime;
	float _WorldScale;
	float _OptTime = 0;
	float _LineWidth;
	float _GlitchSegment;
	float _LinesGlow;

	v2f vert(appdata_t v)
	{
		v2f o;

		o.vertex = UnityObjectToClipPos(v.vertex);
		o.projPos = ComputeScreenPos(o.vertex);

		// Vertex glitching
		_OptTime = _OptTime == 0 ? sin(_Time.w * _GlitchTime) : _OptTime;// optimisation
		float glitchtime = step(0.99, _OptTime); // returns 1 when sine is near top, otherwise returns 0;
		float glitchPos = v.vertex.y + _SinTime.y;// position on model
		float glitchPosClamped = step(0, glitchPos) * step(glitchPos, _GlitchSegment);// clamped segment of model
		o.vertex.xz += glitchPosClamped * 0.1 * glitchtime * _SinTime.y;// moving the vertices when glitchtime returns 1;

		
		o.color = v.color;
		o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);

		// world position and normal direction
		o.wpos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
		
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		float4 text = tex2D(_MainTex, i.texcoord) * _TintColor;// texture
		
		
		// rim lighting
		float3 viewDirection = _WorldSpaceCameraPos.xyz - i.wpos.xyz;
	
		half rim = 1.0 - saturate(dot(normalize(viewDirection), i.normalDir));// rimlight based on view and normal

		// small scanlines down
		float fraclines = frac((i.wpos.y * _WorldScale) + _Time.y);//small lines 
		float scanlines = step(fraclines, _LineWidth);// cut off based on 0.5
		// big scanline up
		float bigfracline = frac((i.wpos.y ) - _Time.x * 4);// big gradient line

		fixed4 col = text + (bigfracline * 0.2 * _TintColor) +(rim * _RimColor) + ((scanlines * _TintColor)* _LinesGlow);// end result color 
		col.a = 0.6 * (scanlines + rim + (bigfracline * 0.2));// alpha based on scanlines and rim
#if GRID
		float vertfraclines = frac(i.projPos.x * _WorldScale);//small lines 
		float vertscanlines = step(vertfraclines, _LineWidth);// cut off based on 0.5
		col = text + (bigfracline * 1 * _TintColor) + (rim * _RimColor) + ((scanlines * _TintColor) + (vertscanlines * _TintColor) * _LinesGlow);// end result color 
		col.a = 0.8 * (scanlines + rim + vertscanlines);// alpha based on scanlines and rim
		

#endif
		
	
		return col;
		}
		ENDCG
	}
	}
	}
}
