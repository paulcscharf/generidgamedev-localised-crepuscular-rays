#version 150

// uniforms
uniform sampler2D diffuseTexture;

// varyings
in vec2 p_position;
in vec2 p_uvOffset;
in vec2 p_uvCenter;
in float p_alpha;

// output
out vec4 fragColor;

void main()
{
	float d = length(p_position);

	if (d >= 1)
		discard;

	int samples = 100;
	float sampleStep = 1.0 / samples;

	vec4 argb = vec4(0);

	vec2 uvStep = -p_uvOffset * sampleStep;

	vec2 uv = p_uvCenter + p_uvOffset;

	float decay = 1;
	float decayStep = 1;

	for (int i = 0; i < samples; i++)
	{
		vec4 texel = texture(diffuseTexture, uv) - vec4(0.1);

		argb += texel * decay;

		uv += uvStep;

		decay *= decayStep;
	}

	argb *= sampleStep;

	float a = p_alpha * (1 - d);
	
	fragColor = vec4(argb.xyz * a, 0);
}