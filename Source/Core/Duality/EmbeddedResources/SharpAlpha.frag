uniform sampler2D mainTex;
uniform float smoothness;

in vec4 programColor;
in vec2 programTexCoord;

out vec4 fragColor;

const float Gamma = 2.2;

void main()
{
	// Retrieve base color
	vec4 texClr = texture(mainTex, programTexCoord);
	
	// Do some anti-aliazing
	float w = clamp(smoothness * (abs(dFdx(programTexCoord.x)) + abs(dFdy(programTexCoord.y))), 0.0, 0.5);
	float a = smoothstep(0.5 - w, 0.5 + w, texClr.a);

	// Perform Gamma Correction to achieve a linear attenuation
	texClr.a = pow(a, 1.0 / Gamma);

	// Compose result color
	vec4 result = programColor * texClr;
	AlphaTest(result.a);
	fragColor = result;
}