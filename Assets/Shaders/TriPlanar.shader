Shader "Tri-Planar World" {

	Properties{
		_Side("Side", 2D) = "white" {}
	_Top("Top", 2D) = "white" {}
	_Bottom("Bottom", 2D) = "white" {}
	_BumpMap("BumpMap", 2D) = "bump" {}
	}

		SubShader{

		Tags{
		"Queue" = "Geometry"
		"IgnoreProjector" = "False"
		"RenderType" = "Opaque"
	}

		Cull Back
		ZWrite On

		CGPROGRAM

#pragma surface surf Standard
#pragma exclude_renderers flash
#pragma shader_feature _NORMALMAP_ON

		sampler2D _Side, _Top, _Bottom, _BumpMap;

	struct Input {
		float3 worldPos;
		float3 worldNormal;
#if _NORMALMAP_ON
		INTERNAL_DATA
#endif
	};

	void surf(Input IN, inout SurfaceOutputStandard o) {
		float3 projNormal = saturate(pow(IN.worldNormal * 1.4, 4));

		// SIDE X
		float3 x = tex2D(_Side, frac(IN.worldPos.zy * 0.7)) * abs(IN.worldNormal.x);

		// TOP / BOTTOM
		float3 y = 0;
		float absny = abs(IN.worldNormal.y);
		float2 fraczx = frac(IN.worldPos.zx * 0.7);
		if (IN.worldNormal.y > 0) {
			y = tex2D(_Top, fraczx) * absny;
		}
		else {
			y = tex2D(_Bottom, fraczx) * absny;
		}

		// SIDE Z
		float3 z = tex2D(_Side, frac(IN.worldPos.xy * 0.7)) * abs(IN.worldNormal.z);

#if _NORMALMAP_ON
		o.Normal = UnpackNormal(tex2D(_BumpMap, fraczx) * absny);
#endif
		o.Albedo = z;
		o.Albedo = lerp(o.Albedo, x, projNormal.x);
		o.Albedo = lerp(o.Albedo, y, projNormal.y);
	}

	ENDCG
	}
		Fallback "Diffuse"
}