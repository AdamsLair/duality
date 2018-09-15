<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1375075025">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2739208755">
        <_items dataType="Array" type="Duality.Component[]" id="1622104614">
          <item dataType="Struct" type="Duality.Components.Transform" id="1432352243">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">1375075025</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </pos>
            <posAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="3446209492">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1375075025</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="2921461502">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">163</B>
              <G dataType="Byte">132</G>
              <R dataType="Byte">98</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1375075025</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="4103331970" custom="true">
              <body />
            </shaderParameters>
            <targetRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">1</H>
              <W dataType="Float">1</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </targetRect>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="3407727552">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1375075025</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3976118456" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4052994137">
            <item dataType="Type" id="357083086" value="Duality.Components.Transform" />
            <item dataType="Type" id="535184714" value="Duality.Components.Camera" />
            <item dataType="Type" id="1415442302" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="455536474" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="279133184">
            <item dataType="ObjectRef">1432352243</item>
            <item dataType="ObjectRef">2921461502</item>
            <item dataType="ObjectRef">3407727552</item>
            <item dataType="ObjectRef">3446209492</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1432352243</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2829806619">ilukqsClz0q2XjHcXsFImA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1056485149">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3236744367">
        <_items dataType="Array" type="Duality.Component[]" id="1622538222" length="4">
          <item dataType="Struct" type="AudioHandling.ManualAudioHandling" id="60807211">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1056485149</gameobj>
            <soundsInside dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Sound]][]" id="3935907243">
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                <contentPath dataType="String">Data\AudioHandling\Audio\Fireplace.Sound.res</contentPath>
              </item>
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                <contentPath dataType="String">Data\AudioHandling\Audio\Guitar.Sound.res</contentPath>
              </item>
            </soundsInside>
            <soundsOutside dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Sound]][]" id="1854145864">
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                <contentPath dataType="String">Data\AudioHandling\Audio\Rain.Sound.res</contentPath>
              </item>
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                <contentPath dataType="String">Data\AudioHandling\Audio\DogsBarking.Sound.res</contentPath>
              </item>
            </soundsOutside>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="950745248" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="267773829">
            <item dataType="Type" id="1822157142" value="AudioHandling.ManualAudioHandling" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="762475176">
            <item dataType="ObjectRef">60807211</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1709089935">g3AoF4tEokaHY5nVvCJIdg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">ManualAudioHandling</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3486528804">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2563303066">
        <_items dataType="Array" type="Duality.Component[]" id="4057473920" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3543806022">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">3486528804</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <pos dataType="Struct" type="Duality.Vector3" />
            <posAbs dataType="Struct" type="Duality.Vector3" />
            <scale dataType="Float">0.5</scale>
            <scaleAbs dataType="Float">0.5</scaleAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="660180788">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">3486528804</gameobj>
            <offset dataType="Float">0</offset>
            <pixelGrid dataType="Bool">false</pixelGrid>
            <rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">507</H>
              <W dataType="Float">1024</W>
              <X dataType="Float">-512</X>
              <Y dataType="Float">-253.5</Y>
            </rect>
            <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
              <contentPath dataType="String">Default:Material:DualityLogoBig</contentPath>
            </sharedMat>
            <spriteIndex dataType="Int">-1</spriteIndex>
            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4277146426" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1257639136">
            <item dataType="ObjectRef">357083086</item>
            <item dataType="Type" id="2705215452" value="Duality.Components.Renderers.SpriteRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1673242510">
            <item dataType="ObjectRef">3543806022</item>
            <item dataType="ObjectRef">660180788</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3543806022</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3544906748">DAXfpacW0UaURfly31D2/g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">DualityLogoBig</name>
      <parent />
      <prefabLink />
    </item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
