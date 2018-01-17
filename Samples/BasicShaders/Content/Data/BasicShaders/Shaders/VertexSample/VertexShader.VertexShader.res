<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.vert</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">uniform vec4 mainColor;
uniform float floatStrength;
uniform float _gameTime;

attribute vec3 vertexPos;
attribute vec4 vertexColor;
attribute vec2 vertexTexCoord;
attribute float vertexDepthOffset;

varying vec4 programColor;
varying vec2 programTexCoord;

void main()
{
	// Duality submits vertices in world space coordinates
	vec3 worldPos = vertexPos;
	
	// Let the vertex float a bit over time, depending on its 
	// original world space position
	worldPos.xyz += floatStrength * vec3(
		sin(0.00 * 3.14 + _gameTime + worldPos.y * 0.01), 
		sin(0.25 * 3.14 + _gameTime + worldPos.x * 0.01), 
		sin(0.50 * 3.14 + _gameTime + worldPos.y * 0.01));
	
	gl_Position = TransformVertexDefault(worldPos, vertexDepthOffset);
	programTexCoord = vertexTexCoord;
	programColor = vertexColor * mainColor;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
