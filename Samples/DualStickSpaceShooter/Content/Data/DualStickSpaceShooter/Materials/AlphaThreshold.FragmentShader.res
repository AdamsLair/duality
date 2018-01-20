<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo />
  <source dataType="String">uniform sampler2D mainTex;
uniform float threshold;

in vec4 programColor;
in vec2 programTexCoord;

out vec4 fragColor;

void main()
{
	// Determine alpha value by thresholding the textures (monochrome) brightness value
	vec4 texColor = texture(mainTex, programTexCoord);
	float alpha = smoothstep(threshold - 0.2, threshold, texColor.a);
	vec4 result = vec4(programColor.rgb, programColor.a * alpha);
	AlphaTest(result.a);
	fragColor = result;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
