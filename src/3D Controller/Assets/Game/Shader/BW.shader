Shader "Hidden/BW" {
	Properties {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_bwBlend("Black & White blend", Range(0, 1)) = 0
	}
	SubShader {
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform float _bwBlend;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f_img i) : COLOR {
				float4 c = tex2D(_MainTex, i.uv);

				float lum = c.r * .3 + c.g * .59 + c.b * .11;
				float gamma = 1;
				float v = c.r + c.g + c.b % 255;
				float3 bw = float3(v, v, v);

				float4 result = c;
				result.rgb = bw;
				return result;
			}
			ENDCG
		}
	}
}