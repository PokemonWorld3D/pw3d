// StandInGeoShader for use with stand-in geometry. Basically a simple unlit texture with alpha and a fade parameter
// to make the stand-in object transparent when a player is close by.
//
// By Paul "PVC" Van Camp

Shader "Unlit/StandInGeoShader" {
	Properties
	{
		// Main
		_Fade ("Fade", Range(0,1)) = 0
		_MainTex ("Texture (RGBA)", 2D) = "black" {}
	}
	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	
		CGPROGRAM
		#pragma surface surf Lambert alpha
	
		sampler2D _MainTex;
		uniform float _Fade;
	
		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			o.Albedo= 0;
			float4 c= tex2D(_MainTex, IN.uv_MainTex);
			
			o.Emission= c.rgb;
			o.Alpha= c.a * ( 1- min( 1, _Fade));
		}
		
		ENDCG
	}
	FallBack "Transparent/VertexLit"
}