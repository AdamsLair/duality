uniform sampler2D mainTex;

varying vec4 programColor;
varying vec2 programTexCoord;

void main()
{
	float texAlpha = texture2D(mainTex, programTexCoord).a;
	vec4 result = vec4(programColor.rgb, step(0.5, texAlpha));

	AlphaTest(result.a);

	gl_FragColor = result;
}