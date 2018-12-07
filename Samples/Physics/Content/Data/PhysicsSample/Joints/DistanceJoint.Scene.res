<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="2150928809">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1278564395">
        <_items dataType="Array" type="Duality.Component[]" id="4166403062">
          <item dataType="Struct" type="Duality.Components.Transform" id="2208206027">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <gameobj dataType="ObjectRef">2150928809</gameobj>
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
          <item dataType="Struct" type="Duality.Components.Camera" id="3697315286">
            <active dataType="Bool">true</active>
            <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">203</B>
              <G dataType="Byte">149</G>
              <R dataType="Byte">90</R>
            </clearColor>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">2150928809</gameobj>
            <nearZ dataType="Float">50</nearZ>
            <priority dataType="Int">0</priority>
            <projection dataType="Enum" type="Duality.Drawing.ProjectionMode" name="Perspective" value="1" />
            <renderSetup dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderSetup]]" />
            <renderTarget dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]" />
            <shaderParameters dataType="Struct" type="Duality.Drawing.ShaderParameterCollection" id="1612585914" custom="true">
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
          <item dataType="Struct" type="Duality.Components.VelocityTracker" id="4222063276">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2150928809</gameobj>
          </item>
          <item dataType="Struct" type="Duality.Components.SoundListener" id="4183581336">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2150928809</gameobj>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2576027720" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3095375617">
            <item dataType="Type" id="4256166190" value="Duality.Components.Transform" />
            <item dataType="Type" id="3033563338" value="Duality.Components.Camera" />
            <item dataType="Type" id="1265185950" value="Duality.Components.SoundListener" />
            <item dataType="Type" id="3302667482" value="Duality.Components.VelocityTracker" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="3559150432">
            <item dataType="ObjectRef">2208206027</item>
            <item dataType="ObjectRef">3697315286</item>
            <item dataType="ObjectRef">4183581336</item>
            <item dataType="ObjectRef">4222063276</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">2208206027</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1702062931">4P+437YFPkWJBfWaDOuoaQ==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Camera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2113205685">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1142515399">
        <_items dataType="Array" type="Duality.GameObject[]" id="915481294" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="565951191">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="110109735">
              <_items dataType="Array" type="Duality.Component[]" id="2628493774" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="623228409">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">565951191</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="100880679">
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
                  <gameobj dataType="ObjectRef">565951191</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3641167063">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2650771470">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="541154256">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="4101094076">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="10240580" length="4" />
                          <_size dataType="Int">0</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">100880679</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="1377649302">
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
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3619925149">
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
                  <gameobj dataType="ObjectRef">565951191</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3050750464" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3037519629">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="Type" id="1621942054" value="Duality.Components.Physics.RigidBody" />
                  <item dataType="Type" id="3537758906" value="Duality.Components.Renderers.RigidBodyRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="1252153784">
                  <item dataType="ObjectRef">623228409</item>
                  <item dataType="ObjectRef">100880679</item>
                  <item dataType="ObjectRef">3619925149</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">623228409</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1197062119">0RjXUxH2m0KqcEVxGQQHCA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Wall</name>
            <parent dataType="ObjectRef">2113205685</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2461455864">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1395913060">
              <_items dataType="Array" type="Duality.Component[]" id="173185988" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2518733082">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2461455864</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1996385352">
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
                  <gameobj dataType="ObjectRef">2461455864</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3559218496">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2118268188">
                      <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3273676740">
                        <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="3958115652">
                          <_items dataType="Array" type="Duality.Vector2[][]" id="2671235652" length="4" />
                          <_size dataType="Int">0</_size>
                        </convexPolygons>
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">1996385352</parent>
                        <restitution dataType="Float">0</restitution>
                        <sensor dataType="Bool">false</sensor>
                        <userTag dataType="Int">0</userTag>
                        <vertices dataType="Array" type="Duality.Vector2[]" id="2007839382">
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
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1220462526">
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
                  <gameobj dataType="ObjectRef">2461455864</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3077517846" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1032936494">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">1621942054</item>
                  <item dataType="ObjectRef">3537758906</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3685514442">
                  <item dataType="ObjectRef">2518733082</item>
                  <item dataType="ObjectRef">1996385352</item>
                  <item dataType="ObjectRef">1220462526</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2518733082</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2010361246">goeIM99X6Uuzs2+dAz51Yg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Wall</name>
            <parent dataType="ObjectRef">2113205685</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1964877568">
        <_items dataType="Array" type="Duality.Component[]" id="178663789" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1083673925" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3980068116" length="0" />
          <values dataType="Array" type="System.Object[]" id="2050058550" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="964691888">rXG8G092806WXYH/nsUtKw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Room</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="5037571">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3827611569">
        <_items dataType="Array" type="Duality.GameObject[]" id="847241774" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="2876709606">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2136739290">
              <_items dataType="Array" type="Duality.GameObject[]" id="268214528" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2514324034">
                  <active dataType="Bool">true</active>
                  <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1915827578">
                    <_items dataType="Array" type="Duality.GameObject[]" id="1217331072" length="4" />
                    <_size dataType="Int">0</_size>
                  </children>
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="466214714">
                    <_items dataType="Array" type="Duality.Component[]" id="2001569984" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2571601252">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2514324034</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2049253522">
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
                        <gameobj dataType="ObjectRef">2514324034</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="599075242">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="4241633312" length="4">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="1335516124">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">1</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2411639094">
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
                                <gameobj dataType="ObjectRef">2876709606</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="4177005614">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3250697040" length="4">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2440303548">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">2411639094</parent>
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
                              <parentBody dataType="ObjectRef">2049253522</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3694186202">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="659047312" length="4">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="171661628">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1380691780">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="4250197572" length="4" />
                                <_size dataType="Int">0</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2049253522</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="1969737366">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1273330696">
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
                        <gameobj dataType="ObjectRef">2514324034</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4009730554" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2605812480">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">1621942054</item>
                        <item dataType="ObjectRef">3537758906</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3652846030">
                        <item dataType="ObjectRef">2571601252</item>
                        <item dataType="ObjectRef">2049253522</item>
                        <item dataType="ObjectRef">1273330696</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2571601252</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="602625436">SqV/g4TJ0kqDSmpt6jIhgw==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">2876709606</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3808564922">
              <_items dataType="Array" type="Duality.Component[]" id="438019104" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2933986824">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2876709606</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="ObjectRef">2411639094</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1635716268">
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
                  <gameobj dataType="ObjectRef">2876709606</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3463426778" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3388104192">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">1621942054</item>
                  <item dataType="ObjectRef">3537758906</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2898509774">
                  <item dataType="ObjectRef">2933986824</item>
                  <item dataType="ObjectRef">2411639094</item>
                  <item dataType="ObjectRef">1635716268</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2933986824</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1470291612">GAIJR5xV4UGA3U9K9qa05w==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">5037571</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="439121459">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1659145811">
              <_items dataType="Array" type="Duality.GameObject[]" id="2302041446" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="3510759400">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3222054468">
                    <_items dataType="Array" type="Duality.Component[]" id="1631984196" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3568036618">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">3510759400</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3045688888">
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
                        <gameobj dataType="ObjectRef">3510759400</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="3922232816">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="641258300">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="118957892">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">10</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4269018243">
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
                                <gameobj dataType="ObjectRef">439121459</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="285166319">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2080450286">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2217103440">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">4269018243</parent>
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
                              <parentBody dataType="ObjectRef">3045688888</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1722333422">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1975038786">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="473009168">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="4193408828">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3543119684" length="4" />
                                <_size dataType="Int">0</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">3045688888</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="1270442902">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="2269766062">
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
                        <gameobj dataType="ObjectRef">3510759400</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2657131158" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2080935886">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">1621942054</item>
                        <item dataType="ObjectRef">3537758906</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1123218762">
                        <item dataType="ObjectRef">3568036618</item>
                        <item dataType="ObjectRef">3045688888</item>
                        <item dataType="ObjectRef">2269766062</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3568036618</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2673866622">sM3SHOAEIEOoZif9s8IvhA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">439121459</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2337982840">
              <_items dataType="Array" type="Duality.Component[]" id="581069625" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="496398677">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">439121459</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <pos dataType="Struct" type="Duality.Vector3" />
                  <posAbs dataType="Struct" type="Duality.Vector3" />
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                </item>
                <item dataType="ObjectRef">4269018243</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3493095417">
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
                  <gameobj dataType="ObjectRef">439121459</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1194680889" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="4130900820">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">1621942054</item>
                  <item dataType="ObjectRef">3537758906</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="915441078">
                  <item dataType="ObjectRef">496398677</item>
                  <item dataType="ObjectRef">4269018243</item>
                  <item dataType="ObjectRef">3493095417</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">496398677</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="201965680">arL59cVQFEKBeRbmKm9s/w==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">5037571</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1419768304">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3188333852">
              <_items dataType="Array" type="Duality.GameObject[]" id="4206675908" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2165191949">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2777784673">
                    <_items dataType="Array" type="Duality.Component[]" id="2168513390" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2222469167">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2165191949</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1700121437">
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
                        <gameobj dataType="ObjectRef">2165191949</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="3910787485">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="3738424038">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="2584698240">
                              <breakPoint dataType="Float">20</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">5</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="954697792">
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
                                <gameobj dataType="ObjectRef">1419768304</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1329013136">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2053202236">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2435493700">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">954697792</parent>
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
                              <parentBody dataType="ObjectRef">1700121437</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1473684728">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1396524791">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1942088334">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="566282448">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="593906364" length="4" />
                                <_size dataType="Int">0</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">1700121437</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="383926894">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="924198611">
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
                        <gameobj dataType="ObjectRef">2165191949</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3691087392" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="1349574379">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">1621942054</item>
                        <item dataType="ObjectRef">3537758906</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="923835080">
                        <item dataType="ObjectRef">2222469167</item>
                        <item dataType="ObjectRef">1700121437</item>
                        <item dataType="ObjectRef">924198611</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2222469167</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2311439841">GYmAmYR44kGU5ycvcjcpzA==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">1419768304</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1131834902">
              <_items dataType="Array" type="Duality.Component[]" id="369035190" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1477045522">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1419768304</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="ObjectRef">954697792</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="178774966">
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
                  <gameobj dataType="ObjectRef">1419768304</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2011524744" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="241290904">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">1621942054</item>
                  <item dataType="ObjectRef">3537758906</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2761265438">
                  <item dataType="ObjectRef">1477045522</item>
                  <item dataType="ObjectRef">954697792</item>
                  <item dataType="ObjectRef">178774966</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1477045522</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="368856260">14gLkUnmikWNBiFySM7CjQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">5037571</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2653999034">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1500691550">
              <_items dataType="Array" type="Duality.GameObject[]" id="2435427600" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2903145049">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="871468157">
                    <_items dataType="Array" type="Duality.Component[]" id="2867957542" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2960422267">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2903145049</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2438074537">
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
                        <gameobj dataType="ObjectRef">2903145049</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="2908181673">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="2895287310">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="2704188368">
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
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2188928522">
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
                                <gameobj dataType="ObjectRef">2653999034</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="825707746">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2995250320">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2079208764">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">2188928522</parent>
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
                              <parentBody dataType="ObjectRef">2438074537</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2367697344">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2891867683">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2165688422">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="497198976">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3749183900" length="4" />
                                <_size dataType="Int">0</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2438074537</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="1922893006">
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
                            <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2957117242">
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2438074537</parent>
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1662151711">
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
                        <gameobj dataType="ObjectRef">2903145049</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3302271416" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="687332631">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">1621942054</item>
                        <item dataType="ObjectRef">3537758906</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1074117824">
                        <item dataType="ObjectRef">2960422267</item>
                        <item dataType="ObjectRef">2438074537</item>
                        <item dataType="ObjectRef">1662151711</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2960422267</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3597367477">WRr0NTBKXUGLYiX88z+IyQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">2653999034</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3159319818">
              <_items dataType="Array" type="Duality.Component[]" id="3466539900" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2711276252">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2653999034</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="ObjectRef">2188928522</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1413005696">
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
                  <gameobj dataType="ObjectRef">2653999034</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1746563502" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1103619360">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">1621942054</item>
                  <item dataType="ObjectRef">3537758906</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="4276207502">
                  <item dataType="ObjectRef">2711276252</item>
                  <item dataType="ObjectRef">2188928522</item>
                  <item dataType="ObjectRef">1413005696</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2711276252</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="101170236">+oz0WzOYpE2mzpkDA5nlAA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">5037571</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="742403219">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="243683443">
              <_items dataType="Array" type="Duality.GameObject[]" id="2580861222" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2921420545">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3830323393">
                    <_items dataType="Array" type="Duality.Component[]" id="2145096366" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2978697763">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2921420545</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2456350033">
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
                        <gameobj dataType="ObjectRef">2921420545</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="177789537">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="1674842478">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="62699600">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">2</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="Struct" type="Duality.Components.Physics.RigidBody" id="277332707">
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
                                <gameobj dataType="ObjectRef">742403219</gameobj>
                                <ignoreGravity dataType="Bool">false</ignoreGravity>
                                <joints />
                                <linearDamp dataType="Float">0.3</linearDamp>
                                <linearVel dataType="Struct" type="Duality.Vector2" />
                                <revolutions dataType="Float">0</revolutions>
                                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2013155895">
                                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2867873422">
                                    <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2589163728">
                                      <density dataType="Float">1</density>
                                      <friction dataType="Float">0.3</friction>
                                      <parent dataType="ObjectRef">277332707</parent>
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
                              <parentBody dataType="ObjectRef">2456350033</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="750037024">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2801708011" length="2">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="311082614">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="1720858592">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="3679857628" length="4" />
                                <_size dataType="Int">0</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2456350033</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="4049179534">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1680427207">
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
                        <gameobj dataType="ObjectRef">2921420545</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="592145120" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="181948683">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">1621942054</item>
                        <item dataType="ObjectRef">3537758906</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="505411784">
                        <item dataType="ObjectRef">2978697763</item>
                        <item dataType="ObjectRef">2456350033</item>
                        <item dataType="ObjectRef">1680427207</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2978697763</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3562672321">T9zczqveMEOTX8esHi7C7Q==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">742403219</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="2628232989">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4091544973">
                    <_items dataType="Array" type="Duality.Component[]" id="3045840166" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2685510207">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2628232989</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2163162477">
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
                        <gameobj dataType="ObjectRef">2628232989</gameobj>
                        <ignoreGravity dataType="Bool">false</ignoreGravity>
                        <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="1014066141">
                          <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="1376906342">
                            <item dataType="Struct" type="Duality.Components.Physics.DistanceJointInfo" id="3077347200">
                              <breakPoint dataType="Float">-1</breakPoint>
                              <collide dataType="Bool">true</collide>
                              <dampingRatio dataType="Float">0.5</dampingRatio>
                              <enabled dataType="Bool">true</enabled>
                              <frequency dataType="Float">2</frequency>
                              <length dataType="Float">128</length>
                              <localAnchorA dataType="Struct" type="Duality.Vector2" />
                              <localAnchorB dataType="Struct" type="Duality.Vector2" />
                              <otherBody dataType="ObjectRef">2456350033</otherBody>
                              <parentBody dataType="ObjectRef">2163162477</parentBody>
                            </item>
                          </_items>
                          <_size dataType="Int">1</_size>
                        </joints>
                        <linearDamp dataType="Float">0.3</linearDamp>
                        <linearVel dataType="Struct" type="Duality.Vector2" />
                        <revolutions dataType="Float">0</revolutions>
                        <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2557891704">
                          <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="368806583">
                            <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="2495105934">
                              <convexPolygons dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Vector2[]]]" id="511022288">
                                <_items dataType="Array" type="Duality.Vector2[][]" id="55376572" length="4" />
                                <_size dataType="Int">0</_size>
                              </convexPolygons>
                              <density dataType="Float">1</density>
                              <friction dataType="Float">0.3</friction>
                              <parent dataType="ObjectRef">2163162477</parent>
                              <restitution dataType="Float">0.3</restitution>
                              <sensor dataType="Bool">false</sensor>
                              <userTag dataType="Int">0</userTag>
                              <vertices dataType="Array" type="Duality.Vector2[]" id="796840558">
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
                      <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1387239651">
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
                        <gameobj dataType="ObjectRef">2628232989</gameobj>
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3603647416" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3985938151">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">1621942054</item>
                        <item dataType="ObjectRef">3537758906</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="131444608">
                        <item dataType="ObjectRef">2685510207</item>
                        <item dataType="ObjectRef">2163162477</item>
                        <item dataType="ObjectRef">1387239651</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2685510207</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3671919781">10U5zb+ppk6c8ZSeRwMvww==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Box</name>
                  <parent dataType="ObjectRef">742403219</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4213954488">
              <_items dataType="Array" type="Duality.Component[]" id="2182601497" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="799680437">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">742403219</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="ObjectRef">277332707</item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3796377177">
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
                  <gameobj dataType="ObjectRef">742403219</gameobj>
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3445301657" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3330643476">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">1621942054</item>
                  <item dataType="ObjectRef">3537758906</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2169726774">
                  <item dataType="ObjectRef">799680437</item>
                  <item dataType="ObjectRef">277332707</item>
                  <item dataType="ObjectRef">3796377177</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">799680437</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3678310064">vVxGOylZ+EWZv5VStVBK+A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Anchor</name>
            <parent dataType="ObjectRef">5037571</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3794879584">
        <_items dataType="Array" type="Duality.Component[]" id="169867931" length="4" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1792707171" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="58091428" length="0" />
          <values dataType="Array" type="System.Object[]" id="2131570454" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1222096544">T4kmnIvOvEK63C7yAxYu4Q==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Objects</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1997834671">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2272391757">
        <_items dataType="Array" type="Duality.Component[]" id="2390512166" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleController" id="787551588">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1997834671</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1065662648" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1814470695">
            <item dataType="Type" id="4158167502" value="Duality.Samples.Physics.PhysicsSampleController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="4282169856">
            <item dataType="ObjectRef">787551588</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="751962469">I3eAE1F/fE21FDEvpvD0ng==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleController</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="1862942247">
        <changes />
        <obj dataType="ObjectRef">1997834671</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleController.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3546873502">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="13668360">
        <_items dataType="Array" type="Duality.GameObject[]" id="3039555948" length="8">
          <item dataType="Struct" type="Duality.GameObject" id="1299465844">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1242838228">
              <_items dataType="Array" type="Duality.GameObject[]" id="2405868260" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="4054600094">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="710205702">
                    <_items dataType="Array" type="Duality.Component[]" id="3165247872" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="4111877312">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">4054600094</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="642570452">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">4054600094</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1786723132">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1534465860">
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1697111354" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2735733876">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="Type" id="3945990052" value="Duality.Components.Renderers.TextRenderer" />
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="2523291126">
                        <item dataType="ObjectRef">4111877312</item>
                        <item dataType="ObjectRef">642570452</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">4111877312</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="647372368">vrTfPSVpjEmlh8gzwqWqoQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">1299465844</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2330090934">
              <_items dataType="Array" type="Duality.Component[]" id="4014852222" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1356743062">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">1299465844</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2182403498">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">1299465844</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2093276462">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="2101000016">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3617239536" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3035288264">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">3945990052</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1312925406">
                  <item dataType="ObjectRef">1356743062</item>
                  <item dataType="ObjectRef">2182403498</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1356743062</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3985071924">za21tby7V0mGqhzFlz49Kg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">3546873502</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4224281640">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3422804864">
              <_items dataType="Array" type="Duality.GameObject[]" id="2485137820" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1615456303">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3213325715">
                    <_items dataType="Array" type="Duality.Component[]" id="236907750" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1672733521">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">1615456303</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2498393957">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">1615456303</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3254207637">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1385196662">
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3128290040" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="3435796473">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">3945990052</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="3328290944">
                        <item dataType="ObjectRef">1672733521</item>
                        <item dataType="ObjectRef">2498393957</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1672733521</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="2223347195">d0N77Y+9wEOTv9YJBhbW0w==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">4224281640</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3288339662">
              <_items dataType="Array" type="Duality.Component[]" id="2246370642" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="4281558858">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">4224281640</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="812251998">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">4224281640</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="174868530">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3493671376">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2445528348" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1358734264">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">3945990052</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2892886238">
                  <item dataType="ObjectRef">4281558858</item>
                  <item dataType="ObjectRef">812251998</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">4281558858</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2782738916">uCasgcKJ6E+GTTJm0q9XhA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">3546873502</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="2200981177">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="457847013">
              <_items dataType="Array" type="Duality.GameObject[]" id="860967062" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1202092558">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="876347250">
                    <_items dataType="Array" type="Duality.Component[]" id="387163344" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1259369776">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">1202092558</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2085030212">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">1202092558</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3222916140">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="4036699364">
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2832688202" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2430726504">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">3945990052</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="573051166">
                        <item dataType="ObjectRef">1259369776</item>
                        <item dataType="ObjectRef">2085030212</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1259369776</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="1529511892">Rr7CnKAYw0+0mkHR69Lvxg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">2200981177</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3534802792">
              <_items dataType="Array" type="Duality.Component[]" id="3517353359" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2258258395">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">2200981177</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3083918831">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">2200981177</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2511676517">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3913779094">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="204397615" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3202356452">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">3945990052</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3002261014">
                  <item dataType="ObjectRef">2258258395</item>
                  <item dataType="ObjectRef">3083918831</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2258258395</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="15882976">UzxuTPNmnkCk7IvB7GSc/Q==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">3546873502</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3738899078">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="235824542">
              <_items dataType="Array" type="Duality.GameObject[]" id="575434640" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="2181997663">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2023274075">
                    <_items dataType="Array" type="Duality.Component[]" id="2567262614" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2239274881">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">2181997663</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="3064935317">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">2181997663</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1295286693">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="1661469078">
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3539423336" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="2189914545">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">3945990052</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1148970080">
                        <item dataType="ObjectRef">2239274881</item>
                        <item dataType="ObjectRef">3064935317</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2239274881</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="4243858531">DXJgNe7LdE+YKyuaE41WvQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">3738899078</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3658654090">
              <_items dataType="Array" type="Duality.Component[]" id="2568481980" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3796176296">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3738899078</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="326869436">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3738899078</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="3937046572">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="4281380068">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1028991086" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2877709984">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">3945990052</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="2445902990">
                  <item dataType="ObjectRef">3796176296</item>
                  <item dataType="ObjectRef">326869436</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3796176296</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="458581692">BWoyG2PQN06HQjWc7qx1MQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">3546873502</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3753350229">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1396814297">
              <_items dataType="Array" type="Duality.GameObject[]" id="3473852878" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="1149910244">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3365366000">
                    <_items dataType="Array" type="Duality.Component[]" id="901904188" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="1207187462">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <gameobj dataType="ObjectRef">1149910244</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
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
                      </item>
                      <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="2032847898">
                        <active dataType="Bool">true</active>
                        <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                        <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">128</A>
                          <B dataType="Byte">0</B>
                          <G dataType="Byte">0</G>
                          <R dataType="Byte">0</R>
                        </colorTint>
                        <customMat />
                        <gameobj dataType="ObjectRef">1149910244</gameobj>
                        <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                        <offset dataType="Float">0</offset>
                        <text dataType="Struct" type="Duality.Drawing.FormattedText" id="2916606330">
                          <flowAreas />
                          <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="454381440">
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
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="555207918" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Object[]" id="159788098">
                        <item dataType="ObjectRef">4256166190</item>
                        <item dataType="ObjectRef">3945990052</item>
                      </keys>
                      <values dataType="Array" type="System.Object[]" id="1802333194">
                        <item dataType="ObjectRef">1207187462</item>
                        <item dataType="ObjectRef">2032847898</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">1207187462</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="958497842">ZSe/0Rbt8UKBX+y1WhYvtg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Description</name>
                  <parent dataType="ObjectRef">3753350229</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">1</_size>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3970796032">
              <_items dataType="Array" type="Duality.Component[]" id="3189882611" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3810627447">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <gameobj dataType="ObjectRef">3753350229</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
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
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.TextRenderer" id="341320587">
                  <active dataType="Bool">true</active>
                  <blockAlign dataType="Enum" type="Duality.Alignment" name="Center" value="0" />
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customMat />
                  <gameobj dataType="ObjectRef">3753350229</gameobj>
                  <iconMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]" />
                  <offset dataType="Float">0</offset>
                  <text dataType="Struct" type="Duality.Drawing.FormattedText" id="1365315465">
                    <flowAreas />
                    <fonts dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Font]][]" id="3335243150">
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
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3347072155" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2292682708">
                  <item dataType="ObjectRef">4256166190</item>
                  <item dataType="ObjectRef">3945990052</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="744543158">
                  <item dataType="ObjectRef">3810627447</item>
                  <item dataType="ObjectRef">341320587</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3810627447</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2937208560">SL41p+rc7UOaYCeD5FnsVA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Title</name>
            <parent dataType="ObjectRef">3546873502</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">5</_size>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3492852190">
        <_items dataType="Array" type="Duality.Component[]" id="2572390602" length="0" />
        <_size dataType="Int">0</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="312295412" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="1302673480" length="0" />
          <values dataType="Array" type="System.Object[]" id="1158101214" length="0" />
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3569491636">pPe2Tvus8ESFWZG38EwsSg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Descriptions</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="2374733344">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1045721766">
        <_items dataType="Array" type="Duality.Component[]" id="3147986432" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleInfo" id="320565617">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2374733344</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2903383994" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="4035350292">
            <item dataType="Type" id="1753931876" value="Duality.Samples.Physics.PhysicsSampleInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="2803564854">
            <item dataType="ObjectRef">320565617</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2283997616">Y9JycVV+pkSYpOK4kIvI4g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleInfo</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="3021118630">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="2522153984">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="222093468" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="3558419656">
                <_items dataType="Array" type="System.Int32[]" id="35107436"></_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">1753931876</componentType>
              <prop dataType="MemberInfo" id="2721503966" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleDesc" />
              <val dataType="String">The /cFF8888FFDistance Joint/cFFFFFFFF ensures that two points on different bodies remain at a constant distance.</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="342098228">
                <_items dataType="ObjectRef">35107436</_items>
                <_size dataType="Int">0</_size>
              </childIndex>
              <componentType dataType="ObjectRef">1753931876</componentType>
              <prop dataType="MemberInfo" id="1792306978" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleName" />
              <val dataType="String">Distance Joint</val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
        </changes>
        <obj dataType="ObjectRef">2374733344</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleInfo.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="ObjectRef">565951191</item>
    <item dataType="ObjectRef">2461455864</item>
    <item dataType="ObjectRef">2876709606</item>
    <item dataType="ObjectRef">439121459</item>
    <item dataType="ObjectRef">1419768304</item>
    <item dataType="ObjectRef">2653999034</item>
    <item dataType="ObjectRef">742403219</item>
    <item dataType="ObjectRef">1299465844</item>
    <item dataType="ObjectRef">4224281640</item>
    <item dataType="ObjectRef">2200981177</item>
    <item dataType="ObjectRef">3738899078</item>
    <item dataType="ObjectRef">3753350229</item>
    <item dataType="ObjectRef">2514324034</item>
    <item dataType="ObjectRef">3510759400</item>
    <item dataType="ObjectRef">2165191949</item>
    <item dataType="ObjectRef">2903145049</item>
    <item dataType="ObjectRef">2921420545</item>
    <item dataType="ObjectRef">2628232989</item>
    <item dataType="ObjectRef">4054600094</item>
    <item dataType="ObjectRef">1615456303</item>
    <item dataType="ObjectRef">1202092558</item>
    <item dataType="ObjectRef">2181997663</item>
    <item dataType="ObjectRef">1149910244</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
