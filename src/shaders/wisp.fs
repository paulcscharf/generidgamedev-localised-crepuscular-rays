#version 130

// uniforms

// varyings
in vec2 fUV;
in vec4 fColor;

// output
out vec4 fragColor;

void main()
{
	vec2 uv = fUV * 2 - vec2(1, 1);

	float dSquared = dot(uv, uv);

	if (dSquared > 1)
		discard;

	float a = 1 - dSquared;

	a *= a * a;

	fragColor = fColor * a;
}