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
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-256</X>
                          <Y dataType="Float">192</Y>
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
                            <item dataType="Struct" type="Duality.Components.Physics.AngleJointInfo" id="441357344">
                              <angle dataType="Float">0</angle>
                              <biasFactor dataType="Float">0.2</biasFactor>
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <maxImpulse dataType="Float">-1</maxImpulse>
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
                              <softness dataType="Float">0.95</softness>
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
          <item dataType="Struct" type="Duality.GameObject" id="2501230230">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="513143758">
              <_items dataType="Array" type="Duality.GameObject[]" id="1553015248" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3356722690">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3260655642">
                    <_items dataType="Array" type="Duality.Component[]" id="2097080704" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1422070326">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3356722690</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="566577866">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">2501230230</gameobj>
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
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2124531918">
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
                        <gameobj dataType="ObjectRef">3356722690</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="4158923726">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3627205072">
                            <item dataType="Struct" type="Duality.Components.Physics.AngleJointInfo" id="2295195324">
                              <angle dataType="Float">0</angle>
                              <biasFactor dataType="Float">0.2</biasFactor>
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <maxImpulse dataType="Float">-1</maxImpulse>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1269039458">
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
                                <gameobj dataType="ObjectRef">2501230230</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3993698642">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1193659728">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="4256101308">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">1269039458</parent>
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
                              <parentBody dataType="ObjectRef">2124531918</parentBody>
                              <softness dataType="Float">0</softness>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2337142090">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="995563148">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1499894180">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1305332932">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="2058938180" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="805310020">
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
                              <parent dataType="ObjectRef">2124531918</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="1802075030">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3955415880">
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
                        <gameobj dataType="ObjectRef">3356722690</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="702942522" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2585779040">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2577074318">
                        <item dataType="ObjectRef">1422070326</item>
                        <item dataType="ObjectRef">2124531918</item>
                        <item dataType="ObjectRef">3955415880</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1422070326</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="911352188">1cOcbY9A8kWPs+UcOiReJQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">2501230230</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1143067978">
              <_items dataType="Array" type="Duality.Component[]" id="620119692" length="4">
                <item dataType="ObjectRef">566577866</item>
                <item dataType="ObjectRef">1269039458</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3099923420">
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
                  <gameobj dataType="ObjectRef">2501230230</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3264383870" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1859397280">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3719926926">
                  <item dataType="ObjectRef">566577866</item>
                  <item dataType="ObjectRef">1269039458</item>
                  <item dataType="ObjectRef">3099923420</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">566577866</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3066089148">RvU2eyNSE0u777yYE0Yoww==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="908058986">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3624811018">
              <_items dataType="Array" type="Duality.GameObject[]" id="123686624" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3297445292">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2701613172">
                    <_items dataType="Array" type="Duality.Component[]" id="4038784932" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1362792928">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0.7853982</angle>
                        <angleAbs dataType="Float">0.7853982</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3297445292</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3268373918">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">908058986</gameobj>
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
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2065254520">
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
                        <gameobj dataType="ObjectRef">3297445292</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="3745632176">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3730876860">
                            <item dataType="Struct" type="Duality.Components.Physics.AngleJointInfo" id="2693536324">
                              <angle dataType="Float">0</angle>
                              <biasFactor dataType="Float">0.2</biasFactor>
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <maxImpulse dataType="Float">0.1</maxImpulse>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3970835510">
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
                                <gameobj dataType="ObjectRef">908058986</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="506553774">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="531244368">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3317588924">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">3970835510</parent>
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
                              <parentBody dataType="ObjectRef">2065254520</parentBody>
                              <softness dataType="Float">0</softness>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2878855534">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2678743426">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="47955088">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3881338172">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="2537152324" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="3453173316">
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
                              <parent dataType="ObjectRef">2065254520</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="393434006">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3896138482">
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
                        <gameobj dataType="ObjectRef">3297445292</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1095822838" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="1066808670">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2645484298">
                        <item dataType="ObjectRef">1362792928</item>
                        <item dataType="ObjectRef">2065254520</item>
                        <item dataType="ObjectRef">3896138482</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1362792928</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="800258222">JUKvuSRnrUeJoBrY4o+aJA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">908058986</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1066609690">
              <_items dataType="Array" type="Duality.Component[]" id="1820097776" length="4">
                <item dataType="ObjectRef">3268373918</item>
                <item dataType="ObjectRef">3970835510</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1506752176">
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
                  <gameobj dataType="ObjectRef">908058986</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2507409642" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1515318720">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1214669902">
                  <item dataType="ObjectRef">3268373918</item>
                  <item dataType="ObjectRef">3970835510</item>
                  <item dataType="ObjectRef">1506752176</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3268373918</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3047074140">SKISYQ0hn0ObJHbOQsWJvQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2095853469">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3360728305">
              <_items dataType="Array" type="Duality.GameObject[]" id="326715310" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2125163495">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2804976535">
                    <_items dataType="Array" type="Duality.Component[]" id="3589529870" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="190511131">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0.7853982</angle>
                        <angleAbs dataType="Float">0.7853982</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">2125163495</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="161201105">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">2095853469</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">192</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">192</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">128</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">640</X>
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="892972723">
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
                        <gameobj dataType="ObjectRef">2125163495</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="3047573843">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="2332819302">
                            <item dataType="Struct" type="Duality.Components.Physics.AngleJointInfo" id="3892266880">
                              <angle dataType="Float">-0.785</angle>
                              <biasFactor dataType="Float">0.2</biasFactor>
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <maxImpulse dataType="Float">-1</maxImpulse>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="863662697">
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
                                <gameobj dataType="ObjectRef">2095853469</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="3293746317">
                                  <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3632050470" length="1" />
                                  <_size dataType="Int">0</_size>
                                </joints>
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="68040632">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1970794983">
                                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2645046606">
                                      <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3883965136">
                                        <_items dataType="Array" type="Duality.Vector2[][]" id="2369826492" length="4">
                                          <item dataType="Array" type="Duality.Vector2[]" id="459067972">
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
                                      <parent dataType="ObjectRef">863662697</parent>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <userTag dataType="Int">0</userTag>
                                      <vertices dataType="Array" type="Duality.Vector2[]" id="479778414">
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
                              </otherBody>
                              <parentBody dataType="ObjectRef">892972723</parentBody>
                              <softness dataType="Float">0.9</softness>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="670685048">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3317859385">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1557194958">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="4116611024">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="2722630332" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="661676612">
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
                              <parent dataType="ObjectRef">892972723</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3702719086">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2723856685">
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
                        <gameobj dataType="ObjectRef">2125163495</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1531156160" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="1370323229">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2876134136">
                        <item dataType="ObjectRef">190511131</item>
                        <item dataType="ObjectRef">892972723</item>
                        <item dataType="ObjectRef">2723856685</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">190511131</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2932266935">uC12Dt24+EaKwMgvVCEVpw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">2095853469</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1126167923">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2582334995">
                    <_items dataType="Array" type="Duality.Component[]" id="2797929190" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3486482855">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0.7853982</angle>
                        <angleAbs dataType="Float">0.7853982</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1126167923</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="ObjectRef">161201105</parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">256</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">768</X>
                          <Y dataType="Float">192</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4188944447">
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
                        <gameobj dataType="ObjectRef">1126167923</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="4058164223">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3769497390">
                            <item dataType="Struct" type="Duality.Components.Physics.AngleJointInfo" id="599797584">
                              <angle dataType="Float">-0.785</angle>
                              <biasFactor dataType="Float">0.2</biasFactor>
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <maxImpulse dataType="Float">-1</maxImpulse>
                              <otherBody dataType="ObjectRef">892972723</otherBody>
                              <parentBody dataType="ObjectRef">4188944447</parentBody>
                              <softness dataType="Float">0.9</softness>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1656720736">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="443154485">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1737269750">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="2900434656">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3334895580" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="1350351556">
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
                              <parent dataType="ObjectRef">4188944447</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="2947182478">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1724861113">
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
                        <gameobj dataType="ObjectRef">1126167923</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3079582968" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="265145977">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3624048000">
                        <item dataType="ObjectRef">3486482855</item>
                        <item dataType="ObjectRef">4188944447</item>
                        <item dataType="ObjectRef">1724861113</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3486482855</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3725249403">fL7sF/NqDEuNET7mgSN8Aw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">2095853469</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2990273504">
              <_items dataType="Array" type="Duality.Component[]" id="3802419547" length="4">
                <item dataType="ObjectRef">161201105</item>
                <item dataType="ObjectRef">863662697</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2694546659">
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
                  <gameobj dataType="ObjectRef">2095853469</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1312162723" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2757333924">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2334342934">
                  <item dataType="ObjectRef">161201105</item>
                  <item dataType="ObjectRef">863662697</item>
                  <item dataType="ObjectRef">2694546659</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">161201105</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1529328288">/GTCbS5hVEuxHnCA1QHsNQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Box</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
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
                          <sourceText dataType="String">Angles can be enforced smoothly, without exerting too much force.</sourceText>
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
          <item dataType="Struct" type="Duality.GameObject" id="1628154645">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1835776921">
              <_items dataType="Array" type="Duality.GameObject[]" id="981191758" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="271813358">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1198522658">
                    <_items dataType="Array" type="Duality.Component[]" id="2041635600" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2632128290">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">271813358</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3988469577">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1628154645</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2014442180">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">271813358</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2319371564">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="957107940">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">300</maxWidth>
                          <sourceText dataType="String">Decreasing the joints Smoothness value creates a more rigid connection.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1603458826" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="40415416">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2430848222">
                        <item dataType="ObjectRef">2632128290</item>
                        <item dataType="ObjectRef">2014442180</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2632128290</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1366781156">QcZ2wUUY/Ey7oskOw+XVUw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">1628154645</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4151969408">
              <_items dataType="Array" type="Duality.Component[]" id="3326740915" length="4">
                <item dataType="ObjectRef">3988469577</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3370783467">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1628154645</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2613824745">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="880965390">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Rigid Constraint</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3066037979" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3964565588">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2922871734">
                  <item dataType="ObjectRef">3988469577</item>
                  <item dataType="ObjectRef">3370783467</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3988469577</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1537129328">FODQGtvOd0uwcLUAi5ICdg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3654709876">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1424932052">
              <_items dataType="Array" type="Duality.GameObject[]" id="2394505956" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3247753179">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="679654519">
                    <_items dataType="Array" type="Duality.Component[]" id="64312718" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1313100815">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3247753179</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1720057512">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">3654709876</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="695414705">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3247753179</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3751234305">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1336443182">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">Decreasing the MaxImpulse value can create joints that work only in good conditions.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2195188032" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2293028285">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="599450296">
                        <item dataType="ObjectRef">1313100815</item>
                        <item dataType="ObjectRef">695414705</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1313100815</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="809435543">lIP7xW9CPkSICUaNva4ucQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">3654709876</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2438692278">
              <_items dataType="Array" type="Duality.Component[]" id="3765424254" length="4">
                <item dataType="ObjectRef">1720057512</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1102371402">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3654709876</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1606292110">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3867842768">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Limited Force</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3194477040" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4210119368">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3104876254">
                  <item dataType="ObjectRef">1720057512</item>
                  <item dataType="ObjectRef">1102371402</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1720057512</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1677975348">8z/4yS6a7E2p6oq2+G6oaw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3167659149">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="592701889">
              <_items dataType="Array" type="Duality.GameObject[]" id="2774334638" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="48512766">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1762835602">
                    <_items dataType="Array" type="Duality.Component[]" id="1813146704" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2408827698">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">48512766</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1233006785">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">3167659149</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1791141588">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">48512766</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="942334620">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="129145796">
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3809982922" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="105472712">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3470542558">
                        <item dataType="ObjectRef">2408827698</item>
                        <item dataType="ObjectRef">1791141588</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2408827698</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3988344628">3351YpGjXkCJysN1d60Jfw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">3167659149</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3586676960">
              <_items dataType="Array" type="Duality.Component[]" id="2353193483" length="4">
                <item dataType="ObjectRef">1233006785</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="615320675">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3167659149</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1651921105">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2470877422">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Complex Setup</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2978391315" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2078856228">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1508190486">
                  <item dataType="ObjectRef">1233006785</item>
                  <item dataType="ObjectRef">615320675</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1233006785</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1048655392">5+5ibKzqM0qGADrZbbw0WQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
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
              <prop dataType="MemberInfo" id="408413406" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleName" />
              <val dataType="String">Angle Joint</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3392522676">
                <_items dataType="ObjectRef">2774288492</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">713320942</componentType>
              <prop dataType="MemberInfo" id="1253331490" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleDesc" />
              <val dataType="String">The /cFF8888FFAngle Joint/cFFFFFFFF ensures that bodies remain in the same relative local angle, regardless of their world positions.</val>
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
    <item dataType="ObjectRef">2501230230</item>
    <item dataType="ObjectRef">908058986</item>
    <item dataType="ObjectRef">2095853469</item>
    <item dataType="ObjectRef">302638256</item>
    <item dataType="ObjectRef">1628154645</item>
    <item dataType="ObjectRef">3654709876</item>
    <item dataType="ObjectRef">3167659149</item>
    <item dataType="ObjectRef">553381659</item>
    <item dataType="ObjectRef">3356722690</item>
    <item dataType="ObjectRef">3297445292</item>
    <item dataType="ObjectRef">2125163495</item>
    <item dataType="ObjectRef">1126167923</item>
    <item dataType="ObjectRef">2633694611</item>
    <item dataType="ObjectRef">271813358</item>
    <item dataType="ObjectRef">3247753179</item>
    <item dataType="ObjectRef">48512766</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
