<?xml version="1.0" encoding="utf-8"?>
<root>
  <object dataType="Class" type="Duality.Resources.FragmentShader" id="1">
    <source dataType="String">uniform sampler2D mainTex;

varying vec3 lightIntensity;

void main()
{
	vec4 color = gl_Color * texture2D(mainTex, gl_TexCoord[0].st);
	color.rgb *= lightIntensity;
	gl_FragColor = color;
}</source>
    <sourcePath dataType="String">Source\Media\PerVertexLighting\VertexLight.frag</sourcePath>
  </object>
</root>