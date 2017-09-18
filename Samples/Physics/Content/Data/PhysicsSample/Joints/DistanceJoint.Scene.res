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
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">240</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-64</X>
                    <Y dataType="Float">240</Y>
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
          <item dataType="Struct" type="Duality.GameObject" id="190303241">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="963312889">
              <_items dataType="Array" type="Duality.Component[]" id="292994126" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2550618173">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">190303241</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">576</X>
                    <Y dataType="Float">240</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">576</X>
                    <Y dataType="Float">240</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3253079765">
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
                  <gameobj dataType="ObjectRef">190303241</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3192008149">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="238185462">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="726128352">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1900582876">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="3914742468" length="4">
                            <item dataType="Array" type="Duality.Vector2[]" id="1989369668">
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
                        <parent dataType="ObjectRef">3253079765</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="2516240662">
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
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="788996431">
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
                  <gameobj dataType="ObjectRef">190303241</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="97244800" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3572148051">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="792546168">
                  <item dataType="ObjectRef">2550618173</item>
                  <item dataType="ObjectRef">3253079765</item>
                  <item dataType="ObjectRef">788996431</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2550618173</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="571643193">goeIM99X6Uuzs2+dAz51Yg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Wall</name>
            <parent dataType="ObjectRef">3975364469</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
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
          <item dataType="Struct" type="Duality.GameObject" id="1570063134">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1777473286">
              <_items dataType="Array" type="Duality.GameObject[]" id="1091838336" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="553381659">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3849577647">
                    <_items dataType="Array" type="Duality.GameObject[]" id="290220014" length="4" />
                    <_size dataType="Int">0</_size>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1482508448">
                    <_items dataType="Array" type="Duality.Component[]" id="841871237" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2913696591">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">553381659</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3930378066">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1570063134</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">-256</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">-256</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-256</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3616158183">
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
                        <gameobj dataType="ObjectRef">553381659</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1810842533">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3493198230" length="4">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="441357344">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">1</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="337872362">
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
                                <gameobj dataType="ObjectRef">1570063134</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2257678050">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="403263632" length="4">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3784508732">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">337872362</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <userTag dataType="Int">0</userTag>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                                <useCCD dataType="Bool">false</useCCD>
                              </otherBody>
                              <parentBody dataType="ObjectRef">3616158183</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2346358888">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3608732751" length="4">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="313346606">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3977495376">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="2525033404" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="2842725956">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">-32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">32</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">3616158183</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3571117934">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">-32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">32</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1152074849">
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
                        <gameobj dataType="ObjectRef">553381659</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="348140605" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3242160932">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3056011542">
                        <item dataType="ObjectRef">2913696591</item>
                        <item dataType="ObjectRef">3616158183</item>
                        <item dataType="ObjectRef">1152074849</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2913696591</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3795459360">SqV/g4TJ0kqDSmpt6jIhgw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">1570063134</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2913805626">
              <_items dataType="Array" type="Duality.Component[]" id="2763400820" length="4">
                <item dataType="ObjectRef">3930378066</item>
                <item dataType="ObjectRef">337872362</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2168756324">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">204</B>
                    <G dataType="Byte">204</G>
                    <R dataType="Byte">204</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">1570063134</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2290840966" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1053105920">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="353515982">
                  <item dataType="ObjectRef">3930378066</item>
                  <item dataType="ObjectRef">337872362</item>
                  <item dataType="ObjectRef">2168756324</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3930378066</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2086205852">GAIJR5xV4UGA3U9K9qa05w==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3007115393">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1835754013">
              <_items dataType="Array" type="Duality.GameObject[]" id="2607733990" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1060277780">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3229097840">
                    <_items dataType="Array" type="Duality.Component[]" id="4265248060" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3420592712">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1060277780</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1072463029">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">3007115393</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3" />
                          <posAbs dataType="Struct" type="Duality.Vector3" />
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4123054304">
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
                        <gameobj dataType="ObjectRef">1060277780</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1915423208">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="841419820">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="402171108">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">10</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1774924621">
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
                                <gameobj dataType="ObjectRef">3007115393</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1709587489">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2210843246">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1098800720">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">1774924621</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <userTag dataType="Int">0</userTag>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                                <useCCD dataType="Bool">false</useCCD>
                              </otherBody>
                              <parentBody dataType="ObjectRef">4123054304</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1141596958">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3084207786">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="612605472">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="280783836">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="414390980" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="535895876">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">-32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">32</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">4123054304</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="1833314582">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">-32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">32</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1658970970">
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
                        <gameobj dataType="ObjectRef">1060277780</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="316274414" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="4122724546">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2024744458">
                        <item dataType="ObjectRef">3420592712</item>
                        <item dataType="ObjectRef">4123054304</item>
                        <item dataType="ObjectRef">1658970970</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3420592712</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3618845106">sM3SHOAEIEOoZif9s8IvhA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">3007115393</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3478108920">
              <_items dataType="Array" type="Duality.Component[]" id="2379244919" length="4">
                <item dataType="ObjectRef">1072463029</item>
                <item dataType="ObjectRef">1774924621</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3605808583">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">204</B>
                    <G dataType="Byte">204</G>
                    <R dataType="Byte">204</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">3007115393</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2820564663" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="142438164">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2212987190">
                  <item dataType="ObjectRef">1072463029</item>
                  <item dataType="ObjectRef">1774924621</item>
                  <item dataType="ObjectRef">3605808583</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1072463029</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3643976112">arL59cVQFEKBeRbmKm9s/w==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1447218640">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1960639512">
              <_items dataType="Array" type="Duality.GameObject[]" id="3041965100" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3322202167">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1208542475">
                    <_items dataType="Array" type="Duality.Component[]" id="1263473270" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1387549803">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3322202167</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3807533572">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1447218640</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">256</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">256</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2090011395">
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
                        <gameobj dataType="ObjectRef">3322202167</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="2580410931">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="2695445030">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="4076261632">
                              <breakPoint dataType="Float">20</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">5</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="215027868">
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
                                <gameobj dataType="ObjectRef">1447218640</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="747323844">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1624197444">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1508254276">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">215027868</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <userTag dataType="Int">0</userTag>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                                <useCCD dataType="Bool">false</useCCD>
                              </otherBody>
                              <parentBody dataType="ObjectRef">2090011395</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2158698680">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1395374169">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="663381966">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="524579280">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="1073866428" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="993608260">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">-32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">32</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2090011395</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3468414574">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">-32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">32</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3920895357">
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
                        <gameobj dataType="ObjectRef">3322202167</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1075112136" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2876920993">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="27508512">
                        <item dataType="ObjectRef">1387549803</item>
                        <item dataType="ObjectRef">2090011395</item>
                        <item dataType="ObjectRef">3920895357</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1387549803</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2438826803">GYmAmYR44kGU5ycvcjcpzA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">1447218640</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="422816542">
              <_items dataType="Array" type="Duality.Component[]" id="48464858" length="4">
                <item dataType="ObjectRef">3807533572</item>
                <item dataType="ObjectRef">215027868</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2045911830">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">204</B>
                    <G dataType="Byte">204</G>
                    <R dataType="Byte">204</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">1447218640</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2921713988" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2576799464">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1727275294">
                  <item dataType="ObjectRef">3807533572</item>
                  <item dataType="ObjectRef">215027868</item>
                  <item dataType="ObjectRef">2045911830</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3807533572</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1198542932">14gLkUnmikWNBiFySM7CjQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1523040606">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="676544582">
              <_items dataType="Array" type="Duality.GameObject[]" id="1630586880" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1522817997">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3071098809">
                    <_items dataType="Array" type="Duality.Component[]" id="882686158" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3883132929">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1522817997</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3883355538">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1523040606</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">160</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">512</X>
                          <Y dataType="Float">160</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="290627225">
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
                        <gameobj dataType="ObjectRef">1522817997</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="665881961">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="941819150">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="2500476368">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">10</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">-32</Y>
                              </localAnchorA>
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="290849834">
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
                                <gameobj dataType="ObjectRef">1523040606</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3290772098">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="608746128">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3668622652">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">290849834</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <userTag dataType="Int">0</userTag>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                                <useCCD dataType="Bool">false</useCCD>
                              </otherBody>
                              <parentBody dataType="ObjectRef">290627225</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1118350016">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2050281955">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3309622502">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="243051904">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="2656695708" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="3598454212">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">-32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">290627225</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3145615566">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">-32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                              </vertices>
                            </item>
                            <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="438853946">
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">290627225</parent>
                              <position dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">-32</Y>
                              </position>
                              <radius dataType="Float">8</radius>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2121511187">
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
                        <gameobj dataType="ObjectRef">1522817997</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3090804992" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2896455699">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1389427960">
                        <item dataType="ObjectRef">3883132929</item>
                        <item dataType="ObjectRef">290627225</item>
                        <item dataType="ObjectRef">2121511187</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3883132929</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3749113081">WRr0NTBKXUGLYiX88z+IyQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">1523040606</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2853336506">
              <_items dataType="Array" type="Duality.Component[]" id="2643299252" length="4">
                <item dataType="ObjectRef">3883355538</item>
                <item dataType="ObjectRef">290849834</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2121733796">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">204</B>
                    <G dataType="Byte">204</G>
                    <R dataType="Byte">204</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">1523040606</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1690568774" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2352074752">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2151701454">
                  <item dataType="ObjectRef">3883355538</item>
                  <item dataType="ObjectRef">290849834</item>
                  <item dataType="ObjectRef">2121733796</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3883355538</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="178350236">+oz0WzOYpE2mzpkDA5nlAA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4151253437">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2717829201">
              <_items dataType="Array" type="Duality.GameObject[]" id="3093387246" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3614941074">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="101673910">
                    <_items dataType="Array" type="Duality.Component[]" id="4157859168" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1680288710">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3614941074</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2216601073">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">4151253437</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">768</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2382750302">
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
                        <gameobj dataType="ObjectRef">3614941074</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="431057406">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="4265599376">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="2481905980">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">2</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2919062665">
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
                                <gameobj dataType="ObjectRef">4151253437</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3549719381">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1533718774">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1737721056">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">2919062665</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <userTag dataType="Int">0</userTag>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                                <useCCD dataType="Bool">false</useCCD>
                              </otherBody>
                              <parentBody dataType="ObjectRef">2382750302</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="736935818">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1692869596" length="2">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="4024257220">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1717755716">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="102448708" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="1088289348">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">-32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2382750302</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="4089687702">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">-32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="4213634264">
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
                        <gameobj dataType="ObjectRef">3614941074</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4211215514" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="220898436">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3483013270">
                        <item dataType="ObjectRef">1680288710</item>
                        <item dataType="ObjectRef">2382750302</item>
                        <item dataType="ObjectRef">4213634264</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1680288710</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="131567680">T9zczqveMEOTX8esHi7C7Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">4151253437</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="4035316658">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4126427158">
                    <_items dataType="Array" type="Duality.Component[]" id="2155062560" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2100664294">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">4035316658</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="ObjectRef">2216601073</parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">256</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">768</X>
                          <Y dataType="Float">128</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2803125886">
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
                        <gameobj dataType="ObjectRef">4035316658</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="2234272222">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="43969808">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="798464828">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">2</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="ObjectRef">2382750302</otherBody>
                              <parentBody dataType="ObjectRef">2803125886</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2504697098">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2058065404">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="231115588">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3845580356">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3297876548" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="628238916">
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">-32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">0</X>
                                      <Y dataType="Float">32</Y>
                                    </item>
                                    <item dataType="Struct" type="Duality.Vector2">
                                      <X dataType="Float">-32</X>
                                      <Y dataType="Float">0</Y>
                                    </item>
                                  </item>
                                </_items>
                                <_size dataType="Int">1</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2803125886</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3240696470">
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">-32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">0</X>
                                  <Y dataType="Float">32</Y>
                                </item>
                                <item dataType="Struct" type="Duality.Vector2">
                                  <X dataType="Float">-32</X>
                                  <Y dataType="Float">0</Y>
                                </item>
                              </vertices>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </shapes>
                        <useCCD dataType="Bool">false</useCCD>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="339042552">
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
                        <gameobj dataType="ObjectRef">4035316658</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3134855130" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="1221962980">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="329772566">
                        <item dataType="ObjectRef">2100664294</item>
                        <item dataType="ObjectRef">2803125886</item>
                        <item dataType="ObjectRef">339042552</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2100664294</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="764052192">10U5zb+ppk6c8ZSeRwMvww==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">4151253437</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1592268960">
              <_items dataType="Array" type="Duality.Component[]" id="3250273147" length="4">
                <item dataType="ObjectRef">2216601073</item>
                <item dataType="ObjectRef">2919062665</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="454979331">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">204</B>
                    <G dataType="Byte">204</G>
                    <R dataType="Byte">204</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">4151253437</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3114713795" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3926146340">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3318222102">
                  <item dataType="ObjectRef">2216601073</item>
                  <item dataType="ObjectRef">2919062665</item>
                  <item dataType="ObjectRef">454979331</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2216601073</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2956440352">vVxGOylZ+EWZv5VStVBK+A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="64059286">
        <_items dataType="Array" type="Duality.Component[]" id="3137633622" length="4" />
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
    <item dataType="Struct" type="Duality.GameObject" id="1934471966">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1787440776">
        <_items dataType="Array" type="Duality.GameObject[]" id="2638940012" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="302638256">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1842699864">
              <_items dataType="Array" type="Duality.GameObject[]" id="2601623212" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2633694611">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3830143583">
                    <_items dataType="Array" type="Duality.Component[]" id="1884929134" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="699042247">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">2633694611</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2662953188">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">302638256</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">-256</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">-256</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </posAbs>
                          <scale dataType="Float">0.75</scale>
                          <scaleAbs dataType="Float">0.75</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-106.666672</Y>
                          <Z dataType="Float">2.66666675</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-256</X>
                          <Y dataType="Float">-208</Y>
                          <Z dataType="Float">1</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="81356137">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">2633694611</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="717662233">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2689717582">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">300</maxWidth>
                          <sourceText dataType="String">Distances can be enforced smoothly, without exerting too much force.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1926308640" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2014308693">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="Type" id="1098533110" value="Duality.Components.Renderers.TextRenderer" />
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2213079368">
                        <item dataType="ObjectRef">699042247</item>
                        <item dataType="ObjectRef">81356137</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">699042247</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1920322399">vrTfPSVpjEmlh8gzwqWqoQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">302638256</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3642470302">
              <_items dataType="Array" type="Duality.Component[]" id="3075866394" length="4">
                <item dataType="ObjectRef">2662953188</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2045267078">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">302638256</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="656317386">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2403213664">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">200</maxWidth>
                    <sourceText dataType="String">Smooth Constraint</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2836869380" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3674925416">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2630447390">
                  <item dataType="ObjectRef">2662953188</item>
                  <item dataType="ObjectRef">2045267078</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2662953188</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1552787412">za21tby7V0mGqhzFlz49Kg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="980314580">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2556991540">
              <_items dataType="Array" type="Duality.GameObject[]" id="4192818340" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2264137828">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4108391460">
                    <_items dataType="Array" type="Duality.Component[]" id="2121079492" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="329485464">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">2264137828</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3340629512">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">980314580</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </posAbs>
                          <scale dataType="Float">0.75</scale>
                          <scaleAbs dataType="Float">0.75</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-106.666672</Y>
                          <Z dataType="Float">2.66666675</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-208</Y>
                          <Z dataType="Float">1</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="4006766650">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">2264137828</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1937484890">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="225170432">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">300</maxWidth>
                          <sourceText dataType="String">Increasing the Frequency value creates a more rigid connection.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1830824214" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3587175278">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3609156042">
                        <item dataType="ObjectRef">329485464</item>
                        <item dataType="ObjectRef">4006766650</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">329485464</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2233199966">d0N77Y+9wEOTv9YJBhbW0w==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">980314580</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3574244086">
              <_items dataType="Array" type="Duality.Component[]" id="3813178398" length="4">
                <item dataType="ObjectRef">3340629512</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2722943402">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">980314580</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3721216814">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="640663376">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">180</maxWidth>
                    <sourceText dataType="String">Rigid Constraint</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2979558288" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1778286856">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2263732190">
                  <item dataType="ObjectRef">3340629512</item>
                  <item dataType="ObjectRef">2722943402</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3340629512</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2746240756">uCasgcKJ6E+GTTJm0q9XhA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2711452720">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3396726488">
              <_items dataType="Array" type="Duality.GameObject[]" id="3243100076" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3311595843">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="442742543">
                    <_items dataType="Array" type="Duality.Component[]" id="2820630446" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1376943479">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3311595843</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="776800356">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">2711452720</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">256</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">256</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </posAbs>
                          <scale dataType="Float">0.75</scale>
                          <scaleAbs dataType="Float">0.75</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-106.666672</Y>
                          <Z dataType="Float">2.66666675</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
                          <Y dataType="Float">-208</Y>
                          <Z dataType="Float">1</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="759257369">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3311595843</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1156445033">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1932930318">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">Distance joints can be defined as breakable, so they're disabled when a certain error is exceeded.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1362007008" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="944095909">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3836390504">
                        <item dataType="ObjectRef">1376943479</item>
                        <item dataType="ObjectRef">759257369</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1376943479</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3577829231">Rr7CnKAYw0+0mkHR69Lvxg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">2711452720</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4232771742">
              <_items dataType="Array" type="Duality.Component[]" id="1235913370" length="4">
                <item dataType="ObjectRef">776800356</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="159114246">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2711452720</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2209711690">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="687944032">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">180</maxWidth>
                    <sourceText dataType="String">Breakable Constraint</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="961658500" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1524931944">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="407693598">
                  <item dataType="ObjectRef">776800356</item>
                  <item dataType="ObjectRef">159114246</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">776800356</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="641522644">UzxuTPNmnkCk7IvB7GSc/Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4266730499">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="527755599">
              <_items dataType="Array" type="Duality.GameObject[]" id="4127059502" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3582868932">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2996906256">
                    <_items dataType="Array" type="Duality.Component[]" id="587295548" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1648216568">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3582868932</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2332078135">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">4266730499</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </posAbs>
                          <scale dataType="Float">0.75</scale>
                          <scaleAbs dataType="Float">0.75</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-106.666672</Y>
                          <Z dataType="Float">2.66666675</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">512</X>
                          <Y dataType="Float">-208</Y>
                          <Z dataType="Float">1</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1030530458">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3582868932</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2735593210">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3686869376">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">300</maxWidth>
                          <sourceText dataType="String">Joints can be anchored at any relative position. No need to be centered!</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1365029102" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="225428322">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1488022922">
                        <item dataType="ObjectRef">1648216568</item>
                        <item dataType="ObjectRef">1030530458</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1648216568</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3640515730">DXJgNe7LdE+YKyuaE41WvQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">4266730499</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="162529376">
              <_items dataType="Array" type="Duality.Component[]" id="2828440677" length="4">
                <item dataType="ObjectRef">2332078135</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1714392025">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">4266730499</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3650759515">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2305078678">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">180</maxWidth>
                    <sourceText dataType="String">Anchor Positions</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1855570077" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="565415844">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1424776982">
                  <item dataType="ObjectRef">2332078135</item>
                  <item dataType="ObjectRef">1714392025</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2332078135</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2188258976">BWoyG2PQN06HQjWc7qx1MQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1558183409">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="586861677">
              <_items dataType="Array" type="Duality.GameObject[]" id="4056209638" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3500762402">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2315280246">
                    <_items dataType="Array" type="Duality.Component[]" id="2284248032" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1566110038">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3500762402</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3918498341">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1558183409</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
                            <Y dataType="Float">-256</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
                            <Y dataType="Float">-256</Y>
                            <Z dataType="Float">-1</Z>
                          </posAbs>
                          <scale dataType="Float">0.75</scale>
                          <scaleAbs dataType="Float">0.75</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-106.666672</Y>
                          <Z dataType="Float">2.66666675</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">768</X>
                          <Y dataType="Float">-336</Y>
                          <Z dataType="Float">1</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="948423928">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3500762402</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1953760744">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="554544172">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">300</maxWidth>
                          <sourceText dataType="String">More complex setups are possible by combining multiple joints and bodies.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="433442074" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="1012196932">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2636454550">
                        <item dataType="ObjectRef">1566110038</item>
                        <item dataType="ObjectRef">948423928</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1566110038</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2589975552">ZSe/0Rbt8UKBX+y1WhYvtg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">1558183409</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1917781752">
              <_items dataType="Array" type="Duality.Component[]" id="1876751879" length="4">
                <item dataType="ObjectRef">3918498341</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3300812231">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1558183409</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3601831149">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="258830054">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">180</maxWidth>
                    <sourceText dataType="String">Complex Setup</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3437084551" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1856059220">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1347870134">
                  <item dataType="ObjectRef">3918498341</item>
                  <item dataType="ObjectRef">3300812231</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3918498341</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3526505584">SL41p+rc7UOaYCeD5FnsVA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="193807326">
        <_items dataType="Array" type="Duality.Component[]" id="2329980746" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3758793844" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="2356199752" length="0" />
          <values dataType="Array" type="System.Object[]" id="3941524702" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1872759220">pPe2Tvus8ESFWZG38EwsSg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Descriptions</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3978794535">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1850700165">
        <_items dataType="Array" type="Duality.Component[]" id="1305821526" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleInfo" id="3457929408">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3978794535</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1683622568" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3396284527">
            <item dataType="Type" id="713320942" value="Duality.Samples.Physics.PhysicsSampleInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3136042656">
            <item dataType="ObjectRef">3457929408</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1052067325">Y9JycVV+pkSYpOK4kIvI4g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleInfo</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2674434703">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="4016720740">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="2794460100" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2127135048">
                <_items dataType="Array" type="System.Int32[]" id="2774288492"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">713320942</componentType>
              <prop dataType="MemberInfo" id="408413406" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleDesc" />
              <val dataType="String">The /cFF8888FFDistance Joint/cFFFFFFFF ensures that two points on different bodies remain at a constant distance.</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3392522676">
                <_items dataType="ObjectRef">2774288492</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">713320942</componentType>
              <prop dataType="MemberInfo" id="1253331490" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleName" />
              <val dataType="String">Distance Joint</val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
        </changes>
        <obj dataType="ObjectRef">3978794535</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleInfo.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="ObjectRef">3771095618</item>
    <item dataType="ObjectRef">190303241</item>
    <item dataType="ObjectRef">1570063134</item>
    <item dataType="ObjectRef">3007115393</item>
    <item dataType="ObjectRef">1447218640</item>
    <item dataType="ObjectRef">1523040606</item>
    <item dataType="ObjectRef">4151253437</item>
    <item dataType="ObjectRef">302638256</item>
    <item dataType="ObjectRef">980314580</item>
    <item dataType="ObjectRef">2711452720</item>
    <item dataType="ObjectRef">4266730499</item>
    <item dataType="ObjectRef">1558183409</item>
    <item dataType="ObjectRef">553381659</item>
    <item dataType="ObjectRef">1060277780</item>
    <item dataType="ObjectRef">3322202167</item>
    <item dataType="ObjectRef">1522817997</item>
    <item dataType="ObjectRef">3614941074</item>
    <item dataType="ObjectRef">4035316658</item>
    <item dataType="ObjectRef">2633694611</item>
    <item dataType="ObjectRef">2264137828</item>
    <item dataType="ObjectRef">3311595843</item>
    <item dataType="ObjectRef">3582868932</item>
    <item dataType="ObjectRef">3500762402</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
