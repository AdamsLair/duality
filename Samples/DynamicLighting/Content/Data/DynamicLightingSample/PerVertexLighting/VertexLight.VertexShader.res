<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo />
  <source dataType="String">attribute vec3 vertexPos;
attribute vec4 vertexColor;
attribute vec2 vertexTexCoord;
attribute float vertexDepthOffset;
attribute vec4 vertexLightingParam;

varying vec4 programColor;
varying vec2 programTexCoord;
varying vec3 lightIntensity;

void main()
{
	gl_Position = TransformVertexDefault(vertexPos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor;
	lightIntensity = vertexLightingParam.xyz;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
