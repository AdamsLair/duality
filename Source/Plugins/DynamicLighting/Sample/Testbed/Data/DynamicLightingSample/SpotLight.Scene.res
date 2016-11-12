﻿<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
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
            <vel dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </vel>
            <velAbs dataType="Struct" type="Duality.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </velAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="4254655817">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3717380010</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="2874030229">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="588643446" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="4158771168">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <CollectDrawcalls />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="1420780430">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
                  <CollectDrawcalls />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="OrthoScreen" value="1" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </passes>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
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
          <keys dataType="Array" type="System.Type[]" id="2123804574">
            <item dataType="Type" id="1996527504" value="Duality.Components.Transform" />
            <item dataType="Type" id="1192166126" value="Duality.Components.Camera" />
            <item dataType="Type" id="3355905644" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1726310794">
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
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="2641404349">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="1638342256">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="Type" id="3493388604" value="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3715048174">
                  <item dataType="ObjectRef">2794196586</item>
                  <item dataType="ObjectRef">2641404349</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="2080947339">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="564869410">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2462033674">
                  <item dataType="ObjectRef">2233739576</item>
                  <item dataType="ObjectRef">2080947339</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="2899987420">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="2512716671">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3473688416">
                  <item dataType="ObjectRef">3052779657</item>
                  <item dataType="ObjectRef">2899987420</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="1838477519">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="787456830">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3280049674">
                  <item dataType="ObjectRef">1991269756</item>
                  <item dataType="ObjectRef">1838477519</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="3934557467">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="1466098322">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2702374346">
                  <item dataType="ObjectRef">4087349704</item>
                  <item dataType="ObjectRef">3934557467</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="2326617287">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="1615017670">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3761071802">
                  <item dataType="ObjectRef">2479409524</item>
                  <item dataType="ObjectRef">2326617287</item>
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
        <_items dataType="Array" type="Duality.Component[]" id="1167249795" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3991194603" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="2135263028" />
          <values dataType="Array" type="Duality.Component[]" id="1276358390" />
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
          <item dataType="Struct" type="Duality.GameObject" id="2346786567">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="932196343">
              <_items dataType="Array" type="Duality.Component[]" id="3092307598" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="412134203">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">1.26219642</angle>
                  <angleAbs dataType="Float">1.26219642</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2346786567</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-74.5494</X>
                    <Y dataType="Float">673.1811</Y>
                    <Z dataType="Float">62.6235962</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-74.5494</X>
                    <Y dataType="Float">673.1811</Y>
                    <Z dataType="Float">62.6235962</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.Light" id="3820515322">
                  <active dataType="Bool">true</active>
                  <ambientColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </ambientColor>
                  <ambientIntensity dataType="Float">0</ambientIntensity>
                  <color dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">0</R>
                  </color>
                  <dir dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-1</Y>
                    <Z dataType="Float">0</Z>
                  </dir>
                  <gameobj dataType="ObjectRef">2346786567</gameobj>
                  <intensity dataType="Float">0.9764706</intensity>
                  <range dataType="Float">2500</range>
                  <spotFocus dataType="Float">3</spotFocus>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="522656320" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2348392765">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="Type" id="2534779430" value="Duality.Plugins.DynamicLighting.Light" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3485692088">
                  <item dataType="ObjectRef">412134203</item>
                  <item dataType="ObjectRef">3820515322</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">412134203</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="974765847">N1mqI0tnMEalo8/UzrnOAA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">SpotLight</name>
            <parent dataType="ObjectRef">3857244827</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="604315623">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3354661399">
              <_items dataType="Array" type="Duality.Component[]" id="2950781710" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2964630555">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">2.669335</angle>
                  <angleAbs dataType="Float">2.669335</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">604315623</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">13.2804222</X>
                    <Y dataType="Float">48.09619</Y>
                    <Z dataType="Float">237.662048</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">13.2804222</X>
                    <Y dataType="Float">48.09619</Y>
                    <Z dataType="Float">237.662048</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.Light" id="2078044378">
                  <active dataType="Bool">true</active>
                  <ambientColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </ambientColor>
                  <ambientIntensity dataType="Float">0</ambientIntensity>
                  <color dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">255</R>
                  </color>
                  <dir dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-1</Y>
                    <Z dataType="Float">0</Z>
                  </dir>
                  <gameobj dataType="ObjectRef">604315623</gameobj>
                  <intensity dataType="Float">0.972549</intensity>
                  <range dataType="Float">2500</range>
                  <spotFocus dataType="Float">3</spotFocus>
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3146953920" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="3006078365">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">2534779430</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2968442104">
                  <item dataType="ObjectRef">2964630555</item>
                  <item dataType="ObjectRef">2078044378</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2964630555</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4148689463">u1IhoSQVkU2075AG+HxlzQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">SpotLight</name>
            <parent dataType="ObjectRef">3857244827</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">8</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1808580800">
        <_items dataType="Array" type="Duality.Component[]" id="39109475" length="4" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2067264843" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1144029236" />
          <values dataType="Array" type="Duality.Component[]" id="2038309622" />
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="2009866415">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="760745690">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="4258196154">
                  <item dataType="ObjectRef">2162658652</item>
                  <item dataType="ObjectRef">2009866415</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="562909269">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="4004156716">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2341685686">
                  <item dataType="ObjectRef">715701506</item>
                  <item dataType="ObjectRef">562909269</item>
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
                  <vel dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="Duality.Plugins.DynamicLighting.LightingSpriteRenderer" id="1993847917">
                  <active dataType="Bool">true</active>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
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
                <keys dataType="Array" type="System.Type[]" id="2337396852">
                  <item dataType="ObjectRef">1996527504</item>
                  <item dataType="ObjectRef">3493388604</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="67890678">
                  <item dataType="ObjectRef">2146640154</item>
                  <item dataType="ObjectRef">1993847917</item>
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
          <keys dataType="Array" type="System.Type[]" id="475763232" />
          <values dataType="Array" type="Duality.Component[]" id="3825073038" />
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
    <item dataType="ObjectRef">4097311016</item>
    <item dataType="ObjectRef">2650353870</item>
    <item dataType="ObjectRef">4081292518</item>
    <item dataType="ObjectRef">2346786567</item>
    <item dataType="ObjectRef">604315623</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
