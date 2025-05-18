sampler2D TextureSampler : register(s0);

float4 MainPS(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
	float4 texColor = tex2D(TextureSampler, texCoord);
	return texColor * color; // This just multiplies the texture color with the given color.
}

technique BasicTechnique
{
	pass P0
	{
		PixelShader = compile ps_4_0_level_9_1 MainPS();
	}
}
