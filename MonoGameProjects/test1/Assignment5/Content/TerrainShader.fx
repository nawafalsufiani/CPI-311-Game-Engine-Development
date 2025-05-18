// Parameters that should be set from the program
float4x4 World; // World Matrix
float4x4 View; // View Matrix
float4x4 Projection; // Projection Matrix
float3x3 NormalMatrix; // Normal Matrix (calculated on the CPU and passed here)
float3 LightPosition; // in world space
float3 CameraPosition; // in world space
float Shininess; // scalar value
float3 AmbientColor;
float3 DiffuseColor;
float3 SpecularColor;
texture NormalMap;

sampler NormalSampler = sampler_state
{
    Texture = <NormalMap>;
    AddressU = Wrap;
    AddressV = Wrap;
};

// Structures to manage inputs/outputs to vertex and pixel shaders
struct VertexInput
{
    float4 Position : POSITION; // Position of the vertex
    float2 UV : TEXCOORD0; // Texture coordinates
};

struct PhongVertexOutput
{
    float4 Position : POSITION; // Final transformed position
    float2 UV : TEXCOORD0; // Pass texture coordinates
    float4 WorldPosition : TEXCOORD1; // World position for lighting
};

// Vertex Shader: Transforms vertex positions and passes texture coordinates
PhongVertexOutput TerrainVertexShader(VertexInput input)
{
    PhongVertexOutput output;
    output.WorldPosition = mul(input.Position, World); // Transform position to world space
    float4 viewPosition = mul(output.WorldPosition, View); // Transform to view space
    output.Position = mul(viewPosition, Projection); // Transform to clip space
    output.UV = input.UV; // Pass UV coordinates
    return output;
}

// Pixel Shader: Handles lighting calculations and texture sampling
float4 TerrainPixelShader(PhongVertexOutput input) : COLOR0
{
    // Sample the normal map and unpack the normal
    float3 normal = tex2D(NormalSampler, input.UV).xyz * 2.0 - 1.0; // Unpack normal from [0,1] range to [-1,1]
    normal = normalize(mul(normal, (float3x3) World)); // Convert to world space and normalize

    // Compute lighting vectors
    float3 lightDirection = normalize(LightPosition - input.WorldPosition.xyz);
    float3 viewDirection = normalize(CameraPosition - input.WorldPosition.xyz);
    float3 reflectDirection = reflect(-lightDirection, normal); // Compute reflection vector

    // Compute lighting components
    float diffuse = max(dot(lightDirection, normal), 0); // Diffuse lighting
    float specular = pow(max(dot(reflectDirection, viewDirection), 0), Shininess); // Specular lighting

    // Combine lighting components with material colors
    float3 lighting = AmbientColor + diffuse * DiffuseColor + specular * SpecularColor;

    return float4(lighting, 1); // Output final color with full alpha
}

// Technique to use the vertex and pixel shaders
technique TerrainShader
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 TerrainVertexShader();
        PixelShader = compile ps_4_0 TerrainPixelShader();
    }
}
