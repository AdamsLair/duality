<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo />
  <source dataType="String">uniform sampler2D mainTex;

varying vec3 lightIntensity;

void main()
{
	vec4 color = gl_Color * texture2D(mainTex, gl_TexCoord[0].st);
	color.rgb *= lightIntensity;
	gl_FragColor = color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
