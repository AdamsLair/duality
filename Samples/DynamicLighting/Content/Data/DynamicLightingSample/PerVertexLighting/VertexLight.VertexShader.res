<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo />
  <source dataType="String">attribute float vertexDepthOffset;
attribute vec4 vertexLightingParam;

varying vec3 lightIntensity;

void main()
{
	gl_Position = TransformVertexDefault(gl_Vertex.xyz, vertexDepthOffset);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	lightIntensity = vertexLightingParam.xyz;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
