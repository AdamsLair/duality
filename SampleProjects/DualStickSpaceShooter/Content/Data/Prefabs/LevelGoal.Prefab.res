<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="1914390279">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1025269228">
      <_items dataType="Array" type="Duality.GameObject[]" id="1983435364" length="4">
        <item dataType="Struct" type="Duality.GameObject" id="1562013821">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1087461009">
            <_items dataType="Array" type="Duality.Component[]" id="1441408494" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="3922328753">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">1562013821</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="Struct" type="Duality.Components.Transform" id="4274705211">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">1914390279</gameobj>
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
                  <Z dataType="Float">1</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">1</Z>
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
              <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="2588646914">
                <active dataType="Bool">true</active>
                <angularDrag dataType="Float">0.05</angularDrag>
                <constantForce dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </constantForce>
                <disposeWhenEmpty dataType="Bool">false</disposeWhenEmpty>
                <emitters dataType="Struct" type="System.Collections.Generic.List`1[[DualStickSpaceShooter.ParticleEmitter]]" id="34699846">
                  <_items dataType="Array" type="DualStickSpaceShooter.ParticleEmitter[]" id="648028160" length="4">
                    <item dataType="Struct" type="DualStickSpaceShooter.ParticleEmitter" id="871946396">
                      <basePos dataType="Struct" type="OpenTK.Vector3">
                        <X dataType="Float">0</X>
                        <Y dataType="Float">0</Y>
                        <Z dataType="Float">-1</Z>
                      </basePos>
                      <baseVel dataType="Struct" type="OpenTK.Vector3">
                        <X dataType="Float">0</X>
                        <Y dataType="Float">0</Y>
                        <Z dataType="Float">0</Z>
                      </baseVel>
                      <burstDelay dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">6000</MaxValue>
                        <MinValue dataType="Float">4000</MinValue>
                      </burstDelay>
                      <burstParticleNum dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">1</MaxValue>
                        <MinValue dataType="Float">1</MinValue>
                      </burstParticleNum>
                      <maxBurstCount dataType="Int">-1</maxBurstCount>
                      <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                        <A dataType="Float">0.549019635</A>
                        <H dataType="Float">0.22256729</H>
                        <S dataType="Float">0.6313726</S>
                        <V dataType="Float">1</V>
                      </maxColor>
                      <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                        <A dataType="Float">0.392156869</A>
                        <H dataType="Float">0.254248351</H>
                        <S dataType="Float">1</S>
                        <V dataType="Float">1</V>
                      </minColor>
                      <particleLifetime dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">20000</MaxValue>
                        <MinValue dataType="Float">16000</MinValue>
                      </particleLifetime>
                      <randomAngle dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">6.28318548</MaxValue>
                        <MinValue dataType="Float">0</MinValue>
                      </randomAngle>
                      <randomAngleVel dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">0.05</MaxValue>
                        <MinValue dataType="Float">-0.05</MinValue>
                      </randomAngleVel>
                      <randomPos dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">96</MaxValue>
                        <MinValue dataType="Float">32</MinValue>
                      </randomPos>
                      <randomVel dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">0</MaxValue>
                        <MinValue dataType="Float">0</MinValue>
                      </randomVel>
                      <spriteIndex dataType="Struct" type="Duality.Range">
                        <MaxValue dataType="Float">0</MaxValue>
                        <MinValue dataType="Float">0</MinValue>
                      </spriteIndex>
                    </item>
                  </_items>
                  <_size dataType="Int">1</_size>
                  <_version dataType="Int">2</_version>
                </emitters>
                <fadeInAt dataType="Float">0.5</fadeInAt>
                <fadeOutAt dataType="Float">0.5</fadeOutAt>
                <gameobj dataType="ObjectRef">1562013821</gameobj>
                <linearDrag dataType="Float">0</linearDrag>
                <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Glow.Material.res</contentPath>
                </material>
                <particleSize dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">256</X>
                  <Y dataType="Float">256</Y>
                </particleSize>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                <worldSpace dataType="Bool">false</worldSpace>
              </item>
            </_items>
            <_size dataType="Int">2</_size>
            <_version dataType="Int">2</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2656780960" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="928287035">
                <item dataType="Type" id="3579095766" value="Duality.Components.Transform" />
                <item dataType="Type" id="3391360986" value="DualStickSpaceShooter.ParticleEffect" />
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="397879848">
                <item dataType="ObjectRef">3922328753</item>
                <item dataType="ObjectRef">2588646914</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">3922328753</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="3745489585">4TVjNnkLCUWZlWATCWrRlQ==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">Glow</name>
          <parent dataType="ObjectRef">1914390279</parent>
          <prefabLink />
        </item>
      </_items>
      <_size dataType="Int">1</_size>
      <_version dataType="Int">1</_version>
    </children>
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2458033974">
      <_items dataType="Array" type="Duality.Component[]" id="458312230" length="8">
        <item dataType="ObjectRef">4274705211</item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="682199507">
          <active dataType="Bool">true</active>
          <angularDamp dataType="Float">0.3</angularDamp>
          <angularVel dataType="Float">0</angularVel>
          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
          <continous dataType="Bool">false</continous>
          <explicitMass dataType="Float">0</explicitMass>
          <fixedAngle dataType="Bool">false</fixedAngle>
          <gameobj dataType="ObjectRef">1914390279</gameobj>
          <ignoreGravity dataType="Bool">false</ignoreGravity>
          <joints />
          <linearDamp dataType="Float">0.3</linearDamp>
          <linearVel dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </linearVel>
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="61528163">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3713391334" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2986720640">
                <density dataType="Float">1</density>
                <friction dataType="Float">0.3</friction>
                <parent dataType="ObjectRef">682199507</parent>
                <position dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </position>
                <radius dataType="Float">64</radius>
                <restitution dataType="Float">0.3</restitution>
                <sensor dataType="Bool">true</sensor>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">24</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.ParticleEffect" id="2941023372">
          <active dataType="Bool">true</active>
          <angularDrag dataType="Float">0.3</angularDrag>
          <constantForce dataType="Struct" type="OpenTK.Vector3">
            <X dataType="Float">0</X>
            <Y dataType="Float">-0.5</Y>
            <Z dataType="Float">0</Z>
          </constantForce>
          <disposeWhenEmpty dataType="Bool">false</disposeWhenEmpty>
          <emitters dataType="Struct" type="System.Collections.Generic.List`1[[DualStickSpaceShooter.ParticleEmitter]]" id="1545311144">
            <_items dataType="Array" type="DualStickSpaceShooter.ParticleEmitter[]" id="3962073772" length="4">
              <item dataType="Struct" type="DualStickSpaceShooter.ParticleEmitter" id="3445257956">
                <basePos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </basePos>
                <baseVel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">-0.1</Z>
                </baseVel>
                <burstDelay dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">300</MaxValue>
                  <MinValue dataType="Float">50</MinValue>
                </burstDelay>
                <burstParticleNum dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">1</MaxValue>
                  <MinValue dataType="Float">1</MinValue>
                </burstParticleNum>
                <maxBurstCount dataType="Int">-1</maxBurstCount>
                <maxColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                  <A dataType="Float">0.5019608</A>
                  <H dataType="Float">0.2218814</H>
                  <S dataType="Float">0.6392157</S>
                  <V dataType="Float">1</V>
                </maxColor>
                <minColor dataType="Struct" type="Duality.Drawing.ColorHsva">
                  <A dataType="Float">0.2509804</A>
                  <H dataType="Float">0.254248351</H>
                  <S dataType="Float">1</S>
                  <V dataType="Float">1</V>
                </minColor>
                <particleLifetime dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">5000</MaxValue>
                  <MinValue dataType="Float">1000</MinValue>
                </particleLifetime>
                <randomAngle dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">6.28318548</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomAngle>
                <randomAngleVel dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">0.1</MaxValue>
                  <MinValue dataType="Float">-0.1</MinValue>
                </randomAngleVel>
                <randomPos dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">96</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomPos>
                <randomVel dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">0.5</MaxValue>
                  <MinValue dataType="Float">0</MinValue>
                </randomVel>
                <spriteIndex dataType="Struct" type="Duality.Range">
                  <MaxValue dataType="Float">3</MaxValue>
                  <MinValue dataType="Float">1</MinValue>
                </spriteIndex>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">2</_version>
          </emitters>
          <fadeInAt dataType="Float">0.35</fadeInAt>
          <fadeOutAt dataType="Float">0.65</fadeOutAt>
          <gameobj dataType="ObjectRef">1914390279</gameobj>
          <linearDrag dataType="Float">0.1</linearDrag>
          <material dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\Materials\GlowBubbles.Material.res</contentPath>
          </material>
          <particleSize dataType="Struct" type="OpenTK.Vector2">
            <X dataType="Float">8</X>
            <Y dataType="Float">8</Y>
          </particleSize>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
          <worldSpace dataType="Bool">true</worldSpace>
        </item>
        <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3657019101">
          <active dataType="Bool">true</active>
          <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">96</A>
            <B dataType="Byte">0</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">121</R>
          </colorTint>
          <customMat dataType="Struct" type="Duality.Resources.BatchInfo" id="3983175501">
            <dirtyFlag dataType="Enum" type="Duality.Resources.BatchInfo+DirtyFlag" name="None" value="0" />
            <hashCode dataType="Int">0</hashCode>
            <mainColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </mainColor>
            <technique dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.DrawTechnique]]">
              <contentPath dataType="String">Data\Materials\SharpAdd.DrawTechnique.res</contentPath>
            </technique>
            <textures dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[Duality.ContentRef`1[[Duality.Resources.Texture]]]]" id="1038241830" surrogate="true">
              <header />
              <body>
                <mainTex dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Texture]]">
                  <contentPath dataType="String">Default:Texture:White</contentPath>
                </mainTex>
              </body>
            </textures>
            <uniforms dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.String],[System.Single[]]]" id="586350266" surrogate="true">
              <header />
              <body>
                <smoothness dataType="Array" type="System.Single[]" id="3445896084">
                  <item dataType="Float">100</item>
                </smoothness>
              </body>
            </uniforms>
          </customMat>
          <gameobj dataType="ObjectRef">1914390279</gameobj>
          <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath />
          </iconMat>
          <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1304687288">
            <displayedText dataType="String">Happiness</displayedText>
            <elements dataType="Array" type="Duality.Drawing.FormattedText+Element[]" id="1292139815">
              <item dataType="Struct" type="Duality.Drawing.FormattedText+TextElement" id="3111904718">
                <text dataType="String">Happiness</text>
              </item>
            </elements>
            <flowAreas />
            <fontGlyphCount dataType="Array" type="System.Int32[]" id="3777869312">
              <item dataType="Int">9</item>
            </fontGlyphCount>
            <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1254897765">
              <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                <contentPath dataType="String">Data\Materials\WorldFont.Font.res</contentPath>
              </item>
            </fonts>
            <iconCount dataType="Int">0</iconCount>
            <icons />
            <lineAlign dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
            <maxHeight dataType="Int">0</maxHeight>
            <maxWidth dataType="Int">0</maxWidth>
            <sourceText dataType="String">Happiness</sourceText>
            <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
          </text>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.Trigger" id="420196983">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">1914390279</gameobj>
          <targets />
        </item>
        <item dataType="Struct" type="DualStickSpaceShooter.LevelGoal" id="549817556">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">1914390279</gameobj>
        </item>
        <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="3556556847">
          <active dataType="Bool">true</active>
          <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
            <A dataType="Byte">64</A>
            <B dataType="Byte">0</B>
            <G dataType="Byte">255</G>
            <R dataType="Byte">152</R>
          </colorTint>
          <customMat />
          <gameobj dataType="ObjectRef">1914390279</gameobj>
          <offset dataType="Int">-1</offset>
          <pixelGrid dataType="Bool">false</pixelGrid>
          <rect dataType="Struct" type="Duality.Rect">
            <H dataType="Float">256</H>
            <W dataType="Float">256</W>
            <X dataType="Float">-128</X>
            <Y dataType="Float">-128</Y>
          </rect>
          <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
          <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
            <contentPath dataType="String">Data\Materials\Glow.Material.res</contentPath>
          </sharedMat>
          <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
        </item>
      </_items>
      <_size dataType="Int">7</_size>
      <_version dataType="Int">9</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="682266808" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="4041440632">
          <item dataType="ObjectRef">3579095766</item>
          <item dataType="Type" id="2035388268" value="Duality.Components.Physics.RigidBody" />
          <item dataType="Type" id="1753121846" value="Duality.Components.Renderers.SpriteRenderer" />
          <item dataType="ObjectRef">3391360986</item>
          <item dataType="Type" id="3872205368" value="Duality.Components.Renderers.TextRenderer" />
          <item dataType="Type" id="3634707730" value="DualStickSpaceShooter.Trigger" />
          <item dataType="Type" id="2123244324" value="DualStickSpaceShooter.LevelGoal" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="3891391454">
          <item dataType="ObjectRef">4274705211</item>
          <item dataType="ObjectRef">682199507</item>
          <item dataType="ObjectRef">3556556847</item>
          <item dataType="ObjectRef">2941023372</item>
          <item dataType="ObjectRef">3657019101</item>
          <item dataType="ObjectRef">420196983</item>
          <item dataType="ObjectRef">549817556</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">4274705211</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="680444964">Lg6ZoIK3pUmynmWlVrqSjA==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">LevelGoal</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">LevelGoal</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
