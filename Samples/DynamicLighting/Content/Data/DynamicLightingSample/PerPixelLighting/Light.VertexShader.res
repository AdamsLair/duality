<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.vert</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">varying vec3 worldSpacePos;
varying mat2 objTransform;

attribute float vertexDepthOffset;
attribute vec4 vertexLightingParam;

void main()
{
	worldSpacePos = gl_Vertex.xyz;
	
	gl_Position = TransformVertexDefault(worldSpacePos, vertexDepthOffset);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	
	objTransform = mat2(
		vertexLightingParam.x, 
		vertexLightingParam.y, 
		vertexLightingParam.z, 
		vertexLightingParam.w);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
