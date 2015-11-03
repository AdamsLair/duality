﻿<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId />
    <nameHint dataType="String">Obstacle</nameHint>
  </assetInfo>
  <objTree dataType="Struct" type="Duality.GameObject" id="903249358">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2238146713">
      <_items dataType="Array" type="Duality.GameObject[]" id="3282903630" length="4">
        <item dataType="Struct" type="Duality.GameObject" id="1347508094">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1191165842">
            <_items dataType="Array" type="Duality.Component[]" id="3709343312" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="3707823026">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">1347508094</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3263564290">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">903249358</gameobj>
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
                </parentTransform>
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
              <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="1054943475">
                <active dataType="Bool">true</active>
                <animDuration dataType="Float">5</animDuration>
                <animFirstFrame dataType="Int">0</animFirstFrame>
                <animFrameCount dataType="Int">0</animFrameCount>
                <animLoopMode dataType="Enum" type="Duality.Components.Renderers.AnimSpriteRenderer+LoopMode" name="RandomSingle" value="3" />
                <animPaused dataType="Bool">false</animPaused>
                <animTime dataType="Float">3.32663727</animTime>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customFrameSequence />
                <customMat />
                <gameobj dataType="ObjectRef">1347508094</gameobj>
                <offset dataType="Int">0</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">500</H>
                  <W dataType="Float">100</W>
                  <X dataType="Float">-50</X>
                  <Y dataType="Float">-600</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\FlapOrDie\Graphics\columns-top.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="115317322">
                <active dataType="Bool">true</active>
                <angularDamp dataType="Float">0</angularDamp>
                <angularVel dataType="Float">0</angularVel>
                <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                <colFilter />
                <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                <continous dataType="Bool">false</continous>
                <explicitMass dataType="Float">0</explicitMass>
                <fixedAngle dataType="Bool">false</fixedAngle>
                <gameobj dataType="ObjectRef">1347508094</gameobj>
                <ignoreGravity dataType="Bool">true</ignoreGravity>
                <joints />
                <linearDamp dataType="Float">0</linearDamp>
                <linearVel dataType="Struct" type="Duality.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </linearVel>
                <revolutions dataType="Float">0</revolutions>
                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3345711778">
                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2143866640" length="4">
                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1347217212">
                      <density dataType="Float">1</density>
                      <friction dataType="Float">0.3</friction>
                      <parent dataType="ObjectRef">115317322</parent>
                      <restitution dataType="Float">0.3</restitution>
                      <sensor dataType="Bool">false</sensor>
                      <vertices dataType="Array" type="Duality.Vector2[]" id="2097821508">
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-100</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">50</X>
                          <Y dataType="Float">-500</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">-50</X>
                          <Y dataType="Float">-500</Y>
                        </item>
                      </vertices>
                    </item>
                  </_items>
                  <_size dataType="Int">1</_size>
                  <_version dataType="Int">6</_version>
                </shapes>
              </item>
            </_items>
            <_size dataType="Int">3</_size>
            <_version dataType="Int">3</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1811525578" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Object[]" id="2076419016">
                <item dataType="Type" id="3989138028" value="Duality.Components.Transform" />
                <item dataType="Type" id="3622625334" value="Duality.Components.Renderers.AnimSpriteRenderer" />
                <item dataType="Type" id="3430433592" value="Duality.Components.Physics.RigidBody" />
              </keys>
              <values dataType="Array" type="System.Object[]" id="2603053790">
                <item dataType="ObjectRef">3707823026</item>
                <item dataType="ObjectRef">1054943475</item>
                <item dataType="ObjectRef">115317322</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">3707823026</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="1869713972">iBQNeFzbdEOtlvPoNaCUgg==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">Top</name>
          <parent dataType="ObjectRef">903249358</parent>
          <prefabLink />
        </item>
        <item dataType="Struct" type="Duality.GameObject" id="3541755688">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1195036852">
            <_items dataType="Array" type="Duality.Component[]" id="2353008036" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="1607103324">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">3541755688</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="ObjectRef">3263564290</parentTransform>
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
              <item dataType="Struct" type="Duality.Components.Renderers.AnimSpriteRenderer" id="3249191069">
                <active dataType="Bool">true</active>
                <animDuration dataType="Float">5</animDuration>
                <animFirstFrame dataType="Int">0</animFirstFrame>
                <animFrameCount dataType="Int">0</animFrameCount>
                <animLoopMode dataType="Enum" type="Duality.Components.Renderers.AnimSpriteRenderer+LoopMode" name="RandomSingle" value="3" />
                <animPaused dataType="Bool">false</animPaused>
                <animTime dataType="Float">1.221462</animTime>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customFrameSequence />
                <customMat />
                <gameobj dataType="ObjectRef">3541755688</gameobj>
                <offset dataType="Int">0</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">500</H>
                  <W dataType="Float">100</W>
                  <X dataType="Float">-50</X>
                  <Y dataType="Float">100</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\FlapOrDie\Graphics\columns-bottom.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2309564916">
                <active dataType="Bool">true</active>
                <angularDamp dataType="Float">0</angularDamp>
                <angularVel dataType="Float">0</angularVel>
                <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                <colFilter />
                <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                <continous dataType="Bool">false</continous>
                <explicitMass dataType="Float">0</explicitMass>
                <fixedAngle dataType="Bool">false</fixedAngle>
                <gameobj dataType="ObjectRef">3541755688</gameobj>
                <ignoreGravity dataType="Bool">true</ignoreGravity>
                <joints />
                <linearDamp dataType="Float">0</linearDamp>
                <linearVel dataType="Struct" type="Duality.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </linearVel>
                <revolutions dataType="Float">0</revolutions>
                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3112637236">
                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3493882020" length="4">
                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2740076740">
                      <density dataType="Float">1</density>
                      <friction dataType="Float">0.3</friction>
                      <parent dataType="ObjectRef">2309564916</parent>
                      <restitution dataType="Float">0.3</restitution>
                      <sensor dataType="Bool">false</sensor>
                      <vertices dataType="Array" type="Duality.Vector2[]" id="3369019204">
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">100</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">-50</X>
                          <Y dataType="Float">500</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">50</X>
                          <Y dataType="Float">500</Y>
                        </item>
                      </vertices>
                    </item>
                  </_items>
                  <_size dataType="Int">1</_size>
                  <_version dataType="Int">6</_version>
                </shapes>
              </item>
            </_items>
            <_size dataType="Int">3</_size>
            <_version dataType="Int">3</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="904708086" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Object[]" id="3340480926">
                <item dataType="ObjectRef">3989138028</item>
                <item dataType="ObjectRef">3622625334</item>
                <item dataType="ObjectRef">3430433592</item>
              </keys>
              <values dataType="Array" type="System.Object[]" id="33741194">
                <item dataType="ObjectRef">1607103324</item>
                <item dataType="ObjectRef">3249191069</item>
                <item dataType="ObjectRef">2309564916</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">1607103324</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="3801019502">6Kfl0v9sYECZg21CAg5DoA==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">Bottom</name>
          <parent dataType="ObjectRef">903249358</parent>
          <prefabLink />
        </item>
      </_items>
      <_size dataType="Int">2</_size>
      <_version dataType="Int">2</_version>
    </children>
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2210599040">
      <_items dataType="Array" type="Duality.Component[]" id="2364087987" length="4">
        <item dataType="ObjectRef">3263564290</item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3966025882">
          <active dataType="Bool">true</active>
          <angularDamp dataType="Float">0</angularDamp>
          <angularVel dataType="Float">0</angularVel>
          <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
          <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat2" value="2" />
          <colFilter />
          <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
          <continous dataType="Bool">false</continous>
          <explicitMass dataType="Float">0</explicitMass>
          <fixedAngle dataType="Bool">false</fixedAngle>
          <gameobj dataType="ObjectRef">903249358</gameobj>
          <ignoreGravity dataType="Bool">false</ignoreGravity>
          <joints />
          <linearDamp dataType="Float">0</linearDamp>
          <linearVel dataType="Struct" type="Duality.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </linearVel>
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3461381236">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4047112100" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1329919172">
                <density dataType="Float">1</density>
                <friction dataType="Float">0</friction>
                <parent dataType="ObjectRef">3966025882</parent>
                <restitution dataType="Float">0</restitution>
                <sensor dataType="Bool">true</sensor>
                <vertices dataType="Array" type="Duality.Vector2[]" id="244373316">
                  <item dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-100</Y>
                  </item>
                  <item dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">100</Y>
                  </item>
                  <item dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">15</X>
                    <Y dataType="Float">0</Y>
                  </item>
                </vertices>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">6</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="FlapOrDie.Tags.Obstacle" id="3310321378">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">903249358</gameobj>
        </item>
      </_items>
      <_size dataType="Int">3</_size>
      <_version dataType="Int">3</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="114970587" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Object[]" id="2768244820">
          <item dataType="ObjectRef">3989138028</item>
          <item dataType="ObjectRef">3430433592</item>
          <item dataType="Type" id="3159999716" value="FlapOrDie.Tags.Obstacle" />
        </keys>
        <values dataType="Array" type="System.Object[]" id="372091830">
          <item dataType="ObjectRef">3263564290</item>
          <item dataType="ObjectRef">3966025882</item>
          <item dataType="ObjectRef">3310321378</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">3263564290</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="2213766000">7z93I6Eb20KGPQ0R/zKE+g==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">Obstacle</name>
    <parent />
    <prefabLink />
  </objTree>
</root>
<!-- XmlFormatterBase Document Separator -->
