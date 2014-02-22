uniform sampler2D mainTex;

void main()
{
	gl_FragColor = gl_Color * texture2D(mainTex, gl_TexCoord[0].st);
}