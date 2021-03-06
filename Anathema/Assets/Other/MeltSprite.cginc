// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

#ifndef UNITY_SPRITES_INCLUDED
#define UNITY_SPRITES_INCLUDED

#include "UnityCG.cginc"

#ifdef UNITY_INSTANCING_ENABLED

    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
        // SpriteRenderer.Color while Non-Batched/Instanced.
        UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
        // this could be smaller but that's how bit each entry is regardless of type
        UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

    #define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
    #define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

#endif // instancing

CBUFFER_START(UnityPerDrawSprite)
#ifndef UNITY_INSTANCING_ENABLED
    fixed4 _RendererColor;
    fixed2 _Flip;
#endif
    float _BurnProgress;
    float _EnableExternalAlpha;
CBUFFER_END

// Material Color.
fixed4 _Color;
fixed4 _BurnColor;
float _BurnPercentOffset;
float _GradientStartBurnPercent;
float _GradientEndBurnPercent;
float _BurnProgressMultiplier;

struct appdata_t
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct v2f
{
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
    UNITY_VERTEX_OUTPUT_STEREO
};

inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
{
    return float4(pos.xy * flip, pos.z, 1.0);
}

v2f SpriteVert(appdata_t IN)
{
    v2f OUT;

    UNITY_SETUP_INSTANCE_ID (IN);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

    OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
    OUT.vertex = UnityObjectToClipPos(OUT.vertex);
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color * _RendererColor;

    #ifdef PIXELSNAP_ON
    OUT.vertex = UnityPixelSnap (OUT.vertex);
    #endif

    return OUT;
}

sampler2D _MainTex;
sampler2D _AlphaTex;
sampler2D _NoiseTex;

fixed4 SampleSpriteTexture (float2 uv)
{
    fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
    fixed4 alpha = tex2D (_AlphaTex, uv);
    color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif

    return color;
}

fixed4 SpriteFrag(v2f IN) : SV_Target
{
    fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
    float pixelValue;
    if (IN.texcoord.y <= 0.75f)
        pixelValue = (5*pow((IN.texcoord.x*2.0)-1.0, 2.0) + pow((IN.texcoord.y*2.0)-1.6, 2.0))/6;
    else
        pixelValue = 0.0;
    
    float x = pow(exp(_BurnProgress * _BurnProgressMultiplier), 2.0) - pixelValue; 
    if (x < 0.0025)
    {
        fixed3 a = _BurnColor.rgb;
        fixed3 b = fixed3(1,1,0);
        float s = (x/0.0025);
        
        c.rgb = lerp(a,b,s);
    }
    else if (x < 0.005)
    {
        fixed3 a = fixed3(1,1,0);
        fixed3 b = _BurnColor;
        float s = ((x-0.0025)/0.0025);
        
        c.rgb = lerp(a,b,s);
    }
    else if (x < 0.0075)
    {
        fixed3 a = _BurnColor;
        fixed3 b = fixed3(0,0,0);
        float s = ((x-0.005)/0.005);
        
        c.rgb = lerp(a,b,s);
    }
    if (x < 0)
    {
        c.a = 0.0;
    }
    
    c.rgb *= c.a;
    c.r *= 1 + (0.3 * _BurnProgress);
    c.g *=  1 + (0.05 * _BurnProgress);
    return c;
}

#endif // UNITY_SPRITES_INCLUDED
