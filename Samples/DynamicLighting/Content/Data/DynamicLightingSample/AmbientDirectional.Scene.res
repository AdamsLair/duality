<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3717380010">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3661281972">
        <_items dataType="Array" type="Duality.Component[]" id="1721512356" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="1782727646">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3717380010</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="4254655817">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3717380010</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="75894085">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3717380010</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">3</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3591042038" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2123804574">
            <item dataType="Type" id="1996527504" value="Duality.Components.Transform" />
            <item dataType="Type" id="1192166126" value="Duality.Components.Camera" />
            <item dataType="Type" id="3355905644" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1726310794">
            <item dataType="ObjectRef">1782727646</item>
            <item dataType="ObjectRef">4254655817</item>
            <item dataType="ObjectRef">75894085</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1782727646</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1443777134">CPW/MS2uqUKBWkJKE9w5Cw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="426532027">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="124632521">
        <_items dataType="Array" type="Duality.GameObject[]" id="55248526" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="433881654">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1340291978">
              <_items dataType="Array" type="Duality.Component[]" id="1153339360" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2794196586">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">433881654</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="458190827">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">433881654</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1302589722" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1638342256">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="Type" id="3493388604" value="DynamicLighting.LightingSpriteRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3715048174">
                  <item dataType="ObjectRef">2794196586</item>
                  <item dataType="ObjectRef">458190827</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2794196586</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1914574284">jcdv87M550WzGtONMRGCXA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">LogoBall</name>
            <parent dataType="ObjectRef">426532027</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4168391940">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3125937232">
              <_items dataType="Array" type="Duality.Component[]" id="1284747708" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2233739576">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.0296492577</angle>
                  <angleAbs dataType="Float">0.0296492577</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4168391940</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="4192701113">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">4168391940</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3565805934" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="564869410">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2462033674">
                  <item dataType="ObjectRef">2233739576</item>
                  <item dataType="ObjectRef">4192701113</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2233739576</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3867282130">S9USHma6DEifIFwjAkv0Qw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship</name>
            <parent dataType="ObjectRef">426532027</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="692464725">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="238052949">
              <_items dataType="Array" type="Duality.Component[]" id="2924406006" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3052779657">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.399004817</angle>
                  <angleAbs dataType="Float">0.399004817</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">692464725</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="716773898">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">692464725</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4123432264" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2512716671">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3473688416">
                  <item dataType="ObjectRef">3052779657</item>
                  <item dataType="ObjectRef">716773898</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3052779657</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1817570349">uWZMeQroYEqm5Lho2JOzjA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship2</name>
            <parent dataType="ObjectRef">426532027</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3925922120">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4148631188">
              <_items dataType="Array" type="Duality.Component[]" id="3147336548" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1991269756">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.04730867</angle>
                  <angleAbs dataType="Float">0.04730867</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3925922120</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="3950231293">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">3925922120</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3102316598" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="787456830">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3280049674">
                  <item dataType="ObjectRef">1991269756</item>
                  <item dataType="ObjectRef">3950231293</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1991269756</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2525973070">pZmIt4iOEkikltPxD6PN/w==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship</name>
            <parent dataType="ObjectRef">426532027</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1727034772">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3253342528">
              <_items dataType="Array" type="Duality.Component[]" id="1100426524" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="4087349704">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.637401</angle>
                  <angleAbs dataType="Float">0.637401</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">1727034772</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="1751343945">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">1727034772</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="539546190" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1466098322">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2702374346">
                  <item dataType="ObjectRef">4087349704</item>
                  <item dataType="ObjectRef">1751343945</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">4087349704</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2675002530">mj9A8LA5aUqYwK/FMpo1Zw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Spaceship2</name>
            <parent dataType="ObjectRef">426532027</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="119094592">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2379618124">
              <_items dataType="Array" type="Duality.Component[]" id="605515172" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2479409524">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">119094592</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="143403765">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">119094592</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3498418166" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1615017670">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3761071802">
                  <item dataType="ObjectRef">2479409524</item>
                  <item dataType="ObjectRef">143403765</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2479409524</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="630153670">mXM8mSUkcU6cIrpvkic4qA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">LogoBall</name>
            <parent dataType="ObjectRef">426532027</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">6</_size>
        <_version dataType="Int">6</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1460257344">
        <_items dataType="Array" type="Duality.Component[]" id="1167249795" length="0" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3991194603" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2135263028" length="0" />
          <values dataType="Array" type="System.Object[]" id="1276358390" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3634170000">OBEOYmITJE2KeWiULaVtpw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PerPixelLightObjects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3857244827">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3462005993">
        <_items dataType="Array" type="Duality.GameObject[]" id="979769102" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="4032599880">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4272747924">
              <_items dataType="Array" type="Duality.Component[]" id="96162660" length="4">
                <item dataType="Struct" type="DynamicLighting.Light" id="1050109645">
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
                  <gameobj dataType="ObjectRef">4032599880</gameobj>
                  <intensity dataType="Float">0.996078432</intensity>
                  <range dataType="Float">1500</range>
                  <spotFocus dataType="Float">0</spotFocus>
                </item>
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3090767926" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3027640382">
                  <item dataType="Type" id="827401744" value="DynamicLighting.Light" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="3963954698">
                  <item dataType="ObjectRef">1050109645</item>
                </values>
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2386025806">ftt3lysUtkybB3PtolK2gA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">AmbientDirectional</name>
            <parent dataType="ObjectRef">3857244827</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">3</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1808580800">
        <_items dataType="Array" type="Duality.Component[]" id="39109475" length="4" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2067264843" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1144029236" length="0" />
          <values dataType="Array" type="System.Object[]" id="2038309622" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1890364304">86GF9m2lj0OhJLZk6sTq5w==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Lights</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1966538984">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1567091758">
        <_items dataType="Array" type="Duality.GameObject[]" id="4223660880" length="4">
          <item dataType="Struct" type="Duality.GameObject" id="4097311016">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2507695384">
              <_items dataType="Array" type="Duality.Component[]" id="3449245228" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2162658652">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">1.32828844</angle>
                  <angleAbs dataType="Float">1.32828844</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4097311016</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="4121620189">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">4097311016</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1837852958" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="760745690">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="4258196154">
                  <item dataType="ObjectRef">2162658652</item>
                  <item dataType="ObjectRef">4121620189</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2162658652</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4099462618">nkvGDf6QTkqhzWkpfwse+A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Particle</name>
            <parent dataType="ObjectRef">1966538984</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2650353870">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2611129518">
              <_items dataType="Array" type="Duality.Component[]" id="3570393424" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="715701506">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">5.718203</angle>
                  <angleAbs dataType="Float">5.718203</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2650353870</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="2674663043">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">2650353870</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3922577098" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4004156716">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2341685686">
                  <item dataType="ObjectRef">715701506</item>
                  <item dataType="ObjectRef">2674663043</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">715701506</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="401295224">cn0xUciEGU2Hn0j0OoACpQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Particle</name>
            <parent dataType="ObjectRef">1966538984</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4081292518">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3108523270">
              <_items dataType="Array" type="Duality.Component[]" id="439359872" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2146640154">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">6.21469831</angle>
                  <angleAbs dataType="Float">6.21469831</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4081292518</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
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
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="DynamicLighting.LightingSpriteRenderer" id="4105601691">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <flipMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+FlipMode" name="None" value="0" />
                  <gameobj dataType="ObjectRef">4081292518</gameobj>
                  <offset dataType="Int">0</offset>
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
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2891849018" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2337396852">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="67890678">
                  <item dataType="ObjectRef">2146640154</item>
                  <item dataType="ObjectRef">4105601691</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2146640154</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="377006672">fNGbWdhvmEW2EsVdkXgAAg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Particle</name>
            <parent dataType="ObjectRef">1966538984</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
        <_version dataType="Int">5</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="958594250">
        <_items dataType="ObjectRef">1167249795</_items>
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="395402654" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="475763232" length="0" />
          <values dataType="Array" type="System.Object[]" id="3825073038" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3761520956">z0SeU6nZj0S2iv+SHPOgqA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PerVertexLightObjects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">433881654</item>
    <item dataType="ObjectRef">4168391940</item>
    <item dataType="ObjectRef">692464725</item>
    <item dataType="ObjectRef">3925922120</item>
    <item dataType="ObjectRef">1727034772</item>
    <item dataType="ObjectRef">119094592</item>
    <item dataType="ObjectRef">4032599880</item>
    <item dataType="ObjectRef">4097311016</item>
    <item dataType="ObjectRef">2650353870</item>
    <item dataType="ObjectRef">4081292518</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
