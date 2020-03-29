Shader "Unlit/HealthBarShader"
{
    Properties
    {
		_Health ("Health", Float) = 0.0
		_HealthyColor("Healty Color", Color) = (0.0, 1.0, 0.0, 1.0)
		_UnhealthyColor("Unhealthy Color", Color) = (1.0, 0.0, 0.0, 1.0)
		_BackgroundColor("Background", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

			float4 _HealthyColor;
			float4 _UnhealthyColor; 
			float4 _BackgroundColor;
			float _Health;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				
				fixed4 result;
				float u = 1.0 - i.uv.x;
				if (_Health < u) {
					result = _Health * _HealthyColor + (1.0 - _Health) * _UnhealthyColor;;
				}
				else {
					result = _BackgroundColor;
				}

				result *= smoothstep(0.0, 0.03, i.uv.x);
				result *= 1.0 - smoothstep(0.97, 1.0, i.uv.x);

				result *= smoothstep(0.0, 0.15, i.uv.y);
				result *= 1.0 - smoothstep(0.85, 1.0, i.uv.y);
				return result;
                
            }
            ENDCG
        }
    }
}
