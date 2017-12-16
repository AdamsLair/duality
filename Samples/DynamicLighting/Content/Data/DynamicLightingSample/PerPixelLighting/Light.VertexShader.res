<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint />
  </assetInfo>
  <source dataType="String">uniform float _CameraFocusDist;
uniform vec3 _CameraPosition;

varying vec3 worldSpacePos;
varying mat2 objTransform;

attribute vec4 objTrAttrib;

void main()
{
	gl_Position = TransformWorldToClip(gl_Vertex);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	
	float camDistScaleInv = gl_Vertex.z / _CameraFocusDist;
	worldSpacePos = _CameraPosition + vec3(gl_Vertex.xy * camDistScaleInv, gl_Vertex.z);
	
	objTransform = mat2(objTrAttrib.x, objTrAttrib.y, objTrAttrib.z, objTrAttrib.w);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
