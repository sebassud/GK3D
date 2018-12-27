#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_1
#define PS_SHADERMODEL ps_4_1
#endif

float4x4  WorldViewProj;

struct CreateShadowMap_VSOut
{
	float4 Position : POSITION;
	float Depth : TEXCOORD0;
};

//  CREATE SHADOW MAP
CreateShadowMap_VSOut CreateShadowMap_VertexShader(float4 Position : SV_POSITION)
{
	CreateShadowMap_VSOut Out;
	Out.Position = mul(Position, WorldViewProj);
	Out.Depth = Out.Position.z / Out.Position.w;

	return Out;
}

float4 CreateShadowMap_PixelShader(CreateShadowMap_VSOut input) : COLOR
{
	return input.Depth;
}

// Technique for creating the shadow map
technique CreateShadowMap
{
	pass Pass1
	{
		VertexShader = compile vs_5_0 CreateShadowMap_VertexShader();
		PixelShader = compile ps_5_0 CreateShadowMap_PixelShader();
	}
}
