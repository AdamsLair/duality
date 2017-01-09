<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint />
  </assetInfo>
  <source dataType="String">uniform float GameTime;

uniform sampler2D mainTex;
uniform float ColorShiftSpeed;

void main()
{
    vec2 texCoord = gl_TexCoord[0].st;
	texCoord += 0.1 * vec2(
		sin(GameTime + gl_FragCoord.x * 0.01),
		cos(GameTime + gl_FragCoord.y * 0.01));
	
	vec4 color = texture2D(mainTex, texCoord);
	color.rgb = vec3(
		color.r * sin(ColorShiftSpeed * GameTime), 
		color.g * sin(ColorShiftSpeed * GameTime * 0.5), 
		color.b * sin(ColorShiftSpeed * GameTime * 0.25));
	gl_FragColor = color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
