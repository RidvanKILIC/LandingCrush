Shader "Holistic/ToonRamp(Light)" {
	Properties{
			_Colour("Colour", Color) = (1,1,1,1)
			_RampTex("Ramp Texture", 2D) = "white"{}
			_DiffTex("Diffuce Texture",2D) = "white"{}
			_OutlineColor("Outline Color",Color) = (0,0,0,1)
			_Outline("Outline Width", Range(.002,0.1)) = 0.005
	}
		SubShader{

			CGPROGRAM
			#pragma surface surf ToonRamp

			float4 _Colour;
			sampler2D _RampTex;
			sampler2D _DiffTex;

			half4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten) {

				float diff = dot(s.Normal, lightDir);

				float h = diff * 0.5 + 0.5;
				
				float2 rh = h;
				float3 ramp = tex2D(_RampTex, rh).rgb ;

				float4 c;
				c.rgb = s.Albedo * _LightColor0.rgb * _Colour.rgb * (ramp) ;
				c.a = s.Alpha;
				return c;
			}



			struct Input {
				float2 uv_MainTex;
				float2 uv_DiffTex;
				float3 viewDir;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				//o.Albedo = tex2D(_RampTex, IN.uv_MainTex).rgb + _Colour.rgb;// Just Test
				float diff = dot(o.Normal, IN.viewDir);
				float h = diff * 0.5 + 0.5;
				float2 rh = h;
				o.Albedo = (tex2D((_RampTex), rh)).rgb;
			}
			ENDCG

				Pass{

			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata {
				float3 vertex : POSITION;
				float3 normal : NORMAL;
			 };

			struct v2f {
				float4 pos : SV_POSITION;
				fixed4 color : COLOR;
			};

			float _Outline;
			float4 _OutlineColor;

			v2f vert(appdata v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
				float2 offset = TransformViewToProjection(norm.xy);
				//o.pos.xy += offset * o.pos.z * _Outline;
				//o.color = _OutlineColor;
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				return i.color;
			}
			ENDCG
			}
	}
		FallBack "Diffuse"

}