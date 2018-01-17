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
attribute vec2 vertexTexCoord;
attribute float vertexDepthOffset;
attribute vec4 vertexLightingParam;

varying vec4 programColor;
varying vec2 programTexCoord;
varying vec3 worldSpacePos;
varying mat2 objTransform;

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
