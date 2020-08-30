Shader "Sprites/Alpha"
{
	Properties
	{
		[PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Alpha("Alpha", Float) = 1.0
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
        Blend SrcAlpha OneMinusSrcAlpha

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
            float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
                float4 color = tex2D(_MainTex, i.uv);
                color.a *= _Alpha;
                color.rgb *= color.a;
                return color;
			}
			ENDCG
		}
	}
}
