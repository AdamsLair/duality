<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3845221357">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2247383903">
        <_items dataType="Array" type="Duality.Component[]" id="4174514286">
          <item dataType="Struct" type="Duality.Components.Transform" id="3902498575">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">3845221357</gameobj>
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
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="1621388528">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3845221357</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="1096640538">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3845221357</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="2839793918" custom="true">
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1582906588">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3845221357</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1573601056" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3253634645">
            <item dataType="Type" id="2403034358" value="Duality.Components.Transform" />
            <item dataType="Type" id="2846887450" value="Duality.Components.Camera" />
            <item dataType="Type" id="3143041558" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="614320378" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="4094956872">
            <item dataType="ObjectRef">3902498575</item>
            <item dataType="ObjectRef">1096640538</item>
            <item dataType="ObjectRef">1582906588</item>
            <item dataType="ObjectRef">1621388528</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3902498575</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3529708639">CPW/MS2uqUKBWkJKE9w5Cw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1504337260">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="820506658">
        <_items dataType="Array" type="Duality.GameObject[]" id="1116573968" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="2138178799">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3263185035">
              <_items dataType="Array" type="Duality.Component[]" id="1022948982" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2195456017">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2138178799</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="3085146302">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2138178799</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\LogoBall.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2913467592" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2535929889">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="Type" id="337787502" value="DynamicLighting.LightingSpriteRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2935878944">
                  <item dataType="ObjectRef">2195456017</item>
                  <item dataType="ObjectRef">3085146302</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2195456017</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3046964659">jcdv87M550WzGtONMRGCXA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">LogoBall</name>
            <parent dataType="ObjectRef">1504337260</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2937734662">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1145692838">
              <_items dataType="Array" type="Duality.Component[]" id="2611688960" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2995011880">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.0296492577</angle>
                  <angleAbs dataType="Float">0.0296492577</angleAbs>
                  <gameobj dataType="ObjectRef">2937734662</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">382.066223</X>
                    <Y dataType="Float">-86.70662</Y>
                    <Z dataType="Float">255.312622</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">382.066223</X>
                    <Y dataType="Float">-86.70662</Y>
                    <Z dataType="Float">255.312622</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="3884702165">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2937734662</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">106</H>
                    <W dataType="Float">110</W>
                    <X dataType="Float">-55</X>
                    <Y dataType="Float">-53</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Spaceship.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2763710394" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4010196756">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2820358454">
                  <item dataType="ObjectRef">2995011880</item>
                  <item dataType="ObjectRef">3884702165</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2995011880</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2888612272">S9USHma6DEifIFwjAkv0Qw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship</name>
            <parent dataType="ObjectRef">1504337260</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1184955908">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2978753516">
              <_items dataType="Array" type="Duality.Component[]" id="1575849572" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1242233126">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.399004817</angle>
                  <angleAbs dataType="Float">0.399004817</angleAbs>
                  <gameobj dataType="ObjectRef">1184955908</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">121.429626</X>
                    <Y dataType="Float">278.038269</Y>
                    <Z dataType="Float">284.434967</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">121.429626</X>
                    <Y dataType="Float">278.038269</Y>
                    <Z dataType="Float">284.434967</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="2131923411">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1184955908</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">340</H>
                    <W dataType="Float">172</W>
                    <X dataType="Float">-86</X>
                    <Y dataType="Float">-170</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Spaceship2.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1982638902" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="960676902">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="497643194">
                  <item dataType="ObjectRef">1242233126</item>
                  <item dataType="ObjectRef">2131923411</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1242233126</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="222825254">uWZMeQroYEqm5Lho2JOzjA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship2</name>
            <parent dataType="ObjectRef">1504337260</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1121595201">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="16179269">
              <_items dataType="Array" type="Duality.Component[]" id="3368740054" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1178872419">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.04730867</angle>
                  <angleAbs dataType="Float">0.04730867</angleAbs>
                  <gameobj dataType="ObjectRef">1121595201</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-237.4337</X>
                    <Y dataType="Float">533.737854</Y>
                    <Z dataType="Float">166.036377</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-237.4337</X>
                    <Y dataType="Float">533.737854</Y>
                    <Z dataType="Float">166.036377</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="2068562704">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1121595201</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">106</H>
                    <W dataType="Float">110</W>
                    <X dataType="Float">-55</X>
                    <Y dataType="Float">-53</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Spaceship.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1068632104" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1390524463">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1141092256">
                  <item dataType="ObjectRef">1178872419</item>
                  <item dataType="ObjectRef">2068562704</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1178872419</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3328906429">pZmIt4iOEkikltPxD6PN/w==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship</name>
            <parent dataType="ObjectRef">1504337260</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1246395357">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3247075977">
              <_items dataType="Array" type="Duality.Component[]" id="2665031566" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1303672575">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.637401</angle>
                  <angleAbs dataType="Float">0.637401</angleAbs>
                  <gameobj dataType="ObjectRef">1246395357</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">101.21965</X>
                    <Y dataType="Float">652.844543</Y>
                    <Z dataType="Float">71.8888245</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">101.21965</X>
                    <Y dataType="Float">652.844543</Y>
                    <Z dataType="Float">71.8888245</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="2193362860">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1246395357</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">340</H>
                    <W dataType="Float">172</W>
                    <X dataType="Float">-86</X>
                    <Y dataType="Float">-170</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Spaceship2.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1530906432" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3128478531">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2656519352">
                  <item dataType="ObjectRef">1303672575</item>
                  <item dataType="ObjectRef">2193362860</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1303672575</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2907243369">mj9A8LA5aUqYwK/FMpo1Zw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship2</name>
            <parent dataType="ObjectRef">1504337260</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2480809397">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3486975009">
              <_items dataType="Array" type="Duality.Component[]" id="1476917870" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2538086615">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2480809397</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">508.088074</X>
                    <Y dataType="Float">452.933716</Y>
                    <Z dataType="Float">1241.081</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">508.088074</X>
                    <Y dataType="Float">452.933716</Y>
                    <Z dataType="Float">1241.081</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="3427776900">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2480809397</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\LogoBall.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3420108064" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1329080875">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1410054216">
                  <item dataType="ObjectRef">2538086615</item>
                  <item dataType="ObjectRef">3427776900</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2538086615</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1195212321">mXM8mSUkcU6cIrpvkic4qA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">LogoBall</name>
            <parent dataType="ObjectRef">1504337260</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">6</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2621733130">
        <_items dataType="Array" type="Duality.Component[]" id="2463985080" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3068994514" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2955444000" length="0" />
          <values dataType="Array" type="System.Object[]" id="2320918414" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1902115388">OBEOYmITJE2KeWiULaVtpw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PerPixelLightObjects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="166344421">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2315498135">
        <_items dataType="Array" type="Duality.GameObject[]" id="3712779534" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="1195195268">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="547432144">
              <_items dataType="Array" type="Duality.Component[]" id="1773676220" length="4">
                <item dataType="Struct" type="DynamicLighting.Light" id="3464896279">
                  <active dataType="Bool">true</active>
                  <ambientColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">137</G>
                    <R dataType="Byte">117</R>
                  </ambientColor>
                  <ambientIntensity dataType="Float">0.2</ambientIntensity>
                  <color dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">187</B>
                    <G dataType="Byte">230</G>
                    <R dataType="Byte">255</R>
                  </color>
                  <dir dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0.5347977</X>
                    <Y dataType="Float">0.527753353</Y>
                    <Z dataType="Float">0.659899831</Z>
                  </dir>
                  <gameobj dataType="ObjectRef">1195195268</gameobj>
                  <intensity dataType="Float">0.996078432</intensity>
                  <range dataType="Float">1500</range>
                  <spotFocus dataType="Float">0</spotFocus>
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1960971886" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2094880162">
                  <item dataType="Type" id="4046874896" value="DynamicLighting.Light" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3525488906">
                  <item dataType="ObjectRef">3464896279</item>
                </values>
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3741387858">ftt3lysUtkybB3PtolK2gA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">AmbientDirectional</name>
            <parent dataType="ObjectRef">166344421</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1284451008">
        <_items dataType="Array" type="Duality.Component[]" id="1522691613" length="4" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3021943349" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1794658100" length="0" />
          <values dataType="Array" type="System.Object[]" id="948014838" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1307488400">86GF9m2lj0OhJLZk6sTq5w==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Lights</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3351318985">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1110209419">
        <_items dataType="Array" type="Duality.GameObject[]" id="3209109622" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="217309286">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2020491402">
              <_items dataType="Array" type="Duality.Component[]" id="1278041056" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="274586504">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">1.32828844</angle>
                  <angleAbs dataType="Float">1.32828844</angleAbs>
                  <gameobj dataType="ObjectRef">217309286</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-13.6297</X>
                    <Y dataType="Float">542.386536</Y>
                    <Z dataType="Float">114.599152</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-13.6297</X>
                    <Y dataType="Float">542.386536</Y>
                    <Z dataType="Float">114.599152</Z>
                  </posAbs>
                  <scale dataType="Float">1.94464064</scale>
                  <scaleAbs dataType="Float">1.94464064</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="1164276789">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">217309286</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Particle.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0.5</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2414169370" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1413215088">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="195491566">
                  <item dataType="ObjectRef">274586504</item>
                  <item dataType="ObjectRef">1164276789</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">274586504</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1138654412">nkvGDf6QTkqhzWkpfwse+A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Particle</name>
            <parent dataType="ObjectRef">3351318985</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="966080730">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3933025678">
              <_items dataType="Array" type="Duality.Component[]" id="3135801552" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1023357948">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">5.718203</angle>
                  <angleAbs dataType="Float">5.718203</angleAbs>
                  <gameobj dataType="ObjectRef">966080730</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">190.9804</X>
                    <Y dataType="Float">458.597534</Y>
                    <Z dataType="Float">283.355927</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">190.9804</X>
                    <Y dataType="Float">458.597534</Y>
                    <Z dataType="Float">283.355927</Z>
                  </posAbs>
                  <scale dataType="Float">1.94464064</scale>
                  <scaleAbs dataType="Float">1.94464064</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="1913048233">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">966080730</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Particle.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0.5</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1515812938" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3072103244">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2498555894">
                  <item dataType="ObjectRef">1023357948</item>
                  <item dataType="ObjectRef">1913048233</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1023357948</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3652868184">cn0xUciEGU2Hn0j0OoACpQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Particle</name>
            <parent dataType="ObjectRef">3351318985</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3229634318">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3035571122">
              <_items dataType="Array" type="Duality.Component[]" id="300324560" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3286911536">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">6.21469831</angle>
                  <angleAbs dataType="Float">6.21469831</angleAbs>
                  <gameobj dataType="ObjectRef">3229634318</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-37.8626862</X>
                    <Y dataType="Float">319.671631</Y>
                    <Z dataType="Float">150.466522</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-37.8626862</X>
                    <Y dataType="Float">319.671631</Y>
                    <Z dataType="Float">150.466522</Z>
                  </posAbs>
                  <scale dataType="Float">1.94464064</scale>
                  <scaleAbs dataType="Float">1.94464064</scaleAbs>
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="4176601821">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3229634318</gameobj>
                  <offset dataType="Float">0</offset>
                  <pixelGrid dataType="Bool">false</pixelGrid>
                  <rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">256</H>
                    <W dataType="Float">256</W>
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-128</Y>
                  </rect>
                  <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                  <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\DynamicLightingSample\Materials\Particle.Material.res</contentPath>
                  </sharedMat>
                  <spriteIndex dataType="Int">-1</spriteIndex>
                  <vertexTranslucency dataType="Float">0.5</vertexTranslucency>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="181920330" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2168345256">
                  <item dataType="ObjectRef">2403034358</item>
                  <item dataType="ObjectRef">337787502</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2658731934">
                  <item dataType="ObjectRef">3286911536</item>
                  <item dataType="ObjectRef">4176601821</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3286911536</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3955673236">fNGbWdhvmEW2EsVdkXgAAg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Particle</name>
            <parent dataType="ObjectRef">3351318985</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3979121352">
        <_items dataType="ObjectRef">2463985080</_items>
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="801818945" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2277708100" length="0" />
          <values dataType="Array" type="System.Object[]" id="111325846" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3971432704">z0SeU6nZj0S2iv+SHPOgqA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PerVertexLightObjects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2138178799</item>
    <item dataType="ObjectRef">2937734662</item>
    <item dataType="ObjectRef">1184955908</item>
    <item dataType="ObjectRef">1121595201</item>
    <item dataType="ObjectRef">1246395357</item>
    <item dataType="ObjectRef">2480809397</item>
    <item dataType="ObjectRef">1195195268</item>
    <item dataType="ObjectRef">217309286</item>
    <item dataType="ObjectRef">966080730</item>
    <item dataType="ObjectRef">3229634318</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
