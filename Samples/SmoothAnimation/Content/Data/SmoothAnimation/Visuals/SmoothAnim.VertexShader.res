<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.vert</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">attribute vec3 vertexPos;
attribute vec4 vertexColor;
attribute vec4 vertexTexCoord;
attribute float vertexDepthOffset;
attribute float vertexAnimBlend;

varying vec4 programColor;
varying vec4 programTexCoord;
varying float animBlendVar;

void main()
{
	gl_Position = TransformVertexDefault(vertexPos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor;
	animBlendVar = vertexAnimBlend;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
