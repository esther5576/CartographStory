// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Toon/Aquarel" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
			_WorldScale("WorldSCale", Range(.002, 20)) = 1
		_Offset ("Offset", Range (-1, 1)) = .005
		_RainTex ("Base (RGB)", 2D) = "white" {}
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp vertex:vert
#pragma target 4.0
sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
	c.a = 0;
	return c;
}


sampler2D _MainTex;
sampler2D _RainTex;
float4 _Color;
float _WorldScale;


float _Offset;
struct Input {
	float2 uv_MainTex : TEXCOORD0;
	float4 screenPos;
	float3 lightDir;
	
	float2 objectPos  : TEXCOORD2;
	
};
   void vert (inout appdata_full v, out Input o) {
            UNITY_INITIALIZE_OUTPUT(Input,o);
          o.lightDir = ObjSpaceLightDir(v.vertex);
		  o.objectPos = v.vertex.xyz;
		
        }

void surf (Input IN, inout SurfaceOutput o) {

  half d = dot (o.Normal, IN.lightDir) + _Offset ;
	half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	
	
	half3 r = tex2D(_RainTex, IN.objectPos * _WorldScale);
	
	o.Albedo = c.rgb + (r.rgb * _Offset);
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
}
