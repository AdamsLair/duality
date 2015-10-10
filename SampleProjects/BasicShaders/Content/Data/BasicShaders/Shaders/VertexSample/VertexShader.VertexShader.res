<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">VertexShader</nameHint>
  </assetInfo>
  <source dataType="String">uniform float GameTime;
uniform float CameraFocusDist;
uniform bool CameraParallax;

uniform float FloatStrength;

void main()
{
	// Duality uses software pre-transformation of vertices
	// gl_Vertex is already in parallax (scaled) view space when arriving here.
	vec4 vertex = gl_Vertex;
	
	// Reverse-engineer the scale that was previously applied to the vertex
	float scale = 1.0;
	if (CameraParallax)
	{
		scale = CameraFocusDist / vertex.z;
	}
	
	// Move the vertex around, keeping scale in mind
	vertex.xy += FloatStrength * scale * vec2(
		sin(GameTime + mod(gl_VertexID, 4)), 
		cos(GameTime + mod(gl_VertexID, 4)));
	
	gl_Position = gl_ProjectionMatrix * gl_ModelViewMatrix * vertex;
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
