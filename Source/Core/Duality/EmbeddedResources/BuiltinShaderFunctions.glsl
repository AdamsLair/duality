// By default, we assume GLSL version 1.2, which corresponds to OpenGL 2.1.
// You can override this on a per-shader basis by adding your own version directive.
#version 120

// Transforms the specified vertex from world space into clip space.
// (Clip space is the expected vertex shader output)
vec4 TransformWorldToClip(vec4 worldPos)
{
	return gl_ModelViewProjectionMatrix * worldPos;
}

// Transforms the specified vertex from world space into view space.
vec4 TransformWorldToView(vec4 worldPos)
{
	return gl_ModelViewMatrix * worldPos;
}

// Transforms the specified vertex from view space into clip space.
// (Clip space is the expected vertex shader output)
vec4 TransformViewToClip(vec4 viewPos)
{
	return gl_ProjectionMatrix * viewPos;
}