<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="3054164894">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1124529609">
      <_items dataType="Array" type="Duality.GameObject[]" id="4086748814" length="4">
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
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1119512530">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3054164894</gameobj>
                  <ignoreParent dataType="Bool">true</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
      </_items>
      <_size dataType="Int">1</_size>
      <_version dataType="Int">1</_version>
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
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <joints />
          <linearDamp dataType="Float">0.3</linearDamp>
          <linearVel dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </linearVel>
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2311246660">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1655711300" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="427891268">
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
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">10</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="4276676260">
          <active dataType="Bool">true</active>
          <bulletType dataType="Struct" type="Duality.ContentRef`1[[DualStickSpaceShooter.BulletBlueprint]]">
            <contentPath />
          </bulletType>
          <gameobj dataType="ObjectRef">3054164894</gameobj>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <maxSpeed dataType="Float">3</maxSpeed>
          <maxTurnSpeed dataType="Float">0.5</maxTurnSpeed>
          <owner />
          <targetAngle dataType="Float">0</targetAngle>
          <targetAngleRatio dataType="Float">0.1</targetAngleRatio>
          <targetThrust dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </targetThrust>
          <thrusterPower dataType="Float">0</thrusterPower>
          <weaponDelay dataType="Float">0</weaponDelay>
          <weaponTimer dataType="Float">0</weaponTimer>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.EnemyClaymore" id="4130272826">
          <active dataType="Bool">true</active>
          <blinkTimer dataType="Float">0</blinkTimer>
          <eyeBlinking dataType="Bool">false</eyeBlinking>
          <eyeOpenTarget dataType="Float">0</eyeOpenTarget>
          <eyeOpenValue dataType="Float">0</eyeOpenValue>
          <eyeSpeed dataType="Float">0</eyeSpeed>
          <gameobj dataType="ObjectRef">3054164894</gameobj>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
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
          <item dataType="Type" id="4084404388" value="Duality.Components.Renderers.SpriteRenderer" />
          <item dataType="Type" id="3098016534" value="Duality.Components.Physics.RigidBody" />
          <item dataType="Type" id="423808928" value="DualStickSpaceShooter.Ship" />
          <item dataType="Type" id="2859407970" value="DualStickSpaceShooter.EnemyClaymore" />
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
