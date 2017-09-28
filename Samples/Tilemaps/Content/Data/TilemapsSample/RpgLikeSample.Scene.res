<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2" />
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="3761029062">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="664753936">
        <_items dataType="Array" type="Duality.Component[]" id="83719996">
          <item dataType="Struct" type="Duality.Components.Transform" id="1826376698">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="3337573">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <priority dataType="Int">0</priority>
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="119543137">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.CameraController" id="2728393213">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
            <smoothness dataType="Float">1</smoothness>
            <targetObj dataType="Struct" type="Duality.GameObject" id="1593919710">
              <active dataType="Bool">true</active>
              <children />
              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4086686021">
                <_items dataType="Array" type="Duality.Component[]" id="2890465494" length="8">
                  <item dataType="Struct" type="Duality.Components.Transform" id="3954234642">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <ignoreParent dataType="Bool">false</ignoreParent>
                    <parentTransform />
                    <pos dataType="Struct" type="Duality.Vector3" />
                    <posAbs dataType="Struct" type="Duality.Vector3" />
                    <scale dataType="Float">1</scale>
                    <scaleAbs dataType="Float">1</scaleAbs>
                    <vel dataType="Struct" type="Duality.Vector3" />
                    <velAbs dataType="Struct" type="Duality.Vector3" />
                  </item>
                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="361728938">
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
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <ignoreGravity dataType="Bool">false</ignoreGravity>
                    <joints />
                    <linearDamp dataType="Float">0.3</linearDamp>
                    <linearVel dataType="Struct" type="Duality.Vector2" />
                    <revolutions dataType="Float">0</revolutions>
                    <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="879589214">
                      <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="463189776" length="4">
                        <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1425700668">
                          <density dataType="Float">1</density>
                          <friction dataType="Float">0.3</friction>
                          <parent dataType="ObjectRef">361728938</parent>
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
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="1523684570">
                    <active dataType="Bool">true</active>
                    <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                      <A dataType="Byte">255</A>
                      <B dataType="Byte">255</B>
                      <G dataType="Byte">255</G>
                      <R dataType="Byte">255</R>
                    </colorTint>
                    <customMat />
                    <depthScale dataType="Float">0.01</depthScale>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
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
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator" id="2825026600">
                    <active dataType="Bool">true</active>
                    <activeAnim />
                    <activeLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="Loop" value="2" />
                    <animations dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Samples.Tilemaps.RpgLike.ActorAnimation]]" id="3621637028">
                      <_items dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation[]" id="1024382148" length="4">
                        <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation" id="893933380">
                          <duration dataType="Float">2</duration>
                          <frameCount dataType="Int">1</frameCount>
                          <name dataType="String">Idle</name>
                          <preferredLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="PingPong" value="3" />
                          <startFrame dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping[]" id="3279277636">
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
                        <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation" id="1865850518">
                          <duration dataType="Float">0.5</duration>
                          <frameCount dataType="Int">3</frameCount>
                          <name dataType="String">Walk</name>
                          <preferredLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="PingPong" value="3" />
                          <startFrame dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping[]" id="3898763982">
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
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                  </item>
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.CharacterController" id="3117089289">
                    <acceleration dataType="Float">0.15</acceleration>
                    <active dataType="Bool">true</active>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <speed dataType="Float">3.5</speed>
                    <targetMovement dataType="Struct" type="Duality.Vector2" />
                  </item>
                </_items>
                <_size dataType="Int">5</_size>
              </compList>
              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2205103144" surrogate="true">
                <header />
                <body>
                  <keys dataType="Array" type="System.Object[]" id="1433274159">
                    <item dataType="Type" id="3338094830" value="Duality.Components.Transform" />
                    <item dataType="Type" id="3312523722" value="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" />
                    <item dataType="Type" id="4001243870" value="Duality.Components.Physics.RigidBody" />
                    <item dataType="Type" id="2076820314" value="Duality.Samples.Tilemaps.RpgLike.CharacterController" />
                    <item dataType="Type" id="2187056014" value="Duality.Samples.Tilemaps.RpgLike.ActorAnimator" />
                  </keys>
                  <values dataType="Array" type="System.Object[]" id="3650817440">
                    <item dataType="ObjectRef">3954234642</item>
                    <item dataType="ObjectRef">1523684570</item>
                    <item dataType="ObjectRef">361728938</item>
                    <item dataType="ObjectRef">3117089289</item>
                    <item dataType="ObjectRef">2825026600</item>
                  </values>
                </body>
              </compMap>
              <compTransform dataType="ObjectRef">3954234642</compTransform>
              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                <header>
                  <data dataType="Array" type="System.Byte[]" id="2195430845">6QaaXpvsP06njO2/8LNlCA==</data>
                </header>
                <body />
              </identifier>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
              <name dataType="String">MainChar</name>
              <parent dataType="Struct" type="Duality.GameObject" id="2989622809">
                <active dataType="Bool">true</active>
                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="4202398178">
                  <_items dataType="Array" type="Duality.GameObject[]" id="470726288" length="16">
                    <item dataType="ObjectRef">1593919710</item>
                    <item dataType="Struct" type="Duality.GameObject" id="823148500">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4278090012">
                        <_items dataType="Array" type="Duality.Component[]" id="2411532228">
                          <item dataType="Struct" type="Duality.Components.Transform" id="3183463432">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">823148500</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3885925024">
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
                            <gameobj dataType="ObjectRef">823148500</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1845950120">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1003257516" length="4">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2595940068">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">3885925024</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="752913360">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">823148500</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2894164495">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3013493237">
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
                              <gameobj dataType="Struct" type="Duality.GameObject" id="4245684009">
                                <active dataType="Bool">true</active>
                                <children />
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2424276985">
                                  <_items dataType="Array" type="Duality.Component[]" id="87982670" length="4">
                                    <item dataType="Struct" type="Duality.Components.Transform" id="2311031645">
                                      <active dataType="Bool">true</active>
                                      <angle dataType="Float">0</angle>
                                      <angleAbs dataType="Float">0</angleAbs>
                                      <angleVel dataType="Float">0</angleVel>
                                      <angleVelAbs dataType="Float">0</angleVelAbs>
                                      <deriveAngle dataType="Bool">true</deriveAngle>
                                      <gameobj dataType="ObjectRef">4245684009</gameobj>
                                      <ignoreParent dataType="Bool">false</ignoreParent>
                                      <parentTransform />
                                      <pos dataType="Struct" type="Duality.Vector3" />
                                      <posAbs dataType="Struct" type="Duality.Vector3" />
                                      <scale dataType="Float">1</scale>
                                      <scaleAbs dataType="Float">1</scaleAbs>
                                      <vel dataType="Struct" type="Duality.Vector3" />
                                      <velAbs dataType="Struct" type="Duality.Vector3" />
                                    </item>
                                    <item dataType="ObjectRef">3013493237</item>
                                    <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollider" id="2359977928">
                                      <active dataType="Bool">true</active>
                                      <gameobj dataType="ObjectRef">4245684009</gameobj>
                                      <origin dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                                      <roundedCorners dataType="Bool">true</roundedCorners>
                                      <shapeFriction dataType="Float">0.300000161</shapeFriction>
                                      <shapeRestitution dataType="Float">0.300000161</shapeRestitution>
                                      <solidOuterEdges dataType="Bool">true</solidOuterEdges>
                                      <source dataType="Array" type="Duality.Plugins.Tilemaps.TilemapCollisionSource[]" id="3923551764">
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="668293575">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="Struct" type="Duality.GameObject" id="2961408854">
                                              <active dataType="Bool">true</active>
                                              <children />
                                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3123208323">
                                                <_items dataType="Array" type="Duality.Component[]" id="1229118246" length="4">
                                                  <item dataType="Struct" type="Duality.Components.Transform" id="1026756490">
                                                    <active dataType="Bool">true</active>
                                                    <angle dataType="Float">0</angle>
                                                    <angleAbs dataType="Float">0</angleAbs>
                                                    <angleVel dataType="Float">0</angleVel>
                                                    <angleVelAbs dataType="Float">0</angleVelAbs>
                                                    <deriveAngle dataType="Bool">true</deriveAngle>
                                                    <gameobj dataType="ObjectRef">2961408854</gameobj>
                                                    <ignoreParent dataType="Bool">false</ignoreParent>
                                                    <parentTransform />
                                                    <pos dataType="Struct" type="Duality.Vector3" />
                                                    <posAbs dataType="Struct" type="Duality.Vector3" />
                                                    <scale dataType="Float">1</scale>
                                                    <scaleAbs dataType="Float">1</scaleAbs>
                                                    <vel dataType="Struct" type="Duality.Vector3" />
                                                    <velAbs dataType="Struct" type="Duality.Vector3" />
                                                  </item>
                                                  <item dataType="ObjectRef">668293575</item>
                                                  <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="1659517006">
                                                    <active dataType="Bool">true</active>
                                                    <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                                      <A dataType="Byte">255</A>
                                                      <B dataType="Byte">255</B>
                                                      <G dataType="Byte">255</G>
                                                      <R dataType="Byte">255</R>
                                                    </colorTint>
                                                    <externalTilemap />
                                                    <gameobj dataType="ObjectRef">2961408854</gameobj>
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
                                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2170791352" surrogate="true">
                                                <header />
                                                <body>
                                                  <keys dataType="Array" type="System.Object[]" id="620474345">
                                                    <item dataType="Type" id="2044790542" value="Duality.Plugins.Tilemaps.Tilemap" />
                                                    <item dataType="ObjectRef">3338094830</item>
                                                    <item dataType="Type" id="3802331466" value="Duality.Plugins.Tilemaps.TilemapRenderer" />
                                                  </keys>
                                                  <values dataType="Array" type="System.Object[]" id="3290271936">
                                                    <item dataType="ObjectRef">668293575</item>
                                                    <item dataType="ObjectRef">1026756490</item>
                                                    <item dataType="ObjectRef">1659517006</item>
                                                  </values>
                                                </body>
                                              </compMap>
                                              <compTransform dataType="ObjectRef">1026756490</compTransform>
                                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                <header>
                                                  <data dataType="Array" type="System.Byte[]" id="765568587">KMXuZW3ZOEaxigxAvZjLRg==</data>
                                                </header>
                                                <body />
                                              </identifier>
                                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                              <name dataType="String">BaseLayer</name>
                                              <parent dataType="Struct" type="Duality.GameObject" id="10461599">
                                                <active dataType="Bool">true</active>
                                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3085913554">
                                                  <_items dataType="Array" type="Duality.GameObject[]" id="3348431696">
                                                    <item dataType="ObjectRef">2961408854</item>
                                                    <item dataType="Struct" type="Duality.GameObject" id="3085774208">
                                                      <active dataType="Bool">true</active>
                                                      <children />
                                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1153350704">
                                                        <_items dataType="Array" type="Duality.Component[]" id="2733980348" length="4">
                                                          <item dataType="Struct" type="Duality.Components.Transform" id="1151121844">
                                                            <active dataType="Bool">true</active>
                                                            <angle dataType="Float">0</angle>
                                                            <angleAbs dataType="Float">0</angleAbs>
                                                            <angleVel dataType="Float">0</angleVel>
                                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                                            <gameobj dataType="ObjectRef">3085774208</gameobj>
                                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                                            <parentTransform />
                                                            <pos dataType="Struct" type="Duality.Vector3" />
                                                            <posAbs dataType="Struct" type="Duality.Vector3" />
                                                            <scale dataType="Float">1</scale>
                                                            <scaleAbs dataType="Float">1</scaleAbs>
                                                            <vel dataType="Struct" type="Duality.Vector3" />
                                                            <velAbs dataType="Struct" type="Duality.Vector3" />
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="792658929">
                                                            <active dataType="Bool">true</active>
                                                            <gameobj dataType="ObjectRef">3085774208</gameobj>
                                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="154550861" custom="true">
                                                              <body>
                                                                <version dataType="Int">3</version>
                                                                <data dataType="Array" type="System.Byte[]" id="2892059174">H4sIAAAAAAAEAN2YyWpVQRCGb6ImoCYXzYQaNQ8gWRkXLhScsvQV7sKFSxdqIE7giHNUFNRFNGJUnIkzGF/hZukb+BhK/tr88FOnus85MeTC5aO6u7qrTldXDyONRmPk3/9IY/G3sMLxnaRpkn5k9vnt/6h3Fjd5ER7d+0qBD1mNeu4XXEF4TFgWJpXEWpKeEazwOHACOJk50HSmXjUYU9JYxyJOQzoDnKWWE8Dr4hECizkXgdE9XAQuAZcBc/McMAfs6Ejsepak55kGzpG0G0b0Q1oPeLlnCArjwHlykyd8Z9i/R0CbCttUF486Dr59MGIY0v5ik0bRpAlcQ+H1sBESM+QRY6Zc18lJZxcc6wbuovAe1bF6P0kP1QhtVch13ET2Is0tCZvNPcBe5d8wSU9VLweAgwqHqKWt6SepdvJitqxv6TBwtjH0YvQuJ8p5kQxQnYzPNrW0NS2XGmMQGFJ1nA49j7qBNUAXFXp6Zu7W4pYL1HIgrLAN2O408TZhTrE9QG+qubJuKlMvHp+G5E24SVIrVd0WwtVMHwyryqnHcTRT785SGRiHPBAcK9ZrOXXxeZDXMmmSl5CWxRR7N0wJ6fS7THPXpSoEprgOfKmv6/hmWiPk9vkGeEuFs6plHeDV+D5VPaDwCfhcibk/VeGvsPrX1PE8BTmbS52JSg7LPsg3iAlH3Ttf14Hcu+9hQL5B2GmU70CTwCnqRe5cgWvLPBAP01zIN4grqqXd3y9UMuzvVIU+QAatd8W4AdwEbgG3gXGaP+/+Xv9BcItTJ/fi1YDF4H1ID4BRtbj4/m6FndSLHP0l8CrTsVamXjXw0tMf4APwkfQCt1aDd64LhI2XOJtqVniqvPQ0SM9vyZ/OQiNwrtsAbFR10jKr4zDtIckenQLpSQZ7HPJ4YLD0uwnYDPAj118H+k2ZCBwAAA==</data>
                                                              </body>
                                                            </tileData>
                                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                                            </tileset>
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="1783882360">
                                                            <active dataType="Bool">true</active>
                                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                                              <A dataType="Byte">255</A>
                                                              <B dataType="Byte">255</B>
                                                              <G dataType="Byte">255</G>
                                                              <R dataType="Byte">255</R>
                                                            </colorTint>
                                                            <externalTilemap />
                                                            <gameobj dataType="ObjectRef">3085774208</gameobj>
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
                                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2199851630" surrogate="true">
                                                        <header />
                                                        <body>
                                                          <keys dataType="Array" type="System.Object[]" id="702823938">
                                                            <item dataType="ObjectRef">2044790542</item>
                                                            <item dataType="ObjectRef">3338094830</item>
                                                            <item dataType="ObjectRef">3802331466</item>
                                                          </keys>
                                                          <values dataType="Array" type="System.Object[]" id="4058816394">
                                                            <item dataType="ObjectRef">792658929</item>
                                                            <item dataType="ObjectRef">1151121844</item>
                                                            <item dataType="ObjectRef">1783882360</item>
                                                          </values>
                                                        </body>
                                                      </compMap>
                                                      <compTransform dataType="ObjectRef">1151121844</compTransform>
                                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                        <header>
                                                          <data dataType="Array" type="System.Byte[]" id="3595648114">OGFE/6l2ukykXD788OcpAg==</data>
                                                        </header>
                                                        <body />
                                                      </identifier>
                                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                                      <name dataType="String">UpperLayer</name>
                                                      <parent dataType="ObjectRef">10461599</parent>
                                                      <prefabLink />
                                                    </item>
                                                    <item dataType="Struct" type="Duality.GameObject" id="1404051932">
                                                      <active dataType="Bool">true</active>
                                                      <children />
                                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2041346404">
                                                        <_items dataType="Array" type="Duality.Component[]" id="3346320324" length="4">
                                                          <item dataType="Struct" type="Duality.Components.Transform" id="3764366864">
                                                            <active dataType="Bool">true</active>
                                                            <angle dataType="Float">0</angle>
                                                            <angleAbs dataType="Float">0</angleAbs>
                                                            <angleVel dataType="Float">0</angleVel>
                                                            <angleVelAbs dataType="Float">0</angleVelAbs>
                                                            <deriveAngle dataType="Bool">true</deriveAngle>
                                                            <gameobj dataType="ObjectRef">1404051932</gameobj>
                                                            <ignoreParent dataType="Bool">false</ignoreParent>
                                                            <parentTransform />
                                                            <pos dataType="Struct" type="Duality.Vector3" />
                                                            <posAbs dataType="Struct" type="Duality.Vector3" />
                                                            <scale dataType="Float">1</scale>
                                                            <scaleAbs dataType="Float">1</scaleAbs>
                                                            <vel dataType="Struct" type="Duality.Vector3" />
                                                            <velAbs dataType="Struct" type="Duality.Vector3" />
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="3405903949">
                                                            <active dataType="Bool">true</active>
                                                            <gameobj dataType="ObjectRef">1404051932</gameobj>
                                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="2178647969" custom="true">
                                                              <body>
                                                                <version dataType="Int">3</version>
                                                                <data dataType="Array" type="System.Byte[]" id="2318784622">H4sIAAAAAAAEAO2YsU7DMBCGbaA7IIaCGPoAZSsTEh0QMxsMSLDwEGWDJ2HkFRBPEd6EFiJBIBKov5E46XRxnTg5S6lUfbITx/8f3zm2R8aY0e//xix/Lz169FCCV2AOLEjlQoPAmvgAPoGCVLrSG5BrkCthC9gGdgisXWLNuuL/0hfqSg0eJOwCe8A+QQa4oXoHcnIt0+CB4sj63mnIUH0DpXZ/J1D95N1gHQ02gIH361GBtqfDzcBroSha9jcETjkMI/THTh7xbWYC6J2H9fKBnTwuCVTkUc38k6wk7bZY1V9iNj3Ep+lvbBm5/jZVeJAwWdWfJptucfJcfScbfKmMUTP5xzZQIT7Un4QZcKtBfKP+XIM74F6DeA9MMc2wJxJpRiSFW3GyJxIqBNaEW3G6rSw9kYjY7UOTD3Of8gNO9YDEZ2u7wcfAduxuYlI9HPRwrKNYulJ7NOCxixx3Iz73FiiB3UX+hRRAg4jG2QVKZxFeQVktUALN29AjqGs0OI/gby4IPAamQrc0b6WvaEf4AV6zwTUIHAAA</data>
                                                              </body>
                                                            </tileData>
                                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                                            </tileset>
                                                          </item>
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapRenderer" id="102160084">
                                                            <active dataType="Bool">true</active>
                                                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                                                              <A dataType="Byte">255</A>
                                                              <B dataType="Byte">255</B>
                                                              <G dataType="Byte">255</G>
                                                              <R dataType="Byte">255</R>
                                                            </colorTint>
                                                            <externalTilemap />
                                                            <gameobj dataType="ObjectRef">1404051932</gameobj>
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
                                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2398880278" surrogate="true">
                                                        <header />
                                                        <body>
                                                          <keys dataType="Array" type="System.Object[]" id="817726510">
                                                            <item dataType="ObjectRef">2044790542</item>
                                                            <item dataType="ObjectRef">3338094830</item>
                                                            <item dataType="ObjectRef">3802331466</item>
                                                          </keys>
                                                          <values dataType="Array" type="System.Object[]" id="198204618">
                                                            <item dataType="ObjectRef">3405903949</item>
                                                            <item dataType="ObjectRef">3764366864</item>
                                                            <item dataType="ObjectRef">102160084</item>
                                                          </values>
                                                        </body>
                                                      </compMap>
                                                      <compTransform dataType="ObjectRef">3764366864</compTransform>
                                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                        <header>
                                                          <data dataType="Array" type="System.Byte[]" id="623842718">/2KPfz/Jj0O7HOXC4QkFrA==</data>
                                                        </header>
                                                        <body />
                                                      </identifier>
                                                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                                      <name dataType="String">TopLayer</name>
                                                      <parent dataType="ObjectRef">10461599</parent>
                                                      <prefabLink />
                                                    </item>
                                                    <item dataType="ObjectRef">4245684009</item>
                                                  </_items>
                                                  <_size dataType="Int">4</_size>
                                                </children>
                                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="123808970">
                                                  <_items dataType="Array" type="Duality.Component[]" id="760342280" length="0" />
                                                  <_size dataType="Int">0</_size>
                                                </compList>
                                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2775477346" surrogate="true">
                                                  <header />
                                                  <body>
                                                    <keys dataType="Array" type="System.Object[]" id="1471459872" length="0" />
                                                    <values dataType="Array" type="System.Object[]" id="1617525646" length="0" />
                                                  </body>
                                                </compMap>
                                                <compTransform />
                                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                  <header>
                                                    <data dataType="Array" type="System.Byte[]" id="1255858492">HunZh0b630iicX0zyp3aXg==</data>
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
                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="319539647" custom="true">
                                              <body>
                                                <version dataType="Int">3</version>
                                                <data dataType="Array" type="System.Byte[]" id="3677113774">H4sIAAAAAAAEAKVYy2oUURDtDi5m1IXPRXyEzjxijIq604WgmwEVxEFc6A/4Gf7A/EI+RJTYiRpjwLgRXboR8xtKzmnp0xzr3okFw6EfdW+dW4+umqooiurPb7M4kC3APuBEeQAnAVZO4dlpwBlAjWebAXCj725Nqr8KIFK3Jum2dk3KUNiew9V5wIV5+f1wBh5WfVD+26RoMV3Tip6EGliLesZBHlY9kk9p2HV6aosFa2A+5KurA8YSZ6M0jAH7skqd3nYHsJt+8z/VowBbTsMAoPy4u3r6nWy7ClhLG2jBqr932+6KgSPx3+U0cIcl0RtyMQBdXKettgba44lcdaM0u6sDtBzOLRVAXbwT2EliPOubYuBwXvVbpdld+amQLSttD9CXI9C8ZfSoi2ln7QzkyZPYbcAEUM2rfrc0uy85j9nc1O/7KNvTtNPmJk+exO4BHgFW51WPXEWpAcpvmGY0Dl6hnZqb16gHILE7hQFVX5FVVN26SjOuBmjtsW9aRhqtqlABrgKuAJgkZRoq0WOOXXJ2DgI/1LTT6VEiRpFobmqSWEZ9ubLq1wHWcZLLM96sAXoutrBQjuLmMcDxecuvGFE8KFs3LcyKtOiaAr0aV1Gzru2dbc/PCk0bL4ERGdDQ1KVxsfC8fUqLcmRNlcpoJFiCbHt+0TEatHdaCIxvTAIMI5qAIwI8s0Vjwt8qlc8voyYXbZPUCGt1TxQ6J5EO4Q53LsbTrRyViF80jGQw6pubs+yT6KhrAtmAZpXK6ADtBCbEDptcORCNiNYB6gcdhSJ1bSQssSCkwlSzetaWTXkWzYZ8tgzgxzRqjdizj43xETTZn110GlBbNLLsbKijCZ/pV5RFhy2VtkbsaeU7nVEAmxqiktZb1C6vFip2NtwS48du2wpAp7LByhg/Aq80/DYEMvTUlhqQUQepwMZFe7Cos7Ktkdg5kSLXBOYHwGvANuCjUVc3ztQWmzk2j7Shs5I/EWU4jvy2BchvX1ZxZaZZhdHKqGPQrrhnUZxF/acVV0Y7/YTyewOw/KJTqgDMlYnsq8+Un04F9o/rKdYC7AnM7JFrr2j9p24M+HU+18wVzsU6Hts84hygETky/F5M21eE9chxlM+APQdfAOJGW007nY5rhhpP3y/boHPAWCCbX0PMDr1fcfUNMHE3hV/+CKWv9B4DnwIeAqI5QPgNAS8Bb6dCLJKfgF8AhpTeXGrbGY0RYQdh+anYMKVXxI3r+krG7JsffGq86Gk/0dmG/J7Q3CDj7F8Rlh8lmn11MTGwYevSqRMaGw6sPCvb8Bv2FLL8CBwAAA==</data>
                                              </body>
                                            </tileData>
                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                            </tileset>
                                          </SourceTilemap>
                                        </item>
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="ObjectRef">792658929</SourceTilemap>
                                        </item>
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="ObjectRef">3405903949</SourceTilemap>
                                        </item>
                                      </source>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">3</_size>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1397158016" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="3365961299">
                                      <item dataType="ObjectRef">3338094830</item>
                                      <item dataType="ObjectRef">4001243870</item>
                                      <item dataType="Type" id="4069236070" value="Duality.Plugins.Tilemaps.TilemapCollider" />
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="4291037560">
                                      <item dataType="ObjectRef">2311031645</item>
                                      <item dataType="ObjectRef">3013493237</item>
                                      <item dataType="ObjectRef">2359977928</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">2311031645</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="3315028537">VLecLJN/pUiYA65m6S9w2A==</data>
                                  </header>
                                  <body />
                                </identifier>
                                <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                <name dataType="String">WorldGeometry</name>
                                <parent dataType="ObjectRef">10461599</parent>
                                <prefabLink />
                              </gameobj>
                              <ignoreGravity dataType="Bool">false</ignoreGravity>
                              <joints />
                              <linearDamp dataType="Float">0.3</linearDamp>
                              <linearVel dataType="Struct" type="Duality.Vector2" />
                              <revolutions dataType="Float">0</revolutions>
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2313585264">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2074421564" length="64" />
                                <_size dataType="Int">0</_size>
                              </shapes>
                              <useCCD dataType="Bool">false</useCCD>
                            </baseObject>
                            <gameobj dataType="ObjectRef">823148500</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1755575830" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="2552904630">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="Type" id="1969571168" value="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" />
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="646311066">
                            <item dataType="ObjectRef">3183463432</item>
                            <item dataType="ObjectRef">752913360</item>
                            <item dataType="ObjectRef">3885925024</item>
                            <item dataType="ObjectRef">2894164495</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3183463432</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3360159318">e3TD9tOei0uqvFlc2UiUJA==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="443677279">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3419817051">
                        <_items dataType="Array" type="Duality.Component[]" id="492517782">
                          <item dataType="Struct" type="Duality.Components.Transform" id="2803992211">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">443677279</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3506453803">
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
                            <gameobj dataType="ObjectRef">443677279</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4284331163">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1142153110">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2455097376">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">3506453803</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="373442139">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">443677279</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2514693274">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">443677279</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="559208552" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="1510859697">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="3772275808">
                            <item dataType="ObjectRef">2803992211</item>
                            <item dataType="ObjectRef">373442139</item>
                            <item dataType="ObjectRef">3506453803</item>
                            <item dataType="ObjectRef">2514693274</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2803992211</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="2157870691">l7Qnr1YuLE6cy5v//zOa5A==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="187862302">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4112081886">
                        <_items dataType="Array" type="Duality.Component[]" id="1137197328">
                          <item dataType="Struct" type="Duality.Components.Transform" id="2548177234">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">187862302</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3250638826">
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
                            <gameobj dataType="ObjectRef">187862302</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="486559042">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="77786128">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2310707004">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">3250638826</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="117627162">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">187862302</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2258878297">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">187862302</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="922536202" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="1180846076">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2061485974">
                            <item dataType="ObjectRef">2548177234</item>
                            <item dataType="ObjectRef">117627162</item>
                            <item dataType="ObjectRef">3250638826</item>
                            <item dataType="ObjectRef">2258878297</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2548177234</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3917946536">ssaSvKC0V0uDKAPSRpjy+w==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="4164464433">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1291238677">
                        <_items dataType="Array" type="Duality.Component[]" id="3481485430">
                          <item dataType="Struct" type="Duality.Components.Transform" id="2229812069">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">4164464433</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2932273661">
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
                            <gameobj dataType="ObjectRef">4164464433</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3641324237">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="317864998">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2972211456">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">2932273661</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="4094229293">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">4164464433</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1940513132">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">4164464433</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="818164424" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3387299007">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="3808932320">
                            <item dataType="ObjectRef">2229812069</item>
                            <item dataType="ObjectRef">4094229293</item>
                            <item dataType="ObjectRef">2932273661</item>
                            <item dataType="ObjectRef">1940513132</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2229812069</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="2493138541">T+HZGeHns06dRQZDpFMazQ==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="875106965">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3679953537">
                        <_items dataType="Array" type="Duality.Component[]" id="1601086766">
                          <item dataType="Struct" type="Duality.Components.Transform" id="3235421897">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">875106965</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3937883489">
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
                            <gameobj dataType="ObjectRef">875106965</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3572747729">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="297231086">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1375903824">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">3937883489</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="804871825">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">875106965</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2946122960">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">875106965</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1583175520" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="309681483">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2183821128">
                            <item dataType="ObjectRef">3235421897</item>
                            <item dataType="ObjectRef">804871825</item>
                            <item dataType="ObjectRef">3937883489</item>
                            <item dataType="ObjectRef">2946122960</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3235421897</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="4053953025">n1YFWCx+W0SpLxLK1a5tUQ==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="3096133586">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2196543562">
                        <_items dataType="Array" type="Duality.Component[]" id="445138272">
                          <item dataType="Struct" type="Duality.Components.Transform" id="1161481222">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">3096133586</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1863942814">
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
                            <gameobj dataType="ObjectRef">3096133586</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2182003518">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3964778000">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="884310844">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">1863942814</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3025898446">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">3096133586</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="872182285">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">3096133586</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1859447962" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3210815792">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="4177202798">
                            <item dataType="ObjectRef">1161481222</item>
                            <item dataType="ObjectRef">3025898446</item>
                            <item dataType="ObjectRef">1863942814</item>
                            <item dataType="ObjectRef">872182285</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">1161481222</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="4116274572">KHxchtd4OEO4ckavklp8wg==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="2158155438">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3287900942">
                        <_items dataType="Array" type="Duality.Component[]" id="3083668944">
                          <item dataType="Struct" type="Duality.Components.Transform" id="223503074">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">2158155438</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="925964666">
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
                            <gameobj dataType="ObjectRef">2158155438</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1496033970">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4041661136">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3714559676">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">925964666</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="2087920298">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">2158155438</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="4229171433">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">2158155438</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1009381706" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="2375333580">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="3562008310">
                            <item dataType="ObjectRef">223503074</item>
                            <item dataType="ObjectRef">2087920298</item>
                            <item dataType="ObjectRef">925964666</item>
                            <item dataType="ObjectRef">4229171433</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">223503074</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="2997433560">tpeu3QwrwEGRRHYg0s2p2A==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                    <item dataType="Struct" type="Duality.GameObject" id="1548877786">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3028470674">
                        <_items dataType="Array" type="Duality.Component[]" id="1541283408">
                          <item dataType="Struct" type="Duality.Components.Transform" id="3909192718">
                            <active dataType="Bool">true</active>
                            <angle dataType="Float">0</angle>
                            <angleAbs dataType="Float">0</angleAbs>
                            <angleVel dataType="Float">0</angleVel>
                            <angleVelAbs dataType="Float">0</angleVelAbs>
                            <deriveAngle dataType="Bool">true</deriveAngle>
                            <gameobj dataType="ObjectRef">1548877786</gameobj>
                            <ignoreParent dataType="Bool">false</ignoreParent>
                            <parentTransform />
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
                            <vel dataType="Struct" type="Duality.Vector3" />
                            <velAbs dataType="Struct" type="Duality.Vector3" />
                          </item>
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="316687014">
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
                            <gameobj dataType="ObjectRef">1548877786</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2411632710">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3562625024">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="183216284">
                                  <density dataType="Float">1</density>
                                  <friction dataType="Float">0.3</friction>
                                  <parent dataType="ObjectRef">316687014</parent>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="1478642646">
                            <active dataType="Bool">true</active>
                            <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                              <A dataType="Byte">255</A>
                              <B dataType="Byte">255</B>
                              <G dataType="Byte">255</G>
                              <R dataType="Byte">255</R>
                            </colorTint>
                            <customMat />
                            <depthScale dataType="Float">0.01</depthScale>
                            <gameobj dataType="ObjectRef">1548877786</gameobj>
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="3619893781">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">1548877786</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2951935946" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3116447688">
                            <item dataType="ObjectRef">3338094830</item>
                            <item dataType="ObjectRef">3312523722</item>
                            <item dataType="ObjectRef">4001243870</item>
                            <item dataType="ObjectRef">1969571168</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="1201511134">
                            <item dataType="ObjectRef">3909192718</item>
                            <item dataType="ObjectRef">1478642646</item>
                            <item dataType="ObjectRef">316687014</item>
                            <item dataType="ObjectRef">3619893781</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3909192718</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="2800518708">Arkw8mFMK0aJ4uJMFIXC8w==</data>
                        </header>
                        <body />
                      </identifier>
                      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      <name dataType="String">Barrel</name>
                      <parent dataType="ObjectRef">2989622809</parent>
                      <prefabLink />
                    </item>
                  </_items>
                  <_size dataType="Int">9</_size>
                </children>
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4070417546">
                  <_items dataType="Array" type="Duality.Component[]" id="2371088248" length="0" />
                  <_size dataType="Int">0</_size>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2771824658" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="4147830688" length="0" />
                    <values dataType="Array" type="System.Object[]" id="4115168910" length="0" />
                  </body>
                </compMap>
                <compTransform />
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="2741800892">3UfIa/qU9UGLaU0jGSIzMQ==</data>
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
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1778928878" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1373440354">
            <item dataType="ObjectRef">3338094830</item>
            <item dataType="Type" id="1128403856" value="Duality.Components.Camera" />
            <item dataType="Type" id="3457661678" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="3352197740" value="Duality.Samples.Tilemaps.RpgLike.CameraController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="105870730">
            <item dataType="ObjectRef">1826376698</item>
            <item dataType="ObjectRef">3337573</item>
            <item dataType="ObjectRef">119543137</item>
            <item dataType="ObjectRef">2728393213</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1826376698</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1324124818">G/E04Bh2zE6yFHaIss4iEQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">MainCamera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">10461599</item>
    <item dataType="Struct" type="Duality.GameObject" id="3851204924">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="367255378">
        <_items dataType="Array" type="Duality.Component[]" id="2959980880" length="4">
          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.Player" id="1642506005">
            <active dataType="Bool">true</active>
            <character dataType="ObjectRef">3117089289</character>
            <gameobj dataType="ObjectRef">3851204924</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1943215818" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4114983048">
            <item dataType="Type" id="1898588012" value="Duality.Samples.Tilemaps.RpgLike.Player" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="527819742">
            <item dataType="ObjectRef">1642506005</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4191128180">IffF10lNek6ct3+h8WBmZw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Player</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">2989622809</item>
    <item dataType="ObjectRef">2961408854</item>
    <item dataType="ObjectRef">3085774208</item>
    <item dataType="ObjectRef">1404051932</item>
    <item dataType="ObjectRef">4245684009</item>
    <item dataType="ObjectRef">1593919710</item>
    <item dataType="ObjectRef">823148500</item>
    <item dataType="ObjectRef">443677279</item>
    <item dataType="ObjectRef">187862302</item>
    <item dataType="ObjectRef">4164464433</item>
    <item dataType="ObjectRef">875106965</item>
    <item dataType="ObjectRef">3096133586</item>
    <item dataType="ObjectRef">2158155438</item>
    <item dataType="ObjectRef">1548877786</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
