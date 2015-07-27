#version 150

// uniforms

// attributes
in vec3 v_position;
in float v_radius;
in float v_alpha;

// varyings
out vec3 g_position;
out float g_radius;
out float g_alpha;

void main()
{
	g_position = v_position;
	g_radius = v_radius;
	g_alpha = v_alpha;
}