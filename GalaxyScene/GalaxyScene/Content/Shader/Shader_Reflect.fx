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
float SpecularIntensity = 0.5;
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
	float3 Normal : NORMAL;
	float3 WorldPosition : POSITION1;
};

float4 Diffuse(float3 N, float3 L, float4 colorLight, float distanceIntensity)
{
	float lightIntensity = saturate(dot(N, L));
	return colorLight * DiffuseIntensity * lightIntensity*distanceIntensity; // Ilight*Kd*<N,L>
}

float4 SpecPhong(float3 N, float3 L, float3 V, float4 colorLight, float distanceIntensity)
{
	float3 R = normalize(2 * dot(L, N) * N - L);
	float4 dotProduct = saturate(dot(R, V));
	float4 specular = SpecularIntensity * colorLight * pow(dotProduct, Shininess)*distanceIntensity; //Kr*Ilight*<R,V>^m
	return specular;
}

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 VertexPosition = mul(input.Position, World);
	float3 ViewDirection = CameraPosition - worldPosition;

	float3 normal = normalize(mul(input.Normal, WorldInverseTranspose));
	output.Reflection = mul(reflect(-normalize(ViewDirection), normalize(normal)), Rotation);

	output.Normal = normalize(normal);

	output.WorldPosition = worldPosition.xyz;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(float3((CameraPosition - input.WorldPosition).xyz));
	float distanceIntensity = 1;
	float4 textureColor = 0.4f * MaterialColor + 0.6f * texCUBE(SkyboxSampler, normalize(input.Reflection));
	textureColor.a = 1;
	float4 resultColor = textureColor * AmbientIntensity;

	float3 DiffuseLightDirection = DirectionLight;
	float3 L = normalize(DiffuseLightDirection);
	float4 diffColor = Diffuse(N, L, DirectionColor, 1);
	float4 specular = SpecPhong(N, L, V, DirectionColor, 1);
	resultColor += saturate(textureColor * diffColor + specular);
	saturate(resultColor);

	for (int i = 0; i < ReflectorsCount; i++)
	{
		float3 DiffuseLightDirection = float3((PositionVectors[i] - input.WorldPosition).xyz);
		float3 L = normalize(DiffuseLightDirection);
		float d2 = (PositionVectors[i].x - input.WorldPosition.x)*(PositionVectors[i].x - input.WorldPosition.x) + (PositionVectors[i].y - input.WorldPosition.y)*(PositionVectors[i].y - input.WorldPosition.y) + (PositionVectors[i].z - input.WorldPosition.z)*(PositionVectors[i].z - input.WorldPosition.z);
		distanceIntensity = saturate(6 / d2);
		float4 diffColor = Diffuse(N, L, ColorVectors[i], distanceIntensity);
		float4 specular = SpecPhong(N, L, V, ColorVectors[i], distanceIntensity);
		//Reflector
		float4 att = 0;
		float cos = dot(-DirectionVectors[i], L);
		att = pow(saturate(cos), P);
		resultColor += saturate(textureColor * att * diffColor + att * specular);
		saturate(resultColor);
	}
	return resultColor;
}

technique Reflection
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL VertexShaderFunction();
		PixelShader = compile PS_SHADERMODEL PixelShaderFunction();
	}
}