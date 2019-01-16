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
Texture2D ModelTexture2;
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
const float DepthBias = 0.02;
float4x4 LightViewProj;
float2 ShadowMapSize;
Texture2D ShadowMap;
bool DrawShadow = true;
//Reflectors
float P = 8;
int ReflectorsCount = 0;
float3 DirectionVectors[4];
float3 PositionVectors[4];
float4 ColorVectors[4];

sampler2D textureSampler = sampler_state {
	Texture = (ModelTexture2);
};

SamplerState ShadowMapSampler
{
	Texture = (ShadowMap);
	MinFilter = point;
	MagFilter = point;
	MipFilter = point;
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
	float4 WorldPosition : POSITION1;
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

float CalcShadowTermVariance(float light_space_depth, float2 moments)
{
	if (light_space_depth <= moments.x)
		return 1.0;

	float p = step(light_space_depth, moments.x);
	float variance = moments.y - (moments.x*moments.x);
	variance = max(variance, 0.00002);

	float d = light_space_depth - moments.x;
	float p_max = variance / (variance + d*d);

	return max(p_max, p);
}

float CalcShadowTermPCF(float light_space_depth, float ndotl, float2 shadow_coord)
{
	float shadow_term = 0;

	float variableBias = clamp(0.001 * tan(acos(ndotl)), 0, DepthBias);

	float sizex = 1 / ShadowMapSize.x;
	float sizey = 1 / ShadowMapSize.y;

	float samples_result = 0;

	for (int i = 0; i < 2; i++)
	{
		for (int j = 0; j < 2; j++)
		{
			float2 moments = ShadowMap.Sample(ShadowMapSampler, shadow_coord + float2(i*sizex, j*sizey)).rg;
			//if (light_space_depth - variableBias < moments.x) 
			{
				samples_result += CalcShadowTermVariance(light_space_depth, moments);
			}
		}
	}

	shadow_term = samples_result / 4;

	return shadow_term;
}

VertexShaderOutput TexturedVS(in VertexShaderInput input)
{
	VertexShaderOutput output = (VertexShaderOutput)0;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	float3 normal = normalize(mul(input.Normal, World));
	output.Normal = normal;
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;
	output.TextureCoordinate = input.TextureCoordinate;
	return output;
}

float4 TexturedPS(VertexShaderOutput input) : COLOR
{
	float3 N = normalize(input.Normal);
	float3 V = normalize(float3((CameraPosition - input.WorldPosition).xyz));
	float distanceIntensity = 1;
	float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
	textureColor.a = 1;
	float4 resultColor = textureColor * AmbientIntensity;

	float3 DiffuseLightDirection = DirectionLight;
	float3 L = normalize(DiffuseLightDirection);
	float4 diffColor = Diffuse(N, L, DirectionColor, 1);
	float4 specular = SpecPhong(N, L, V, DirectionColor, 1);


	float shadowContribution = 1;
	if (DrawShadow) {
		float4 lightingPosition = mul(input.WorldPosition, LightViewProj);
		float2 ShadowTexCoord = mad(0.5f, lightingPosition.xy / lightingPosition.w, float2(0.5f, 0.5f));
		ShadowTexCoord.y = 1.0f - ShadowTexCoord.y;

		float ourdepth = (lightingPosition.z / lightingPosition.w);

		shadowContribution = CalcShadowTermPCF(ourdepth, saturate(dot(N, L)), ShadowTexCoord);
	}

	resultColor += saturate(textureColor * diffColor * shadowContribution + specular * shadowContribution);
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
