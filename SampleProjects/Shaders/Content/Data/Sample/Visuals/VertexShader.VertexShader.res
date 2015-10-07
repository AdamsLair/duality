<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">VertexShader</nameHint>
  </assetInfo>
  <source dataType="String">uniform float time;
varying float test;

void main() {
  gl_Position = ftransform();
  //gl_Position = gl_Position * vec4((sin(time) + 1.0) / 2.0 * sin(gl_Position.y), 1.0, 1.0, 1.0);
  //gl_Position = gl_Position * vec4(sin(time), cos(time), 1.0, 1.0);
  //gl_Position = gl_Position * vec4(gl_Position.y, 1.0, 1.0, 1.0);
  gl_Position.x = gl_Position.x + 0.1;
  gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
  
  test = time;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
