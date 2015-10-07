<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">VertexShader</nameHint>
  </assetInfo>
  <source dataType="String">void main() {

	gl_Position = ftransform() * vec4(sin(gl_Vertex.x), cos(gl_Vertex.x), 1.0, 1.0);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
