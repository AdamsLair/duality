<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo />
  <source dataType="String">uniform sampler2D mainTex;
uniform float threshold;

void main()
{
	// Determine alpha value by thresholding the textures (monochrome) brightness value
	vec4 texColor = texture2D(mainTex, gl_TexCoord[0].st);
	float alpha = smoothstep(threshold - 0.2, threshold, texColor.a);
	vec4 result = vec4(gl_Color.rgb, gl_Color.a * alpha);
	AlphaTest(result.a);
	gl_FragColor = result);
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
