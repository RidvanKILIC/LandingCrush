Shader "Holistic/AdvancedOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color",Color) = (0,0,0,1)
        _MainColor("Main Color",Color) = (0,0,0,1)
        _Outline ("Outline Width", Range(.002,0.1)) = 0.005
    }
    SubShader
    {
       
        ///Outline
        ZWrite off
        CGPROGRAM

        #pragma surface surf Lambert vertex:vert


        sampler2D _MainTex;
        float _Outline;
        float4 _OutlineColor;
        float4 _MainColor;
        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout appdata_full v) {
            v.vertex.xyz += v.normal * _Outline;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            o.Emission = _OutlineColor.rgb;
        }
        ENDCG
        ///
            Cull Off
        CGPROGRAM
 
        #pragma surface surf Lambert


        sampler2D _MainTex;
        float4 _MainColor;
        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = (c.rgb * _MainColor.rgb);



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
                o.pos.xy += offset * o.pos.z * _Outline;
                o.color = _OutlineColor;
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
