<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">Light</nameHint>
  </assetInfo>
  <source dataType="String">uniform float CameraFocusDist;
uniform vec3 CameraPosition;

varying vec3 worldSpacePos;
varying mat2 objTransform;

attribute vec4 objTrAttrib;

void main()
{
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	
	float camDistScaleInv = gl_Vertex.z / CameraFocusDist;
	worldSpacePos = CameraPosition + vec3(gl_Vertex.xy * camDistScaleInv, gl_Vertex.z);
	
	objTransform = mat2(objTrAttrib.x, objTrAttrib.y, objTrAttrib.z, objTrAttrib.w);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
