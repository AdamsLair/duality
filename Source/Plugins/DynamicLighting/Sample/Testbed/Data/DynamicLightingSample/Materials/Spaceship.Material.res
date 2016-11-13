<root dataType="Struct" type="Duality.Resources.Material" id="129723834">
  <info dataType="Struct" type="Duality.Resources.BatchInfo" id="427169525">
    <dirtyFlag dataType="Enum" type="Duality.Resources.BatchInfo+DirtyFlag" name="None" value="0" />
    <hashCode dataType="Int">1398069199</hashCode>
    <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
      <A dataType="Byte">255</A>
      <B dataType="Byte">255</B>
      <G dataType="Byte">255</G>
      <R dataType="Byte">255</R>
    </mainColor>
    <technique dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.DrawTechnique]]">
      <contentPath dataType="String">Data\DynamicLightingSample\PerPixelLighting\LightMask.LightingTechnique.res</contentPath>
    </technique>
    <textures dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[Duality.ContentRef`1[[Duality.Resources.Texture]]]]" id="1100841590" surrogate="true">
      <header />
      <body>
        <mainTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
          <contentPath dataType="String">Data\DynamicLightingSample\Materials\ship_c.Texture.res</contentPath>
        </mainTex>
        <normalTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
          <contentPath dataType="String">Data\DynamicLightingSample\Materials\ship_n.Texture.res</contentPath>
        </normalTex>
        <specularTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
          <contentPath dataType="String">Data\DynamicLightingSample\Materials\ship_s.Texture.res</contentPath>
        </specularTex>
      </body>
    </textures>
    <uniforms dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[System.Single[]]]" id="649525530" surrogate="true">
      <header />
      <body>
        <_camRefDist dataType="Array" type="System.Single[]" id="411997508" length="1" />
        <_camWorldPos dataType="Array" type="System.Single[]" id="1885627030" length="3" />
        <_lightCount dataType="Array" type="System.Single[]" id="766962944" length="1" />
        <_lightPos dataType="Array" type="System.Single[]" id="1352645666" length="32" />
        <_lightDir dataType="Array" type="System.Single[]" id="3754462812" length="32" />
        <_lightColor dataType="Array" type="System.Single[]" id="3208881918" length="24" />
      </body>
    </uniforms>
  </info>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
