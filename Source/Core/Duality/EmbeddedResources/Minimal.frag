uniform sampler2D mainTex;

void main()
{
	vec4 texClr = texture2D(mainTex, gl_TexCoord[0].st);
	vec4 result = gl_Color * texClr;

	AlphaTest(result.a);

	gl_FragColor = result;
}