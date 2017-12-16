// By default, we assume GLSL version 1.2, which corresponds to OpenGL 2.1.
// You can override this on a per-shader basis by adding your own version directive.
#version 120

// Transforms the specified vertex from world space into clip space.
// (Clip space is the expected vertex shader output)
vec4 TransformWorldToClip(vec4 pos)
{
	return gl_ModelViewProjectionMatrix * pos;
}

// Transforms the specified vertex from world space into view space.
vec4 TransformWorldToView(vec4 pos)
{
	return gl_ModelViewMatrix * pos;
}

// Transforms the specified vertex from view space into clip space.
// (Clip space is the expected vertex shader output)
vec4 TransformViewToClip(vec4 pos)
{
	return gl_ProjectionMatrix * pos;
}