uniform vec4 mainColor;

attribute float vertexDepthOffset;

void main()
{
	gl_Position = TransformVertexDefault(gl_Vertex.xyz, vertexDepthOffset);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color * mainColor;
}