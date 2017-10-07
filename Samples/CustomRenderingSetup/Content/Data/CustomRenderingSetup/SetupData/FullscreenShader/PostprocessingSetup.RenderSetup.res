<root dataType="Struct" type="Duality.Resources.RenderSetup" id="129723834">
  <assetInfo />
  <autoResizeTargets dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.RenderSetupTargetResize]]" id="427169525">
    <_items dataType="Array" type="Duality.Resources.RenderSetupTargetResize[]" id="1100841590" length="4">
      <item dataType="Struct" type="Duality.Resources.RenderSetupTargetResize">
        <ResizeMode dataType="Enum" type="Duality.TargetResize" name="Stretch" value="1" />
        <Scale dataType="Struct" type="Duality.Vector2">
          <X dataType="Float">1</X>
          <Y dataType="Float">1</Y>
        </Scale>
        <Target dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
          <contentPath dataType="String">Data\CustomRenderingSetup\SetupData\FullscreenShader\MainTarget.RenderTarget.res</contentPath>
        </Target>
      </item>
    </_items>
    <_size dataType="Int">1</_size>
  </autoResizeTargets>
  <steps dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.RenderStep]]" id="2035693768">
    <_items dataType="Array" type="Duality.Resources.RenderStep[]" id="2696347487" length="4">
      <item dataType="Struct" type="Duality.Resources.RenderStep" id="1485019246">
        <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
        <clearDepth dataType="Float">1</clearDepth>
        <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
        <defaultClearColor dataType="Bool">true</defaultClearColor>
        <id dataType="String">World</id>
        <input />
        <inputResize dataType="Enum" type="Duality.TargetResize" name="None" value="0" />
        <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="WorldSpace" value="0" />
        <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
          <contentPath dataType="String">Data\CustomRenderingSetup\SetupData\FullscreenShader\MainTarget.RenderTarget.res</contentPath>
        </output>
        <targetRect dataType="Struct" type="Duality.Rect">
          <H dataType="Float">1</H>
          <W dataType="Float">1</W>
          <X dataType="Float">0</X>
          <Y dataType="Float">0</Y>
        </targetRect>
        <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
      </item>
      <item dataType="Struct" type="Duality.Resources.RenderStep" id="1014616010">
        <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
        <clearDepth dataType="Float">1</clearDepth>
        <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
        <defaultClearColor dataType="Bool">false</defaultClearColor>
        <id dataType="String">ScreenOverlay</id>
        <input />
        <inputResize dataType="Enum" type="Duality.TargetResize" name="None" value="0" />
        <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="ScreenSpace" value="1" />
        <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
          <contentPath dataType="String">Data\CustomRenderingSetup\SetupData\FullscreenShader\MainTarget.RenderTarget.res</contentPath>
        </output>
        <targetRect dataType="Struct" type="Duality.Rect">
          <H dataType="Float">1</H>
          <W dataType="Float">1</W>
          <X dataType="Float">0</X>
          <Y dataType="Float">0</Y>
        </targetRect>
        <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
      </item>
      <item dataType="Struct" type="Duality.Resources.RenderStep" id="1845743710">
        <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
        <clearDepth dataType="Float">1</clearDepth>
        <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
        <defaultClearColor dataType="Bool">false</defaultClearColor>
        <id />
        <input dataType="Struct" type="Duality.Drawing.BatchInfo" id="1307056800">
          <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">255</A>
            <B dataType="Byte">255</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">255</R>
          </mainColor>
          <parameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="2583301340" custom="true">
            <body>
              <mainTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
                <contentPath dataType="String">Data\CustomRenderingSetup\SetupData\FullscreenShader\MainTargetTex.Texture.res</contentPath>
              </mainTex>
              <_GameTime dataType="Array" type="System.Single[]" id="4030560964">0</_GameTime>
            </body>
          </parameters>
          <technique dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.DrawTechnique]]">
            <contentPath dataType="String">Data\CustomRenderingSetup\SetupData\FullscreenShader\ScreenTechnique.DrawTechnique.res</contentPath>
          </technique>
        </input>
        <inputResize dataType="Enum" type="Duality.TargetResize" name="None" value="0" />
        <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="ScreenSpace" value="1" />
        <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
        <targetRect dataType="Struct" type="Duality.Rect">
          <H dataType="Float">1</H>
          <W dataType="Float">1</W>
          <X dataType="Float">0</X>
          <Y dataType="Float">0</Y>
        </targetRect>
        <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
      </item>
    </_items>
    <_size dataType="Int">3</_size>
  </steps>
</root>
<!-- XmlFormatterBase Document Separator -->
