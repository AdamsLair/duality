<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.frag</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">uniform float _gameTime;

uniform sampler2D mainTex;
uniform float colorShiftSpeed;

varying vec4 programColor;
varying vec2 programTexCoord;

void main()
{
    vec2 texCoord = programTexCoord;
	texCoord += 0.1 * vec2(
		sin(_gameTime + programTexCoord.x),
		cos(_gameTime + programTexCoord.y));
	
	vec4 color = texture2D(mainTex, texCoord);
	color.rgb = vec3(
		color.r * sin(colorShiftSpeed * _gameTime), 
		color.g * sin(colorShiftSpeed * _gameTime * 0.5), 
		color.b * sin(colorShiftSpeed * _gameTime * 0.25));
	color *= programColor;
	
	AlphaTest(color.a);
	gl_FragColor = color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
