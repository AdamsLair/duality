<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo />
  <source dataType="String">uniform sampler2D mainTex;

varying vec4 programColor;
varying vec2 programTexCoord;
varying vec3 lightIntensity;

void main()
{
	vec4 color = programColor * texture2D(mainTex, programTexCoord);
	color.rgb *= lightIntensity;
	AlphaTest(color.a);
	gl_FragColor = color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
