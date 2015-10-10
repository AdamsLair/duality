<root dataType="Struct" type="Duality.Resources.VertexShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <nameHint dataType="String">VertexShader</nameHint>
  </assetInfo>
  <source dataType="String">uniform float time;
uniform mat4 gl_ModelViewMatrix;
uniform mat4 gl_ProjectionMatrix;
attribute vec4 gl_Vertex;

void main() {
  vec4 v = gl_Vertex;
  //v.x *= abs(sin(gl_Vertex.y - 0.5) * mod(time, 1.0));
  //v.y *= abs(sin(gl_Vertex.x - 0.5) * mod(time, 1.0));
  v.x *= gl_Vertex.y / 20 * sin(time / 2);
  v.y *= 25 / gl_Vertex.x * sin(time);
  gl_Position = gl_ProjectionMatrix * gl_ModelViewMatrix * v;
  gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
