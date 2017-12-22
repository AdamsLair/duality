<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.vert</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">uniform float _CameraFocusDist;
uniform vec3 _CameraPosition;

varying vec3 worldSpacePos;
varying mat2 objTransform;

attribute float VertexDepthOffset;
attribute vec4 VertexLightingParam;

void main()
{
	worldSpacePos = gl_Vertex.xyz;
	
	gl_Position = TransformVertexDefault(worldSpacePos, VertexDepthOffset);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	
	objTransform = mat2(
		VertexLightingParam.x, 
		VertexLightingParam.y, 
		VertexLightingParam.z, 
		VertexLightingParam.w);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
