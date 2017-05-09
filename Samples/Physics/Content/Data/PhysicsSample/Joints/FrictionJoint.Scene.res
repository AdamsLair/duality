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
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">789494455</gameobj>
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="3179422778">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="1517587712" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="4275612316">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">202</B>
                    <G dataType="Byte">149</G>
                    <R dataType="Byte">90</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="3175817750">
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
            </passes>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
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
                  <offset dataType="Int">0</offset>
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
                  <offset dataType="Int">0</offset>
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
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <continous dataType="Bool">false</continous>
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">553381659</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1810842533">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3493198230" length="4">
                            <item dataType="Struct" type="Duality.Components.Physics.FrictionJointInfo" id="441357344">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <maxForce dataType="Float">1</maxForce>
                              <maxTorque dataType="Float">50</maxTorque>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="337872362">
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
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
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
                        <offset dataType="Int">0</offset>
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
                  <offset dataType="Int">0</offset>
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
          <item dataType="Struct" type="Duality.GameObject" id="289270660">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3958422916">
              <_items dataType="Array" type="Duality.GameObject[]" id="2806386756" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1536454540">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3347119676">
                    <_items dataType="Array" type="Duality.Component[]" id="1001179460" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3896769472">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1536454540</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2649585592">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">289270660</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="304263768">
                        <active dataType="Bool">true</active>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <continous dataType="Bool">false</continous>
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">1536454540</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1225250000">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="1603402428">
                            <item dataType="Struct" type="Duality.Components.Physics.FrictionJointInfo" id="4208280132">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <maxForce dataType="Float">5</maxForce>
                              <maxTorque dataType="Float">200</maxTorque>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3352047184">
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
                                <gameobj dataType="ObjectRef">289270660</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="518608536">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3532101164">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3058912484">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">3352047184</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                              </otherBody>
                              <parentBody dataType="ObjectRef">304263768</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1563939438">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="280807330">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3041228048">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3395390268">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="910350148" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="1016581700">
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
                              <parent dataType="ObjectRef">304263768</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3668773782">
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2135147730">
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
                        <gameobj dataType="ObjectRef">1536454540</gameobj>
                        <offset dataType="Int">0</offset>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3342369174" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="701189654">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1294599130">
                        <item dataType="ObjectRef">3896769472</item>
                        <item dataType="ObjectRef">304263768</item>
                        <item dataType="ObjectRef">2135147730</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3896769472</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1848586678">DSQI+Zx+wEuG9twafrtaQQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">289270660</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3868624022">
              <_items dataType="Array" type="Duality.Component[]" id="2811519886" length="4">
                <item dataType="ObjectRef">2649585592</item>
                <item dataType="ObjectRef">3352047184</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="887963850">
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
                  <gameobj dataType="ObjectRef">289270660</gameobj>
                  <offset dataType="Int">0</offset>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4134179648" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1444360904">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2505451230">
                  <item dataType="ObjectRef">2649585592</item>
                  <item dataType="ObjectRef">3352047184</item>
                  <item dataType="ObjectRef">887963850</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2649585592</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1900592948">zqd8JEGXK0yhLqIJbJ2TYQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1974999523">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="923390607">
              <_items dataType="Array" type="Duality.GameObject[]" id="1548510382" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2083943057">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4039433505">
                    <_items dataType="Array" type="Duality.Component[]" id="2790272622" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="149290693">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">2083943057</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="40347159">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1974999523</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="851752285">
                        <active dataType="Bool">true</active>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <continous dataType="Bool">false</continous>
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">2083943057</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1857460637">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="2368164582">
                            <item dataType="Struct" type="Duality.Components.Physics.FrictionJointInfo" id="3510394240">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <maxForce dataType="Float">50</maxForce>
                              <maxTorque dataType="Float">1000</maxTorque>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="742808751">
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
                                <gameobj dataType="ObjectRef">1974999523</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2974345515">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3160972790">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1996472032">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">742808751</parent>
                                      <position dataType="Struct" type="Duality.Vector2" />
                                      <radius dataType="Float">16</radius>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                    </item>
                                  </_items>
                                  <_size dataType="Int">1</_size>
                                </shapes>
                              </otherBody>
                              <parentBody dataType="ObjectRef">851752285</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="222766328">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1065201399">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2727246478">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1359612112">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="2404698812" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="2290377284">
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
                              <parent dataType="ObjectRef">851752285</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="3279667822">
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2682636247">
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
                        <gameobj dataType="ObjectRef">2083943057</gameobj>
                        <offset dataType="Int">0</offset>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="139606304" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3724306219">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2710420040">
                        <item dataType="ObjectRef">149290693</item>
                        <item dataType="ObjectRef">851752285</item>
                        <item dataType="ObjectRef">2682636247</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">149290693</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1150576417">oTyPKbmTzU6xAMaKCofR/Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">1974999523</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2327593184">
              <_items dataType="Array" type="Duality.Component[]" id="668998949" length="4">
                <item dataType="ObjectRef">40347159</item>
                <item dataType="ObjectRef">742808751</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2573692713">
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
                  <gameobj dataType="ObjectRef">1974999523</gameobj>
                  <offset dataType="Int">0</offset>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="272635101" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2318122404">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2807674646">
                  <item dataType="ObjectRef">40347159</item>
                  <item dataType="ObjectRef">742808751</item>
                  <item dataType="ObjectRef">2573692713</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">40347159</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4289595552">s1t/Q8fqTEKFNtwIFk/XHA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="228442758">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1489551678">
              <_items dataType="Array" type="Duality.GameObject[]" id="393069072" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3728469028">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1105561356">
                    <_items dataType="Array" type="Duality.Component[]" id="1888041124" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1793816664">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3728469028</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2588757690">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">228442758</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">128</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">512</X>
                            <Y dataType="Float">128</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="Duality.Vector3" />
                          <velAbs dataType="Struct" type="Duality.Vector3" />
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-64</X>
                          <Y dataType="Float">-128</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">448</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2496278256">
                        <active dataType="Bool">true</active>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <continous dataType="Bool">false</continous>
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">3728469028</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="4265710712">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="2348932460">
                            <item dataType="Struct" type="Duality.Components.Physics.FrictionJointInfo" id="768850788">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <maxForce dataType="Float">5</maxForce>
                              <maxTorque dataType="Float">200</maxTorque>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3291219282">
                                <active dataType="Bool">true</active>
                                <angularDamp dataType="Float">0.3</angularDamp>
                                <angularVel dataType="Float">0</angularVel>
                                <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                                <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                                <colFilter />
                                <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                                <continous dataType="Bool">false</continous>
                                <explicitInertia dataType="Float">0</explicitInertia>
                                <explicitMass dataType="Float">0</explicitMass>
                                <fixedAngle dataType="Bool">false</fixedAngle>
                                <gameobj dataType="ObjectRef">228442758</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="3250027906">
                                  <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="1038746768" length="1" />
                                  <_size dataType="Int">0</_size>
                                </joints>
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="974941834">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4139940952">
                                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="676836012">
                                      <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="373323492">
                                        <_items dataType="Array" type="Duality.Vector2[][]" id="536367044" length="4">
                                          <item dataType="Array" type="Duality.Vector2[]" id="2081013060">
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
                                      <parent dataType="ObjectRef">3291219282</parent>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <vertices dataType="Array" type="Duality.Vector2[]" id="2017670678">
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
                              </otherBody>
                              <parentBody dataType="ObjectRef">2496278256</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2323295710">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2655557818">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="4064009728">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="2226867356">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3302962116" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="26475844">
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
                              <parent dataType="ObjectRef">2496278256</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="2616134166">
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="32194922">
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
                        <gameobj dataType="ObjectRef">3728469028</gameobj>
                        <offset dataType="Int">0</offset>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="162248438" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3624509830">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1693099834">
                        <item dataType="ObjectRef">1793816664</item>
                        <item dataType="ObjectRef">2496278256</item>
                        <item dataType="ObjectRef">32194922</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1793816664</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3884050694">kaNgglvpC0+jQ7XnZeWuvg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">228442758</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4017869322">
              <_items dataType="Array" type="Duality.Component[]" id="1232156444" length="4">
                <item dataType="ObjectRef">2588757690</item>
                <item dataType="ObjectRef">3291219282</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="827135948">
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
                  <gameobj dataType="ObjectRef">228442758</gameobj>
                  <offset dataType="Int">0</offset>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3968114766" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1176594592">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3969347726">
                  <item dataType="ObjectRef">2588757690</item>
                  <item dataType="ObjectRef">3291219282</item>
                  <item dataType="ObjectRef">827135948</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2588757690</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="833580220">voDjiX6S5UK8iRY8GvBt3Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Box</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1419031358">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2192936806">
              <_items dataType="Array" type="Duality.GameObject[]" id="714565504" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2752104523">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4154568831">
                    <_items dataType="Array" type="Duality.Component[]" id="2969914158" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="817452159">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">2752104523</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="3779346290">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1419031358</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
                            <Y dataType="Float">128</Y>
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
                        </parentTransform>
                        <pos dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">-64</X>
                          <Y dataType="Float">-128</Y>
                          <Z dataType="Float">0</Z>
                        </pos>
                        <posAbs dataType="Struct" type="Duality.Vector3">
                          <X dataType="Float">704</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1519913751">
                        <active dataType="Bool">true</active>
                        <angularDamp dataType="Float">0.3</angularDamp>
                        <angularVel dataType="Float">0</angularVel>
                        <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                        <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                        <colFilter />
                        <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                        <continous dataType="Bool">false</continous>
                        <explicitInertia dataType="Float">0</explicitInertia>
                        <explicitMass dataType="Float">0</explicitMass>
                        <fixedAngle dataType="Bool">false</fixedAngle>
                        <gameobj dataType="ObjectRef">2752104523</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="877997415">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="800573006">
                            <item dataType="Struct" type="Duality.Components.Physics.FrictionJointInfo" id="743448272">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <enabled dataType="Bool">true</enabled>
                              <localAnchorA dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">32</Y>
                              </localAnchorA>
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <maxForce dataType="Float">10</maxForce>
                              <maxTorque dataType="Float">0</maxTorque>
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="186840586">
                                <active dataType="Bool">true</active>
                                <angularDamp dataType="Float">0.3</angularDamp>
                                <angularVel dataType="Float">0</angularVel>
                                <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                                <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1" value="1" />
                                <colFilter />
                                <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="All" value="2147483647" />
                                <continous dataType="Bool">false</continous>
                                <explicitInertia dataType="Float">0</explicitInertia>
                                <explicitMass dataType="Float">0</explicitMass>
                                <fixedAngle dataType="Bool">false</fixedAngle>
                                <gameobj dataType="ObjectRef">1419031358</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1337452770">
                                  <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="1609804944" length="0" />
                                  <_size dataType="Int">0</_size>
                                </joints>
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3450132106">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3400794744">
                                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3134376300">
                                      <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="2900233060">
                                        <_items dataType="Array" type="Duality.Vector2[][]" id="1517884356" length="4">
                                          <item dataType="Array" type="Duality.Vector2[]" id="3469720900">
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
                                      <parent dataType="ObjectRef">186840586</parent>
                                      <restitution dataType="Float">0.3</restitution>
                                      <sensor dataType="Bool">false</sensor>
                                      <vertices dataType="Array" type="Duality.Vector2[]" id="2286184982">
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
                              </otherBody>
                              <parentBody dataType="ObjectRef">1519913751</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3049432192">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2437916493">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1439412262">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1445308672">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3624669852" length="4">
                                  <item dataType="Array" type="Duality.Vector2[]" id="3015900100">
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
                              <parent dataType="ObjectRef">1519913751</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="2699811278">
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
                            <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="571096762">
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">1519913751</parent>
                              <position dataType="Struct" type="Duality.Vector2">
                                <X dataType="Float">0</X>
                                <Y dataType="Float">32</Y>
                              </position>
                              <radius dataType="Float">8</radius>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                            </item>
                          </_items>
                          <_size dataType="Int">2</_size>
                        </shapes>
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3350797713">
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
                        <gameobj dataType="ObjectRef">2752104523</gameobj>
                        <offset dataType="Int">0</offset>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3952071008" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="270676917">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">2627584164</item>
                        <item dataType="ObjectRef">557071126</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="923493192">
                        <item dataType="ObjectRef">817452159</item>
                        <item dataType="ObjectRef">1519913751</item>
                        <item dataType="ObjectRef">3350797713</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">817452159</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="4179731199">roC3gb4Z30iXqzxPAfwNOg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">1419031358</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2492647226">
              <_items dataType="Array" type="Duality.Component[]" id="3756134100" length="4">
                <item dataType="ObjectRef">3779346290</item>
                <item dataType="ObjectRef">186840586</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2017724548">
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
                  <gameobj dataType="ObjectRef">1419031358</gameobj>
                  <offset dataType="Int">0</offset>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2168727526" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2513891072">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">2627584164</item>
                  <item dataType="ObjectRef">557071126</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1256241614">
                  <item dataType="ObjectRef">3779346290</item>
                  <item dataType="ObjectRef">186840586</item>
                  <item dataType="ObjectRef">2017724548</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3779346290</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1706584476">nv3Lol/Q606egCYrTE+dSw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Box</name>
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
              <val dataType="String">Friction Joint</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3392522676">
                <_items dataType="ObjectRef">2774288492</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">713320942</componentType>
              <prop dataType="MemberInfo" id="1253331490" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleDesc" />
              <val dataType="String">The /cFF8888FFFriction Joint/cFFFFFFFF simulates friction between two points on different bodies by applying counter-forces to their relative movement.</val>
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
                        <offset dataType="Int">0</offset>
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
                          <sourceText dataType="String">A low friction value for both movement and rotation.</sourceText>
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
                  <offset dataType="Int">0</offset>
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
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Low Friction</sourceText>
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
          <item dataType="Struct" type="Duality.GameObject" id="2413364638">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2605929318">
              <_items dataType="Array" type="Duality.GameObject[]" id="2170931072" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3614180199">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="358882467">
                    <_items dataType="Array" type="Duality.Component[]" id="3100181862" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1679527835">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3614180199</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="478712274">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">2413364638</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1061841725">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3614180199</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Int">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2935381677">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3980713830">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">A more noticeable friction value is characterized by higher counter-force limits.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2834706808" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="1332535241">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3340508736">
                        <item dataType="ObjectRef">1679527835</item>
                        <item dataType="ObjectRef">1061841725</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1679527835</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="416396779">iUDW4rqx4EOex4Q1S48CDg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">2413364638</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1948690234">
              <_items dataType="Array" type="Duality.Component[]" id="1061614292" length="4">
                <item dataType="ObjectRef">478712274</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="4155993460">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2413364638</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="38722132">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3952975076">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Medium Friction</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="35535846" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2193805056">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2024274382">
                  <item dataType="ObjectRef">478712274</item>
                  <item dataType="ObjectRef">4155993460</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">478712274</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3602483612">AUIzAuoCP0e7JY+svWypoA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4038448178">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3840599106">
              <_items dataType="Array" type="Duality.GameObject[]" id="2817361936" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1617479799">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2342192051">
                    <_items dataType="Array" type="Duality.Component[]" id="662864422" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3977794731">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1617479799</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2103795814">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">4038448178</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3360108621">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">1617479799</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Int">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1814966205">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1466248230">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">The constrained body is now barely moveable.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3288697016" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="526832089">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2478508544">
                        <item dataType="ObjectRef">3977794731</item>
                        <item dataType="ObjectRef">3360108621</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3977794731</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2443093147">mIyIL+CniUStuejgQbzlZg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">4038448178</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4178496522">
              <_items dataType="Array" type="Duality.Component[]" id="1962408216" length="4">
                <item dataType="ObjectRef">2103795814</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1486109704">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">4038448178</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3235787208">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="791697004">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">High Friction</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1968528434" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3881479072">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3267102350">
                  <item dataType="ObjectRef">2103795814</item>
                  <item dataType="ObjectRef">1486109704</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2103795814</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3760550844">4J7xoERCGUqdFRJr9jIlyw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1982865461">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1022430137">
              <_items dataType="Array" type="Duality.GameObject[]" id="2560714958" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1272151281">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3775397569">
                    <_items dataType="Array" type="Duality.Component[]" id="3250239150" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3632466213">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1272151281</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="48213097">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">1982865461</gameobj>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3014780103">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">1272151281</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Int">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2435785655">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1330911630">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">Friction can also counter relative movement of two connected bodies.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="992067296" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2718769931">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1509866696">
                        <item dataType="ObjectRef">3632466213</item>
                        <item dataType="ObjectRef">3014780103</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3632466213</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="984895169">OzyUN0fco0ClRqAvp1TSaQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">1982865461</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3399631104">
              <_items dataType="Array" type="Duality.Component[]" id="43621395" length="4">
                <item dataType="ObjectRef">48213097</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3725494283">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1982865461</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="4032525641">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2274773902">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Relative Friction</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1823598139" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1629635860">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3112331574">
                  <item dataType="ObjectRef">48213097</item>
                  <item dataType="ObjectRef">3725494283</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">48213097</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2291025840">NWTSheKogUSQ7Fu7LSgUgQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">1934471966</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3823579905">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1402362877">
              <_items dataType="Array" type="Duality.GameObject[]" id="1109286182" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3702496940">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2503485128">
                    <_items dataType="Array" type="Duality.Component[]" id="2400992876" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1767844576">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">3702496940</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1888927541">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">3823579905</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <parentTransform />
                          <pos dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
                            <Y dataType="Float">-128</Y>
                            <Z dataType="Float">-1</Z>
                          </pos>
                          <posAbs dataType="Struct" type="Duality.Vector3">
                            <X dataType="Float">768</X>
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
                          <X dataType="Float">768</X>
                          <Y dataType="Float">-208</Y>
                          <Z dataType="Float">1</Z>
                        </posAbs>
                        <scale dataType="Float">0.6666667</scale>
                        <scaleAbs dataType="Float">0.5</scaleAbs>
                        <vel dataType="Struct" type="Duality.Vector3" />
                        <velAbs dataType="Struct" type="Duality.Vector3" />
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1150158466">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">3702496940</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Int">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2771638802">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1304929872">
                            <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                              <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                            </item>
                          </fonts>
                          <icons />
                          <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                          <maxHeight dataType="Int">0</maxHeight>
                          <maxWidth dataType="Int">350</maxWidth>
                          <sourceText dataType="String">It is also possible to simulate friction for a specific point on the body.</sourceText>
                          <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                        </text>
                        <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2344949470" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2973428106">
                        <item dataType="ObjectRef">2421658734</item>
                        <item dataType="ObjectRef">1098533110</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1477536026">
                        <item dataType="ObjectRef">1767844576</item>
                        <item dataType="ObjectRef">1150158466</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1767844576</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2040661610">p9Ol2wrLpk2G4sxSMiYCrQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">3823579905</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3186824120">
              <_items dataType="Array" type="Duality.Component[]" id="842925207" length="4">
                <item dataType="ObjectRef">1888927541</item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="1271241431">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3823579905</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Int">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3746092061">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="522672358">
                      <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                        <contentPath dataType="String">Data\PhysicsSample\Content\SourceSansProRegular28.Font.res</contentPath>
                      </item>
                    </fonts>
                    <icons />
                    <lineAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                    <maxHeight dataType="Int">0</maxHeight>
                    <maxWidth dataType="Int">150</maxWidth>
                    <sourceText dataType="String">Anchored Position</sourceText>
                    <wrapMode dataType="Enum" type="Duality.Drawing.FormattedText+WrapMode" name="Word" value="1" />
                  </text>
                  <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4101213911" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3042525652">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">1098533110</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3671741366">
                  <item dataType="ObjectRef">1888927541</item>
                  <item dataType="ObjectRef">1271241431</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1888927541</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2600701168">8Hz+xBncH0+JlPsdLxWc7g==</data>
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
    <item dataType="ObjectRef">3771095618</item>
    <item dataType="ObjectRef">190303241</item>
    <item dataType="ObjectRef">1570063134</item>
    <item dataType="ObjectRef">289270660</item>
    <item dataType="ObjectRef">1974999523</item>
    <item dataType="ObjectRef">228442758</item>
    <item dataType="ObjectRef">1419031358</item>
    <item dataType="ObjectRef">302638256</item>
    <item dataType="ObjectRef">2413364638</item>
    <item dataType="ObjectRef">4038448178</item>
    <item dataType="ObjectRef">1982865461</item>
    <item dataType="ObjectRef">3823579905</item>
    <item dataType="ObjectRef">553381659</item>
    <item dataType="ObjectRef">1536454540</item>
    <item dataType="ObjectRef">2083943057</item>
    <item dataType="ObjectRef">3728469028</item>
    <item dataType="ObjectRef">2752104523</item>
    <item dataType="ObjectRef">2633694611</item>
    <item dataType="ObjectRef">3614180199</item>
    <item dataType="ObjectRef">1617479799</item>
    <item dataType="ObjectRef">1272151281</item>
    <item dataType="ObjectRef">3702496940</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
