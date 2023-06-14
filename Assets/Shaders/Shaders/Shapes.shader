// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Levels/Triangular Time/Shapes"


{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [ShowAsVector2] _Center("Center", Vector) = (0.5, 0.5, 0, 0)
        [ShowAsVector2] _Point1("Point 1", Vector) = (0, 0, 0, 0)
        [ShowAsVector2] _Point2("Point 2", Vector) = (0, 5, 0, 0)
        [ShowAsVector2] _Point3("Point 3", Vector) = (5, 0, 0, 0)
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float2 _Center;
            float2 _Point1;
            float2 _Point2;
            float2 _Point3;
            float2 worldPos;
            float4 worldPosition2;
            float4 worldPosition3;

            // Function to calculate barycentric coordinates
            float2 CalculateBarycentricCoordinates(float2 UV, float2 A, float2 B, float2 C)
            {
                float2 v0 = A - B;
                float2 v1 = C - A;
                float2 v2 = UV - A;

                float d00 = dot(v0, v0);
                float d01 = dot(v0, v1);
                float d11 = dot(v1, v1);
                float d20 = dot(v2, v0);
                float d21 = dot(v2, v1);

                float denom = d00 * d11 - d01 * d01;

                float2 barycentric;
                barycentric.x = (d11 * d20 - d01 * d21) / denom;
                barycentric.y = (d00 * d21 - d01 * d20) / denom;

                return barycentric;
            }
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                 worldPos = mul(unity_ObjectToWorld, v.vertex); // transform vertex position from object to world space

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = _Center.xy; // Center of the triangle
                float2 p = i.uv; // Current pixel UV position

                // Define the points of the triangle
                float2 pointA = _Point1;// mul(unity_ObjectToWorld,_Point1);
                float2 pointB = _Point2;
                float2 pointC = _Point3;

                // Calculate the barycentric coordinates of the current pixel
                float2 barycentric = CalculateBarycentricCoordinates(p, pointA, pointB, pointC);

                // Check if the pixel is inside the triangle using barycentric coordinates
                if (barycentric.x >= 0 && barycentric.y >= 0 && barycentric.x + barycentric.y <= 1)
                {
                    // Pixel is inside the triangle, color it
                    return fixed4(1, 1, 1, 1);
                }
                
                return fixed4(0, 0, 0, 0);
            }

                
                ENDCG
            }
    }
}

//{
//    Properties
//    {
//        _MainTex ("Texture", 2D) = "white" {}
//        [ShowAsVector2] _Point1("Point 1", Vector) = (0, 0, 0, 0)
//        [ShowAsVector2] _Point2("Point 2", Vector) = (0, 5, 0, 0)
//        [ShowAsVector2] _Point3("Point 3", Vector) = (5, 0, 0, 0)
//    }
//    SubShader
//    {
//        Tags { "RenderType"="Opaque" }
//        LOD 100
//
//        Pass
//        {
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            // make fog work
//            #pragma multi_compile_fog
//
//            #include "UnityCG.cginc"
//
//            struct appdata
//            {
//                float4 vertex : POSITION;
//                float2 uv : TEXCOORD0;
//            };
//
//            struct v2f
//            {
//                float2 uv : TEXCOORD0;
//                UNITY_FOG_COORDS(1)
//                float4 vertex : SV_POSITION;
//            };
//
//            sampler2D _MainTex;
//            float4 _MainTex_ST;
//            float2 _Point1;
//            float2 _Point2;
//            float2 _Point3;
//
//
//            v2f vert (appdata v)
//            {
//                v2f o;
//                o.vertex = UnityObjectToClipPos(v.vertex);
//                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                UNITY_TRANSFER_FOG(o,o.vertex);
//                return o;
//            }
//
//            fixed4 frag(v2f i) : SV_Target{
//                // Calculate the barycentric coordinates
//                float2 p1 = _Point1;
//                float2 p2 = _Point2;
//                float2 p3 = _Point3;
//                float2 d = i.vertex - p1;
//                float a = (p2.y - p3.y) * d.x + (p3.x - p2.x) * d.y;
//                float b = (p3.y - p1.y) * d.x + (p1.x - p3.x) * d.y;
//                float c = 1.0 - a - b;
//
//                // Check if the pixel is inside the triangle
//                if (a >= 0 && b >= 0 && c >= 0) 
//                {
//                    return fixed4(1, 1, 1, 1); // White color
//                }
//                else
//                 return fixed4(0, 0, 0, 0);
//            }
//
//            ENDCG
//        }
//    }
//}
