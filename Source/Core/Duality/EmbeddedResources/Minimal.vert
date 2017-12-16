uniform vec4 mainColor;

void main()
{
	gl_Position = TransformWorldToClip(gl_Vertex);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color * mainColor;
}