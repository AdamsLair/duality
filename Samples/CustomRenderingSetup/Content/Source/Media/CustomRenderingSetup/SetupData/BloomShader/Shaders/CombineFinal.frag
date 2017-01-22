uniform sampler2D mainTex;
uniform sampler2D blurFullTex;
uniform sampler2D blurHalfTex;
uniform sampler2D blurQuarterTex;
uniform sampler2D blurEighthTex;

void main()
{
	vec4 main = texture2D(mainTex, gl_TexCoord[0].st);
	vec4 blur0 = texture2D(blurFullTex, gl_TexCoord[0].st);
	vec4 blur1 = texture2D(blurHalfTex, gl_TexCoord[0].st);
	vec4 blur2 = texture2D(blurQuarterTex, gl_TexCoord[0].st);
	vec4 blur3 = texture2D(blurEighthTex, gl_TexCoord[0].st);
	
	vec4 result = main + (blur0 + blur1 + blur2 + blur3);
	gl_FragColor = gl_Color * vec4(result.rgb, main.a);
}