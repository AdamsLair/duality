uniform vec4 mainColor;

attribute vec3 vertexPos;
attribute vec4 vertexColor;
attribute vec2 vertexTexCoord;
attribute float vertexDepthOffset;

varying vec4 programColor;
varying vec2 programTexCoord;

void main()
{
	gl_Position = TransformVertexDefault(vertexPos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor * mainColor;
}