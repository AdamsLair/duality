<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">VisualizeDepth</nameHint>
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
