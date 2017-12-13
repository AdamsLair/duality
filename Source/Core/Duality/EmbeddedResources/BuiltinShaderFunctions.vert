
//
// Transforms the specified vertex from world space into clip space.
// (Clip space is the expected vertex shader output)
//
vec4 _transformWorldToClip(vec4 pos)
{
	return gl_ModelViewProjectionMatrix * pos;
}

//
// Transforms the specified vertex from world space into view space.
//
vec4 _transformWorldToView(vec4 pos)
{
	return gl_ModelViewMatrix * pos;
}

//
// Transforms the specified vertex from view space into clip space.
// (Clip space is the expected vertex shader output)
//
vec4 _transformViewToClip(vec4 pos)
{
	return gl_ProjectionMatrix * pos;
}