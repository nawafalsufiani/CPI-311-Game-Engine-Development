// Shader Variables
float4x4 World; // World transformation matrix
float4x4 View; // View transformation matrix
float4x4 Projection; // Projection transformation matrix

// Light structure
struct Light
{
    float4 Position; // Light position
    float4 Color; // Light color
};

// Input structure
struct VertexInput
{
    float4 Position : POSITION0; // Vertex position
    float4 Normal : NORMAL0; // Vertex normal
    float2 UV : TEXCOORD0; // Texture coordinates
};

// Output structure
struct PixelInput
{
    float4 Position : SV_POSITION; // Transformed position
    float3 Normal : TEXCOORD0; // Normal for lighting calculations
    float2 UV : TEXCOORD1; // Texture coordinates
};

// Texture and sampler
Texture2D MyTexture : register(t0); // Declare a texture
SamplerState MySampler : register(s0); // Declare a sampler

// Vertex shader
PixelInput VS(VertexInput input)
{
    PixelInput output;

    // Transform the position by the world, view, and projection matrices
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(worldPosition, View);
    output.Position = mul(output.Position, Projection);

    // Transform the normal using the world matrix
    output.Normal = normalize(mul(input.Normal, (float3x3) World)); // Use 3x3 for normals
    output.UV = input.UV;

    return output;
}

// Pixel shader
float4 PS(PixelInput input) : SV_TARGET
{
    // Sample the texture
    float4 textureColor = MyTexture.Sample(MySampler, input.UV);

    // Simple lighting (assuming a directional light)
    float3 lightDirection = normalize(float3(0.0, -1.0, -1.0)); // Light direction
    float3 normal = normalize(input.Normal); // Normalize the normal

    // Calculate the dot product between light direction and normal
    float diff = max(dot(normal, lightDirection), 0.0);

    // Final color calculation
    float4 finalColor = diff * textureColor; // Apply diffuse lighting
    finalColor.a = 1.0; // Set alpha to 1.0 for opaque

    return finalColor;
}

// Technique
technique SimpleTechnique
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 VS();
        PixelShader = compile ps_4_0 PS();
    }
}
