uniform sampler2D mainTex;
varying float animBlendVar;

void main()
{
	// Retrieve frames
	vec4 texClrOld = texture2D(mainTex, gl_TexCoord[0].st);
	vec4 texClrNew = texture2D(mainTex, gl_TexCoord[0].pq);

	// This code prevents nasty artifacts when blending between differently masked frames
	float accOldNew = (texClrOld.w - texClrNew.w) / (texClrOld.w + texClrNew.w);
	accOldNew *= mix(min(min(animBlendVar, 1.0 - animBlendVar) * 4.0, 1.0), 1.0, abs(accOldNew));
	texClrNew.xyz = mix(texClrNew.xyz, texClrOld.xyz, max(accOldNew, 0.0));
	texClrOld.xyz = mix(texClrOld.xyz, texClrNew.xyz, max(-accOldNew, 0.0));

	// Blend between frames
	gl_FragColor = gl_Color * mix(texClrOld, texClrNew, animBlendVar);
}