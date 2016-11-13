uniform sampler2D mainTex;

void main()
{
	gl_FragColor = vec4(gl_Color.rgb, step(0.5, texture2D(mainTex, gl_TexCoord[0].st).a));
}