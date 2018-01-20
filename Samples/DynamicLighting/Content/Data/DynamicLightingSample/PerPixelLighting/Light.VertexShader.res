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
in vec2 vertexTexCoord;
in float vertexDepthOffset;
in vec4 vertexLightingParam;

out vec4 programColor;
out vec2 programTexCoord;
out vec3 worldSpacePos;
out mat2 objTransform;

void main()
{
	worldSpacePos = vertexPos;
	
	gl_Position = TransformVertexDefault(worldSpacePos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor;
	
	objTransform = mat2(
		vertexLightingParam.x, 
		vertexLightingParam.y, 
		vertexLightingParam.z, 
		vertexLightingParam.w);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
