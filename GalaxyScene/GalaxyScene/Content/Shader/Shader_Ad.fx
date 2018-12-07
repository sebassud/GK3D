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

//
float4x4 TextureScale;

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
	output.TextureCoordinate = mul(normalize(input.Position), TextureScale).xz;
	return output;
}

float4 TexturedPS(VertexShaderOutput input) : COLOR
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(float3((CameraPosition - input.WorldPosition).xyz));
	float distanceIntensity = 1;
	float2 uv = input.TextureCoordinate;
	float4 textureColor;
	textureColor = tex2D(textureSampler, input.TextureCoordinate);

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

ColoredShaderOutput ColoredVS(in ColoredShaderInput input)
{
	ColoredShaderOutput output = (ColoredShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float3 normal = normalize(mul(input.Normal, World));
	output.Normal = normal;
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition.xyz;
	output.Color = MaterialColor;
	return output;
}

float4 ColoredPS(ColoredShaderOutput input) : COLOR
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(float3((CameraPosition - input.WorldPosition).xyz));
	float distanceIntensity = 1;
	float4 resultColor = input.Color*AmbientIntensity;

	float3 DiffuseLightDirection = DirectionLight;
	float3 L = normalize(DiffuseLightDirection);
	float4 diffColor = Diffuse(N, L, DirectionColor, 1);
	float4 specular = SpecPhong(N, L, V, DirectionColor, 1);
	resultColor += saturate(input.Color * diffColor + specular);
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
		resultColor += saturate(input.Color * att * diffColor + att * specular);
		saturate(resultColor);
	}
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

technique Colored
{
	pass Pass1
	{
		VertexShader = compile VS_SHADERMODEL ColoredVS();
		PixelShader = compile PS_SHADERMODEL ColoredPS();
	}
};
