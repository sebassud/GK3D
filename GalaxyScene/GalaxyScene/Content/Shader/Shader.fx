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
float3 DirectionLight = float3(0, 0, 1);
//Diffuse
float4 DiffuseColor = float4(1, 1, 1, 1);  //object color
float DiffuseIntensity = 0.75; //Kd
//Specular
float Shininess = 50;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = 0.1;
//Ambient
float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.06;
//Reflectors

sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture);
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
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

struct ColoredShaderInput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
};

struct ColoredShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
	float4 Color : TEXCOORD0;
	float3 WorldPosition : POSITION1;
};

struct LightsShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
};

float4 Diffuse(float3 N, float3 L, float distanceIntensity)
{
	float lightIntensity = saturate(dot(N, L));
	return DiffuseColor * DiffuseIntensity * lightIntensity*distanceIntensity; // Ilight*Kd*<N,L>
}

float4 SpecPhong(float3 N, float3 L, float3 V, float distanceIntensity)
{
	float3 R = normalize(2 * dot(L, N) * N - L);
	float4 dotProduct = saturate(dot(R, V));
	float4 specular = SpecularIntensity * SpecularColor * pow(dotProduct, Shininess)*distanceIntensity; //Kr*Ilight*<R,V>^m
	return specular;
}

VertexShaderOutput MainVS(in VertexShaderInput input)
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

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(float3((CameraPosition - input.WorldPosition).xyz));
	float distanceIntensity = 1;
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;
	float4 resultColor = textureColor * AmbientIntensity;
	float3 DiffuseLightDirection = DirectionLight;
	float3 L = normalize(DiffuseLightDirection);
	float4 diffColor = Diffuse(N, L, 1);
	float4 specular = SpecPhong(N, L, V, 1);
	resultColor += saturate(textureColor * diffColor + specular);
	saturate(resultColor);

	//for (int i = 0; i < LightsCount; i++)
	//{
	//	float3 DiffuseLightDirection = float3((LightPositions[i] - input.WorldPosition).xyz);
	//	float3 L = normalize(DiffuseLightDirection);
	//	float d2 = (LightPositions[i].x - input.WorldPosition.x)*(LightPositions[i].x - input.WorldPosition.x) + (LightPositions[i].y - input.WorldPosition.y)*(LightPositions[i].y - input.WorldPosition.y) + (LightPositions[i].z - input.WorldPosition.z)*(LightPositions[i].z - input.WorldPosition.z);
	//	distanceIntensity = saturate(100 / d2);
	//	float4 diffColor = Diffuse(N, L, distanceIntensity);
	//	float4 specular = SpecPhong(N, L, V, distanceIntensity);
	//	//Reflector
	//	float4 att = 0;
	//	float cos = dot(-DirectionVectors[i / 2], L);
	//	att = pow(saturate(cos), P);
	//	resultColor += saturate(textureColor * att * diffColor + att * specular);
	//	saturate(resultColor);
	//}
	return resultColor;
}

technique Textured
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
