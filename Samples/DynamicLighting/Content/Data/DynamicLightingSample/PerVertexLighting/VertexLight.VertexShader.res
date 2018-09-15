<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo />
  <source dataType="String">in vec3 vertexPos;
in vec4 vertexColor;
in vec2 vertexTexCoord;
in float vertexDepthOffset;
in vec4 vertexLightingParam;

out vec4 programColor;
out vec2 programTexCoord;
out vec3 lightIntensity;

void main()
{
	gl_Position = TransformVertexDefault(vertexPos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor;
	lightIntensity = vertexLightingParam.xyz;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
