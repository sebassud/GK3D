#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_1
#define PS_SHADERMODEL ps_4_1
#endif

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 Rotation;
float4 MaterialColor;
texture ModelTexture;
float3 CameraPosition;
//Diffuse
float DiffuseIntensity = 0.75; //Kd
							   //Specular
float Shininess = 50;
float SpecularIntensity = 0.1;
//Ambient
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.05;
//Direction
float3 DirectionLight = float3(0, 1, 1);
float4 DirectionColor = float4(1, 1, 1, 1);
//Reflectors
float P = 8;
int ReflectorsCount = 0;
float3 DirectionVectors[4];
float3 PositionVectors[4];
float4 ColorVectors[4];


samplerCUBE SkyBoxSampler = sampler_state
{
	texture = <ModelTexture>;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.TextureCoordinate = mul(worldPosition - CameraPosition, Rotation);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return texCUBE(SkyBoxSampler, normalize(input.TextureCoordinate));
}

technique Colored
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}
