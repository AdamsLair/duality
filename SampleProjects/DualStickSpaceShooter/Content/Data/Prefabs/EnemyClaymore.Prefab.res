<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="3054164894">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1124529609">
      <_items dataType="Array" type="Duality.GameObject[]" id="4086748814" length="8">
        <item dataType="Struct" type="Duality.GameObject" id="1216619707">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2405041051">
            <_items dataType="Array" type="Duality.Component[]" id="738781078" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="3576934639">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">1216619707</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1119512530">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3054164894</gameobj>
                  <ignoreParent dataType="Bool">true</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </parentTransform>
                <pos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </posAbs>
                <scale dataType="Float">1</scale>
                <scaleAbs dataType="Float">1</scaleAbs>
                <vel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </vel>
                <velAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </velAbs>
              </item>
              <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="924055088">
                <active dataType="Bool">true</active>
                <animDuration dataType="Float">1</animDuration>
                <animFirstFrame dataType="Int">0</animFirstFrame>
                <animFrameCount dataType="Int">6</animFrameCount>
                <animLoopMode dataType="Enum" type="Duality.Components.Renderers.AnimSpriteRenderer+LoopMode" name="FixedSingle" value="4" />
                <animPaused dataType="Bool">false</animPaused>
                <animTime dataType="Float">0</animTime>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customFrameSequence />
                <customMat />
                <gameobj dataType="ObjectRef">1216619707</gameobj>
                <offset dataType="Int">-1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">8</H>
                  <W dataType="Float">10</W>
                  <X dataType="Float">-5</X>
                  <Y dataType="Float">-4</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Enemies\EnemyClaymoreEye.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
            </_items>
            <_size dataType="Int">2</_size>
            <_version dataType="Int">2</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="871064168" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="1736468721">
                <item dataType="Type" id="2819286958" value="Duality.Components.Transform" />
                <item dataType="Type" id="2611451594" value="Duality.Components.Renderers.AnimSpriteRenderer" />
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="3751220192">
                <item dataType="ObjectRef">3576934639</item>
                <item dataType="ObjectRef">924055088</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">3576934639</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="1836359587">XGkWUJInT02uxT8povgqqA==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">Eye</name>
          <parent dataType="ObjectRef">3054164894</parent>
          <prefabLink />
        </item>
        <item dataType="Struct" type="Duality.GameObject" id="4070850922">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="914947886">
            <_items dataType="Array" type="Duality.Component[]" id="599547728" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="2136198558">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0.7853982</angle>
                <angleAbs dataType="Float">0.7853982</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">4070850922</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="ObjectRef">1119512530</parentTransform>
                <pos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">8.499023</X>
                  <Y dataType="Float">-8</Y>
                  <Z dataType="Float">0</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">8.499023</X>
                  <Y dataType="Float">-8</Y>
                  <Z dataType="Float">0</Z>
                </posAbs>
                <scale dataType="Float">1</scale>
                <scaleAbs dataType="Float">1</scaleAbs>
                <vel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </vel>
                <velAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </velAbs>
              </item>
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1418050194">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">4070850922</gameobj>
                <offset dataType="Int">1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">9</H>
                  <W dataType="Float">2</W>
                  <X dataType="Float">-1</X>
                  <Y dataType="Float">-4.5</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Enemies\EnemyClaymoreSpike.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
            </_items>
            <_size dataType="Int">2</_size>
            <_version dataType="Int">2</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1453693130" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="3732485548">
                <item dataType="ObjectRef">2819286958</item>
                <item dataType="Type" id="3451215076" value="Duality.Components.Renderers.SpriteRenderer" />
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="2412815286">
                <item dataType="ObjectRef">2136198558</item>
                <item dataType="ObjectRef">1418050194</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">2136198558</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="1922855416">LAZpHM33BEe/oBk5enRXQg==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">SpikeTopRight</name>
          <parent dataType="ObjectRef">3054164894</parent>
          <prefabLink />
        </item>
        <item dataType="Struct" type="Duality.GameObject" id="2286597987">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4271244579">
            <_items dataType="Array" type="Duality.Component[]" id="3242930278" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="351945623">
                <active dataType="Bool">true</active>
                <angle dataType="Float">3.92699075</angle>
                <angleAbs dataType="Float">3.92699075</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">2286597987</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="ObjectRef">1119512530</parentTransform>
                <pos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">-8</X>
                  <Y dataType="Float">8.5</Y>
                  <Z dataType="Float">0</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">-8</X>
                  <Y dataType="Float">8.5</Y>
                  <Z dataType="Float">0</Z>
                </posAbs>
                <scale dataType="Float">1</scale>
                <scaleAbs dataType="Float">1</scaleAbs>
                <vel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </vel>
                <velAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </velAbs>
              </item>
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3928764555">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">2286597987</gameobj>
                <offset dataType="Int">1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">9</H>
                  <W dataType="Float">2</W>
                  <X dataType="Float">-1</X>
                  <Y dataType="Float">-4.5</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Enemies\EnemyClaymoreSpike.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
            </_items>
            <_size dataType="Int">2</_size>
            <_version dataType="Int">2</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2586970232" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="2384107593">
                <item dataType="ObjectRef">2819286958</item>
                <item dataType="ObjectRef">3451215076</item>
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="3223635264">
                <item dataType="ObjectRef">351945623</item>
                <item dataType="ObjectRef">3928764555</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">351945623</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="2926496875">rvumzOzVs0yGEZGK868Ofg==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">SpikeBottomLeft</name>
          <parent dataType="ObjectRef">3054164894</parent>
          <prefabLink />
        </item>
        <item dataType="Struct" type="Duality.GameObject" id="1526931573">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2122450869">
            <_items dataType="Array" type="Duality.Component[]" id="541404918" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="3887246505">
                <active dataType="Bool">true</active>
                <angle dataType="Float">5.49778748</angle>
                <angleAbs dataType="Float">5.49778748</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">1526931573</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="ObjectRef">1119512530</parentTransform>
                <pos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">-8</X>
                  <Y dataType="Float">-8.5</Y>
                  <Z dataType="Float">0</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">-8</X>
                  <Y dataType="Float">-8.5</Y>
                  <Z dataType="Float">0</Z>
                </posAbs>
                <scale dataType="Float">1</scale>
                <scaleAbs dataType="Float">1</scaleAbs>
                <vel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </vel>
                <velAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </velAbs>
              </item>
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3169098141">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">1526931573</gameobj>
                <offset dataType="Int">1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">9</H>
                  <W dataType="Float">2</W>
                  <X dataType="Float">-1</X>
                  <Y dataType="Float">-4.5</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Enemies\EnemyClaymoreSpike.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
            </_items>
            <_size dataType="Int">2</_size>
            <_version dataType="Int">2</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1912580936" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="2661178783">
                <item dataType="ObjectRef">2819286958</item>
                <item dataType="ObjectRef">3451215076</item>
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="4251736096">
                <item dataType="ObjectRef">3887246505</item>
                <item dataType="ObjectRef">3169098141</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">3887246505</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="3446135053">VG0wBDPhxU29Qb4tST/T5A==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">SpikeTopLeft</name>
          <parent dataType="ObjectRef">3054164894</parent>
          <prefabLink />
        </item>
        <item dataType="Struct" type="Duality.GameObject" id="2781767734">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3760157066">
            <_items dataType="Array" type="Duality.Component[]" id="99956704" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="847115370">
                <active dataType="Bool">true</active>
                <angle dataType="Float">2.3561945</angle>
                <angleAbs dataType="Float">2.3561945</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">2781767734</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="ObjectRef">1119512530</parentTransform>
                <pos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">8.499023</X>
                  <Y dataType="Float">8</Y>
                  <Z dataType="Float">0</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">8.499023</X>
                  <Y dataType="Float">8</Y>
                  <Z dataType="Float">0</Z>
                </posAbs>
                <scale dataType="Float">1</scale>
                <scaleAbs dataType="Float">1</scaleAbs>
                <vel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </vel>
                <velAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </velAbs>
              </item>
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="128967006">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">2781767734</gameobj>
                <offset dataType="Int">1</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">9</H>
                  <W dataType="Float">2</W>
                  <X dataType="Float">-1</X>
                  <Y dataType="Float">-4.5</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Enemies\EnemyClaymoreSpike.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
            </_items>
            <_size dataType="Int">2</_size>
            <_version dataType="Int">2</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="762001690" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="602779760">
                <item dataType="ObjectRef">2819286958</item>
                <item dataType="ObjectRef">3451215076</item>
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="2382230254">
                <item dataType="ObjectRef">847115370</item>
                <item dataType="ObjectRef">128967006</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">847115370</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="1080472524">MiLRvx95TEqXi2PfaqN4SA==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">SpikeBottomRight</name>
          <parent dataType="ObjectRef">3054164894</parent>
          <prefabLink />
        </item>
      </_items>
      <_size dataType="Int">5</_size>
      <_version dataType="Int">5</_version>
    </children>
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3059937856">
      <_items dataType="Array" type="Duality.Component[]" id="1880738179" length="8">
        <item dataType="ObjectRef">1119512530</item>
        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="401364166">
          <active dataType="Bool">true</active>
          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">255</A>
            <B dataType="Byte">255</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">255</R>
          </colorTint>
          <customMat />
          <gameobj dataType="ObjectRef">3054164894</gameobj>
          <offset dataType="Int">0</offset>
          <pixelGrid dataType="Bool">false</pixelGrid>
          <rect dataType="Struct" type="Duality.Rect">
            <H dataType="Float">16</H>
            <W dataType="Float">16</W>
            <X dataType="Float">-8</X>
            <Y dataType="Float">-8</Y>
          </rect>
          <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
          <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\Materials\Enemies\EnemyClaymoreBody.Material.res</contentPath>
          </sharedMat>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
        </item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1821974122">
          <active dataType="Bool">true</active>
          <angularDamp dataType="Float">0.3</angularDamp>
          <angularVel dataType="Float">0</angularVel>
          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
          <continous dataType="Bool">false</continous>
          <explicitMass dataType="Float">5</explicitMass>
          <fixedAngle dataType="Bool">false</fixedAngle>
          <gameobj dataType="ObjectRef">3054164894</gameobj>
          <ignoreGravity dataType="Bool">false</ignoreGravity>
          <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="2311246660">
            <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="1655711300" />
            <_size dataType="Int">0</_size>
            <_version dataType="Int">0</_version>
          </joints>
          <linearDamp dataType="Float">0.3</linearDamp>
          <linearVel dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </linearVel>
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4160378518">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3633725134" length="8">
              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1783790544">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">1821974122</parent>
                <position dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </position>
                <radius dataType="Float">7</radius>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">false</sensor>
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2969195118">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">1821974122</parent>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">true</sensor>
                <vertices dataType="Array" type="OpenTK.Vector2[]" id="1613245602">
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">4</X>
                    <Y dataType="Float">-5.000004</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">11</X>
                    <Y dataType="Float">-10.9999943</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">5</X>
                    <Y dataType="Float">-4.000003</Y>
                  </item>
                </vertices>
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2807915436">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">1821974122</parent>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">true</sensor>
                <vertices dataType="Array" type="OpenTK.Vector2[]" id="3610146168">
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">4.882392</X>
                    <Y dataType="Float">3.87699175</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">11.1894836</X>
                    <Y dataType="Float">10.6016026</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">3.92812228</X>
                    <Y dataType="Float">4.92072</Y>
                  </item>
                </vertices>
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="181375506">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">1821974122</parent>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">true</sensor>
                <vertices dataType="Array" type="OpenTK.Vector2[]" id="3642005174">
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-3.908016</X>
                    <Y dataType="Float">5.19087029</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-10.811264</X>
                    <Y dataType="Float">11.3019333</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-4.92386627</X>
                    <Y dataType="Float">4.206972</Y>
                  </item>
                </vertices>
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1809642888">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">1821974122</parent>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">true</sensor>
                <vertices dataType="Array" type="OpenTK.Vector2[]" id="220208212">
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-5.03235435</X>
                    <Y dataType="Float">-4.49726</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-10.9958992</X>
                    <Y dataType="Float">-11.52832</Y>
                  </item>
                  <item dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">-4.02716827</X>
                    <Y dataType="Float">-5.4920454</Y>
                  </item>
                </vertices>
              </item>
            </_items>
            <_size dataType="Int">5</_size>
            <_version dataType="Int">186</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="4276676260">
          <active dataType="Bool">true</active>
          <blueprint dataType="Struct" type="Duality.ContentRef`1[[DualStickSpaceShooter.ShipBlueprint]]">
            <contentPath dataType="String">Data\Blueprints\ClaymoreShip.ShipBlueprint.res</contentPath>
          </blueprint>
          <damageEffect />
          <gameobj dataType="ObjectRef">3054164894</gameobj>
          <hitpoints dataType="Float">1</hitpoints>
          <isDead dataType="Bool">false</isDead>
          <owner />
          <targetAngle dataType="Float">0</targetAngle>
          <targetAngleRatio dataType="Float">0.1</targetAngleRatio>
          <targetThrust dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </targetThrust>
          <weaponTimer dataType="Float">0</weaponTimer>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="4130272826">
          <active dataType="Bool">true</active>
          <behavior dataType="Enum" type="DualStickSpaceShooter.EnemyClaymore+BehaviorFlags" name="Chase" value="1" />
          <blinkTimer dataType="Float">0</blinkTimer>
          <exploDamage dataType="Float">125</exploDamage>
          <exploEffects dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Prefab]][]" id="3219443732">
            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
              <contentPath dataType="String">Data\Prefabs\ExplosionClaymoreGlow.Prefab.res</contentPath>
            </item>
            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
              <contentPath dataType="String">Data\Prefabs\ExplosionClaymoreSmoke.Prefab.res</contentPath>
            </item>
          </exploEffects>
          <exploForce dataType="Float">50</exploForce>
          <exploMaxVel dataType="Float">5</exploMaxVel>
          <exploRadius dataType="Float">150</exploRadius>
          <eyeBlinking dataType="Bool">false</eyeBlinking>
          <eyeOpenTarget dataType="Float">0</eyeOpenTarget>
          <eyeOpenValue dataType="Float">0</eyeOpenValue>
          <eyeSpeed dataType="Float">0</eyeSpeed>
          <gameobj dataType="ObjectRef">3054164894</gameobj>
          <idleTimer dataType="Float">0</idleTimer>
          <spikesActive dataType="Bool">false</spikesActive>
          <spikeState dataType="Array" type="DualStickSpaceShooter.EnemyClaymore+SpikeState[]" id="2841410358" length="4" />
          <state dataType="Enum" type="DualStickSpaceShooter.EnemyClaymore+MindState" name="Asleep" value="0" />
        </item>
      </_items>
      <_size dataType="Int">5</_size>
      <_version dataType="Int">5</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2756486123" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="2870368052">
          <item dataType="ObjectRef">2819286958</item>
          <item dataType="ObjectRef">3451215076</item>
          <item dataType="Type" id="4084404388" value="Duality.Components.Physics.RigidBody" />
          <item dataType="Type" id="3098016534" value="DualStickSpaceShooter.Ship" />
          <item dataType="Type" id="423808928" value="DualStickSpaceShooter.EnemyClaymore" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="146657014">
          <item dataType="ObjectRef">1119512530</item>
          <item dataType="ObjectRef">401364166</item>
          <item dataType="ObjectRef">1821974122</item>
          <item dataType="ObjectRef">4276676260</item>
          <item dataType="ObjectRef">4130272826</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">1119512530</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="385185936">dZxIBUKDIkezYjBW6Bu9yQ==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">EnemyClaymore</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">EnemyClaymore</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
