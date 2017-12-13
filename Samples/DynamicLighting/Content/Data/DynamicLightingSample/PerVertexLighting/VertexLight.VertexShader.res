<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo />
  <source dataType="String">attribute vec4 lightAttrib;

varying vec3 lightIntensity;

vec4 _transformWorldToClip(vec4 pos);

void main()
{
	gl_Position = _transformWorldToClip(gl_Vertex);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	lightIntensity = lightAttrib.xyz;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
