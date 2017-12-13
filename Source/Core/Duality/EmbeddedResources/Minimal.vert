uniform vec4 mainColor;

vec4 _transformWorldToClip(vec4 pos);

void main()
{
	gl_Position = _transformWorldToClip(gl_Vertex);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color * mainColor;
}