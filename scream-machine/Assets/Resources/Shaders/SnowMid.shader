Shader "Erebus/SnowMid" {
    Properties {
    		//_Thickness = Thickness texture (invert normals, bake AO).
		//_Power = "Sharpness" of translucent glow.
		//_Distortion = Subsurface distortion, shifts surface normal, effectively a refractive index.
		//_Scale = Multiplier for translucent glow - should be per-light, really.
		//_SubColor = Subsurface colour.
		_Power ("Subsurface Power", Float) = 1.0
		_Distortion ("Subsurface Distortion", Float) = 0.0
		_Scale ("Subsurface Scale", Float) = 0.5
		_SubColor ("Subsurface Color", Color) = (1.0, 1.0, 1.0, 1.0)
    
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [PerRendererData] _Color ("Tint", Color) = (1,1,1,1)
        [PerRendererData] _Flash ("Flash", Color) = (1,1,1,0)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        [PerRendererData] _Alpha("Alpha", Float) = 1.0
    }
    
    SubShader {
    
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Lighting Off
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Translucent vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "Glitch.cginc"
        
        float _ResolutionX;
        float _ResolutionY;
        float _Alpha;
        float _Desaturation;
        fixed4 _Flash;
        
        float _Scale, _Power, _Distortion;
		fixed4 _SubColor;
		half _Shininess;

        struct Input {
            float2 uv_MainTex;
            fixed4 color;
            float2 texcoord : TEXCOORD0;
            float2 screenPos: TEXCOORD2;
        };

        void vert(inout appdata_full v, out Input o) {
            v.vertex.xy *= _Flip.xy;

            #if defined(PIXELSNAP_ON)
            v.vertex = UnityPixelSnap (v.vertex);
            #endif

            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
            o.color.a = o.color.a * _Alpha;
        }

        void surf(Input IN, inout SurfaceOutput o) {
            float2 xy = IN.uv_MainTex;
            fixed4 c = SampleSpriteTexture(xy) * IN.color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Albedo *= o.Alpha;
        }
        
        inline fixed4 LightingTranslucent(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten) {		
			// You can remove these two lines,
			// to save some instructions. They're just
			// here for visual fidelity.
			viewDir = normalize ( viewDir );
			lightDir = normalize ( lightDir );
 
			// Translucency.
			half3 transLightDir = lightDir + s.Normal * _Distortion;
			float transDot = pow ( max (0, dot ( viewDir, -transLightDir ) ), _Power ) * _Scale;
			fixed3 transLight = (atten * 2) * ( transDot ) * s.Alpha * _SubColor.rgb;
            if (transLight[0] > 1.0) transLight[0] = 1.0;
            if (transLight[1] > 1.0) transLight[1] = 1.0;
            if (transLight[2] > 1.0) transLight[2] = 1.0;
			fixed3 transAlbedo = s.Albedo * _LightColor0.rgb * transLight;
 
			// Regular BlinnPhong.
			half3 h = normalize (lightDir + viewDir);
			fixed diff = max (0, dot (s.Normal, lightDir));
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;
			fixed3 diffAlbedo = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten * 2);
 
			// Add the two together.
			fixed4 c;
			c.rgb = diffAlbedo + transAlbedo;
            if (c.r > 1.0) c.r = 1.0;
            if (c.g > 1.0) c.g = 1.0;
            if (c.b > 1.0) c.b = 1.0;
			c.a = 1.0 - ((c.r +c.g + c.b) / 3.0);
            c.a = (c.r + c.g + c.b) / 3.0;
			return c;
		}
        
        ENDCG
    }

Fallback "Transparent/VertexLit"
}