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
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="293189449">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="79299470" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="2606380240">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="1919160942">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba" />
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="OrthoScreen" value="1" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </passes>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="119543137">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.CameraController" id="1844544486">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3761029062</gameobj>
            <smoothness dataType="Float">1</smoothness>
            <targetObj dataType="Struct" type="Duality.GameObject" id="1593919710">
              <active dataType="Bool">true</active>
              <children />
              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3195412880">
                <_items dataType="Array" type="Duality.Component[]" id="420700476" length="8">
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
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3907431145">
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
                  <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="361728938">
                    <active dataType="Bool">true</active>
                    <angularDamp dataType="Float">0.3</angularDamp>
                    <angularVel dataType="Float">0</angularVel>
                    <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                    <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                    <colFilter />
                    <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                    <continous dataType="Bool">false</continous>
                    <explicitInertia dataType="Float">0</explicitInertia>
                    <explicitMass dataType="Float">80</explicitMass>
                    <fixedAngle dataType="Bool">true</fixedAngle>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <ignoreGravity dataType="Bool">false</ignoreGravity>
                    <joints />
                    <linearDamp dataType="Float">0.3</linearDamp>
                    <linearVel dataType="Struct" type="Duality.Vector2" />
                    <revolutions dataType="Float">0</revolutions>
                    <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3790767434">
                      <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2806249312" length="4">
                        <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="4276353244">
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
                        </item>
                      </_items>
                      <_size dataType="Int">1</_size>
                      <_version dataType="Int">1</_version>
                    </shapes>
                  </item>
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.CharacterController" id="479935388">
                    <acceleration dataType="Float">0.15</acceleration>
                    <active dataType="Bool">true</active>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                    <speed dataType="Float">3.5</speed>
                    <targetMovement dataType="Struct" type="Duality.Vector2" />
                  </item>
                  <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator" id="2170487535">
                    <active dataType="Bool">true</active>
                    <activeAnim />
                    <activeLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="Loop" value="2" />
                    <animations dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Samples.Tilemaps.RpgLike.ActorAnimation]]" id="301622227">
                      <_items dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation[]" id="118561382" length="4">
                        <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation" id="2771520384">
                          <duration dataType="Float">2</duration>
                          <frameCount dataType="Int">1</frameCount>
                          <name dataType="String">Idle</name>
                          <preferredLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="PingPong" value="3" />
                          <startFrame dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping[]" id="2235101596">
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
                        <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimation" id="1460458702">
                          <duration dataType="Float">0.5</duration>
                          <frameCount dataType="Int">3</frameCount>
                          <name dataType="String">Walk</name>
                          <preferredLoopMode dataType="Enum" type="Duality.Samples.Tilemaps.RpgLike.ActorAnimator+LoopMode" name="PingPong" value="3" />
                          <startFrame dataType="Array" type="Duality.Samples.Tilemaps.RpgLike.AnimDirMapping[]" id="2299729234">
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
                      <_version dataType="Int">4</_version>
                    </animations>
                    <animDirection dataType="Float">0</animDirection>
                    <animSpeed dataType="Float">1</animSpeed>
                    <animTime dataType="Float">0</animTime>
                    <gameobj dataType="ObjectRef">1593919710</gameobj>
                  </item>
                </_items>
                <_size dataType="Int">5</_size>
                <_version dataType="Int">7</_version>
              </compList>
              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="111452910" surrogate="true">
                <header />
                <body>
                  <keys dataType="Array" type="System.Object[]" id="3864501218">
                    <item dataType="Type" id="4219969168" value="Duality.Components.Transform" />
                    <item dataType="Type" id="3361562350" value="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" />
                    <item dataType="Type" id="794497900" value="Duality.Components.Physics.RigidBody" />
                    <item dataType="Type" id="1223005970" value="Duality.Samples.Tilemaps.RpgLike.CharacterController" />
                    <item dataType="Type" id="3118878792" value="Duality.Samples.Tilemaps.RpgLike.ActorAnimator" />
                  </keys>
                  <values dataType="Array" type="System.Object[]" id="3342871690">
                    <item dataType="ObjectRef">3954234642</item>
                    <item dataType="ObjectRef">3907431145</item>
                    <item dataType="ObjectRef">361728938</item>
                    <item dataType="ObjectRef">479935388</item>
                    <item dataType="ObjectRef">2170487535</item>
                  </values>
                </body>
              </compMap>
              <compTransform dataType="ObjectRef">3954234642</compTransform>
              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                <header>
                  <data dataType="Array" type="System.Byte[]" id="967860754">6QaaXpvsP06njO2/8LNlCA==</data>
                </header>
                <body />
              </identifier>
              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
              <name dataType="String">MainChar</name>
              <parent dataType="Struct" type="Duality.GameObject" id="2989622809">
                <active dataType="Bool">true</active>
                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="634022185">
                  <_items dataType="Array" type="Duality.GameObject[]" id="3205646350" length="16">
                    <item dataType="ObjectRef">1593919710</item>
                    <item dataType="Struct" type="Duality.GameObject" id="823148500">
                      <active dataType="Bool">true</active>
                      <children />
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2773236480">
                        <_items dataType="Array" type="Duality.Component[]" id="1003330204">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3136659935">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3885925024">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">823148500</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4245766440">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2165311404" length="4">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="58569956">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="611422666">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3013493237">
                              <active dataType="Bool">true</active>
                              <angularDamp dataType="Float">0.3</angularDamp>
                              <angularVel dataType="Float">0</angularVel>
                              <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                              <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                              <colFilter />
                              <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                              <continous dataType="Bool">false</continous>
                              <explicitInertia dataType="Float">0</explicitInertia>
                              <explicitMass dataType="Float">0</explicitMass>
                              <fixedAngle dataType="Bool">false</fixedAngle>
                              <gameobj dataType="Struct" type="Duality.GameObject" id="4245684009">
                                <active dataType="Bool">true</active>
                                <children />
                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="140911484">
                                  <_items dataType="Array" type="Duality.Component[]" id="4099394628" length="4">
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
                                      <source dataType="Array" type="Duality.Plugins.Tilemaps.TilemapCollisionSource[]" id="2136833472">
                                        <item dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapCollisionSource">
                                          <Layers dataType="Enum" type="Duality.Plugins.Tilemaps.TileCollisionLayer" name="Layer0" value="1" />
                                          <SourceTilemap dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="668293575">
                                            <active dataType="Bool">true</active>
                                            <gameobj dataType="Struct" type="Duality.GameObject" id="2961408854">
                                              <active dataType="Bool">true</active>
                                              <children />
                                              <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4198584043">
                                                <_items dataType="Array" type="Duality.Component[]" id="2376419446" length="4">
                                                  <item dataType="ObjectRef">668293575</item>
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
                                                <_version dataType="Int">3</_version>
                                              </compList>
                                              <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1199659720" surrogate="true">
                                                <header />
                                                <body>
                                                  <keys dataType="Array" type="System.Object[]" id="2711123777">
                                                    <item dataType="Type" id="2178666926" value="Duality.Plugins.Tilemaps.Tilemap" />
                                                    <item dataType="ObjectRef">4219969168</item>
                                                    <item dataType="Type" id="1377381066" value="Duality.Plugins.Tilemaps.TilemapRenderer" />
                                                  </keys>
                                                  <values dataType="Array" type="System.Object[]" id="3362341344">
                                                    <item dataType="ObjectRef">668293575</item>
                                                    <item dataType="ObjectRef">1026756490</item>
                                                    <item dataType="ObjectRef">1659517006</item>
                                                  </values>
                                                </body>
                                              </compMap>
                                              <compTransform dataType="ObjectRef">1026756490</compTransform>
                                              <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                <header>
                                                  <data dataType="Array" type="System.Byte[]" id="529874323">KMXuZW3ZOEaxigxAvZjLRg==</data>
                                                </header>
                                                <body />
                                              </identifier>
                                              <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                                              <name dataType="String">BaseLayer</name>
                                              <parent dataType="Struct" type="Duality.GameObject" id="10461599">
                                                <active dataType="Bool">true</active>
                                                <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1068242970">
                                                  <_items dataType="Array" type="Duality.GameObject[]" id="3216525696">
                                                    <item dataType="ObjectRef">2961408854</item>
                                                    <item dataType="Struct" type="Duality.GameObject" id="3085774208">
                                                      <active dataType="Bool">true</active>
                                                      <children />
                                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="297071696">
                                                        <_items dataType="Array" type="Duality.Component[]" id="592294332" length="4">
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="792658929">
                                                            <active dataType="Bool">true</active>
                                                            <gameobj dataType="ObjectRef">3085774208</gameobj>
                                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="1434028621" custom="true">
                                                              <body>
                                                                <version dataType="Int">3</version>
                                                                <data dataType="Array" type="System.Byte[]" id="3685429798">H4sIAAAAAAAEAN2YyWpVQRCGb6ImoCYXzYQaNQ8gWRkXLhScsvQV7sKFSxdqIE7giHNUFNRFNGJUnIkzGF/hZukb+BhK/tr88FOnus85MeTC5aO6u7qrTldXDyONRmPk3/9IY/G3sMLxnaRpkn5k9vnt/6h3Fjd5ER7d+0qBD1mNeu4XXEF4TFgWJpXEWpKeEazwOHACOJk50HSmXjUYU9JYxyJOQzoDnKWWE8Dr4hECizkXgdE9XAQuAZcBc/McMAfs6Ejsepak55kGzpG0G0b0Q1oPeLlnCArjwHlykyd8Z9i/R0CbCttUF486Dr59MGIY0v5ik0bRpAlcQ+H1sBESM+QRY6Zc18lJZxcc6wbuovAe1bF6P0kP1QhtVch13ET2Is0tCZvNPcBe5d8wSU9VLweAgwqHqKWt6SepdvJitqxv6TBwtjH0YvQuJ8p5kQxQnYzPNrW0NS2XGmMQGFJ1nA49j7qBNUAXFXp6Zu7W4pYL1HIgrLAN2O408TZhTrE9QG+qubJuKlMvHp+G5E24SVIrVd0WwtVMHwyryqnHcTRT785SGRiHPBAcK9ZrOXXxeZDXMmmSl5CWxRR7N0wJ6fS7THPXpSoEprgOfKmv6/hmWiPk9vkGeEuFs6plHeDV+D5VPaDwCfhcibk/VeGvsPrX1PE8BTmbS52JSg7LPsg3iAlH3Ttf14Hcu+9hQL5B2GmU70CTwCnqRe5cgWvLPBAP01zIN4grqqXd3y9UMuzvVIU+QAatd8W4AdwEbgG3gXGaP+/+Xv9BcItTJ/fi1YDF4H1ID4BRtbj4/m6FndSLHP0l8CrTsVamXjXw0tMf4APwkfQCt1aDd64LhI2XOJtqVniqvPQ0SM9vyZ/OQiNwrtsAbFR10jKr4zDtIckenQLpSQZ7HPJ4YLD0uwnYDPAj118H+k2ZCBwAAA==</data>
                                                              </body>
                                                            </tileData>
                                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                                            </tileset>
                                                          </item>
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
                                                        <_version dataType="Int">3</_version>
                                                      </compList>
                                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="980148590" surrogate="true">
                                                        <header />
                                                        <body>
                                                          <keys dataType="Array" type="System.Object[]" id="3559000354">
                                                            <item dataType="ObjectRef">2178666926</item>
                                                            <item dataType="ObjectRef">4219969168</item>
                                                            <item dataType="ObjectRef">1377381066</item>
                                                          </keys>
                                                          <values dataType="Array" type="System.Object[]" id="1301088010">
                                                            <item dataType="ObjectRef">792658929</item>
                                                            <item dataType="ObjectRef">1151121844</item>
                                                            <item dataType="ObjectRef">1783882360</item>
                                                          </values>
                                                        </body>
                                                      </compMap>
                                                      <compTransform dataType="ObjectRef">1151121844</compTransform>
                                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                        <header>
                                                          <data dataType="Array" type="System.Byte[]" id="933935826">OGFE/6l2ukykXD788OcpAg==</data>
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
                                                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2089240964">
                                                        <_items dataType="Array" type="Duality.Component[]" id="4157978692" length="4">
                                                          <item dataType="Struct" type="Duality.Plugins.Tilemaps.Tilemap" id="3405903949">
                                                            <active dataType="Bool">true</active>
                                                            <gameobj dataType="ObjectRef">1404051932</gameobj>
                                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="1721446049" custom="true">
                                                              <body>
                                                                <version dataType="Int">3</version>
                                                                <data dataType="Array" type="System.Byte[]" id="1212744814">H4sIAAAAAAAEAO2YsU7DMBCGbaA7IIaCGPoAZSsTEh0QMxsMSLDwEGWDJ2HkFRBPEd6EFiJBIBKov5E46XRxnTg5S6lUfbITx/8f3zm2R8aY0e//xix/Lz169FCCV2AOLEjlQoPAmvgAPoGCVLrSG5BrkCthC9gGdgisXWLNuuL/0hfqSg0eJOwCe8A+QQa4oXoHcnIt0+CB4sj63mnIUH0DpXZ/J1D95N1gHQ02gIH361GBtqfDzcBroSha9jcETjkMI/THTh7xbWYC6J2H9fKBnTwuCVTkUc38k6wk7bZY1V9iNj3Ep+lvbBm5/jZVeJAwWdWfJptucfJcfScbfKmMUTP5xzZQIT7Un4QZcKtBfKP+XIM74F6DeA9MMc2wJxJpRiSFW3GyJxIqBNaEW3G6rSw9kYjY7UOTD3Of8gNO9YDEZ2u7wcfAduxuYlI9HPRwrKNYulJ7NOCxixx3Iz73FiiB3UX+hRRAg4jG2QVKZxFeQVktUALN29AjqGs0OI/gby4IPAamQrc0b6WvaEf4AV6zwTUIHAAA</data>
                                                              </body>
                                                            </tileData>
                                                            <tileset dataType="Struct" type="Duality.ContentRef`1[[Duality.Plugins.Tilemaps.Tileset]]">
                                                              <contentPath dataType="String">Data\TilemapsSample\Tilesets\MagicTown.Tileset.res</contentPath>
                                                            </tileset>
                                                          </item>
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
                                                        <_version dataType="Int">3</_version>
                                                      </compList>
                                                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="930335894" surrogate="true">
                                                        <header />
                                                        <body>
                                                          <keys dataType="Array" type="System.Object[]" id="1377859470">
                                                            <item dataType="ObjectRef">2178666926</item>
                                                            <item dataType="ObjectRef">4219969168</item>
                                                            <item dataType="ObjectRef">1377381066</item>
                                                          </keys>
                                                          <values dataType="Array" type="System.Object[]" id="2538357834">
                                                            <item dataType="ObjectRef">3405903949</item>
                                                            <item dataType="ObjectRef">3764366864</item>
                                                            <item dataType="ObjectRef">102160084</item>
                                                          </values>
                                                        </body>
                                                      </compMap>
                                                      <compTransform dataType="ObjectRef">3764366864</compTransform>
                                                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                        <header>
                                                          <data dataType="Array" type="System.Byte[]" id="1026513086">/2KPfz/Jj0O7HOXC4QkFrA==</data>
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
                                                  <_version dataType="Int">6</_version>
                                                </children>
                                                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1436675386">
                                                  <_items dataType="Array" type="Duality.Component[]" id="2721827680" length="0" />
                                                  <_size dataType="Int">0</_size>
                                                  <_version dataType="Int">0</_version>
                                                </compList>
                                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3976384154" surrogate="true">
                                                  <header />
                                                  <body>
                                                    <keys dataType="Array" type="System.Object[]" id="1447817984" length="0" />
                                                    <values dataType="Array" type="System.Object[]" id="2065027534" length="0" />
                                                  </body>
                                                </compMap>
                                                <compTransform />
                                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                                  <header>
                                                    <data dataType="Array" type="System.Byte[]" id="1690951068">HunZh0b630iicX0zyp3aXg==</data>
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
                                            <tileData dataType="Struct" type="Duality.Plugins.Tilemaps.TilemapData" id="3619145399" custom="true">
                                              <body>
                                                <version dataType="Int">3</version>
                                                <data dataType="Array" type="System.Byte[]" id="3510518670">H4sIAAAAAAAEAKVYy2oUURDtDi5m1IXPRXyEzjxijIq604WgmwEVxEFc6A/4Gf7A/EI+RJTYiRpjwLgRXboR8xtKzmnp0xzr3okFw6EfdW+dW4+umqooiurPb7M4kC3APuBEeQAnAVZO4dlpwBlAjWebAXCj725Nqr8KIFK3Jum2dk3KUNiew9V5wIV5+f1wBh5WfVD+26RoMV3Tip6EGliLesZBHlY9kk9p2HV6aosFa2A+5KurA8YSZ6M0jAH7skqd3nYHsJt+8z/VowBbTsMAoPy4u3r6nWy7ClhLG2jBqr932+6KgSPx3+U0cIcl0RtyMQBdXKettgba44lcdaM0u6sDtBzOLRVAXbwT2EliPOubYuBwXvVbpdld+amQLSttD9CXI9C8ZfSoi2ln7QzkyZPYbcAEUM2rfrc0uy85j9nc1O/7KNvTtNPmJk+exO4BHgFW51WPXEWpAcpvmGY0Dl6hnZqb16gHILE7hQFVX5FVVN26SjOuBmjtsW9aRhqtqlABrgKuAJgkZRoq0WOOXXJ2DgI/1LTT6VEiRpFobmqSWEZ9ubLq1wHWcZLLM96sAXoutrBQjuLmMcDxecuvGFE8KFs3LcyKtOiaAr0aV1Gzru2dbc/PCk0bL4ERGdDQ1KVxsfC8fUqLcmRNlcpoJFiCbHt+0TEatHdaCIxvTAIMI5qAIwI8s0Vjwt8qlc8voyYXbZPUCGt1TxQ6J5EO4Q53LsbTrRyViF80jGQw6pubs+yT6KhrAtmAZpXK6ADtBCbEDptcORCNiNYB6gcdhSJ1bSQssSCkwlSzetaWTXkWzYZ8tgzgxzRqjdizj43xETTZn110GlBbNLLsbKijCZ/pV5RFhy2VtkbsaeU7nVEAmxqiktZb1C6vFip2NtwS48du2wpAp7LByhg/Aq80/DYEMvTUlhqQUQepwMZFe7Cos7Ktkdg5kSLXBOYHwGvANuCjUVc3ztQWmzk2j7Shs5I/EWU4jvy2BchvX1ZxZaZZhdHKqGPQrrhnUZxF/acVV0Y7/YTyewOw/KJTqgDMlYnsq8+Un04F9o/rKdYC7AnM7JFrr2j9p24M+HU+18wVzsU6Hts84hygETky/F5M21eE9chxlM+APQdfAOJGW007nY5rhhpP3y/boHPAWCCbX0PMDr1fcfUNMHE3hV/+CKWv9B4DnwIeAqI5QPgNAS8Bb6dCLJKfgF8AhpTeXGrbGY0RYQdh+anYMKVXxI3r+krG7JsffGq86Gk/0dmG/J7Q3CDj7F8Rlh8lmn11MTGwYevSqRMaGw6sPCvb8Bv2FLL8CBwAAA==</data>
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
                                  <_version dataType="Int">3</_version>
                                </compList>
                                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3613511830" surrogate="true">
                                  <header />
                                  <body>
                                    <keys dataType="Array" type="System.Object[]" id="1166927830">
                                      <item dataType="ObjectRef">4219969168</item>
                                      <item dataType="ObjectRef">794497900</item>
                                      <item dataType="Type" id="3896459552" value="Duality.Plugins.Tilemaps.TilemapCollider" />
                                    </keys>
                                    <values dataType="Array" type="System.Object[]" id="3164832730">
                                      <item dataType="ObjectRef">2311031645</item>
                                      <item dataType="ObjectRef">3013493237</item>
                                      <item dataType="ObjectRef">2359977928</item>
                                    </values>
                                  </body>
                                </compMap>
                                <compTransform dataType="ObjectRef">2311031645</compTransform>
                                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                                  <header>
                                    <data dataType="Array" type="System.Byte[]" id="136676598">VLecLJN/pUiYA65m6S9w2A==</data>
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
                              <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1214195307">
                                <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1426464374" length="64" />
                                <_size dataType="Int">0</_size>
                                <_version dataType="Int">3932</_version>
                              </shapes>
                            </baseObject>
                            <gameobj dataType="ObjectRef">823148500</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1378181582" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="1970803666">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="Type" id="260043600" value="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" />
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="878902474">
                            <item dataType="ObjectRef">3183463432</item>
                            <item dataType="ObjectRef">3136659935</item>
                            <item dataType="ObjectRef">3885925024</item>
                            <item dataType="ObjectRef">611422666</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3183463432</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="512664162">e3TD9tOei0uqvFlc2UiUJA==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1046276079">
                        <_items dataType="Array" type="Duality.Component[]" id="3850265838">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="2757188714">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3506453803">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">443677279</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1850120363">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3666827510">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2670208224">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="231951445">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">443677279</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1256536480" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="2715291205">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="4245181480">
                            <item dataType="ObjectRef">2803992211</item>
                            <item dataType="ObjectRef">2757188714</item>
                            <item dataType="ObjectRef">3506453803</item>
                            <item dataType="ObjectRef">231951445</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2803992211</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3377713103">l7Qnr1YuLE6cy5v//zOa5A==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="561206258">
                        <_items dataType="Array" type="Duality.Component[]" id="52962256">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="2501373737">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3250638826">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">187862302</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="373111490">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2979498512">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1595921212">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="4271103764">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">187862302</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1915454282" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="1971035112">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="976341790">
                            <item dataType="ObjectRef">2548177234</item>
                            <item dataType="ObjectRef">2501373737</item>
                            <item dataType="ObjectRef">3250638826</item>
                            <item dataType="ObjectRef">4271103764</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2548177234</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="666039124">ssaSvKC0V0uDKAPSRpjy+w==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2566564481">
                        <_items dataType="Array" type="Duality.Component[]" id="4071998766">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="2183008572">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2932273661">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">4164464433</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3831715325">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3357331750">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="949945600">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="3952738599">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">4164464433</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3989698400" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="2035217739">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2231052104">
                            <item dataType="ObjectRef">2229812069</item>
                            <item dataType="ObjectRef">2183008572</item>
                            <item dataType="ObjectRef">2932273661</item>
                            <item dataType="ObjectRef">3952738599</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">2229812069</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="1220166145">T+HZGeHns06dRQZDpFMazQ==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="819565845">
                        <_items dataType="Array" type="Duality.Component[]" id="1629491318">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3188618400">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3937883489">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">875106965</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1393785473">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2496570670">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="20272976">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="663381131">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">875106965</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3473547976" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3856985279">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="2708328928">
                            <item dataType="ObjectRef">3235421897</item>
                            <item dataType="ObjectRef">3188618400</item>
                            <item dataType="ObjectRef">3937883489</item>
                            <item dataType="ObjectRef">663381131</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3235421897</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="2633043565">n1YFWCx+W0SpLxLK1a5tUQ==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1954074358">
                        <_items dataType="Array" type="Duality.Component[]" id="3188465888">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="1114677725">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1863942814">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">3096133586</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4086908990">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3864905232">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1491051324">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="2884407752">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">3096133586</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3039300122" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="4138125764">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="3780342166">
                            <item dataType="ObjectRef">1161481222</item>
                            <item dataType="ObjectRef">1114677725</item>
                            <item dataType="ObjectRef">1863942814</item>
                            <item dataType="ObjectRef">2884407752</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">1161481222</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="4055828608">KHxchtd4OEO4ckavklp8wg==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1902438242">
                        <_items dataType="Array" type="Duality.Component[]" id="3622665104">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="176699577">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="925964666">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">2158155438</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1075188274">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="729356752">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2100725436">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1946429604">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">2158155438</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3941039498" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="4188382712">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="959985118">
                            <item dataType="ObjectRef">223503074</item>
                            <item dataType="ObjectRef">176699577</item>
                            <item dataType="ObjectRef">925964666</item>
                            <item dataType="ObjectRef">1946429604</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">223503074</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="3811464868">tpeu3QwrwEGRRHYg0s2p2A==</data>
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
                      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="947747006">
                        <_items dataType="Array" type="Duality.Component[]" id="2646348816">
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
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.ActorRenderer" id="3862389221">
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
                          <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="316687014">
                            <active dataType="Bool">true</active>
                            <angularDamp dataType="Float">0.3</angularDamp>
                            <angularVel dataType="Float">0</angularVel>
                            <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                            <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                            <colFilter />
                            <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                            <continous dataType="Bool">false</continous>
                            <explicitInertia dataType="Float">0</explicitInertia>
                            <explicitMass dataType="Float">60</explicitMass>
                            <fixedAngle dataType="Bool">true</fixedAngle>
                            <gameobj dataType="ObjectRef">1548877786</gameobj>
                            <ignoreGravity dataType="Bool">false</ignoreGravity>
                            <joints />
                            <linearDamp dataType="Float">0.3</linearDamp>
                            <linearVel dataType="Struct" type="Duality.Vector2" />
                            <revolutions dataType="Float">0</revolutions>
                            <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1232123334">
                              <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="715795712">
                                <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3255167644">
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
                                </item>
                              </_items>
                              <_size dataType="Int">1</_size>
                              <_version dataType="Int">1</_version>
                            </shapes>
                          </item>
                          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.MoveableObjectPhysics" id="1337151952">
                            <active dataType="Bool">true</active>
                            <baseFriction dataType="Float">10</baseFriction>
                            <baseObject dataType="ObjectRef">3013493237</baseObject>
                            <gameobj dataType="ObjectRef">1548877786</gameobj>
                            <initialFriction dataType="Float">25</initialFriction>
                          </item>
                        </_items>
                        <_size dataType="Int">4</_size>
                        <_version dataType="Int">4</_version>
                      </compList>
                      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4165928970" surrogate="true">
                        <header />
                        <body>
                          <keys dataType="Array" type="System.Object[]" id="3676899228">
                            <item dataType="ObjectRef">4219969168</item>
                            <item dataType="ObjectRef">3361562350</item>
                            <item dataType="ObjectRef">794497900</item>
                            <item dataType="ObjectRef">260043600</item>
                          </keys>
                          <values dataType="Array" type="System.Object[]" id="3364238358">
                            <item dataType="ObjectRef">3909192718</item>
                            <item dataType="ObjectRef">3862389221</item>
                            <item dataType="ObjectRef">316687014</item>
                            <item dataType="ObjectRef">1337151952</item>
                          </values>
                        </body>
                      </compMap>
                      <compTransform dataType="ObjectRef">3909192718</compTransform>
                      <identifier dataType="Struct" type="System.Guid" surrogate="true">
                        <header>
                          <data dataType="Array" type="System.Byte[]" id="183659016">Arkw8mFMK0aJ4uJMFIXC8w==</data>
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
                  <_version dataType="Int">9</_version>
                </children>
                <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1577465280">
                  <_items dataType="Array" type="Duality.Component[]" id="1686662307" length="0" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">0</_version>
                </compList>
                <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="175275787" surrogate="true">
                  <header />
                  <body>
                    <keys dataType="Array" type="System.Object[]" id="1896042676" length="0" />
                    <values dataType="Array" type="System.Object[]" id="1707130870" length="0" />
                  </body>
                </compMap>
                <compTransform />
                <identifier dataType="Struct" type="System.Guid" surrogate="true">
                  <header>
                    <data dataType="Array" type="System.Byte[]" id="1148546832">3UfIa/qU9UGLaU0jGSIzMQ==</data>
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
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1778928878" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1373440354">
            <item dataType="ObjectRef">4219969168</item>
            <item dataType="Type" id="1128403856" value="Duality.Components.Camera" />
            <item dataType="Type" id="3457661678" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="3352197740" value="Duality.Samples.Tilemaps.RpgLike.CameraController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="105870730">
            <item dataType="ObjectRef">1826376698</item>
            <item dataType="ObjectRef">3337573</item>
            <item dataType="ObjectRef">119543137</item>
            <item dataType="ObjectRef">1844544486</item>
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
          <item dataType="Struct" type="Duality.Samples.Tilemaps.RpgLike.Player" id="1378947066">
            <active dataType="Bool">true</active>
            <character dataType="ObjectRef">479935388</character>
            <gameobj dataType="ObjectRef">3851204924</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1943215818" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4114983048">
            <item dataType="Type" id="1898588012" value="Duality.Samples.Tilemaps.RpgLike.Player" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="527819742">
            <item dataType="ObjectRef">1378947066</item>
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
