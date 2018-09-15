<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.vert</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">in vec3 vertexPos;
in vec4 vertexColor;
in vec4 vertexTexCoord;
in float vertexDepthOffset;
in float vertexAnimBlend;

out vec4 programColor;
out vec4 programTexCoord;
out float animBlendVar;

void main()
{
	gl_Position = TransformVertexDefault(vertexPos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor;
	animBlendVar = vertexAnimBlend;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
