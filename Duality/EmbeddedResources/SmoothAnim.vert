attribute float animBlend;
varying float animBlendVar;

void main()
{
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	animBlendVar = animBlend;
}