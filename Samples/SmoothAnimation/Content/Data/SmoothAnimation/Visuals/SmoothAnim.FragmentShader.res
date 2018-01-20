<root dataType="Struct" type="Duality.Resources.FragmentShader" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <customData />
    <importerId dataType="String">BasicShaderAssetImporter</importerId>
    <sourceFileHint dataType="Array" type="System.String[]" id="1100841590">
      <item dataType="String">{Name}.frag</item>
    </sourceFileHint>
  </assetInfo>
  <source dataType="String">uniform sampler2D mainTex;

in vec4 programColor;
in vec4 programTexCoord;
in float animBlendVar;

out vec4 fragColor;

void main()
{
	// Retrieve frames
	vec4 texClrOld = texture(mainTex, programTexCoord.xy);
	vec4 texClrNew = texture(mainTex, programTexCoord.zw);

	// This code prevents nasty artifacts when blending between differently masked frames
	float accOldNew = (texClrOld.w - texClrNew.w) / (texClrOld.w + texClrNew.w);
	accOldNew *= mix(min(min(animBlendVar, 1.0 - animBlendVar) * 4.0, 1.0), 1.0, abs(accOldNew));
	texClrNew.xyz = mix(texClrNew.xyz, texClrOld.xyz, max(accOldNew, 0.0));
	texClrOld.xyz = mix(texClrOld.xyz, texClrNew.xyz, max(-accOldNew, 0.0));

	// Blend between frames
	vec4 result = programColor * mix(texClrOld, texClrNew, animBlendVar);
	
	AlphaTest(result.a);
	fragColor = result;
}</source>
</root>
<!-- XmlFormatterBase Document Separator -->
