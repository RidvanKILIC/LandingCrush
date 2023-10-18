// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Holistic/Shadow" {
Properties{
		_Color("Main Color", Color) = (1,1,1,.5)
		_ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.6

	}
	SubShader{
		Tags{
			"Queue" = "Transparent"
		}


		CGPROGRAM
		#pragma surface surf Lambert alpha:fade

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
        
	FallBack "Diffuse"

}
