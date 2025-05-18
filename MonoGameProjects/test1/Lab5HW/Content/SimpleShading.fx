// SimpleShading.fx
// Simple shader for basic vertex and pixel shading

// Define the WorldViewProjection matrix
float4x4 WorldViewProjection;

// Define lighting parameters
float4 LightPosition; // Position of the light
float4 CameraPosition; // Position of the camera
float4 AmbientColor; // Ambient light color
float4 DiffuseColor; // Diffuse light color
float4 SpecularColor; // Specular light color
float Shininess; // Shininess factor for specular highlights

// Input structure for the vertex shader
struct VertexShaderInput
{
    float4 Position : POSITION0; // Vertex position
    float4 Color : COLOR0; // Vertex color
};

// Output structure for the vertex shader
struct VertexShaderOutput
{
    float4 Position : SV_POSITION; // Clip space position
    float4 Color : COLOR0; // Color to pass to pixel shader
    float3 Normal : NORMAL0; // Normal vector for lighting calculations
};

// Vertex shader
VertexShaderOutput VS(VertexShaderInput input)
{
    VertexShaderOutput output;

    // Transform the vertex position using the WorldViewProjection matrix
    output.Position = mul(input.Position, WorldViewProjection);

    // Pass through the color
    output.Color = input.Color;

    // Calculate normal (assuming vertices are in the XY plane and normals are (0, 0, 1))
    output.Normal = float3(0, 0, 1);

    return output;
}

// Pixel shader
float4 PS(VertexShaderOutput input) : SV_Target
{
    // Ambient light contribution
    float3 ambient = AmbientColor.rgb;

    // Compute light direction
    float3 lightDir = normalize(LightPosition.xyz - input.Position.xyz);

    // Compute diffuse contribution (using Lambert's cosine law)
    float diff = max(dot(input.Normal, lightDir), 0.0);
    float3 diffuse = diff * DiffuseColor.rgb;

    // Compute view direction
    float3 viewDir = normalize(CameraPosition.xyz - input.Position.xyz);

    // Compute reflection vector
    float3 reflectDir = reflect(-lightDir, input.Normal);
    
    // Compute specular contribution using Blinn-Phong shading
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), Shininess);
    float3 specular = spec * SpecularColor.rgb;

    // Combine the results
    float3 finalColor = ambient + diffuse + specular;

    return float4(finalColor, 1.0); // Return the final color with full alpha
}

// Techniques and passes
technique SimpleTechnique
{
    pass P0
    {
        VertexShader = compile vs_4_0 VS();
        PixelShader = compile ps_4_0 PS();
    }
}
