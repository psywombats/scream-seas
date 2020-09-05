Shader "Seas/Static"
{
	Properties
	{
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Elapsed("Elapsed", Range(0,1)) = 0.0
        _MultX("MultX", Range(0, 1)) = 0.0
        _MultY("MultY", Range(0, 1)) = 0.0
	}
	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
    
		Cull Off 
        ZTest Always
        ZWrite Off
        Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
                float4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
				o.uv = v.uv;
                o.color = v.color;
				return o;
			}
			
			sampler2D _MainTex;
            float _Elapsed;
            float _MultX;
            float _MultY;
            
            float rand2(float seed1, float seed2) {
                return frac(sin(dot(float2(seed1, seed2), float2(12.9898, 78.233))) * 43758.5453);
            }
            
            float rand3(float seed1, float seed2, float seed3) {
                return frac(sin(dot(float3(seed1, seed2, seed3), float3(45.5432, 12.9898, 78.233))) * 43758.5453);
            }
            
            // same as interval, except it should covary based on a given seed
            float intervalR(float source, float interval, float seed) {
                float stagger = rand2(seed, _Elapsed);
                if (interval == 0) return source;
                float result = ((float)((int)((source + stagger) * (1.0/interval)))) * interval - stagger;
                return clamp(result, 0.0, 1.0);
            }
            
            // argument is in range 0-1
            // but we need to clamp it to say, 0.0, 0.2, 0.4 etc for 1/5 chunks
            float interval(float source, float interval) {
                return intervalR(source, interval, 12.34);
            }

			fixed4 frag (v2f i) : SV_Target
			{
                float2 offset = float2(0.0, 0.0);
                float width = _ScreenParams.x / 2.0;
                float height = _ScreenParams.y / 2.0;
                float x = ceil(width * i.uv.x);
                float y = ceil(height * i.uv.y);
                
                float4 color = tex2D(_MainTex, i.uv);
                float x2 = intervalR(i.screenPos.x / width, _MultX, _Elapsed);
                float y2 = intervalR(i.screenPos.y / height, _MultY, _Elapsed);
                if (color[0] > 0 || color[1] > 0 || color[2] > 0) {
                    float roll = rand3(_Elapsed, x2, y2);
                    if (roll > 0.5 * color[3]) {
                        color[0] = 0;
                        color[1] = 0;
                        color[2] = 0;
                    } else {
                        color[0] = 1;
                        color[1] = 1;
                        color[2] = 1;
                    }
                }
                
                
                return color;
			}
			ENDCG
		}
	}
}
