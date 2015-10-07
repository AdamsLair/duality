<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">FragmentShader</nameHint>
  </assetInfo>
  <source dataType="String">uniform sampler2D mainTex;
uniform float time;
varying float test;

out vec4 colorOut;

void main()
{
	//gl_FragColor = vec4(0.1, 0.1, 0.1, 1.0) * texture2D(mainTex, gl_TexCoord[0].st);
  float red = (sin(time) + 1) / 2.0;
  float green = (cos(time) + 1) / 2.0;
  colorOut = vec4(red, green, 0.0, 1.0) * texture2D(mainTex, gl_TexCoord[0].st);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
