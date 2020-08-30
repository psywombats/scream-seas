Shader "SaGa/MainScreen"
{
	Properties
	{
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _AquaTex ("Aqua Displacement Map", 2D) = "white" {}
        _Elapsed("Elapsed", Range(0,1)) = 0.0
        _AquaElapsed("Aqua Elapsed", Range(0,20)) = 0.0
        _MultX("MultX", Range(1, 1000)) = 40.0
        _MultY("MultY", Range(1, 1000)) = 40.0
        _Power("Power", Range(0, 100)) = 6
        _Speed("Speed", Range(.1, 50)) = 1
        _Narrow("Narrow", Range(0, 1)) = .8
        _Widen("Widen", Range(0, 1)) = 0
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
                float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
                float4 color : COLOR;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
                o.color = v.color;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _AquaTex;
            float _Elapsed;
            float _AquaElapsed;
            float _MultX;
            float _MultY;
            float _Power;
            float _Speed;
            float _Narrow;
            float _Widen;

			fixed4 frag (v2f i) : SV_Target
			{
                float2 offset = float2(0.0, 0.0);
                float width = _ScreenParams.x / 2.0;
                float height = _ScreenParams.y / 2.0;
                float x = ceil(width * i.uv.x);
                float y = ceil(height * i.uv.y);
                
                float fromMidY = y - (height / 2.0);
                if (fromMidY < 0) fromMidY *= -1.0;
                float fromMidX = x - (width / 2.0);
                if (fromMidX < 0) fromMidX *= -1.0;
                float startMovingAt = (fromMidY / height);
                float stopMovingAt = startMovingAt + (fromMidX / width);
                
                _Elapsed *= 1.15;
                if (y % 4 >= 2) _Elapsed -= 0.15;
                
                if (_Elapsed > stopMovingAt) {
                    return fixed4(1.0, 0.0, 0.0, 0.0);
                }
                if (_Elapsed > startMovingAt) {
                    float offsetT = (_Elapsed - startMovingAt) * 2.0;
                    offset.x = offsetT * width / 2.0;
                    if (x > width / 2) offset.x *= -1.0;
                }
                
                if (_AquaElapsed > 0) {
                    float2 aquaUV = float2(x / width / 3, y / height);
                    aquaUV.x += _AquaElapsed * _Speed * 1.0/_MultX;
                    aquaUV.y += _AquaElapsed * _Speed * 1.0/_MultY;
                    float4 aquaMap = tex2D(_AquaTex, aquaUV);
                    offset.x += aquaMap[0] * _Power - _Power/2.0;
                    offset.y += aquaMap[1] * _Power - _Power/2.0;
                }
                
                float mult = 1.0 - (i.uv.y * _Narrow) + ((1.0 - i.uv.y) * _Widen);
                offset.y = offset.y * mult;
                offset.x = offset.x * mult;
                
                offset.x /= width;
                offset.y /= height;
                float4 color = tex2D(_MainTex, i.uv + offset);
                return color;
			}
			ENDCG
		}
	}
}
