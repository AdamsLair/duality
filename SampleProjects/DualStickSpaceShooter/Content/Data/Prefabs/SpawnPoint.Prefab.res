<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="2717718507">
    <active dataType="Bool">true</active>
    <children />
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1040459480">
      <_items dataType="Array" type="Duality.Component[]" id="815970220" length="8">
        <item dataType="Struct" type="Duality.Components.Transform" id="783066143">
          <active dataType="Bool">true</active>
          <angle dataType="Float">0</angle>
          <angleAbs dataType="Float">0</angleAbs>
          <angleVel dataType="Float">0</angleVel>
          <angleVelAbs dataType="Float">0</angleVelAbs>
          <deriveAngle dataType="Bool">true</deriveAngle>
          <gameobj dataType="ObjectRef">2717718507</gameobj>
          <ignoreParent dataType="Bool">false</ignoreParent>
          <parentTransform />
          <pos dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">2369.00513</X>
            <Y dataType="Float">-86.11029</Y>
            <Z dataType="Float">1</Z>
          </pos>
          <posAbs dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">2369.00513</X>
            <Y dataType="Float">-86.11029</Y>
            <Z dataType="Float">1</Z>
          </posAbs>
          <scale dataType="Float">0.75</scale>
          <scaleAbs dataType="Float">0.75</scaleAbs>
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
        <item dataType="Struct" type="DualStickSpaceShooter.SpawnPoint" id="954146828">
          <activated dataType="Bool">false</activated>
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">2717718507</gameobj>
          <index dataType="Int">0</index>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="3744351600">
          <active dataType="Bool">true</active>
          <angularDrag dataType="Float">0</angularDrag>
          <constantForce dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">-0.01</Y>
            <Z dataType="Float">0</Z>
          </constantForce>
          <disposeWhenEmpty dataType="Bool">false</disposeWhenEmpty>
          <emitters dataType="Struct" type="System.Collections.Generic.List`1[[DualStickSpaceShooter.ParticleEmitter]]" id="4213506200">
            <_items dataType="Array" type="DualStickSpaceShooter.ParticleEmitter[]" id="2872375852" length="4">
              <item dataType="Struct" type="DualStickSpaceShooter.ParticleEmitter" id="2819175652">
                <basePos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">-10</Z>
                </basePos>
                <baseVel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </baseVel>
                <burstDelay dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">500</MaxValue>
                  <MinValue dataType="Float">500</MinValue>
                </burstDelay>
                <burstParticleNum dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">1</MaxValue>
                  <MinValue dataType="Float">1</MinValue>
                </burstParticleNum>
                <depthMult dataType="Float">0.35</depthMult>
                <maxBurstCount dataType="Int">-1</maxBurstCount>
                <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                  <A dataType="Float">0.5019608</A>
                  <H dataType="Float">0</H>
                  <S dataType="Float">0</S>
                  <V dataType="Float">0</V>
                </maxColor>
                <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                  <A dataType="Float">0.2509804</A>
                  <H dataType="Float">0</H>
                  <S dataType="Float">0</S>
                  <V dataType="Float">0</V>
                </minColor>
                <particleLifetime dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">20000</MaxValue>
                  <MinValue dataType="Float">10000</MinValue>
                </particleLifetime>
                <randomAngle dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">6.28318548</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomAngle>
                <randomAngleVel dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">0.01</MaxValue>
                  <MinValue dataType="Float">-0.01</MinValue>
                </randomAngleVel>
                <randomPos dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">100</MaxValue>
                  <MinValue dataType="Float">15</MinValue>
                </randomPos>
                <randomVel dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">0.1</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomVel>
                <spriteIndex dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">0</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </spriteIndex>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">5</_version>
          </emitters>
          <fadeInAt dataType="Float">0.25</fadeInAt>
          <fadeOutAt dataType="Float">0.75</fadeOutAt>
          <gameobj dataType="ObjectRef">2717718507</gameobj>
          <linearDrag dataType="Float">0</linearDrag>
          <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\Materials\AlphaBubbles.Material.res</contentPath>
          </material>
          <particleSize dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">16</X>
            <Y dataType="Float">16</Y>
          </particleSize>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          <worldSpace dataType="Bool">false</worldSpace>
        </item>
        <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="165380033">
          <active dataType="Bool">true</active>
          <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">69</A>
            <B dataType="Byte">255</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">255</R>
          </colorTint>
          <customMat />
          <gameobj dataType="ObjectRef">2717718507</gameobj>
          <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath />
          </iconMat>
          <text dataType="Struct" type="Duality.Drawing.FormattedText" id="639194685">
            <displayedText dataType="String">A WarmPlace</displayedText>
            <elements dataType="Array" type="Duality.Drawing.FormattedText+Element[]" id="3328578598">
              <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="3849976064">
                <text dataType="String">A Warm</text>
              </item>
              <item dataType="Struct" type="Duality.Drawing.FormattedText+NewLineElement" id="3988199886" />
              <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="1485032348">
                <text dataType="String">Place</text>
              </item>
            </elements>
            <flowAreas />
            <fontGlyphCount dataType="Array" type="System.Int32[]" id="356953786">
              <item dataType="Int">11</item>
            </fontGlyphCount>
            <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1663900454">
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                <contentPath dataType="String">Data\Materials\WorldFont.Font.res</contentPath>
              </item>
            </fonts>
            <iconCount dataType="Int">0</iconCount>
            <icons />
            <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
            <maxHeight dataType="Int">0</maxHeight>
            <maxWidth dataType="Int">100</maxWidth>
            <sourceText dataType="String">A Warm/nPlace</sourceText>
            <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
          </text>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
        </item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1485527735">
          <active dataType="Bool">true</active>
          <angularDamp dataType="Float">0.3</angularDamp>
          <angularVel dataType="Float">0</angularVel>
          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
          <continous dataType="Bool">false</continous>
          <explicitMass dataType="Float">0</explicitMass>
          <fixedAngle dataType="Bool">false</fixedAngle>
          <gameobj dataType="ObjectRef">2717718507</gameobj>
          <ignoreGravity dataType="Bool">false</ignoreGravity>
          <joints />
          <linearDamp dataType="Float">0.3</linearDamp>
          <linearVel dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </linearVel>
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1971723147">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="219107446" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3883950048">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">1485527735</parent>
                <position dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </position>
                <radius dataType="Float">150</radius>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">true</sensor>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">16</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.Trigger" id="1223525211">
          <active dataType="Bool">true</active>
          <collisionCounter dataType="Int">0</collisionCounter>
          <fireOnce dataType="Bool">true</fireOnce>
          <gameobj dataType="ObjectRef">2717718507</gameobj>
          <targets />
        </item>
      </_items>
      <_size dataType="Int">6</_size>
      <_version dataType="Int">6</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="866486430" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="2102842010">
          <item dataType="Type" id="4117674880" value="Duality.Components.Transform" />
          <item dataType="Type" id="1650660558" value="DualStickSpaceShooter.SpawnPoint" />
          <item dataType="Type" id="3609586972" value="DualStickSpaceShooter.ParticleEffect" />
          <item dataType="Type" id="2765008338" value="Duality.Components.Renderers.TextRenderer" />
          <item dataType="Type" id="2301861688" value="Duality.Components.Physics.RigidBody" />
          <item dataType="Type" id="2470027046" value="DualStickSpaceShooter.Trigger" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="3261262650">
          <item dataType="ObjectRef">783066143</item>
          <item dataType="ObjectRef">954146828</item>
          <item dataType="ObjectRef">3744351600</item>
          <item dataType="ObjectRef">165380033</item>
          <item dataType="ObjectRef">1485527735</item>
          <item dataType="ObjectRef">1223525211</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">783066143</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="934232602">rlJ8TIHZx0WfVzS1WTItgg==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">SpawnPoint</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">SpawnPoint</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
