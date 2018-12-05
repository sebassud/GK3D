#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_1
	#define PS_SHADERMODEL ps_4_1
#endif

float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xCamPos;
float3 xAllowedRotDir;
float scale;

//------- Texture Samplers --------
Texture xBillboardTexture;

sampler textureSampler = sampler_state { texture = <xBillboardTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = CLAMP; AddressV = CLAMP;};
struct BBVertexToPixel
{
    float4 Position : POSITION;
    float2 TexCoord    : TEXCOORD0;
	float2 TexCoordCorner1    : TEXCOORD1;
	float2 TexCoordCorner2    : TEXCOORD2;
	float Ratio : PSIZE;
};
struct BBPixelToFrame
{
    float4 Color     : COLOR0;
};

//------- Technique: CylBillboard --------
BBVertexToPixel CylBillboardVS(float3 inPos: POSITION0, float2 inTexCoord: TEXCOORD0, float2 inTexCoordCorner1 : TEXCOORD1, float2 inTexCoordCorner2 : TEXCOORD2, float inScale: PSIZE0, float inRatio: PSIZE1)
{
    BBVertexToPixel Output = (BBVertexToPixel)0;

    float3 center = mul(inPos, xWorld);
    float3 eyeVector = center - xCamPos;

    float3 upVector = xAllowedRotDir;
    upVector = normalize(upVector)*scale*inScale;
    float3 sideVector = cross(eyeVector,upVector);
    sideVector = normalize(sideVector)*scale*inScale;

    float3 finalPosition = center;
    finalPosition += (inTexCoord.x-0.5f)*sideVector;
    finalPosition += (1.5f-inTexCoord.y*1.5f)*upVector;

    float4 finalPosition4 = float4(finalPosition, 1);

    float4x4 preViewProjection = mul (xView, xProjection);
    Output.Position = mul(finalPosition4, preViewProjection);

    Output.TexCoord = inTexCoord;
	Output.TexCoordCorner1 = inTexCoordCorner1;
	Output.TexCoordCorner2 = inTexCoordCorner2;
	Output.Ratio = inRatio;

    return Output;
}

BBPixelToFrame BillboardPS(BBVertexToPixel PSIn) : COLOR0
{
    BBPixelToFrame Output = (BBPixelToFrame)0;
	Output.Color = PSIn.Ratio * tex2D(textureSampler, PSIn.TexCoordCorner1) + (1 - PSIn.Ratio) * tex2D(textureSampler, PSIn.TexCoordCorner2);

    return Output;
}

technique CylBillboard
{
    pass Pass0
    {        
        VertexShader = compile VS_SHADERMODEL CylBillboardVS();
        PixelShader = compile PS_SHADERMODEL BillboardPS();        
    }
}