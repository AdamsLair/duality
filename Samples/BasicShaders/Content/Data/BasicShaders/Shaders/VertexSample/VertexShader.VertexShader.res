<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.vert</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">uniform vec4 mainColor;
uniform float FloatStrength;
uniform float _GameTime;

attribute float VertexDepthOffset;

void main()
{
	// Duality submits vertices in world space coordinates
	vec3 worldPos = gl_Vertex.xyz;
	
	// Let the vertex float a bit over time, depending on its 
	// original world space position
	worldPos.xyz += FloatStrength * vec3(
		sin(0.00 * 3.14 + _GameTime + worldPos.y * 0.01), 
		sin(0.25 * 3.14 + _GameTime + worldPos.x * 0.01), 
		sin(0.50 * 3.14 + _GameTime + worldPos.y * 0.01));
	
	gl_Position = TransformVertexDefault(worldPos, VertexDepthOffset);
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color * mainColor;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
