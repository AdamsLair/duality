uniform sampler2D mainTex;

void main()
{
	vec4 texClr = texture2D(mainTex, gl_TexCoord[0].st);
	gl_FragColor = gl_Color * texClr;
}