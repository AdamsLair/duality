uniform sampler2D mainTex;
uniform vec2 blurDirection;

void main()
{
	ivec2 texSize = textureSize(mainTex, 0);
	vec2 pixelOffset = vec2(1.0 / texSize.x, 1.0 / texSize.y) * blurDirection;
	vec4 samples = vec4(0, 0, 0, 0);
	samples += texture2D(mainTex, gl_TexCoord[0].st - pixelOffset * 3) * 1.0 / 64.0;
	samples += texture2D(mainTex, gl_TexCoord[0].st - pixelOffset * 2) * 6.0 / 64.0;
	samples += texture2D(mainTex, gl_TexCoord[0].st - pixelOffset * 1) * 15.0 / 64.0;
	samples += texture2D(mainTex, gl_TexCoord[0].st + pixelOffset * 0) * 20.0 / 64.0;
	samples += texture2D(mainTex, gl_TexCoord[0].st + pixelOffset * 1) * 15.0 / 64.0;
	samples += texture2D(mainTex, gl_TexCoord[0].st + pixelOffset * 2) * 6.0 / 64.0;
	samples += texture2D(mainTex, gl_TexCoord[0].st + pixelOffset * 3) * 1.0 / 64.0;
	gl_FragColor = gl_Color * samples;
}