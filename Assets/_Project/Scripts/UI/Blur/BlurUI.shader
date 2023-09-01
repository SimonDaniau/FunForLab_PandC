Shader "Custom/BlurUI"
{
    Properties
    {
        [PerRendererData]_MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Size ("Size", Range(0,4)) = 3
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
        Lighting Off
        Zwrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        GrabPass
        {
            Tags
            {
                "LightMode"="Always"
            }
        }
        Pass
        {
            Tags
            {
                "LightMode"="Always"
            }
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma vertex vert
            #pragma fragment frag

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
            #include <UnityShaderUtilities.cginc>

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float4 uvgrab : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            half _Size;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                half4 sum = half4(0, 0, 0, 0);
                #define BLURPIXEL(weight,kernelx)tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx * _Size / (16/9), i.uvgrab.y, i.uvgrab.z,i.uvgrab.w)))*weight
                sum += BLURPIXEL(0.05, -4.0);
                sum += BLURPIXEL(0.09, -3.0);
                sum += BLURPIXEL(0.12, -2.0);
                sum += BLURPIXEL(0.15, -1.0);
                sum += BLURPIXEL(0.18, 0.0);
                sum += BLURPIXEL(0.15, +1.0);
                sum += BLURPIXEL(0.12, +2.0);
                sum += BLURPIXEL(0.09, +3.0);
                sum += BLURPIXEL(0.05, +4.0);

                return sum;
            }
            ENDCG
        }
        Pass
        {
            Tags
            {
                "LightMode"="Always"
            }
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma vertex vert
            #pragma fragment frag

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
            #include <UnityShaderUtilities.cginc>

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : POSITION;
                float4 uvgrab : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            half _Size;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;
                o.uvgrab.zw = o.vertex.zw;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                half4 sum = half4(0, 0, 0, 0);
                #define BLURPIXEL(weight,kernelx)tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x , i.uvgrab.y+ _GrabTexture_TexelSize.x * kernelx * _Size, i.uvgrab.z,i.uvgrab.w)))*weight
                sum += BLURPIXEL(0.05, -4.0);
                sum += BLURPIXEL(0.09, -3.0);
                sum += BLURPIXEL(0.12, -2.0);
                sum += BLURPIXEL(0.15, -1.0);
                sum += BLURPIXEL(0.18, 0.0);
                sum += BLURPIXEL(0.15, +1.0);
                sum += BLURPIXEL(0.12, +2.0);
                sum += BLURPIXEL(0.09, +3.0);
                sum += BLURPIXEL(0.05, +4.0);

                return sum;
            }
            ENDCG
        }
        Pass
        {
            Name "Default"
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma vertex vert
            #pragma fragment frag

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 2.0
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOOR1;
                float4 uvgrab : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            
            half _Size;

            v2f vert(appdata_t v)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);

                #if UNITY_UV_STARTS_AT_TOP
                float scale = -1.0;
                #else
                float scale = 1.0;
                #endif
                
                OUT.uvgrab.xy = (float2(OUT.vertex.x, OUT.vertex.y * scale) + OUT.vertex.w) * 0.5;
                OUT.uvgrab.zw = OUT.vertex.zw;
                OUT.color = v.color;
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex,IN.texcoord)+_TextureSampleAdd)*IN.color;
                half4 blur = tex2Dproj(_GrabTexture,UNITY_PROJ_COORD(IN.uvgrab));
             
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}