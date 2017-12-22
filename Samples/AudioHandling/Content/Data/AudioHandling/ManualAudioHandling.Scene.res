<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="1046745565">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="745009135">
        <_items dataType="Array" type="Duality.Component[]" id="1925022958" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3407060497">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1046745565</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
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
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="1584021372">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">163</B>
              <G dataType="Byte">132</G>
              <R dataType="Byte">98</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">1046745565</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <priority dataType="Int">0</priority>
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <targetRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">1</H>
              <W dataType="Float">1</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </targetRect>
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1700226936">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1046745565</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="279828896" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3768464965">
            <item dataType="Type" id="1624031446" value="Duality.Components.Transform" />
            <item dataType="Type" id="3976031194" value="Duality.Components.Camera" />
            <item dataType="Type" id="4261988854" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3357733928">
            <item dataType="ObjectRef">3407060497</item>
            <item dataType="ObjectRef">1584021372</item>
            <item dataType="ObjectRef">1700226936</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3407060497</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2485585871">ilukqsClz0q2XjHcXsFImA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3399693911">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1718472661">
        <_items dataType="Array" type="Duality.Component[]" id="3178533878" length="4">
          <item dataType="Struct" type="AudioHandling.ManualAudioHandling" id="94799073">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3399693911</gameobj>
            <soundsInside dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Sound]][]" id="4143797761">
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                <contentPath dataType="String">Data\AudioHandling\Audio\Fireplace.Sound.res</contentPath>
              </item>
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Sound]]">
                <contentPath dataType="String">Data\AudioHandling\Audio\Guitar.Sound.res</contentPath>
              </item>
            </soundsInside>
            <soundsOutside dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Sound]][]" id="2867775840">
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
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3278995528" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="129546495">
            <item dataType="Type" id="3800736046" value="AudioHandling.ManualAudioHandling" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3179062112">
            <item dataType="ObjectRef">94799073</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2298051757">g3AoF4tEokaHY5nVvCJIdg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">ManualAudioHandling</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1249395491">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1172988689">
        <_items dataType="Array" type="Duality.Component[]" id="3049362158" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3609710423">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1249395491</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <parentTransform />
            <pos dataType="Struct" type="Duality.Vector3" />
            <posAbs dataType="Struct" type="Duality.Vector3" />
            <scale dataType="Float">0.5</scale>
            <scaleAbs dataType="Float">0.5</scaleAbs>
            <vel dataType="Struct" type="Duality.Vector3" />
            <velAbs dataType="Struct" type="Duality.Vector3" />
          </item>
          <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2891562059">
            <active dataType="Bool">true</active>
            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </colorTint>
            <customMat />
            <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
            <gameobj dataType="ObjectRef">1249395491</gameobj>
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
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1106789280" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3645576891">
            <item dataType="ObjectRef">1624031446</item>
            <item dataType="Type" id="1276387542" value="Duality.Components.Renderers.SpriteRenderer" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3203126312">
            <item dataType="ObjectRef">3609710423</item>
            <item dataType="ObjectRef">2891562059</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3609710423</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2197679921">DAXfpacW0UaURfly31D2/g==</data>
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
