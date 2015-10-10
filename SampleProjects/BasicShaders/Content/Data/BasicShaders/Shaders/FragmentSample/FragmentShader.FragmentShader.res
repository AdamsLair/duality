<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">FragmentShader</nameHint>
  </assetInfo>
  <source dataType="String">uniform float GameTime;
uniform float CameraFocusDist;
uniform vec3 CameraPosition;
uniform bool CameraParallax;

uniform sampler2D mainTex;
uniform float ColorShiftSpeed;

void main()
{
	vec4 color = texture2D(mainTex, gl_TexCoord[0].st);
	color.rgb = vec3(
		color.r * sin(ColorShiftSpeed * GameTime), 
		color.g * sin(ColorShiftSpeed * GameTime * 0.5), 
		color.b * sin(ColorShiftSpeed * GameTime * 0.25));
	gl_FragColor = color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
