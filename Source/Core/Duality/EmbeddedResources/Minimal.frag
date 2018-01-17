uniform sampler2D mainTex;

varying vec4 programColor;
varying vec2 programTexCoord;

void main()
{
	vec4 texClr = texture2D(mainTex, programTexCoord);
	vec4 result = programColor * texClr;

	AlphaTest(result.a);

	gl_FragColor = result;
}