<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="4249629983">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2192273885">
        <_items dataType="Array" type="Duality.Component[]" id="726669414">
          <item dataType="Struct" type="Duality.Components.Transform" id="11939905">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">4249629983</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="1501049164">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">0</A>
              <B dataType="Byte">64</B>
              <G dataType="Byte">53</G>
              <R dataType="Byte">44</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">4249629983</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="560348904" custom="true">
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
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="2025797154">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">4249629983</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1987315214">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">4249629983</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2073735288" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2146109623">
            <item dataType="Type" id="1647248270" value="Duality.Components.Transform" />
            <item dataType="Type" id="1518490698" value="Duality.Components.Camera" />
            <item dataType="Type" id="3453400254" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="3828585690" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2018227008">
            <item dataType="ObjectRef">11939905</item>
            <item dataType="ObjectRef">1501049164</item>
            <item dataType="ObjectRef">1987315214</item>
            <item dataType="ObjectRef">2025797154</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">11939905</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4076128917">jaYMuf/Mfku/iGTj6t6pbA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2703700299">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3647457849">
        <_items dataType="Array" type="Duality.GameObject[]" id="4205105358">
          <item dataType="Struct" type="Duality.GameObject" id="704957688">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1689788516">
              <_items dataType="Array" type="Duality.Component[]" id="545663428" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="762234906">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">704957688</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2173576968">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">704957688</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="445154228">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">704957688</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1551381526" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2836937518">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="Type" id="534597456" value="Duality.Components.Renderers.SpriteRenderer" />
                  <item dataType="Type" id="1495603054" value="Duality.Components.Renderers.SpriteAnimator" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="4285212874">
                  <item dataType="ObjectRef">762234906</item>
                  <item dataType="ObjectRef">2173576968</item>
                  <item dataType="ObjectRef">445154228</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">762234906</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="526907038">iiBUMwcGTE+6Pxhw1sxQmA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">2703700299</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1279259533">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4139975021">
              <_items dataType="Array" type="Duality.Component[]" id="3949524198" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1336536751">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1279259533</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="2121873993">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1279259533</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1019456073">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">1279259533</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1148859128" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="130027783">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="Type" id="2962457678" value="SmoothAnimation.BlendedSpriteRenderer" />
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3662069376">
                  <item dataType="ObjectRef">1336536751</item>
                  <item dataType="ObjectRef">2121873993</item>
                  <item dataType="ObjectRef">1019456073</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1336536751</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3008459525">vTpAdXcXH0a9E4W7A7vrIQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">2703700299</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1749049658">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3616249182">
              <_items dataType="Array" type="Duality.Component[]" id="1675763472" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1806326876">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1749049658</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3217668938">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1749049658</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1489246198">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">1749049658</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3226624778" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4089843324">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">534597456</item>
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3686542486">
                  <item dataType="ObjectRef">1806326876</item>
                  <item dataType="ObjectRef">3217668938</item>
                  <item dataType="ObjectRef">1489246198</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1806326876</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1975284776">6rWJNorcwkuoeGdoA+mXTw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">2703700299</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2136296987">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="78059195">
              <_items dataType="Array" type="Duality.Component[]" id="1659117782" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2193574205">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2136296987</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="2978911447">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2136296987</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="1876493527">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">10</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">2136296987</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3472610344" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3825434833">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">2962457678</item>
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="158211488">
                  <item dataType="ObjectRef">2193574205</item>
                  <item dataType="ObjectRef">2978911447</item>
                  <item dataType="ObjectRef">1876493527</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2193574205</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2124601923">uH81wljS/km9lLuVM1gE3A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">2703700299</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1802651904">
        <_items dataType="Array" type="Duality.Component[]" id="280317331" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4249774523" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3201036052" length="0" />
          <values dataType="Array" type="System.Object[]" id="3056173366" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2429168048">IFIj4dp0Jkyd2qgFxVHlVg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Slow</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1171198935">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3030130517">
        <_items dataType="Array" type="Duality.GameObject[]" id="872065270">
          <item dataType="Struct" type="Duality.GameObject" id="3879447610">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3580982830">
              <_items dataType="Array" type="Duality.Component[]" id="3942091600" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3936724828">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3879447610</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1053099594">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3879447610</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="3619644150">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">3879447610</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2115630282" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3853676204">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">534597456</item>
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2930504118">
                  <item dataType="ObjectRef">3936724828</item>
                  <item dataType="ObjectRef">1053099594</item>
                  <item dataType="ObjectRef">3619644150</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3936724828</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3031607544">F1ay3H5IxkeD/Ds1ZfLUag==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">1171198935</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="608523014">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2126699498">
              <_items dataType="Array" type="Duality.Component[]" id="3207267616" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="665800232">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">608523014</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="1451137474">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">608523014</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="348719554">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">608523014</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3964925914" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="153882832">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">2962457678</item>
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="126886510">
                  <item dataType="ObjectRef">665800232</item>
                  <item dataType="ObjectRef">1451137474</item>
                  <item dataType="ObjectRef">348719554</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">665800232</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="411025068">rFX50pV4sUSmLEoJCzPQbQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">1171198935</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1182041655">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="717964439">
              <_items dataType="Array" type="Duality.Component[]" id="572431630" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1239318873">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1182041655</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="2650660935">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1182041655</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="922238195">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">1182041655</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1668522688" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="299008029">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">534597456</item>
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2798827256">
                  <item dataType="ObjectRef">1239318873</item>
                  <item dataType="ObjectRef">2650660935</item>
                  <item dataType="ObjectRef">922238195</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1239318873</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3670277303">ALOKjbRzv0urAeHO92mqzw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_Regular</name>
            <parent dataType="ObjectRef">1171198935</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="687263456">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1191054812">
              <_items dataType="Array" type="Duality.Component[]" id="1897471684" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="744540674">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">687263456</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="SmoothAnimation.BlendedSpriteRenderer" id="1529877916">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">687263456</gameobj>
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
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteAnimator" id="427459996">
                  <active dataType="Bool">true</active>
                  <animDuration dataType="Float">2</animDuration>
                  <animLoopMode dataType="Enum" type="Duality.Components.Renderers.SpriteAnimator+LoopMode" name="Loop" value="1" />
                  <animTime dataType="Float">0</animTime>
                  <customFrameSequence />
                  <firstFrame dataType="Int">0</firstFrame>
                  <frameCount dataType="Int">64</frameCount>
                  <gameobj dataType="ObjectRef">687263456</gameobj>
                  <paused dataType="Bool">false</paused>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2120932630" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3855908598">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">2962457678</item>
                  <item dataType="ObjectRef">1495603054</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="4252658202">
                  <item dataType="ObjectRef">744540674</item>
                  <item dataType="ObjectRef">1529877916</item>
                  <item dataType="ObjectRef">427459996</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">744540674</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4196312086">hhq4l/S6M0GHJ7cmKW71jA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Explosion_SmoothAnim</name>
            <parent dataType="ObjectRef">1171198935</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4130785608">
        <_items dataType="ObjectRef">280317331</_items>
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="976734559" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="267958788" length="0" />
          <values dataType="Array" type="System.Object[]" id="2052681622" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1250007744">9CoFNxOjI0mnbU7H7Zm0BA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Regular</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="933604207">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1912716173">
        <_items dataType="Array" type="Duality.GameObject[]" id="3834493222" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="3297732976">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1866533900">
              <_items dataType="Array" type="Duality.Component[]" id="78609572" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3355010194">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3297732976</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="471384960">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">82</B>
                    <G dataType="Byte">82</G>
                    <R dataType="Byte">82</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3297732976</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1559718646" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2222328966">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">534597456</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3609187130">
                  <item dataType="ObjectRef">3355010194</item>
                  <item dataType="ObjectRef">471384960</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3355010194</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2246797318">IWgvnKkeZUax2spIe1Wg1Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Separator</name>
            <parent dataType="ObjectRef">933604207</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3844397005">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4269561149">
              <_items dataType="Array" type="Duality.Component[]" id="2196443686" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3901674223">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3844397005</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="432367363">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Right" value="2" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">185</B>
                    <G dataType="Byte">185</G>
                    <R dataType="Byte">185</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3844397005</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1794505747">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3933510374">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2784327864" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="69032535">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="Type" id="2306504206" value="Duality.Components.Renderers.TextRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2628935616">
                  <item dataType="ObjectRef">3901674223</item>
                  <item dataType="ObjectRef">432367363</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3901674223</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1851852405">XNUvgIuwgE+EMulddqm/8Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextRenderer</name>
            <parent dataType="ObjectRef">933604207</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1883681321">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="241046057">
              <_items dataType="Array" type="Duality.Component[]" id="3152073230" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1940958539">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1883681321</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2766618975">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">185</B>
                    <G dataType="Byte">185</G>
                    <R dataType="Byte">185</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1883681321</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2847612655">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="590413550">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1489603520" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4062246819">
                  <item dataType="ObjectRef">1647248270</item>
                  <item dataType="ObjectRef">2306504206</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1463581560">
                  <item dataType="ObjectRef">1940958539</item>
                  <item dataType="ObjectRef">2766618975</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1940958539</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1969851401">TK6Cj4+fUEuaBR6HibnSlQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextRenderer</name>
            <parent dataType="ObjectRef">933604207</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3214827448">
        <_items dataType="Array" type="Duality.Component[]" id="2603858151" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="847848039" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4089992724" length="0" />
          <values dataType="Array" type="System.Object[]" id="463791926" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="781831856">/UUs7ZkdSU2mEZdA84Wkqg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Description</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">704957688</item>
    <item dataType="ObjectRef">1279259533</item>
    <item dataType="ObjectRef">1749049658</item>
    <item dataType="ObjectRef">2136296987</item>
    <item dataType="ObjectRef">3879447610</item>
    <item dataType="ObjectRef">608523014</item>
    <item dataType="ObjectRef">1182041655</item>
    <item dataType="ObjectRef">687263456</item>
    <item dataType="ObjectRef">3297732976</item>
    <item dataType="ObjectRef">3844397005</item>
    <item dataType="ObjectRef">1883681321</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
