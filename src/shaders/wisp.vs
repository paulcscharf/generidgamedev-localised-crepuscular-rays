#version 130

// attributes
in vec3 v_position;
in vec2 v_texcoord;
in vec4 v_color;

// varyings
out vec2 fUV;
out vec4 fColor;

void main()
{
	gl_Position = vec4(v_position, 1);
	fUV = v_texcoord;
	fColor = v_color;
}