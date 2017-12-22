<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="789494455">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1051363573">
        <_items dataType="Array" type="Duality.Component[]" id="1701256822" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3149809387">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">789494455</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="1326770262">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">203</B>
              <G dataType="Byte">149</G>
              <R dataType="Byte">90</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">789494455</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
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
          <item dataType="Struct" type="Duality.Components.SoundListener" id="1442975826">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">789494455</gameobj>
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3235226824" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3980437855">
            <item dataType="Type" id="2421658734" value="Duality.Components.Transform" />
            <item dataType="Type" id="4096309194" value="Duality.Components.Camera" />
            <item dataType="Type" id="3112146014" value="Duality.Components.SoundListener" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3871832864">
            <item dataType="ObjectRef">3149809387</item>
            <item dataType="ObjectRef">1326770262</item>
            <item dataType="ObjectRef">1442975826</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3149809387</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2969479885">4P+437YFPkWJBfWaDOuoaQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Camera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3975364469">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1169807623">
        <_items dataType="Array" type="Duality.GameObject[]" id="1329521742" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="3771095618">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2335940742">
              <_items dataType="Array" type="Duality.Component[]" id="1035824000" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1836443254">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3771095618</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">272</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">272</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2538904846">
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
                  <gameobj dataType="ObjectRef">3771095618</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3790448014">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1950009552">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="643451580">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3350684228">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="3576965700" length="4">
                            <item dataType="Array" type="Duality.Vector2[]" id="986892868">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-319.999969</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">319.999969</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">319.999969</X>
                                <Y dataType="Float">16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-319.999969</X>
                                <Y dataType="Float">16</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2538904846</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="3229928086">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-320</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">320</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">320</X>
                            <Y dataType="Float">16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-320</X>
                            <Y dataType="Float">16</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="74821512">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidBlack</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">3771095618</gameobj>
                  <offset dataType="Float">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3011869498" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1971837684">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="Type" id="2627584164" value="Duality.Components.Physics.RigidBody" />
                  <item dataType="Type" id="557071126" value="Duality.Components.Renderers.RigidBodyRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="354705142">
                  <item dataType="ObjectRef">1836443254</item>
                  <item dataType="ObjectRef">2538904846</item>
                  <item dataType="ObjectRef">74821512</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1836443254</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1205964752">0RjXUxH2m0KqcEVxGQQHCA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Wall</name>
            <parent dataType="ObjectRef">3975364469</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="826064256">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="434544652">
              <_items dataType="Array" type="Duality.Component[]" id="997178532" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3186379188">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0.5235988</angle>
                  <angleAbs dataType="Float">0.5235988</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">826064256</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-464</X>
                    <Y dataType="Float">192</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-464</X>
                    <Y dataType="Float">192</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3888840780">
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
                  <gameobj dataType="ObjectRef">826064256</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3378360444">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1031755844">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2091701828">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3986364996">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="606612036" length="8">
                            <item dataType="Array" type="Duality.Vector2[]" id="215665220">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-176</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">176</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">176</X>
                                <Y dataType="Float">16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-176</X>
                                <Y dataType="Float">16</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3888840780</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="1665190550">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-176</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">176</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">176</X>
                            <Y dataType="Float">16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-176</X>
                            <Y dataType="Float">16</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1424757446">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidBlack</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">826064256</gameobj>
                  <offset dataType="Float">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2500438774" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2647849094">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="912456506">
                  <item dataType="ObjectRef">3186379188</item>
                  <item dataType="ObjectRef">3888840780</item>
                  <item dataType="ObjectRef">1424757446</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3186379188</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3027796998">4vNqSlasRU6rw9cF8HNoJw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Wall</name>
            <parent dataType="ObjectRef">3975364469</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3179955376">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2208924636">
              <_items dataType="Array" type="Duality.Component[]" id="3197110980" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1245303012">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">5.759587</angle>
                  <angleAbs dataType="Float">5.759587</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3179955376</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">464</X>
                    <Y dataType="Float">192</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">464</X>
                    <Y dataType="Float">192</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1947764604">
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
                  <gameobj dataType="ObjectRef">3179955376</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="265370092">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2920602212">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3868216772">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3623702852">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="3042234948" length="8">
                            <item dataType="Array" type="Duality.Vector2[]" id="1832962628">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-176</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">176</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">176</X>
                                <Y dataType="Float">16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-176</X>
                                <Y dataType="Float">16</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">1947764604</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="2149462678">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-176</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">176</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">176</X>
                            <Y dataType="Float">16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-176</X>
                            <Y dataType="Float">16</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3778648566">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidBlack</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">3179955376</gameobj>
                  <offset dataType="Float">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
              </_items>
              <_size dataType="Int">3</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3225024790" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3106125046">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2548780570">
                  <item dataType="ObjectRef">1245303012</item>
                  <item dataType="ObjectRef">1947764604</item>
                  <item dataType="ObjectRef">3778648566</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1245303012</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="344265238">0AmpBSOprU2K4xE3FNJLzQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Wall</name>
            <parent dataType="ObjectRef">3975364469</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4076576384">
        <_items dataType="Array" type="Duality.Component[]" id="3589598893" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3668253445" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2376502804" length="0" />
          <values dataType="Array" type="System.Object[]" id="1344669494" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3303495344">rXG8G092806WXYH/nsUtKw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Room</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2091764498">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2774523900">
        <_items dataType="Array" type="Duality.GameObject[]" id="520565572" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="1295111380">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="724218068">
              <_items dataType="Array" type="Duality.GameObject[]" id="3261577956">
                <item dataType="Struct" type="Duality.GameObject" id="4180770938">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1107506714">
                    <_items dataType="Array" type="Duality.Component[]" id="3935576448" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2246118574">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">4180770938</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform />
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">64</X>
                          <Y dataType="Float">208</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">64</X>
                          <Y dataType="Float">208</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2948580166">
                        <active dataType="Bool">true</active>
                        <allowParent dataType="Bool">false</allowParent>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">4180770938</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints />
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="95155462">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3989956992">
                            <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="812809628">
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2948580166</parent>
                              <position dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-16</X>
                                <Y dataType="Float">-16</Y>
                              </position>
                              <radius dataType="Float">32</radius>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                            </item>
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="309205014">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="733752118">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="1003406176" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="143249628">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-4.000001</X>
                                      <Y dataType="Float">-8.000002</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">43.9999924</X>
                                      <Y dataType="Float">-7.99998856</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">44.00001</X>
                                      <Y dataType="Float">39.99998</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-20.0000038</X>
                                      <Y dataType="Float">40.0000076</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2948580166</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="388933274">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-4.000001</X>
                                  <Y dataType="Float">-8.000002</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">43.9999962</X>
                                  <Y dataType="Float">-7.99998856</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">44.00001</X>
                                  <Y dataType="Float">39.9999847</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-20.0000038</X>
                                  <Y dataType="Float">40.0000076</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="484496832">
                        <active dataType="Bool">true</active>
                        <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                        </areaMaterial>
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">191</B>
                          <G dataType="Byte">191</G>
                          <R dataType="Byte">191</R>
                        </colorTint>
                        <customAreaMaterial />
                        <customOutlineMaterial />
                        <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                        <gameobj dataType="ObjectRef">4180770938</gameobj>
                        <offset dataType="Float">0</offset>
                        <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                        </outlineMaterial>
                        <outlineWidth dataType="Float">3</outlineWidth>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        <wrapTexture dataType="Bool">true</wrapTexture>
                      </item>
                    </_items>
                    <_size dataType="Int">3</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1294804282" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2077044064">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2529674382">
                        <item dataType="ObjectRef">2246118574</item>
                        <item dataType="ObjectRef">2948580166</item>
                        <item dataType="ObjectRef">484496832</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2246118574</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="417271676">X8L5BMiYekKz0GVHq77vXw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">MultiShape</name>
                  <parent dataType="ObjectRef">1295111380</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="3609771450">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1400759386">
                    <_items dataType="Array" type="Duality.Component[]" id="1750840320" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1675119086">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3609771450</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform />
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-64</X>
                          <Y dataType="Float">208</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-64</X>
                          <Y dataType="Float">208</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2377580678">
                        <active dataType="Bool">true</active>
                        <allowParent dataType="Bool">false</allowParent>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">3609771450</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints />
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2032584902">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="106137856">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1966969500">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="874183620">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3204918596" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="650973764">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-6.37249041</X>
                                      <Y dataType="Float">-38.00241</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32.5329361</X>
                                      <Y dataType="Float">-14.8863316</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">44.37501</X>
                                      <Y dataType="Float">31.62987</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-21.5940018</X>
                                      <Y dataType="Float">31.9138145</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-48.9414558</X>
                                      <Y dataType="Float">-10.6550541</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2377580678</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="1031083414">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-6.372491</X>
                                  <Y dataType="Float">-38.00241</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32.5329361</X>
                                  <Y dataType="Float">-14.8863325</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">44.37501</X>
                                  <Y dataType="Float">31.6298714</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-21.5940018</X>
                                  <Y dataType="Float">31.9138145</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-48.94146</X>
                                  <Y dataType="Float">-10.655055</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="4208464640">
                        <active dataType="Bool">true</active>
                        <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                        </areaMaterial>
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">191</B>
                          <G dataType="Byte">191</G>
                          <R dataType="Byte">191</R>
                        </colorTint>
                        <customAreaMaterial />
                        <customOutlineMaterial />
                        <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                        <gameobj dataType="ObjectRef">3609771450</gameobj>
                        <offset dataType="Float">0</offset>
                        <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                        </outlineMaterial>
                        <outlineWidth dataType="Float">3</outlineWidth>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        <wrapTexture dataType="Bool">true</wrapTexture>
                      </item>
                    </_items>
                    <_size dataType="Int">3</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1022267834" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="677478304">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3652189838">
                        <item dataType="ObjectRef">1675119086</item>
                        <item dataType="ObjectRef">2377580678</item>
                        <item dataType="ObjectRef">4208464640</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1675119086</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="882290620">e/ysv4RV2UGBzz085kt0GA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">ConvexPolygon</name>
                  <parent dataType="ObjectRef">1295111380</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="475944367">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1174914579">
                    <_items dataType="Array" type="Duality.Component[]" id="1623139046" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2836259299">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">475944367</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform />
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-64</X>
                          <Y dataType="Float">16</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-64</X>
                          <Y dataType="Float">16</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3538720891">
                        <active dataType="Bool">true</active>
                        <allowParent dataType="Bool">false</allowParent>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">475944367</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints />
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3005727435">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2795375606">
                            <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3106903776">
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">3538720891</parent>
                              <position dataType="Struct" type="Duality.Vector2" />
                              <radius dataType="Float">32</radius>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1074637557">
                        <active dataType="Bool">true</active>
                        <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                        </areaMaterial>
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">191</B>
                          <G dataType="Byte">191</G>
                          <R dataType="Byte">191</R>
                        </colorTint>
                        <customAreaMaterial />
                        <customOutlineMaterial />
                        <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                        <gameobj dataType="ObjectRef">475944367</gameobj>
                        <offset dataType="Float">0</offset>
                        <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                        </outlineMaterial>
                        <outlineWidth dataType="Float">3</outlineWidth>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        <wrapTexture dataType="Bool">true</wrapTexture>
                      </item>
                    </_items>
                    <_size dataType="Int">3</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1016878328" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3214620281">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="578147712">
                        <item dataType="ObjectRef">2836259299</item>
                        <item dataType="ObjectRef">3538720891</item>
                        <item dataType="ObjectRef">1074637557</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2836259299</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="4121404283">KRKiXPyWY0ezvta0HAwf5Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Circle</name>
                  <parent dataType="ObjectRef">1295111380</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1882913398">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="828030062">
                    <_items dataType="Array" type="Duality.Component[]" id="4000534096" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4243228330">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1882913398</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform />
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">64</X>
                          <Y dataType="Float">16</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">64</X>
                          <Y dataType="Float">16</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="650722626">
                        <active dataType="Bool">true</active>
                        <allowParent dataType="Bool">false</allowParent>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">1882913398</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints />
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2698635226">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4147923200">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3069285020">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="425393092">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="4253494596" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="2966229572">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-48.0000076</X>
                                      <Y dataType="Float">31.99999</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-48.0000153</X>
                                      <Y dataType="Float">-32.0000153</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">1.88056E-06</X>
                                      <Y dataType="Float">7.345666E-07</Y>
                                    </item>
                                  </item>
                                  <item dataType="Array" type="Duality.Vector2[]" id="1287547542">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">1.88056E-06</X>
                                      <Y dataType="Float">7.345666E-07</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-48.0000153</X>
                                      <Y dataType="Float">-32.0000153</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">48</X>
                                      <Y dataType="Float">-31.99998</Y>
                                    </item>
                                  </item>
                                  <item dataType="Array" type="Duality.Vector2[]" id="352897024">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">1.88056E-06</X>
                                      <Y dataType="Float">7.345666E-07</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">48</X>
                                      <Y dataType="Float">-31.99998</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">48.00001</X>
                                      <Y dataType="Float">32.0000038</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">3</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">650722626</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="368383382">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-48.0000153</X>
                                  <Y dataType="Float">-32.0000153</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">48.0000038</X>
                                  <Y dataType="Float">-31.99998</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">48.00001</X>
                                  <Y dataType="Float">32.0000038</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">1.88056015E-06</X>
                                  <Y dataType="Float">7.345666E-07</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-48.0000076</X>
                                  <Y dataType="Float">31.9999924</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2481606588">
                        <active dataType="Bool">true</active>
                        <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                        </areaMaterial>
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">191</B>
                          <G dataType="Byte">191</G>
                          <R dataType="Byte">191</R>
                        </colorTint>
                        <customAreaMaterial />
                        <customOutlineMaterial />
                        <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                        <gameobj dataType="ObjectRef">1882913398</gameobj>
                        <offset dataType="Float">0</offset>
                        <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                          <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                        </outlineMaterial>
                        <outlineWidth dataType="Float">3</outlineWidth>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        <wrapTexture dataType="Bool">true</wrapTexture>
                      </item>
                    </_items>
                    <_size dataType="Int">3</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1323747274" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2757724652">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1573742390">
                        <item dataType="ObjectRef">4243228330</item>
                        <item dataType="ObjectRef">650722626</item>
                        <item dataType="ObjectRef">2481606588</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4243228330</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3294921912">/LBUV25SiUqUTHWZfGexAw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">NonConvexPolygon</name>
                  <parent dataType="ObjectRef">1295111380</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">4</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1591334326">
              <_items dataType="Array" type="Duality.Component[]" id="4287605886" length="0" />
              <_size dataType="Int">0</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="523077104" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1847139016" length="0" />
                <values dataType="Array" type="System.Object[]" id="1963525854" length="0" />
              </body>
            </compMap>
            <compTransform />
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2097493812">zZQmrxdMXkCVHK4X/iyzRw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RegularObjects</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4212726542">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="640912886">
              <_items dataType="Array" type="Duality.Component[]" id="694755040">
                <item dataType="Struct" type="Duality.Components.Transform" id="2278074178">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4212726542</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-112</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-112</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2980535770">
                  <active dataType="Bool">true</active>
                  <allowParent dataType="Bool">false</allowParent>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colFilter dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="4232103922" multi="true">
                    <method dataType="MemberInfo" id="1116310480" value="M:Duality.Samples.Physics.OneWayCollision:CollisionFilter(Duality.Components.Physics.CollisionFilterData)" />
                    <target dataType="Struct" type="Duality.Samples.Physics.OneWayCollision" id="2198211861">
                      <active dataType="Bool">true</active>
                      <gameobj dataType="ObjectRef">4212726542</gameobj>
                    </target>
                    <invocationList dataType="Array" type="System.Delegate[]" id="2949182062">
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="1101241506" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2198211861</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="3407900432">
                          <item dataType="ObjectRef">1101241506</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="2421691146" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2198211861</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="4097017144">
                          <item dataType="ObjectRef">2421691146</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="1460659538" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2198211861</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="1643719456">
                          <item dataType="ObjectRef">1460659538</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="1869279706" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2198211861</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="532394728">
                          <item dataType="ObjectRef">1869279706</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="405905346" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2198211861</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="3584587440">
                          <item dataType="ObjectRef">405905346</item>
                        </invocationList>
                      </item>
                    </invocationList>
                  </colFilter>
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <explicitInertia dataType="Float">0</explicitInertia>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">4212726542</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="311528266">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2119360488">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2837226540">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="4188345572">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="4190230468" length="4">
                            <item dataType="Array" type="Duality.Vector2[]" id="591596868">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-159.999985</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">159.999985</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">159.999985</X>
                                <Y dataType="Float">16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-159.999985</X>
                                <Y dataType="Float">16</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2980535770</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="3265715734">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-160</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">160</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">160</X>
                            <Y dataType="Float">16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-160</X>
                            <Y dataType="Float">16</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="516452436">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">191</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">4212726542</gameobj>
                  <offset dataType="Float">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
                <item dataType="ObjectRef">2198211861</item>
              </_items>
              <_size dataType="Int">4</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="685923354" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4773060">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                  <item dataType="Type" id="412899140" value="Duality.Samples.Physics.OneWayCollision" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2835188630">
                  <item dataType="ObjectRef">2278074178</item>
                  <item dataType="ObjectRef">2980535770</item>
                  <item dataType="ObjectRef">516452436</item>
                  <item dataType="ObjectRef">2198211861</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2278074178</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2978289024">+VRPF1aAa0+iPGyw9TlgnA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">OneWayPlatform</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4272538120">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3395279616">
              <_items dataType="Array" type="Duality.Component[]" id="187128476">
                <item dataType="Struct" type="Duality.Components.Transform" id="2337885756">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4272538120</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">80</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">80</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3040347348">
                  <active dataType="Bool">true</active>
                  <allowParent dataType="Bool">false</allowParent>
                  <angularDamp dataType="Float">0.3</angularDamp>
                  <angularVel dataType="Float">0</angularVel>
                  <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Static" value="0" />
                  <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                  <colFilter dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="3135472724" multi="true">
                    <method dataType="ObjectRef">1116310480</method>
                    <target dataType="Struct" type="Duality.Samples.Physics.OneWayCollision" id="2258023439">
                      <active dataType="Bool">true</active>
                      <gameobj dataType="ObjectRef">4272538120</gameobj>
                    </target>
                    <invocationList dataType="Array" type="System.Delegate[]" id="3278449892">
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="2748663748" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2258023439</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="39087428">
                          <item dataType="ObjectRef">2748663748</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="2016368022" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2258023439</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="1145446478">
                          <item dataType="ObjectRef">2016368022</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="3408472704" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2258023439</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="3380346440">
                          <item dataType="ObjectRef">3408472704</item>
                        </invocationList>
                      </item>
                      <item dataType="Delegate" type="Duality.Components.Physics.CollisionFilter" id="3469047842" multi="true">
                        <method dataType="ObjectRef">1116310480</method>
                        <target dataType="ObjectRef">2258023439</target>
                        <invocationList dataType="Array" type="System.Delegate[]" id="273734290">
                          <item dataType="ObjectRef">3469047842</item>
                        </invocationList>
                      </item>
                    </invocationList>
                  </colFilter>
                  <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                  <explicitInertia dataType="Float">0</explicitInertia>
                  <explicitMass dataType="Float">0</explicitMass>
                  <fixedAngle dataType="Bool">false</fixedAngle>
                  <gameobj dataType="ObjectRef">4272538120</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3358708662">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3172232190">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="794379664">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="2379090236">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="1508740932" length="4">
                            <item dataType="Array" type="Duality.Vector2[]" id="1025482308">
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-159.999985</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">159.999985</X>
                                <Y dataType="Float">-16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">159.999985</X>
                                <Y dataType="Float">16</Y>
                              </item>
                              <item dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">-159.999985</X>
                                <Y dataType="Float">16</Y>
                              </item>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3040347348</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="3924337558">
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-160</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">160</X>
                            <Y dataType="Float">-16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">160</X>
                            <Y dataType="Float">16</Y>
                          </item>
                          <item dataType="Struct" type="Duality.Vector2">
                            <X dataType="Float">-160</X>
                            <Y dataType="Float">16</Y>
                          </item>
                        </vertices>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                  </shapes>
                  <useCCD dataType="Bool">false</useCCD>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="576264014">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">191</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">4272538120</gameobj>
                  <offset dataType="Float">0</offset>
                  <outlineMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                  </outlineMaterial>
                  <outlineWidth dataType="Float">3</outlineWidth>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <wrapTexture dataType="Bool">true</wrapTexture>
                </item>
                <item dataType="ObjectRef">2258023439</item>
              </_items>
              <_size dataType="Int">4</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2957910478" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2304046034">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                  <item dataType="ObjectRef">412899140</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2500836554">
                  <item dataType="ObjectRef">2337885756</item>
                  <item dataType="ObjectRef">3040347348</item>
                  <item dataType="ObjectRef">576264014</item>
                  <item dataType="ObjectRef">2258023439</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2337885756</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1181794914">AId79tLRZ0WPi9lGA+cpbw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">OneWayPlatform</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">3</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="64059286">
        <_items dataType="ObjectRef">3589598893</_items>
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2630997672" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3546066008" length="0" />
          <values dataType="Array" type="System.Object[]" id="1210708894" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2281770756">T4kmnIvOvEK63C7yAxYu4Q==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Objects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1573993153">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1809186627">
        <_items dataType="Array" type="Duality.Component[]" id="3768589862" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleController" id="3509149724">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1573993153</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3248960696" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2332854825">
            <item dataType="Type" id="3252412942" value="Duality.Samples.Physics.PhysicsSampleController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1762589632">
            <item dataType="ObjectRef">3509149724</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2394299915">I3eAE1F/fE21FDEvpvD0ng==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleController</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="889055081">
        <changes />
        <obj dataType="ObjectRef">1573993153</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleController.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="4018801489">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3871265971">
        <_items dataType="Array" type="Duality.Component[]" id="4253644838" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleInfo" id="3497936362">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">4018801489</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="485931704" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1195890393">
            <item dataType="Type" id="2540647886" value="Duality.Samples.Physics.PhysicsSampleInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2912101888">
            <item dataType="ObjectRef">3497936362</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="896158619">hEq4EzJJnUGRorX0WGmySA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleInfo</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="293065945">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="3637267092">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="915200868" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3463740616">
                <_items dataType="Array" type="System.Int32[]" id="3178459756"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2540647886</componentType>
              <prop dataType="MemberInfo" id="1923193566" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleName" />
              <val dataType="String">Collision Filters</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="179335476">
                <_items dataType="Array" type="System.Int32[]" id="2898900296"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">2540647886</componentType>
              <prop dataType="MemberInfo" id="2510065442" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleDesc" />
              <val dataType="String">RigidBodies allow users to provide /cFF8888FFCollision Filters/cFFFFFFFF in code for more specialized or complex filtering behavior.</val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
        </changes>
        <obj dataType="ObjectRef">4018801489</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleInfo.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="4243299910">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3046317968">
        <_items dataType="Array" type="Duality.GameObject[]" id="122483004" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="2197599797">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1171848473">
              <_items dataType="Array" type="Duality.GameObject[]" id="699560270" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3190397052">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="326897576">
                    <_items dataType="Array" type="Duality.Component[]" id="2592731820" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1255744688">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3190397052</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="262947433">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">2197599797</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-192</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-192</Y>
                            <Z dataType="Float">-1</Z>
                          </posAbs>
                          <scale dataType="Float">0.75</scale>
                          <scaleAbs dataType="Float">0.75</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-128</Y>
                          <Z dataType="Float">4</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-288</Y>
                          <Z dataType="Float">2</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="638058578">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3190397052</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3792479074">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1524566928">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">The platforms in this sample use a collision filter to implement one-way collision.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3044078494" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2254638186">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="Type" id="3690285088" value="Duality.Components.Renderers.TextRenderer" />
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="4019209946">
                        <item dataType="ObjectRef">1255744688</item>
                        <item dataType="ObjectRef">638058578</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1255744688</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="413794250">BdAspQUGJUiI5h1fsgNRXA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">2197599797</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3507502976">
              <_items dataType="Array" type="Duality.Component[]" id="562368563" length="4">
                <item dataType="ObjectRef">262947433</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3940228619">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2197599797</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1808832265">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3382924942">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">200</maxWidth>
                    <sourceText dataType="String">One-Way Collision</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="373897051" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1045083732">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3690285088</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="190843830">
                  <item dataType="ObjectRef">262947433</item>
                  <item dataType="ObjectRef">3940228619</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">262947433</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4175355248">jPnHajv8d0+QZhT4NjXezg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Force</name>
            <parent dataType="ObjectRef">4243299910</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1333957358">
        <_items dataType="Array" type="Duality.Component[]" id="3385823714" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="77477484" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="332279416" length="0" />
          <values dataType="Array" type="System.Object[]" id="877132254" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3978519332">jdgUF/9TiUaPL7jvRkyyQg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Descriptions</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">3771095618</item>
    <item dataType="ObjectRef">826064256</item>
    <item dataType="ObjectRef">3179955376</item>
    <item dataType="ObjectRef">1295111380</item>
    <item dataType="ObjectRef">4212726542</item>
    <item dataType="ObjectRef">4272538120</item>
    <item dataType="ObjectRef">2197599797</item>
    <item dataType="ObjectRef">4180770938</item>
    <item dataType="ObjectRef">3609771450</item>
    <item dataType="ObjectRef">475944367</item>
    <item dataType="ObjectRef">1882913398</item>
    <item dataType="ObjectRef">3190397052</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
