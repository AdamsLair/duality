uniform sampler2D mainTex;

in vec4 programColor;
in vec2 programTexCoord;

out vec4 fragColor;

void main()
{
	float texAlpha = texture(mainTex, programTexCoord).a;
	vec4 result = vec4(programColor.rgb, step(0.5, texAlpha));

	AlphaTest(result.a);

	fragColor = result;
}