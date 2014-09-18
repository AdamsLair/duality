<?xml version="1.0" encoding="utf-8"?>
<root>
  <object dataType="Class" type="Duality.Resources.VertexShader" id="1">
    <source dataType="String">attribute vec4 lightAttrib;

varying vec3 lightIntensity;

void main()
{
	gl_Position = ftransform();
	gl_TexCoord[0] = gl_MultiTexCoord0;
	gl_FrontColor = gl_Color;
	lightIntensity = lightAttrib;
}</source>
    <sourcePath dataType="String">Source\Media\PerVertexLighting\Light (2).vert</sourcePath>
  </object>
</root>