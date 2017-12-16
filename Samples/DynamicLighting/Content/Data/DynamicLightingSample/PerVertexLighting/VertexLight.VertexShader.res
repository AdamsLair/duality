<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo />
  <source dataType="String">attribute vec4 VertexLightingParam;

varying vec3 lightIntensity;

void main()
{
	gl_Position = TransformWorldToClip(gl_Vertex);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	lightIntensity = VertexLightingParam.xyz;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
