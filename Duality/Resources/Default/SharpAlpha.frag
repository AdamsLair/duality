uniform sampler2D mainTex;
uniform float smoothness;

const float Gamma = 2.2;

void main()
{
	// Retrieve base color
	vec4 texClr = texture2D(mainTex, gl_TexCoord[0].st);
	
	// Do some anti-aliazing
	float w = clamp(smoothness * (abs(dFdx(gl_TexCoord[0].s)) + abs(dFdy(gl_TexCoord[0].t))), 0.0, 0.5);
	float a = smoothstep(0.5 - w, 0.5 + w, texClr.a);

	// Perform Gamma Correction to achieve a linear attenuation
	texClr.a = pow(a, 1.0 / Gamma);

	// Compose result color
	gl_FragColor = texClr * gl_Color; 
}