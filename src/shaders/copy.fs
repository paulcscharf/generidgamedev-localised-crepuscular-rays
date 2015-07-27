#version 130

// uniforms
uniform sampler2D diffuseTexture;
uniform vec4 color;

// varyings
in vec2 fUV;

// output
out vec4 fragColor;

void main()
{
	fragColor = texture(diffuseTexture, fUV) * color;
}