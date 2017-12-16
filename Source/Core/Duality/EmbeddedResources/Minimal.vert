uniform vec4 mainColor;

attribute float VertexDepthOffset;

void main()
{
	gl_Position = TransformVertexDefault(gl_Vertex.xyz, VertexDepthOffset);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color * mainColor;
}