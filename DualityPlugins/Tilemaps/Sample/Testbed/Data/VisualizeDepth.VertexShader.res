<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">VisualizeDepth</nameHint>
  </assetInfo>
  <source dataType="String">uniform vec3 CameraPosition;
varying vec4 originalVertex;

void main()
{
	originalVertex = gl_Vertex;
	originalVertex.z += CameraPosition.z;
	
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
