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

sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
};

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
	float2 TextureCoordinate : TEXCOORD0;
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

VertexShaderOutput TexturedVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float3 normal = normalize(mul(input.Normal, World));
	output.Normal = normal;
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition.xyz;
	output.TextureCoordinate = input.TextureCoordinate;
	return output;
}

float4 TexturedPS(VertexShaderOutput input) : COLOR
{
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;
	float4 resultColor = textureColor * 0.8;

	return resultColor;
}

technique Textured
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL TexturedVS();
		PixelShader = compile PS_SHADERMODEL TexturedPS();
	}
};
