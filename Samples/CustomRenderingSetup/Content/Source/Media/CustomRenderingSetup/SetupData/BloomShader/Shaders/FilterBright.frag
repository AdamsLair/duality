uniform sampler2D mainTex;
uniform float minBrightness;

void main()
{
	vec4 color = texture2D(mainTex, gl_TexCoord[0].st);
	color.rgb = max(color.rgb - vec3(1, 1, 1) * minBrightness, 0);
	gl_FragColor = gl_Color * color;
}