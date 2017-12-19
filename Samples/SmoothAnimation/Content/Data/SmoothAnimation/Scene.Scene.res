<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="301198751">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1444170333">
        <_items dataType="Array" type="Duality.Component[]" id="1691359078" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="2661513683">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">301198751</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="838474558">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">64</B>
              <G dataType="Byte">53</G>
              <R dataType="Byte">44</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">301198751</gameobj>
            <nearZ dataType="Float">0</nearZ>
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="954680122">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">301198751</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3085315960" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3163255095">
            <item dataType="Type" id="1520414350" value="Duality.Components.Transform" />
            <item dataType="Type" id="3558043722" value="Duality.Components.Camera" />
            <item dataType="Type" id="3811843518" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="170854976">
            <item dataType="ObjectRef">2661513683</item>
            <item dataType="ObjectRef">838474558</item>
            <item dataType="ObjectRef">954680122</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2661513683</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1412267285">jaYMuf/Mfku/iGTj6t6pbA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1039736400">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="104218870">
        <_items dataType="Array" type="Duality.GameObject[]" id="3231294688">
          <item dataType="Struct" type="Duality.GameObject" id="760442718">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="442893310">
              <_items dataType="Array" type="Duality.Component[]" id="363115920" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3120757650">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">760442718</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">128</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">128</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2402609286">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">760442718</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\Explosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="4073803880">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">760442718</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1225737098" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="200970716">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="Type" id="2829211332" value="Duality.Components.Renderers.SpriteRenderer" />
                  <item dataType="Type" id="3943621526" value="Duality.Components.Renderers.SpriteAnimator" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2877158678">
                  <item dataType="ObjectRef">3120757650</item>
                  <item dataType="ObjectRef">2402609286</item>
                  <item dataType="ObjectRef">4073803880</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3120757650</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1131509576">iiBUMwcGTE+6Pxhw1sxQmA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">1039736400</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3879173251">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1670273719">
              <_items dataType="Array" type="Duality.Component[]" id="1451887502" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1944520887">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3879173251</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">128</X>
                    <Y dataType="Float">128</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">128</X>
                    <Y dataType="Float">128</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="956559607">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3879173251</gameobj>
                  <nextSpriteIndex dataType="Int">1</nextSpriteIndex>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\SmoothAnimExplosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <spriteIndexBlend dataType="Float">0</spriteIndexBlend>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="2897567117">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">3879173251</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2257967936" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4069159165">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="Type" id="3215838502" value="SmoothAnimation.BlendedSpriteRenderer" />
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="509223864">
                  <item dataType="ObjectRef">1944520887</item>
                  <item dataType="ObjectRef">956559607</item>
                  <item dataType="ObjectRef">2897567117</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1944520887</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4109135831">vTpAdXcXH0a9E4W7A7vrIQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">1039736400</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2365849191">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2065485283">
              <_items dataType="Array" type="Duality.Component[]" id="600218854" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="431196827">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2365849191</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-384</X>
                    <Y dataType="Float">56</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-384</X>
                    <Y dataType="Float">56</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">3</scale>
                  <scaleAbs dataType="Float">3</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="4008015759">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2365849191</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\Explosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1384243057">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">2365849191</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2729278200" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2372274825">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">2829211332</item>
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3250112320">
                  <item dataType="ObjectRef">431196827</item>
                  <item dataType="ObjectRef">4008015759</item>
                  <item dataType="ObjectRef">1384243057</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">431196827</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1503017003">6rWJNorcwkuoeGdoA+mXTw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">1039736400</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2876061338">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2155971762">
              <_items dataType="Array" type="Duality.Component[]" id="2396121808" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="941408974">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2876061338</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">384</X>
                    <Y dataType="Float">56</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">384</X>
                    <Y dataType="Float">56</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">3</scale>
                  <scaleAbs dataType="Float">3</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="4248414990">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2876061338</gameobj>
                  <nextSpriteIndex dataType="Int">1</nextSpriteIndex>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\SmoothAnimExplosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <spriteIndexBlend dataType="Float">0</spriteIndexBlend>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1894455204">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">2876061338</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1703085642" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="511456680">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">3215838502</item>
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3804370846">
                  <item dataType="ObjectRef">941408974</item>
                  <item dataType="ObjectRef">4248414990</item>
                  <item dataType="ObjectRef">1894455204</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">941408974</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1130727316">uH81wljS/km9lLuVM1gE3A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">1039736400</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1122582042">
        <_items dataType="Array" type="Duality.Component[]" id="2367506372" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2155998742" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="570010560" length="0" />
          <values dataType="Array" type="System.Object[]" id="4079893582" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3991134044">IFIj4dp0Jkyd2qgFxVHlVg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Slow</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3658900435">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="4112895713">
        <_items dataType="Array" type="Duality.GameObject[]" id="2323174254">
          <item dataType="Struct" type="Duality.GameObject" id="2949386683">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1146386459">
              <_items dataType="Array" type="Duality.Component[]" id="75896470" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1014734319">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2949386683</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="296585955">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2949386683</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\Explosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1967780549">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">2949386683</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="724363624" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4119485809">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">2829211332</item>
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1453777120">
                  <item dataType="ObjectRef">1014734319</item>
                  <item dataType="ObjectRef">296585955</item>
                  <item dataType="ObjectRef">1967780549</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1014734319</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2255764259">F1ay3H5IxkeD/Ds1ZfLUag==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">3658900435</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3765178736">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1148320156">
              <_items dataType="Array" type="Duality.Component[]" id="492406212" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1830526372">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3765178736</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">128</X>
                    <Y dataType="Float">-128</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">128</X>
                    <Y dataType="Float">-128</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="842565092">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3765178736</gameobj>
                  <nextSpriteIndex dataType="Int">1</nextSpriteIndex>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\SmoothAnimExplosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <spriteIndexBlend dataType="Float">0</spriteIndexBlend>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="2783572602">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">3765178736</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2744606742" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4057888566">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">3215838502</item>
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="284012186">
                  <item dataType="ObjectRef">1830526372</item>
                  <item dataType="ObjectRef">842565092</item>
                  <item dataType="ObjectRef">2783572602</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1830526372</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3217364950">rFX50pV4sUSmLEoJCzPQbQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">3658900435</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="524220528">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="986301596">
              <_items dataType="Array" type="Duality.Component[]" id="229004228" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2884535460">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">524220528</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-384</X>
                    <Y dataType="Float">-200</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-384</X>
                    <Y dataType="Float">-200</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">3</scale>
                  <scaleAbs dataType="Float">3</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2166387096">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">524220528</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\Explosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="3837581690">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">524220528</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="383756822" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3725302326">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">2829211332</item>
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="4053972122">
                  <item dataType="ObjectRef">2884535460</item>
                  <item dataType="ObjectRef">2166387096</item>
                  <item dataType="ObjectRef">3837581690</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2884535460</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3472270038">ALOKjbRzv0urAeHO92mqzw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">3658900435</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2588695353">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="209140553">
              <_items dataType="Array" type="Duality.Component[]" id="3381373838" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="654042989">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2588695353</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">384</X>
                    <Y dataType="Float">-200</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">384</X>
                    <Y dataType="Float">-200</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">3</scale>
                  <scaleAbs dataType="Float">3</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="3961049005">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2588695353</gameobj>
                  <nextSpriteIndex dataType="Int">1</nextSpriteIndex>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">128</H>
                    <W dataType="Float">128</W>
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">-64</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\SmoothAnimation\Visuals\SmoothAnimExplosion.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">0</spriteIndex>
                  <spriteIndexBlend dataType="Float">0</spriteIndexBlend>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1607089219">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">2588695353</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="370277184" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="642053379">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">3215838502</item>
                  <item dataType="ObjectRef">3943621526</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1217042360">
                  <item dataType="ObjectRef">654042989</item>
                  <item dataType="ObjectRef">3961049005</item>
                  <item dataType="ObjectRef">1607089219</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">654042989</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2510734889">hhq4l/S6M0GHJ7cmKW71jA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">3658900435</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2346866208">
        <_items dataType="ObjectRef">2367506372</_items>
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2276983923" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2954802084" length="0" />
          <values dataType="Array" type="System.Object[]" id="1507507990" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="778130080">9CoFNxOjI0mnbU7H7Zm0BA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Regular</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1739917342">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="4263018888">
        <_items dataType="Array" type="Duality.GameObject[]" id="2656270700" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="1216753297">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="622818445">
              <_items dataType="Array" type="Duality.Component[]" id="3306893606" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3577068229">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">1216753297</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2858919865">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">82</B>
                    <G dataType="Byte">82</G>
                    <R dataType="Byte">82</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1216753297</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">512</H>
                    <W dataType="Float">1</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-256</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1712830392" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2880000487">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">2829211332</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3603305344">
                  <item dataType="ObjectRef">3577068229</item>
                  <item dataType="ObjectRef">2858919865</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3577068229</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="981061541">IWgvnKkeZUax2spIe1Wg1Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Separator</name>
            <parent dataType="ObjectRef">1739917342</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3651443245">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2589665">
              <_items dataType="Array" type="Duality.Component[]" id="3662260590" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1716790881">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3651443245</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-32</X>
                    <Y dataType="Float">256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-32</X>
                    <Y dataType="Float">256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1099104771">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Right" value="2" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">185</B>
                    <G dataType="Byte">185</G>
                    <R dataType="Byte">185</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3651443245</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="4162120963">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3460735270">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">Regular Sprite Animation</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3340345376" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2053618795">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="Type" id="740282998" value="Duality.Components.Renderers.TextRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3735595208">
                  <item dataType="ObjectRef">1716790881</item>
                  <item dataType="ObjectRef">1099104771</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1716790881</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4118922593">XNUvgIuwgE+EMulddqm/8Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextRenderer</name>
            <parent dataType="ObjectRef">1739917342</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="235791753">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2067906101">
              <_items dataType="Array" type="Duality.Component[]" id="150400502" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2596106685">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">235791753</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">32</X>
                    <Y dataType="Float">256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">32</X>
                    <Y dataType="Float">256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1978420575">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">185</B>
                    <G dataType="Byte">185</G>
                    <R dataType="Byte">185</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">235791753</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1919672383">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="26623150">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Default:Font:GenericMonospace10</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">0</maxWidth>
                    <sourceText dataType="String">Smooth Sprite Animation</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4079224392" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2385241119">
                  <item dataType="ObjectRef">1520414350</item>
                  <item dataType="ObjectRef">740282998</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1637466144">
                  <item dataType="ObjectRef">2596106685</item>
                  <item dataType="ObjectRef">1978420575</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2596106685</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3730598541">TK6Cj4+fUEuaBR6HibnSlQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextRenderer</name>
            <parent dataType="ObjectRef">1739917342</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1696618974">
        <_items dataType="Array" type="Duality.Component[]" id="3829162058" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1048746356" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1482534216" length="0" />
          <values dataType="Array" type="System.Object[]" id="2964956382" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3277555124">/UUs7ZkdSU2mEZdA84Wkqg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Description</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">760442718</item>
    <item dataType="ObjectRef">3879173251</item>
    <item dataType="ObjectRef">2365849191</item>
    <item dataType="ObjectRef">2876061338</item>
    <item dataType="ObjectRef">2949386683</item>
    <item dataType="ObjectRef">3765178736</item>
    <item dataType="ObjectRef">524220528</item>
    <item dataType="ObjectRef">2588695353</item>
    <item dataType="ObjectRef">1216753297</item>
    <item dataType="ObjectRef">3651443245</item>
    <item dataType="ObjectRef">235791753</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
