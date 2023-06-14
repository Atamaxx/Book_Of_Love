#ifndef EXP_HLSL
#define EXP_HLSL

float ADD() {
	return 0;
}

void CalculateBarycentricCoordinates_float(float2 p, float2 a, float2 b, float2 c, out float3 Out)
{
    float2 v0 = b - a;
    float2 v1 = c - a;
    float2 v2 = p - a;

    float d00 = dot(v0, v0);
    float d01 = dot(v0, v1);
    float d11 = dot(v1, v1);
    float d20 = dot(v2, v0);
    float d21 = dot(v2, v1);

    float denom = d00 * d11 - d01 * d01;

    float2 barycentric;
    barycentric.x = (d11 * d20 - d01 * d21) / denom;
    barycentric.y = (d00 * d21 - d01 * d20) / denom;

    if (barycentric.x >= 0 && barycentric.y >= 0 && barycentric.x + barycentric.y <= 1)
    {
        Out = (1, 1, 1);
       
    }
    else Out = (1, 0, 0);
}

void Add3_float(float3 A, float3 B, float3 C, out float3 Out) {
	Out = A+B+C;
	Out = (1, ADD(), ADD());
}
void Add2_float(float3 A, float3 B, out float3 Out) {
	Out = A + B;
}


#endif