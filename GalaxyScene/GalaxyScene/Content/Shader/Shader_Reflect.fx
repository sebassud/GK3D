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
float4x4 WorldInverseTranspose;

float4 TintColor = float4(0.75, 0.75, 0.75, 1);

samplerCUBE SkyboxSampler = sampler_state
{
	texture = <ModelTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};

struct VertexShaderInput
{
	float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float3 Reflection : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 VertexPosition = mul(input.Position, World);
	float3 ViewDirection = CameraPosition - worldPosition;

	float3 Normal = normalize(mul(input.Normal, WorldInverseTranspose));
	output.Reflection = mul(reflect(-normalize(ViewDirection), normalize(Normal)), Rotation);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return 0.70f * MaterialColor + 0.3f * texCUBE(SkyboxSampler, normalize(input.Reflection));
}

technique Reflection
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}