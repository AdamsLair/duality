uniform sampler2D mainTex;

void main()
{
	float texAlpha = texture2D(mainTex, gl_TexCoord[0].st).a;
	vec4 result = vec4(gl_Color.rgb, step(0.5, texAlpha));

	AlphaTest(result.a);

	gl_FragColor = result;
}