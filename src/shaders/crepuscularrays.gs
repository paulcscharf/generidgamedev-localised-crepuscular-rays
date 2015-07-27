#version 150

// layout
layout (points) in;
layout (triangle_strip, max_vertices = 4) out;


// in
in vec3 g_position[];
in float g_radius[];
in float g_alpha[];

// out
out vec2 p_position;
out vec2 p_uvOffset;
out vec2 p_uvCenter;
out float p_alpha;


void main()
{
	vec4 p = vec4(g_position[0], 1);

	float r = g_radius[0];

	p_uvCenter = p.xy / p.w * 0.5 + 0.5;
	p_alpha = g_alpha[0];

	vec4 screenPosition = p + vec4(-r, -r, 0, 0);
	gl_Position = screenPosition;
	p_position = vec2(-1, -1);
	p_uvOffset = screenPosition.xy / screenPosition.w * 0.5 + 0.5 - p_uvCenter;
    EmitVertex();

	screenPosition = p + vec4(r, -r, 0, 0);
	gl_Position = screenPosition;
	p_position = vec2(1, -1);
	p_uvOffset = screenPosition.xy / screenPosition.w * 0.5 + 0.5 - p_uvCenter;
    EmitVertex();

	screenPosition = p + vec4(-r, r, 0, 0);
	gl_Position = screenPosition;
	p_position = vec2(-1, 1);
	p_uvOffset = screenPosition.xy / screenPosition.w * 0.5 + 0.5 - p_uvCenter;
    EmitVertex();

	screenPosition = p + vec4(r, r, 0, 0);
	gl_Position = screenPosition;
	p_position = vec2(1, 1);
	p_uvOffset = screenPosition.xy / screenPosition.w * 0.5 + 0.5 - p_uvCenter;
    EmitVertex();

    EndPrimitive();
}