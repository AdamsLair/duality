<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <source dataType="String">attribute vec4 lightAttrib;

varying vec3 lightIntensity;

void main()
{
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	lightIntensity = lightAttrib.xyz;
}</source>
  <sourcePath dataType="String">Source\Media\DynamicLightingSample\PerVertexLighting\VertexLight.vert</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
