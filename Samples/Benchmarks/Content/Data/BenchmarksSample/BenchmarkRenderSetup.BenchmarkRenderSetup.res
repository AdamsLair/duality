<root dataType="Struct" type="Duality.Samples.Benchmarks.BenchmarkRenderSetup" id="129723834">
  <antialiasingQuality dataType="Enum" type="Duality.AAQuality" name="Off" value="3" />
  <assetInfo />
  <autoResizeTargets dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.RenderSetupTargetResize]]" id="427169525">
    <_items dataType="Array" type="Duality.Resources.RenderSetupTargetResize[]" id="1100841590" length="0" />
    <_size dataType="Int">0</_size>
  </autoResizeTargets>
  <displayTestRunActive dataType="Bool">false</displayTestRunActive>
  <renderingSize dataType="Struct" type="Duality.Point2">
    <X dataType="Int">800</X>
    <Y dataType="Int">600</Y>
  </renderingSize>
  <resolutionScale dataType="Float">0.9999998</resolutionScale>
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
        <matrixMode dataType="Enum" type="Duality.Drawing.RenderMode" name="World" value="0" />
        <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
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
        <matrixMode dataType="Enum" type="Duality.Drawing.RenderMode" name="Screen" value="1" />
        <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
        <targetRect dataType="Struct" type="Duality.Rect">
          <H dataType="Float">1</H>
          <W dataType="Float">1</W>
          <X dataType="Float">0</X>
          <Y dataType="Float">0</Y>
        </targetRect>
        <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
      </item>
    </_items>
    <_size dataType="Int">2</_size>
  </steps>
</root>
<!-- XmlFormatterBase Document Separator -->
