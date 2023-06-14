Shader "Levels/Temp/Shit"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Texture Color", Color) = (1,1,1,1)


    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vertexFunc
                #pragma fragment fragmentFunc


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

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float _ClipValue;
                float _XOffsetSpeed;
                float _YOffsetSpeed;
                float _Random;

                fixed4 _Offset;


                float nrand(float2 uv)
                {
                    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
                }

                v2f vertexFunc(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

                fixed4 fragmentFunc(v2f IN) : SV_Target
                {

                    fixed4 color = tex2D(_MainTex, IN.uv);

                    color = color + _Color;


                    // color = color * sin(color);


                     fixed4 finalColor = color;
                     return finalColor;
                 }




                 ENDCG
             }
        }
}
