﻿Shader "Erebus/GlitchSpriteShader" {
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
        [PerRendererData] _Desaturation("Desaturation", Range(0, 1)) = 0.0
        _Cutoff("Base Alpha cutoff", Range(0,.9)) = .5
        _ResolutionX("Resolution X (px)", Float) = 1066
        _ResolutionY("Resolution Y (px)", Float) = 600
        
        _Elapsed("Elapsed Seconds", Float) = 1.0
        
        [Space(25)][MaterialToggle] _HDispEnabled(" === Horizontal Displacement === ", Float) = 0.0
        [MaterialToggle] _HDispSloppyPower("Sloppy power", Float) = 0
        _HDispChance("Chance", Range(0, 1)) = 0.5
        _HDispPower("Power", Range(0, 1)) = 0.5
        _HDispPowerVariance("Power variance", Range(0, 1)) = 0.5
        _HDispChunking("Chunk size", Range(0, 1)) = 0.5
        _HDispChunkingVariance("Chunk size variance", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _HBleedEnabled(" === Horizontal Streaking === ", Float) = 0
        [MaterialToggle] _HBleedAlphaRestrict("Restrict to alpha", Float) = 0
        _HBleedChance("Chance", Range(0, 1)) = 0.5
        _HBleedChunking("Chunk size", Range(0, 1)) = 0.5
        _HBleedChunkingVariance("Chunk size variance", Range(0, 1)) = 0.5
        _HBleedTailing("Tail length", Range(0, 1)) = 0.5
        _HBleedTailingVariance("Tail length variance", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _SFrameEnabled(" === Static Frames === ", Float) = 0
        [MaterialToggle] _SFrameAlphaIncluded("Include alpha regions", Float) = 0
        _SFrameChance("Chance", Range(0, 1)) = 0.5
        _SFrameChunking("Chunk size", Range(0, 1)) = 0.5
        _SFrameChunkingVariance("Chunk size variance", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _PDistEnabled(" === Palette Distortion === ", Float) = 0
        [MaterialToggle] _PDistAlphaIncluded("Include alpha regions", Float) = 0
        [MaterialToggle] _PDistSimultaneousInvert("Synchronized inversion", Float) = 0
        _PDistInvertR("R inversion chance", Range(0, 1)) = 0.0
        _PDistInvertG("G inversion chance", Range(0, 1)) = 0.0
        _PDistInvertB("B inversion chance", Range(0, 1)) = 0.0
        _PDistMaxR("R max chance", Range(0, 1)) = 0.0
        _PDistMaxG("G max chance", Range(0, 1)) = 0.0
        _PDistMaxB("B max chance", Range(0, 1)) = 0.0
        _PDistMonocolorChance("Monocolor chance", Range(0, 1)) = 0.0
        _PDistMonocolor("Monocolor", Color) = (1.0, 1.0, 1.0, 1.0)
        
        [Space(25)][MaterialToggle] _RDispEnabled(" === Rectangular Displacement === ", Float) = 0.0
        [MaterialToggle] _RDispCopyOnly("Keep source region intact", Range(0, 1)) = 0.0
        [MaterialToggle] _RDispInvertSource("Invert source background", Range(0, 1)) = 0.0
        [MaterialToggle] _RDispKeepAlpha("Preserve source alpha", Range(0, 1)) = 1.0
        _RDispTex("Background texture", 2D) = "black" {}
        _RDispChance("Chance", Range(0, 1)) = 0.5
        _RDispChunkXSize("Chunk X Size", Range(0, 1)) = 0.5
        _RDispChunkYSize("Chunk Y size", Range(0, 1)) = 0.5
        _RDispChunkVariance("Chunk size variance", Range(0, 1)) = 0.5
        _RDispMinPowerX("Displacement min dist X", Range(-1, 1)) = -0.5
        _RDispMaxPowerX("Displacement max dist X", Range(-1, 1)) = 0.5
        _RDispMinPowerY("Displacement min dist Y", Range(-1, 1)) = -0.5
        _RDispMaxPowerY("Displacement max dist Y", Range(-1, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _VSyncEnabled(" === VSync === ", Float) = 0.0
        _VSyncPowerMin("Min jitter power", Range(-1, 1)) = -0.5
        _VSyncPowerMax("Max jitter power", Range(-1, 1)) = 0.5
        _VSyncJitterChance("Jitter chance", Range(0, 1)) = 0.5
        _VSyncJitterDuration("Jitter duration", Range(0, 1)) = 0.5
        _VSyncChance("Loop chance", Range(0, 1)) = 0.5
        _VSyncDuration("Loop duration", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _SShiftEnabled(" === Scanline Shift === ", Float) = 0.0
        _SShiftChance("Chance", Range(0, 1)) = .5
        _SShiftPowerMin("Min power", Range(0, 1)) = 0.25
        _SShiftPowerMax("Max power", Range(0, 1)) = 0.5
        
        [Space(25)][MaterialToggle] _TDistEnabled(" === Traveling Distortion === ", Float) = 0.0
        [MaterialToggle] _TDistTailoff("Linear tailoff", Range(0, 1)) = 1
        [MaterialToggle] _TDistExcludeAlpha("Exclude alpha regions", Range(0, 1)) = 0
        _TDistChance("Chance", Range(0, 1)) = .5
        _TDistDuration("Duration", Range(0, 1)) = .5
        _TDistChunking("Chunk height", Range(0, 1)) = .5
        _TDistStaticBarSize("Static effect height", Range(0, 1)) = .5
        _TDistStaticSize("Static chunk size", Range(0, 1)) = .5
        [MaterialToggle] _TDistHDisp("Horizontal displacement enabled", Range(0, 1)) = 0
        _TDistHDispPower("Displacement power", Range(0, 1)) = .5
        _TDistHDispPowerVariance("Displacement power variance", Range(0, 1)) = .5
        _TDistColorBarSize("Color effect height", Range(0, 1)) = 0
        [MaterialToggle] _TDispPreserveBrightness("Color preserve brightness", Range(0, 1)) = 0
        [MaterialToggle] _TDistInvertR("Color invert R", Range(0, 1)) = 0
        [MaterialToggle] _TDistInvertG("Color invert G", Range(0, 1)) = 0
        [MaterialToggle] _TDistInvertB("Color invert B", Range(0, 1)) = 0
        
        [Space(25)][MaterialToggle] _SColorEnabled(" === Scanline Coloring === ", Float) = 0.0
        [MaterialToggle] _SColorExcludeAlpha("Exclude alpha regions", Range(0, 1)) = 1.0
        _SColorChance("Chance",  Range(0, 1)) = 0.5
        _SColorVelocity("Vertical velocity",  Range(-1, 1)) = 0.0
        _SColorGap("Gap",  Range(0, 1)) = 0.05
        _SColorBrightness("Brightness change",  Range(-1, 1)) = 0.0
        [MaterialToggle] _SColorBleed("Full bleed", Range(0, 1)) = 0.0
        [MaterialToggle] _SColorStatic("Scanline static", Range(0, 1)) = 0.0
        
        [Space(25)][MaterialToggle] _CClampEnabled(" === Color Channel Clamping === ", Float) = 0.0
        _CClampBrightness("Pre-brightness boost",  Range(-1, 1)) = 0.0
        [MaterialToggle] _CClampBlack("Always include true black/white", Float) = 1.0
        _CClampR("R shades allowed",  Range(0, 1)) = 1.0
        _CClampG("G shades allowed",  Range(0, 1)) = 1.0
        _CClampB("B shades allowed",  Range(0, 1)) = 1.0
        [MaterialToggle] _CClampDither("Dithering enabled", Float) = 0.0
        [MaterialToggle] _CClampDitherVary("Varied dithering enabled", Float) = 0.0
        _CClampDitherChunk("Dithering chunk width", Range(0, 1)) = 0.5
        _CClampJitterR("R colors jitter power",  Range(0, 1)) = 0.0
        _CClampJitterG("G colors jitter power",  Range(0, 1)) = 0.0
        _CClampJitterB("B colors jitter power",  Range(0, 1)) = 0.0

        [Space(25)][MaterialToggle] _PEdgeEnabled(" === Pulsing Edge === ", Float) = 0.0
        [MaterialToggle] _PEdgeUseWaveSource("Use wave source", Float) = 0.0
        _PEdgeDuration("Duration",  Range(0, 10)) = 0.5
        _PEdgeDepthMin("Depth Min",  Range(0, 1)) = 0.0
        _PEdgeDepthMax("Depth Max",  Range(0, 1)) = 0.5
        _PEdgePower("Power",  Range(0, 1)) = 1.0
        _PEdgeAmplitude("Amplitude",  Range(0, 1)) = 0.5
        _PEdgeDistanceGrain("Distance Granularity", Range(0, 1)) = 1.0
        _PEdgeWavePower("Wave Source Power", Range(0, 1)) = 1.0
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
            float4 pxXY = float4(xy[0] * (float)_ResolutionX, xy[1] * (float)_ResolutionY, 0.0, 0.0);
            fixed4 c = glitchFragFromCoords(xy, pxXY) * IN.color;
            float avg = (c[0] + c[1] + c[2]) / 3.0;
            float4 desat = float4(avg / 2.0, avg / 2.0, avg / 2.0, c.a);
            o.Albedo = c.rgb * (1.0 - _Desaturation) + desat.rgb * (_Desaturation);
            o.Albedo = o.Albedo * (1.0 - _Flash.a) + _Flash.rgb * _Flash.a;
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
			c.a = s.Alpha;
			return c;
		}
        
        ENDCG
    }

Fallback "Transparent/VertexLit"
}