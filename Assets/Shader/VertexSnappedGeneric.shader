// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VertexSnappedGeneric"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _VertexPrecision("Vertex Placement Percision", Range(0,1)) = 0.5
        [Toggle] _Vert("Enable Fancy Vertex Shit", Float) = 0
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass
            {
                Tags {"LightMode" = "ForwardBase"}
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"
                #include "UnityLightingCommon.cginc" // for _LightColor0

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(2)
                    half4 diff : COLOR0;
                    half3 normal : TEXCOORD1;
                    float4 vertex : SV_POSITION;
                };

                float _VertexPrecision;
                float _Vert;
                sampler2D _MainTex;
                float4 _MainTex_ST;



                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    float4 snapToPixel = UnityObjectToClipPos(v.vertex);
                    float4 vertex = snapToPixel;
                    vertex.xyz = snapToPixel.xyz / snapToPixel.w;
                    vertex.x = floor((_VertexPrecision * 160) * vertex.x) / (_VertexPrecision * 160);
                    vertex.y = floor((_VertexPrecision * 160) * vertex.y) / (_VertexPrecision * 160);
                    vertex.xyz *= snapToPixel.w;
                    vertex = vertex - UnityObjectToClipPos(v.vertex);
                    vertex = UnityObjectToClipPos(v.vertex) + (vertex * _Vert);
                    o.vertex = vertex;

                    half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                    half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                    o.diff = nl * _LightColor0;

                    // the only difference from previous shader:
                    // in addition to the diffuse lighting from the main light,
                    // add illumination from ambient or light probes
                    // ShadeSH9 function from UnityCG.cginc evaluates it,
                    // using world space normal
                    o.diff.rgb += ShadeSH9(half4(worldNormal, 1));  
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = tex2D(_MainTex, i.uv) * i.diff;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
        }
    FallBack "Diffuse"
}
