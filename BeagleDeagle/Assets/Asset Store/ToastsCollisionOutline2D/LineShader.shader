Shader "Unlit/LineShader"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;

			struct appdata
			{
				float4 vertex : POSITION;
                fixed4 color : COLOR;
				float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _TintColor;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				col *= i.color;
				return col;
			}
			ENDCG
		}
	}
}
