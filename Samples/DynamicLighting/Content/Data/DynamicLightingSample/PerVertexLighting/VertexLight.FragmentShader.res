<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo />
  <source dataType="String">uniform sampler2D mainTex;

in vec4 programColor;
in vec2 programTexCoord;
in vec3 lightIntensity;

out vec4 fragColor;

void main()
{
	vec4 color = programColor * texture(mainTex, programTexCoord);
	color.rgb *= lightIntensity;
	AlphaTest(color.a);
	fragColor = color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
