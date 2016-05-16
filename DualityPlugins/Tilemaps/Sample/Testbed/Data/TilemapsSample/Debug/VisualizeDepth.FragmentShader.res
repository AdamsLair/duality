<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.frag</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">uniform sampler2D mainTex;

varying vec4 originalVertex;

void main()
{
	vec4 texColor = texture2D(mainTex, gl_TexCoord[0].st);
	gl_FragColor = vec4(
		0,
		originalVertex.z / 10,
		0,
		texColor.a);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
