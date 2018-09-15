<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2" />
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3389038587">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1998718601">
        <_items dataType="Array" type="Duality.Component[]" id="4156913550" length="8">
          <item dataType="Struct" type="Duality.Components.Transform" id="3446315805">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">3389038587</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="640457768">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3389038587</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="4094847412" custom="true">
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
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="1165205758">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3389038587</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1126723818">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3389038587</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.CameraController" id="2178211320">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3389038587</gameobj>
            <smoothness dataType="Float">1</smoothness>
            <targetObj dataType="Struct" type="Duality.GameObject" id="3943912706">
              <active dataType="Bool">true</active>
              <children />
              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4168280722">
                <_items dataType="Array" type="Duality.Component[]" id="598886480" length="8">
                  <item dataType="Struct" type="Duality.Components.Transform" id="4001189924">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <gameobj dataType="ObjectRef">3943912706</gameobj>
                    <ignoreParent dataType="Bool">false</ignoreParent>
                    <pos dataType="Struct" type="Duality.Vector3" />
                    <posAbs dataType="Struct" type="Duality.Vector3" />
                    <scale dataType="Float">1</scale>
                    <scaleAbs dataType="Float">1</scaleAbs>
                  </item>
                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3478842194">
                    <active dataType="Bool">true</active>
                    <allowParent dataType="Bool">false</allowParent>
                    <angularDamp dataType="Float">0.3</angularDamp>
                    <angularVel dataType="Float">0</angularVel>
                    <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                    <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                    <colFilter />
                    <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                    <explicitInertia dataType="Float">0</explicitInertia>
                    <explicitMass dataType="Float">80</explicitMass>
                    <fixedAngle dataType="Bool">true</fixedAngle>
                    <gameobj dataType="ObjectRef">3943912706</gameobj>
                    <ignoreGravity dataType="Bool">false</ignoreGravity>
                    <joints />
                    <linearDamp dataType="Float">0.3</linearDamp>
                    <linearVel dataType="Struct" type="Duality.Vector2" />
                    <revolutions dataType="Float">0</revolutions>
                    <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="297791306">
                      <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1586773856" length="4">
                        <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="4016662748">
                          <density dataType="Float">1</density>
                          <friction dataType="Float">0.3</friction>
                          <parent dataType="ObjectRef">3478842194</parent>
                          <position dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-7</Y>
                          </position>
                          <radius dataType="Float">15</radius>
                          <restitution dataType="Float">0.3</restitution>
                          <sensor dataType="Bool">false</sensor>
                          <userTag dataType="Int">0</userTag>
                        </item>
                      </_items>
                      <_size dataType="Int">1</_size>
                    </shapes>
                    <useCCD dataType="Bool">false</useCCD>
                  </item>
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="2578304612">
                    <active dataType="Bool">true</active>
                    <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                      <A dataType="Byte">255</A>
                      <B dataType="Byte">255</B>
                      <G dataType="Byte">255</G>
                      <R dataType="Byte">255</R>
                    </colorTint>
                    <customMat />
                    <depthScale dataType="Float">0.01</depthScale>
                    <gameobj dataType="ObjectRef">3943912706</gameobj>
                    <height dataType="Float">0</height>
                    <isVertical dataType="Bool">true</isVertical>
                    <offset dataType="Float">-0.08</offset>
                    <rect dataType="Struct" type="Duality.Rect">
                      <H dataType="Float">48</H>
                      <W dataType="Float">32</W>
                      <X dataType="Float">-16</X>
                      <Y dataType="Float">-40</Y>
                    </rect>
                    <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                      <contentPath dataType="String">Data\TilemapsSample\Actors\Cylinder.Material.res</contentPath>
                    </sharedMat>
                    <spriteIndex dataType="Int">0</spriteIndex>
                    <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  </item>
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator" id="849881872">
                    <active dataType="Bool">true</active>
                    <activeAnim />
                    <activeLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="Loop" value="2" />
                    <animations dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Samples.Tilemaps.RpgLike.ActorAnimation]]" id="3555693472">
                      <_items dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation[]" id="1131950812" length="4">
                        <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation" id="1792708292">
                          <duration dataType="Float">2</duration>
                          <frameCount dataType="Int">1</frameCount>
                          <name dataType="String">Idle</name>
                          <preferredLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="PingPong" value="3" />
                          <startFrame dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping[]" id="2365329220">
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">180</Angle>
                              <Direction dataType="String">Down</Direction>
                              <SpriteSheetIndex dataType="Int">4</SpriteSheetIndex>
                            </item>
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">270</Angle>
                              <Direction dataType="String">Left</Direction>
                              <SpriteSheetIndex dataType="Int">7</SpriteSheetIndex>
                            </item>
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">90</Angle>
                              <Direction dataType="String">Right</Direction>
                              <SpriteSheetIndex dataType="Int">10</SpriteSheetIndex>
                            </item>
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">0</Angle>
                              <Direction dataType="String">Up</Direction>
                              <SpriteSheetIndex dataType="Int">13</SpriteSheetIndex>
                            </item>
                          </startFrame>
                        </item>
                        <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation" id="3047170966">
                          <duration dataType="Float">0.5</duration>
                          <frameCount dataType="Int">3</frameCount>
                          <name dataType="String">Walk</name>
                          <preferredLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="PingPong" value="3" />
                          <startFrame dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping[]" id="796767566">
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">180</Angle>
                              <Direction dataType="String">Down</Direction>
                              <SpriteSheetIndex dataType="Int">3</SpriteSheetIndex>
                            </item>
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">270</Angle>
                              <Direction dataType="String">Left</Direction>
                              <SpriteSheetIndex dataType="Int">6</SpriteSheetIndex>
                            </item>
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">90</Angle>
                              <Direction dataType="String">Right</Direction>
                              <SpriteSheetIndex dataType="Int">9</SpriteSheetIndex>
                            </item>
                            <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping">
                              <Angle dataType="Float">0</Angle>
                              <Direction dataType="String">Up</Direction>
                              <SpriteSheetIndex dataType="Int">12</SpriteSheetIndex>
                            </item>
                          </startFrame>
                        </item>
                      </_items>
                      <_size dataType="Int">2</_size>
                    </animations>
                    <animDirection dataType="Float">0</animDirection>
                    <animSpeed dataType="Float">1</animSpeed>
                    <animTime dataType="Float">0</animTime>
                    <gameobj dataType="ObjectRef">3943912706</gameobj>
                  </item>
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.CharacterController" id="409204419">
                    <acceleration dataType="Float">0.15</acceleration>
                    <active dataType="Bool">true</active>
                    <gameobj dataType="ObjectRef">3943912706</gameobj>
                    <speed dataType="Float">3.5</speed>
                    <targetMovement dataType="Struct" type="Duality.Vector2" />
                  </item>
                </_items>
                <_size dataType="Int">5</_size>
              </compList>
              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2673632714" surrogate="true">
                <header />
                <body>
                  <keys dataType="Array" type="System.Object[]" id="344088776">
                    <item dataType="Type" id="3638581868" value="Duality.Components.Transform" />
                    <item dataType="Type" id="3529220150" value="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" />
                    <item dataType="Type" id="1997632312" value="Duality.Components.Physics.RigidBody" />
                    <item dataType="Type" id="3209055506" value="Duality.Samples.Tilemaps.RpgLike.CharacterController" />
                    <item dataType="Type" id="3537870372" value="Duality.Samples.Tilemaps.RpgLike.ActorAnimator" />
                  </keys>
                  <values dataType="Array" type="System.Object[]" id="1510998750">
                    <item dataType="ObjectRef">4001189924</item>
                    <item dataType="ObjectRef">2578304612</item>
                    <item dataType="ObjectRef">3478842194</item>
                    <item dataType="ObjectRef">409204419</item>
                    <item dataType="ObjectRef">849881872</item>
                  </values>
                </body>
              </compMap>
              <compTransform dataType="ObjectRef">4001189924</compTransform>
              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                <header>
                  <data dataType="Array" type="System.Byte[]" id="247178548">6QaaXpvsP06njO2/8LNlCA==</data>
                </header>
                <body />
              </identifier>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
              <name dataType="String">MainChar</name>
              <parent dataType="Struct" type="Duality.GameObject" id="4235466534">
                <active dataType="Bool">true</active>
                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2877712860">
                  <_items dataType="Array" type="Duality.GameObject[]" id="2666473156" length="16">
                    <item dataType="ObjectRef">3943912706</item>
                    <item dataType="Struct" type="Duality.GameObject" id="4110122891">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2006274471">
                        <_items dataType="Array" type="Duality.Component[]" id="1189522382">
                          <item dataType="Struct" type="Duality.Components.Transform" id="4167400109">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">4110122891</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">111</X>
                              <Y dataType="Float">30</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">111</X>
                              <Y dataType="Float">30</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3645052379">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">4110122891</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3370394363">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3668608598" length="4">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="4126707744">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">3645052379</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="2744514797">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">4110122891</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="754877088">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1832461005">
                              <active dataType="Bool">true</active>
                              <allowParent dataType="Bool">false</allowParent>
                              <angularDamp dataType="Float">0.3</angularDamp>
                              <angularVel dataType="Float">0</angularVel>
                              <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                              <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                              <colFilter />
                              <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                              <explicitInertia dataType="Float">0</explicitInertia>
                              <explicitMass dataType="Float">0</explicitMass>
                              <fixedAngle dataType="Bool">false</fixedAngle>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="2297531517">
                                <active dataType="Bool">true</active>
                                <children />
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1873239474">
                                  <_items dataType="Array" type="Duality.Component[]" id="2291543760" length="4">
                                    <item dataType="Struct" type="Duality.Components.Transform" id="2354808735">
                                      <active dataType="Bool">true</active>
                                      <angle dataType="Float">0</angle>
                                      <angleAbs dataType="Float">0</angleAbs>
                                      <gameobj dataType="ObjectRef">2297531517</gameobj>
                                      <ignoreParent dataType="Bool">false</ignoreParent>
                                      <pos dataType="Struct" type="Duality.Vector3" />
                                      <posAbs dataType="Struct" type="Duality.Vector3" />
                                      <scale dataType="Float">1</scale>
                                      <scaleAbs dataType="Float">1</scaleAbs>
                                    </item>
                                    <item dataType="ObjectRef">1832461005</item>
                                    <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollider" id="3960350400">
                                      <active dataType="Bool">true</active>
                                      <gameobj dataType="ObjectRef">2297531517</gameobj>
                                      <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                      <roundedCorners dataType="Bool">true</roundedCorners>
                                      <shapeFriction dataType="Float">0.300000161</shapeFriction>
                                      <shapeRestitution dataType="Float">0.300000161</shapeRestitution>
                                      <solidOuterEdges dataType="Bool">true</solidOuterEdges>
                                      <source dataType="Array" type="Duality.Plugins.Tilemaps.TilemapCollisionSource[]" id="2736753136">
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="3112643174">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="Struct" type="Duality.GameObject" id="3906997737">
                                              <active dataType="Bool">true</active>
                                              <children />
                                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3981390199">
                                                <_items dataType="Array" type="Duality.Component[]" id="3380664206" length="4">
                                                  <item dataType="Struct" type="Duality.Components.Transform" id="3964274955">
                                                    <active dataType="Bool">true</active>
                                                    <angle dataType="Float">0</angle>
                                                    <angleAbs dataType="Float">0</angleAbs>
                                                    <gameobj dataType="ObjectRef">3906997737</gameobj>
                                                    <ignoreParent dataType="Bool">false</ignoreParent>
                                                    <pos dataType="Struct" type="Duality.Vector3" />
                                                    <posAbs dataType="Struct" type="Duality.Vector3" />
                                                    <scale dataType="Float">1</scale>
                                                    <scaleAbs dataType="Float">1</scaleAbs>
                                                  </item>
                                                  <item dataType="ObjectRef">3112643174</item>
                                                  <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="486063535">
                                                    <active dataType="Bool">true</active>
                                                    <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                                      <A dataType="Byte">255</A>
                                                      <B dataType="Byte">255</B>
                                                      <G dataType="Byte">255</G>
                                                      <R dataType="Byte">255</R>
                                                    </colorTint>
                                                    <externalTilemap />
                                                    <gameobj dataType="ObjectRef">3906997737</gameobj>
                                                    <offset dataType="Float">0</offset>
                                                    <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                                    <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="World" value="2" />
                                                    <tileDepthOffset dataType="Int">0</tileDepthOffset>
                                                    <tileDepthScale dataType="Float">0.01</tileDepthScale>
                                                    <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                                                  </item>
                                                </_items>
                                                <_size dataType="Int">3</_size>
                                              </compList>
                                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4252456768" surrogate="true">
                                                <header />
                                                <body>
                                                  <keys dataType="Array" type="System.Object[]" id="1729161405">
                                                    <item dataType="Type" id="1191800358" value="Duality.Plugins.Tilemaps.Tilemap" />
                                                    <item dataType="ObjectRef">3638581868</item>
                                                    <item dataType="Type" id="2190866106" value="Duality.Plugins.Tilemaps.TilemapRenderer" />
                                                  </keys>
                                                  <values dataType="Array" type="System.Object[]" id="2258937016">
                                                    <item dataType="ObjectRef">3112643174</item>
                                                    <item dataType="ObjectRef">3964274955</item>
                                                    <item dataType="ObjectRef">486063535</item>
                                                  </values>
                                                </body>
                                              </compMap>
                                              <compTransform dataType="ObjectRef">3964274955</compTransform>
                                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                <header>
                                                  <data dataType="Array" type="System.Byte[]" id="788471959">KMXuZW3ZOEaxigxAvZjLRg==</data>
                                                </header>
                                                <body />
                                              </identifier>
                                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                              <name dataType="String">BaseLayer</name>
                                              <parent dataType="Struct" type="Duality.GameObject" id="4275032068">
                                                <active dataType="Bool">true</active>
                                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="4024810249">
                                                  <_items dataType="Array" type="Duality.GameObject[]" id="1213755022">
                                                    <item dataType="ObjectRef">3906997737</item>
                                                    <item dataType="Struct" type="Duality.GameObject" id="477194733">
                                                      <active dataType="Bool">true</active>
                                                      <children />
                                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="78608781">
                                                        <_items dataType="Array" type="Duality.Component[]" id="2186310950" length="4">
                                                          <item dataType="Struct" type="Duality.Components.Transform" id="534471951">
                                                            <active dataType="Bool">true</active>
                                                            <angle dataType="Float">0</angle>
                                                            <angleAbs dataType="Float">0</angleAbs>
                                                            <gameobj dataType="ObjectRef">477194733</gameobj>
                                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                                            <pos dataType="Struct" type="Duality.Vector3" />
                                                            <posAbs dataType="Struct" type="Duality.Vector3" />
                                                            <scale dataType="Float">1</scale>
                                                            <scaleAbs dataType="Float">1</scaleAbs>
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="3977807466">
                                                            <active dataType="Bool">true</active>
                                                            <gameobj dataType="ObjectRef">477194733</gameobj>
                                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2897774590" custom="true">
                                                              <body>
                                                                <version dataType="Int">3</version>
                                                                <data dataType="Array" type="System.Byte[]" id="808869264">H4sIAAAAAAAEAN2YyWpVQRCGb6ImoCYXzYQaNQ8gWRkXLhScsvQV7sKFSxdqIE7giHNUFNRFNGJUnIkzGF/hZukb+BhK/tr88FOnus85MeTC5aO6u7qrTldXDyONRmPk3/9IY/G3sMLxnaRpkn5k9vnt/6h3Fjd5ER7d+0qBD1mNeu4XXEF4TFgWJpXEWpKeEazwOHACOJk50HSmXjUYU9JYxyJOQzoDnKWWE8Dr4hECizkXgdE9XAQuAZcBc/McMAfs6Ejsepak55kGzpG0G0b0Q1oPeLlnCArjwHlykyd8Z9i/R0CbCttUF486Dr59MGIY0v5ik0bRpAlcQ+H1sBESM+QRY6Zc18lJZxcc6wbuovAe1bF6P0kP1QhtVch13ET2Is0tCZvNPcBe5d8wSU9VLweAgwqHqKWt6SepdvJitqxv6TBwtjH0YvQuJ8p5kQxQnYzPNrW0NS2XGmMQGFJ1nA49j7qBNUAXFXp6Zu7W4pYL1HIgrLAN2O408TZhTrE9QG+qubJuKlMvHp+G5E24SVIrVd0WwtVMHwyryqnHcTRT785SGRiHPBAcK9ZrOXXxeZDXMmmSl5CWxRR7N0wJ6fS7THPXpSoEprgOfKmv6/hmWiPk9vkGeEuFs6plHeDV+D5VPaDwCfhcibk/VeGvsPrX1PE8BTmbS52JSg7LPsg3iAlH3Ttf14Hcu+9hQL5B2GmU70CTwCnqRe5cgWvLPBAP01zIN4grqqXd3y9UMuzvVIU+QAatd8W4AdwEbgG3gXGaP+/+Xv9BcItTJ/fi1YDF4H1ID4BRtbj4/m6FndSLHP0l8CrTsVamXjXw0tMf4APwkfQCt1aDd64LhI2XOJtqVniqvPQ0SM9vyZ/OQiNwrtsAbFR10jKr4zDtIckenQLpSQZ7HPJ4YLD0uwnYDPAj118H+k2ZCBwAAA==</data>
                                                              </body>
                                                            </tileData>
                                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                                            </tileset>
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="1351227827">
                                                            <active dataType="Bool">true</active>
                                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                                              <A dataType="Byte">255</A>
                                                              <B dataType="Byte">255</B>
                                                              <G dataType="Byte">255</G>
                                                              <R dataType="Byte">255</R>
                                                            </colorTint>
                                                            <externalTilemap />
                                                            <gameobj dataType="ObjectRef">477194733</gameobj>
                                                            <offset dataType="Float">-0.01</offset>
                                                            <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                                            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="World" value="2" />
                                                            <tileDepthOffset dataType="Int">0</tileDepthOffset>
                                                            <tileDepthScale dataType="Float">0.01</tileDepthScale>
                                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                                                          </item>
                                                        </_items>
                                                        <_size dataType="Int">3</_size>
                                                      </compList>
                                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2168875960" surrogate="true">
                                                        <header />
                                                        <body>
                                                          <keys dataType="Array" type="System.Object[]" id="3949028071">
                                                            <item dataType="ObjectRef">1191800358</item>
                                                            <item dataType="ObjectRef">3638581868</item>
                                                            <item dataType="ObjectRef">2190866106</item>
                                                          </keys>
                                                          <values dataType="Array" type="System.Object[]" id="4185751424">
                                                            <item dataType="ObjectRef">3977807466</item>
                                                            <item dataType="ObjectRef">534471951</item>
                                                            <item dataType="ObjectRef">1351227827</item>
                                                          </values>
                                                        </body>
                                                      </compMap>
                                                      <compTransform dataType="ObjectRef">534471951</compTransform>
                                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                        <header>
                                                          <data dataType="Array" type="System.Byte[]" id="3942475941">OGFE/6l2ukykXD788OcpAg==</data>
                                                        </header>
                                                        <body />
                                                      </identifier>
                                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                                      <name dataType="String">UpperLayer</name>
                                                      <parent dataType="ObjectRef">4275032068</parent>
                                                      <prefabLink />
                                                    </item>
                                                    <item dataType="Struct" type="Duality.GameObject" id="3489595370">
                                                      <active dataType="Bool">true</active>
                                                      <children />
                                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="292666542">
                                                        <_items dataType="Array" type="Duality.Component[]" id="1981956432" length="4">
                                                          <item dataType="Struct" type="Duality.Components.Transform" id="3546872588">
                                                            <active dataType="Bool">true</active>
                                                            <angle dataType="Float">0</angle>
                                                            <angleAbs dataType="Float">0</angleAbs>
                                                            <gameobj dataType="ObjectRef">3489595370</gameobj>
                                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                                            <pos dataType="Struct" type="Duality.Vector3" />
                                                            <posAbs dataType="Struct" type="Duality.Vector3" />
                                                            <scale dataType="Float">1</scale>
                                                            <scaleAbs dataType="Float">1</scaleAbs>
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="2695240807">
                                                            <active dataType="Bool">true</active>
                                                            <gameobj dataType="ObjectRef">3489595370</gameobj>
                                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2522683971" custom="true">
                                                              <body>
                                                                <version dataType="Int">3</version>
                                                                <data dataType="Array" type="System.Byte[]" id="2955968550">H4sIAAAAAAAEAO2YsU7DMBCGbaA7IIaCGPoAZSsTEh0QMxsMSLDwEGWDJ2HkFRBPEd6EFiJBIBKov5E46XRxnTg5S6lUfbITx/8f3zm2R8aY0e//xix/Lz169FCCV2AOLEjlQoPAmvgAPoGCVLrSG5BrkCthC9gGdgisXWLNuuL/0hfqSg0eJOwCe8A+QQa4oXoHcnIt0+CB4sj63mnIUH0DpXZ/J1D95N1gHQ02gIH361GBtqfDzcBroSha9jcETjkMI/THTh7xbWYC6J2H9fKBnTwuCVTkUc38k6wk7bZY1V9iNj3Ep+lvbBm5/jZVeJAwWdWfJptucfJcfScbfKmMUTP5xzZQIT7Un4QZcKtBfKP+XIM74F6DeA9MMc2wJxJpRiSFW3GyJxIqBNaEW3G6rSw9kYjY7UOTD3Of8gNO9YDEZ2u7wcfAduxuYlI9HPRwrKNYulJ7NOCxixx3Iz73FiiB3UX+hRRAg4jG2QVKZxFeQVktUALN29AjqGs0OI/gby4IPAamQrc0b6WvaEf4AV6zwTUIHAAA</data>
                                                              </body>
                                                            </tileData>
                                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                                            </tileset>
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="68661168">
                                                            <active dataType="Bool">true</active>
                                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                                              <A dataType="Byte">255</A>
                                                              <B dataType="Byte">255</B>
                                                              <G dataType="Byte">255</G>
                                                              <R dataType="Byte">255</R>
                                                            </colorTint>
                                                            <externalTilemap />
                                                            <gameobj dataType="ObjectRef">3489595370</gameobj>
                                                            <offset dataType="Float">-0.02</offset>
                                                            <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                                            <tileDepthMode dataType="Enum" type="Duality.Plugins.Tilemaps.TileDepthOffsetMode" name="World" value="2" />
                                                            <tileDepthOffset dataType="Int">0</tileDepthOffset>
                                                            <tileDepthScale dataType="Float">0.01</tileDepthScale>
                                                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                                                          </item>
                                                        </_items>
                                                        <_size dataType="Int">3</_size>
                                                      </compList>
                                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4218267338" surrogate="true">
                                                        <header />
                                                        <body>
                                                          <keys dataType="Array" type="System.Object[]" id="2971739436">
                                                            <item dataType="ObjectRef">1191800358</item>
                                                            <item dataType="ObjectRef">3638581868</item>
                                                            <item dataType="ObjectRef">2190866106</item>
                                                          </keys>
                                                          <values dataType="Array" type="System.Object[]" id="1023396278">
                                                            <item dataType="ObjectRef">2695240807</item>
                                                            <item dataType="ObjectRef">3546872588</item>
                                                            <item dataType="ObjectRef">68661168</item>
                                                          </values>
                                                        </body>
                                                      </compMap>
                                                      <compTransform dataType="ObjectRef">3546872588</compTransform>
                                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                        <header>
                                                          <data dataType="Array" type="System.Byte[]" id="2521470840">/2KPfz/Jj0O7HOXC4QkFrA==</data>
                                                        </header>
                                                        <body />
                                                      </identifier>
                                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                                      <name dataType="String">TopLayer</name>
                                                      <parent dataType="ObjectRef">4275032068</parent>
                                                      <prefabLink />
                                                    </item>
                                                    <item dataType="ObjectRef">2297531517</item>
                                                  </_items>
                                                  <_size dataType="Int">4</_size>
                                                </children>
                                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="147573312">
                                                  <_items dataType="Array" type="Duality.Component[]" id="1597496259" length="0" />
                                                  <_size dataType="Int">0</_size>
                                                </compList>
                                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3233724075" surrogate="true">
                                                  <header />
                                                  <body>
                                                    <keys dataType="Array" type="System.Object[]" id="1542157492" length="0" />
                                                    <values dataType="Array" type="System.Object[]" id="2035757046" length="0" />
                                                  </body>
                                                </compMap>
                                                <compTransform />
                                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                  <header>
                                                    <data dataType="Array" type="System.Byte[]" id="1518691088">HunZh0b630iicX0zyp3aXg==</data>
                                                  </header>
                                                  <body />
                                                </identifier>
                                                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                                <name dataType="String">Map</name>
                                                <parent />
                                                <prefabLink />
                                              </parent>
                                              <prefabLink />
                                            </gameobj>
                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="3242948794" custom="true">
                                              <body>
                                                <version dataType="Int">3</version>
                                                <data dataType="Array" type="System.Byte[]" id="714432000">H4sIAAAAAAAEAKVYy2oUURDtDi5m1IXPRXyEzjxijIq604WgmwEVxEFc6A/4Gf7A/EI+RJTYiRpjwLgRXboR8xtKzmnp0xzr3okFw6EfdW+dW4+umqooiurPb7M4kC3APuBEeQAnAVZO4dlpwBlAjWebAXCj725Nqr8KIFK3Jum2dk3KUNiew9V5wIV5+f1wBh5WfVD+26RoMV3Tip6EGliLesZBHlY9kk9p2HV6aosFa2A+5KurA8YSZ6M0jAH7skqd3nYHsJt+8z/VowBbTsMAoPy4u3r6nWy7ClhLG2jBqr932+6KgSPx3+U0cIcl0RtyMQBdXKettgba44lcdaM0u6sDtBzOLRVAXbwT2EliPOubYuBwXvVbpdld+amQLSttD9CXI9C8ZfSoi2ln7QzkyZPYbcAEUM2rfrc0uy85j9nc1O/7KNvTtNPmJk+exO4BHgFW51WPXEWpAcpvmGY0Dl6hnZqb16gHILE7hQFVX5FVVN26SjOuBmjtsW9aRhqtqlABrgKuAJgkZRoq0WOOXXJ2DgI/1LTT6VEiRpFobmqSWEZ9ubLq1wHWcZLLM96sAXoutrBQjuLmMcDxecuvGFE8KFs3LcyKtOiaAr0aV1Gzru2dbc/PCk0bL4ERGdDQ1KVxsfC8fUqLcmRNlcpoJFiCbHt+0TEatHdaCIxvTAIMI5qAIwI8s0Vjwt8qlc8voyYXbZPUCGt1TxQ6J5EO4Q53LsbTrRyViF80jGQw6pubs+yT6KhrAtmAZpXK6ADtBCbEDptcORCNiNYB6gcdhSJ1bSQssSCkwlSzetaWTXkWzYZ8tgzgxzRqjdizj43xETTZn110GlBbNLLsbKijCZ/pV5RFhy2VtkbsaeU7nVEAmxqiktZb1C6vFip2NtwS48du2wpAp7LByhg/Aq80/DYEMvTUlhqQUQepwMZFe7Cos7Ktkdg5kSLXBOYHwGvANuCjUVc3ztQWmzk2j7Shs5I/EWU4jvy2BchvX1ZxZaZZhdHKqGPQrrhnUZxF/acVV0Y7/YTyewOw/KJTqgDMlYnsq8+Un04F9o/rKdYC7AnM7JFrr2j9p24M+HU+18wVzsU6Hts84hygETky/F5M21eE9chxlM+APQdfAOJGW007nY5rhhpP3y/boHPAWCCbX0PMDr1fcfUNMHE3hV/+CKWv9B4DnwIeAqI5QPgNAS8Bb6dCLJKfgF8AhpTeXGrbGY0RYQdh+anYMKVXxI3r+krG7JsffGq86Gk/0dmG/J7Q3CDj7F8Rlh8lmn11MTGwYevSqRMaGw6sPCvb8Bv2FLL8CBwAAA==</data>
                                              </body>
                                            </tileData>
                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                            </tileset>
                                          </SourceTilemap>
                                        </item>
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="ObjectRef">3977807466</SourceTilemap>
                                        </item>
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="ObjectRef">2695240807</SourceTilemap>
                                        </item>
                                      </source>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">3</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="698808906" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="2028457128">
                                      <item dataType="ObjectRef">3638581868</item>
                                      <item dataType="ObjectRef">1997632312</item>
                                      <item dataType="Type" id="190669484" value="Duality.Plugins.Tilemaps.TilemapCollider" />
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="1797981086">
                                      <item dataType="ObjectRef">2354808735</item>
                                      <item dataType="ObjectRef">1832461005</item>
                                      <item dataType="ObjectRef">3960350400</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">2354808735</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="4190870164">VLecLJN/pUiYA65m6S9w2A==</data>
                                  </header>
                                  <body />
                                </identifier>
                                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                <name dataType="String">WorldGeometry</name>
                                <parent dataType="ObjectRef">4275032068</parent>
                                <prefabLink />
                              </gameobj>
                              <ignoreGravity dataType="Bool">false</ignoreGravity>
                              <joints />
                              <linearDamp dataType="Float">0.3</linearDamp>
                              <linearVel dataType="Struct" type="Duality.Vector2" />
                              <revolutions dataType="Float">0</revolutions>
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="398561065">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3700211726" length="64" />
                                <_size dataType="Int">0</_size>
                              </shapes>
                              <useCCD dataType="Bool">false</useCCD>
                            </baseObject>
                            <gameobj dataType="ObjectRef">4110122891</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2372915200" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="489906573">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="Type" id="934208806" value="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" />
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2874260408">
                            <item dataType="ObjectRef">4167400109</item>
                            <item dataType="ObjectRef">2744514797</item>
                            <item dataType="ObjectRef">3645052379</item>
                            <item dataType="ObjectRef">754877088</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">4167400109</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="906143847">e3TD9tOei0uqvFlc2UiUJA==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1885660048">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="6096600">
                        <_items dataType="Array" type="Duality.Component[]" id="2923853740">
                          <item dataType="Struct" type="Duality.Components.Transform" id="1942937266">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">1885660048</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">163.001</X>
                              <Y dataType="Float">218</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">163.001</X>
                              <Y dataType="Float">218</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1420589536">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">1885660048</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4032140296">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1846706540">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="401222500">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">1420589536</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="520051954">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">1885660048</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2825381541">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">1885660048</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3271510174" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3824824986">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="4061784890">
                            <item dataType="ObjectRef">1942937266</item>
                            <item dataType="ObjectRef">520051954</item>
                            <item dataType="ObjectRef">1420589536</item>
                            <item dataType="ObjectRef">2825381541</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">1942937266</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="2688197146">l7Qnr1YuLE6cy5v//zOa5A==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1597144067">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="23814511">
                        <_items dataType="Array" type="Duality.Component[]" id="3440608750">
                          <item dataType="Struct" type="Duality.Components.Transform" id="1654421285">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">1597144067</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">-57.9979858</X>
                              <Y dataType="Float">116</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">-57.9979858</X>
                              <Y dataType="Float">116</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1132073555">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">1597144067</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2300843699">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="29176870">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="729430272">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">1132073555</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="231535973">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">1597144067</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2536865560">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">1597144067</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="987112096" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="35372741">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2713045544">
                            <item dataType="ObjectRef">1654421285</item>
                            <item dataType="ObjectRef">231535973</item>
                            <item dataType="ObjectRef">1132073555</item>
                            <item dataType="ObjectRef">2536865560</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">1654421285</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1190315343">ssaSvKC0V0uDKAPSRpjy+w==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1042148688">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1473425560">
                        <_items dataType="Array" type="Duality.Component[]" id="2190162476">
                          <item dataType="Struct" type="Duality.Components.Transform" id="1099425906">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">1042148688</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">93.00301</X>
                              <Y dataType="Float">49</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">93.00301</X>
                              <Y dataType="Float">49</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="577078176">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">1042148688</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3814738632">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3610963564">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2868200292">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">577078176</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3971507890">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">1042148688</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1981870181">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">1042148688</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2469692702" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="625195866">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="660474810">
                            <item dataType="ObjectRef">1099425906</item>
                            <item dataType="ObjectRef">3971507890</item>
                            <item dataType="ObjectRef">577078176</item>
                            <item dataType="ObjectRef">1981870181</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">1099425906</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1307502938">T+HZGeHns06dRQZDpFMazQ==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="361886464">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2323001928">
                        <_items dataType="Array" type="Duality.Component[]" id="527352940">
                          <item dataType="Struct" type="Duality.Components.Transform" id="419163682">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">361886464</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">176.002014</X>
                              <Y dataType="Float">243</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">176.002014</X>
                              <Y dataType="Float">243</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4191783248">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">361886464</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3635346680">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3167460204">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1426591588">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">4191783248</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3291245666">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">361886464</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1301607957">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">361886464</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2243358942" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="4210343178">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="754573850">
                            <item dataType="ObjectRef">419163682</item>
                            <item dataType="ObjectRef">3291245666</item>
                            <item dataType="ObjectRef">4191783248</item>
                            <item dataType="ObjectRef">1301607957</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">419163682</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="4212385770">n1YFWCx+W0SpLxLK1a5tUQ==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="137720349">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="664819825">
                        <_items dataType="Array" type="Duality.Component[]" id="2127823534">
                          <item dataType="Struct" type="Duality.Components.Transform" id="194997567">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">137720349</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">-47.9969864</X>
                              <Y dataType="Float">466</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">-47.9969864</X>
                              <Y dataType="Float">466</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3967617133">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">137720349</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="871923853">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2960001318">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="873612544">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">3967617133</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3067079551">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">137720349</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1077441842">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">137720349</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3298867936" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3671973339">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="340419432">
                            <item dataType="ObjectRef">194997567</item>
                            <item dataType="ObjectRef">3067079551</item>
                            <item dataType="ObjectRef">3967617133</item>
                            <item dataType="ObjectRef">1077441842</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">194997567</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="4058274065">KHxchtd4OEO4ckavklp8wg==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="2858365907">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1040399679">
                        <_items dataType="Array" type="Duality.Component[]" id="1239115438">
                          <item dataType="Struct" type="Duality.Components.Transform" id="2915643125">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">2858365907</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">398.0181</X>
                              <Y dataType="Float">-206.787567</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">398.0181</X>
                              <Y dataType="Float">-206.787567</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2393295395">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">2858365907</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1873679075">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="624708838">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2996858240">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">2393295395</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="1492757813">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">2858365907</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="3798087400">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">2858365907</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2426277600" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3930667253">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="3334882504">
                            <item dataType="ObjectRef">2915643125</item>
                            <item dataType="ObjectRef">1492757813</item>
                            <item dataType="ObjectRef">2393295395</item>
                            <item dataType="ObjectRef">3798087400</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2915643125</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1624274239">tpeu3QwrwEGRRHYg0s2p2A==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="433081310">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1158205382">
                        <_items dataType="Array" type="Duality.Component[]" id="2670019840">
                          <item dataType="Struct" type="Duality.Components.Transform" id="490358528">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <gameobj dataType="ObjectRef">433081310</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <pos dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">283.0191</X>
                              <Y dataType="Float">-79.78757</Y>
                              <Z dataType="Float">0</Z>
                            </pos>
                            <posAbs dataType="Struct" type="Duality.Vector3">
                              <X dataType="Float">283.0191</X>
                              <Y dataType="Float">-79.78757</Y>
                              <Z dataType="Float">0</Z>
                            </posAbs>
                            <scale dataType="Float">1</scale>
                            <scaleAbs dataType="Float">1</scaleAbs>
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4262978094">
                            <active dataType="Bool">true</active>
                            <allowParent dataType="Bool">false</allowParent>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">433081310</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2550940910">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3616379984">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="903180732">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">4262978094</parent>
                                  <position dataType="Struct" type="Duality.Vector2">
                                    <X dataType="Float">0</X>
                                    <Y dataType="Float">-3</Y>
                                  </position>
                                  <radius dataType="Float">13</radius>
                                  <restitution dataType="Float">0.3</restitution>
                                  <sensor dataType="Bool">false</sensor>
                                  <userTag dataType="Int">0</userTag>
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                            </shapes>
                            <useCCD dataType="Bool">false</useCCD>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3362440512">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">433081310</gameobj>
                            <height dataType="Float">0</height>
                            <isVertical dataType="Bool">true</isVertical>
                            <offset dataType="Float">-0.1</offset>
                            <rect dataType="Struct" type="Duality.Rect">
                              <H dataType="Float">34</H>
                              <W dataType="Float">32</W>
                              <X dataType="Float">-16</X>
                              <Y dataType="Float">-26</Y>
                            </rect>
                            <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                              <contentPath dataType="String">Data\TilemapsSample\Actors\Barrel.Material.res</contentPath>
                            </sharedMat>
                            <spriteIndex dataType="Int">-1</spriteIndex>
                            <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1372802803">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">1832461005</baseObject>
                            <gameobj dataType="ObjectRef">433081310</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1247783610" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3522233908">
                            <item dataType="ObjectRef">3638581868</item>
                            <item dataType="ObjectRef">3529220150</item>
                            <item dataType="ObjectRef">1997632312</item>
                            <item dataType="ObjectRef">934208806</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="4053076726">
                            <item dataType="ObjectRef">490358528</item>
                            <item dataType="ObjectRef">3362440512</item>
                            <item dataType="ObjectRef">4262978094</item>
                            <item dataType="ObjectRef">1372802803</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">490358528</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1617005968">Arkw8mFMK0aJ4uJMFIXC8w==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">4235466534</parent>
                      <prefabLink />
                    </item>
                  </_items>
                  <_size dataType="Int">9</_size>
                </children>
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4080264470">
                  <_items dataType="Array" type="Duality.Component[]" id="500638454" length="0" />
                  <_size dataType="Int">0</_size>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1976102728" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="3917829272" length="0" />
                    <values dataType="Array" type="System.Object[]" id="310490398" length="0" />
                  </body>
                </compMap>
                <compTransform />
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="1587400388">3UfIa/qU9UGLaU0jGSIzMQ==</data>
                  </header>
                  <body />
                </identifier>
                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                <name dataType="String">Actors</name>
                <parent />
                <prefabLink />
              </parent>
              <prefabLink />
            </targetObj>
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3210901312" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1252704067">
            <item dataType="ObjectRef">3638581868</item>
            <item dataType="Type" id="514498086" value="Duality.Components.Camera" />
            <item dataType="Type" id="4191491770" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="2757364006" value="Duality.Samples.Tilemaps.RpgLike.CameraController" />
            <item dataType="Type" id="3293347770" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="667727032">
            <item dataType="ObjectRef">3446315805</item>
            <item dataType="ObjectRef">640457768</item>
            <item dataType="ObjectRef">1126723818</item>
            <item dataType="ObjectRef">2178211320</item>
            <item dataType="ObjectRef">1165205758</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3446315805</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2650908521">G/E04Bh2zE6yFHaIss4iEQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">4275032068</item>
    <item dataType="Struct" type="Duality.GameObject" id="3020941391">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3075010093">
        <_items dataType="Array" type="Duality.Component[]" id="833798758" length="4">
          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.Player" id="273032724">
            <active dataType="Bool">true</active>
            <character dataType="ObjectRef">409204419</character>
            <gameobj dataType="ObjectRef">3020941391</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="549299832" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2881964359">
            <item dataType="Type" id="3976798926" value="Duality.Samples.Tilemaps.RpgLike.Player" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3405096704">
            <item dataType="ObjectRef">273032724</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2090662085">IffF10lNek6ct3+h8WBmZw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Player</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">4235466534</item>
    <item dataType="ObjectRef">3906997737</item>
    <item dataType="ObjectRef">477194733</item>
    <item dataType="ObjectRef">3489595370</item>
    <item dataType="ObjectRef">2297531517</item>
    <item dataType="ObjectRef">3943912706</item>
    <item dataType="ObjectRef">4110122891</item>
    <item dataType="ObjectRef">1885660048</item>
    <item dataType="ObjectRef">1597144067</item>
    <item dataType="ObjectRef">1042148688</item>
    <item dataType="ObjectRef">361886464</item>
    <item dataType="ObjectRef">137720349</item>
    <item dataType="ObjectRef">2858365907</item>
    <item dataType="ObjectRef">433081310</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
