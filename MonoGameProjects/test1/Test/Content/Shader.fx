// Shader.fx
sampler TextureSampler : register(s0);

struct VertexShaderInput
{
	float4 Position : POSITION;
	float3 Normal : NORMAL;
	float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float3 Normal : NORMAL;
	float2 TexCoord : TEXCOORD0;
};

float4x4 World;
float4x4 View;
float4x4 Projection;

float4 LightDirection; // Normalized light direction
float4 LightColor; // Light color
float4 AmbientColor; // Ambient color

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	output.Position = mul(input.Position, World);
	output.Normal = input.Normal;
	output.TexCoord = input.TexCoord;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_Target
{
    // Basic lighting calculation
	float3 lightDir = normalize(LightDirection.xyz);
	float diff = max(dot(input.Normal, -lightDir), 0.0);

    // Get texture color
	float4 texColor = tex2D(TextureSampler, input.TexCoord);
    
    // Calculate final color
	return texColor * (AmbientColor + diff * LightColor);
}

technique BasicTechnique
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}
