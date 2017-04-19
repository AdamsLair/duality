<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <assetInfo />
  <globalGravity dataType="Struct" type="Duality.Vector2" />
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
              <_version dataType="Int">2</_version>
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
        <_version dataType="Int">103</_version>
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
    <item dataType="Struct" type="Duality.GameObject" id="2091764498">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="2774523900">
        <_items dataType="Array" type="Duality.GameObject[]" id="520565572" length="16">
          <item dataType="Struct" type="Duality.GameObject" id="553381659">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4214270007">
              <_items dataType="Array" type="Duality.Component[]" id="1031811726" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2913696591">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">553381659</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-256</Y>
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
                  <joints />
                  <linearDamp dataType="Float">10</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2405492503">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="465075982" length="4">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1757566416">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3616158183</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">15</_version>
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
              <_version dataType="Int">103</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2033139264" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2521587837">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="Type" id="3054665510" value="Duality.Components.Physics.RigidBody" />
                  <item dataType="Type" id="2535615162" value="Duality.Components.Renderers.RigidBodyRenderer" />
                </keys>
                <values dataType="Array" type="System.Object[]" id="2275047864">
                  <item dataType="ObjectRef">2913696591</item>
                  <item dataType="ObjectRef">3616158183</item>
                  <item dataType="ObjectRef">1152074849</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2913696591</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1157226839">SqV/g4TJ0kqDSmpt6jIhgw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3227250136">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="917058128">
              <_items dataType="Array" type="Duality.Component[]" id="502316476" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1292597772">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3227250136</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-80</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-80</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1995059364">
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
                  <gameobj dataType="ObjectRef">3227250136</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">5</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2593011172">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2207941060">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="594135364">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">1995059364</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="3825943326">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">26</B>
                    <G dataType="Byte">197</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">3227250136</gameobj>
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
              <_version dataType="Int">53</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2583936366" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1347479330">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="4013825802">
                  <item dataType="ObjectRef">1292597772</item>
                  <item dataType="ObjectRef">1995059364</item>
                  <item dataType="ObjectRef">3825943326</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1292597772</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3909225682">yKmEDt8iOUSDUuxXzQK8KQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="439923086">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3056179318">
              <_items dataType="Array" type="Duality.Component[]" id="1702182880" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2800238018">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">439923086</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-32</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-32</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3502699610">
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
                  <gameobj dataType="ObjectRef">439923086</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">2</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="756303474">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4083286224">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="4018531004">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3502699610</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1038616276">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">51</B>
                    <G dataType="Byte">203</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">439923086</gameobj>
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
              <_version dataType="Int">53</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3637395738" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="390334276">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3205521046">
                  <item dataType="ObjectRef">2800238018</item>
                  <item dataType="ObjectRef">3502699610</item>
                  <item dataType="ObjectRef">1038616276</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2800238018</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4255883008">5fjZeOGHp0mKk+VnrJr3zA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="1159983257">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1718323557">
              <_items dataType="Array" type="Duality.Component[]" id="1255889814" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3520298189">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">1159983257</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">16</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">16</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="4222759781">
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
                  <gameobj dataType="ObjectRef">1159983257</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">1</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="775378485">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1145388534">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1089879776">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">4222759781</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1758676447">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">76</B>
                    <G dataType="Byte">209</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">1159983257</gameobj>
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
              <_version dataType="Int">53</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1932994152" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1948838415">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="121952224">
                  <item dataType="ObjectRef">3520298189</item>
                  <item dataType="ObjectRef">4222759781</item>
                  <item dataType="ObjectRef">1758676447</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3520298189</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="4234979165">nbfrFsiMPkeQZt/j+gu7Og==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="120810273">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3890123133">
              <_items dataType="Array" type="Duality.Component[]" id="2275367718" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2481125205">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">120810273</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">64</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">64</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3183586797">
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
                  <gameobj dataType="ObjectRef">120810273</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.5</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="514704477">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="4111098726">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2974230400">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3183586797</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="719503463">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">102</B>
                    <G dataType="Byte">216</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">120810273</gameobj>
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
              <_version dataType="Int">53</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3714131384" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2966205975">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1306715328">
                  <item dataType="ObjectRef">2481125205</item>
                  <item dataType="ObjectRef">3183586797</item>
                  <item dataType="ObjectRef">719503463</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2481125205</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1774355381">cCyDurCHdUeSzjpIVKZXJA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="69165037">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1776893313">
              <_items dataType="Array" type="Duality.Component[]" id="3569999662" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2429479969">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">69165037</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">112</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">112</X>
                    <Y dataType="Float">-256</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3131941561">
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
                  <gameobj dataType="ObjectRef">69165037</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2" />
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3176072137">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="241320590">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3791386832">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3131941561</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.3</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="667858227">
                  <active dataType="Bool">true</active>
                  <areaMaterial dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                    <contentPath dataType="String">Data\PhysicsSample\Content\SolidGrey.Material.res</contentPath>
                  </areaMaterial>
                  <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">128</B>
                    <G dataType="Byte">222</G>
                    <R dataType="Byte">255</R>
                  </colorTint>
                  <customAreaMaterial />
                  <customOutlineMaterial />
                  <fillHollowShapes dataType="Bool">false</fillHollowShapes>
                  <gameobj dataType="ObjectRef">69165037</gameobj>
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
              <_version dataType="Int">53</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4231083360" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="1274972747">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="859680584">
                  <item dataType="ObjectRef">2429479969</item>
                  <item dataType="ObjectRef">3131941561</item>
                  <item dataType="ObjectRef">667858227</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2429479969</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="888206081">EBA7viQ20U+SSIx/WjVaLA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="673903748">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1106104452">
              <_items dataType="Array" type="Duality.Component[]" id="316152900" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3034218680">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">673903748</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-128</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3736680272">
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
                  <gameobj dataType="ObjectRef">673903748</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">20</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2947975576">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="342203436">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3220587748">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3736680272</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.9</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1272596938">
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
                  <gameobj dataType="ObjectRef">673903748</gameobj>
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
              <_version dataType="Int">45</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="443564182" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="906880142">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3148280906">
                  <item dataType="ObjectRef">3034218680</item>
                  <item dataType="ObjectRef">3736680272</item>
                  <item dataType="ObjectRef">1272596938</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3034218680</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1620545470">W7BrokHb1ESeNlENJzyJCw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3585486453">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1868219737">
              <_items dataType="Array" type="Duality.Component[]" id="861688782" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1650834089">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3585486453</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-80</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-80</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2353295681">
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
                  <gameobj dataType="ObjectRef">3585486453</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">20</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3811893745">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2755302318">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="2908604752">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2353295681</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.9</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="4184179643">
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
                  <gameobj dataType="ObjectRef">3585486453</gameobj>
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
              <_version dataType="Int">43</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1359616000" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3040411507">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="3522744248">
                  <item dataType="ObjectRef">1650834089</item>
                  <item dataType="ObjectRef">2353295681</item>
                  <item dataType="ObjectRef">4184179643</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1650834089</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="1923087513">e2ZyfsRXv0im6fw/s6I8sQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="935545938">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3890917890">
              <_items dataType="Array" type="Duality.Component[]" id="1430327696" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3295860870">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">935545938</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-32</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">-32</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3998322462">
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
                  <gameobj dataType="ObjectRef">935545938</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">20</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2822860254">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1499800848">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1536346940">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3998322462</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.9</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1534239128">
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
                  <gameobj dataType="ObjectRef">935545938</gameobj>
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
              <_version dataType="Int">43</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1836355466" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="2407641560">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1363414686">
                  <item dataType="ObjectRef">3295860870</item>
                  <item dataType="ObjectRef">3998322462</item>
                  <item dataType="ObjectRef">1534239128</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3295860870</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="145726852">zwj7bzjQ1USxFIgvLYTeSA==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="723955831">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2674930347">
              <_items dataType="Array" type="Duality.Component[]" id="974266614" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3084270763">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">723955831</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">16</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">16</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3786732355">
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
                  <gameobj dataType="ObjectRef">723955831</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">20</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="486336371">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1726122278">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="1167869184">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3786732355</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.9</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1322649021">
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
                  <gameobj dataType="ObjectRef">723955831</gameobj>
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
              <_version dataType="Int">43</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="4195885384" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3823268225">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="1942054240">
                  <item dataType="ObjectRef">3084270763</item>
                  <item dataType="ObjectRef">3786732355</item>
                  <item dataType="ObjectRef">1322649021</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3084270763</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2564087507">Uf5q4UznA06b5y0G+oi4JQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3794232846">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="140628726">
              <_items dataType="Array" type="Duality.Component[]" id="967915744" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1859580482">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3794232846</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">64</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">64</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2562042074">
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
                  <gameobj dataType="ObjectRef">3794232846</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">20</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2049664754">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3217192400">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3745459900">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">2562042074</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.9</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="97958740">
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
                  <gameobj dataType="ObjectRef">3794232846</gameobj>
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
              <_version dataType="Int">43</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="240353818" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="897043908">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="4271909270">
                  <item dataType="ObjectRef">1859580482</item>
                  <item dataType="ObjectRef">2562042074</item>
                  <item dataType="ObjectRef">97958740</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1859580482</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="372309120">GihymFXhpU6hr5sVo01+sQ==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="912894047">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1912203779">
              <_items dataType="Array" type="Duality.Component[]" id="362696998" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="3273208979">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">912894047</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <parentTransform />
                  <pos dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">112</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </pos>
                  <posAbs dataType="Struct" type="Duality.Vector3">
                    <X dataType="Float">112</X>
                    <Y dataType="Float">-640</Y>
                    <Z dataType="Float">0</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="Duality.Vector3" />
                  <velAbs dataType="Struct" type="Duality.Vector3" />
                </item>
                <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="3975670571">
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
                  <gameobj dataType="ObjectRef">912894047</gameobj>
                  <ignoreGravity dataType="Bool">false</ignoreGravity>
                  <joints />
                  <linearDamp dataType="Float">0.3</linearDamp>
                  <linearVel dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">20</Y>
                  </linearVel>
                  <revolutions dataType="Float">0</revolutions>
                  <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="3353540091">
                    <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="3817552470">
                      <item dataType="Struct" type="Duality.Components.Physics.CircleShapeInfo" id="3924391968">
                        <density dataType="Float">1</density>
                        <friction dataType="Float">0.3</friction>
                        <parent dataType="ObjectRef">3975670571</parent>
                        <position dataType="Struct" type="Duality.Vector2" />
                        <radius dataType="Float">16</radius>
                        <restitution dataType="Float">0.9</restitution>
                        <sensor dataType="Bool">false</sensor>
                      </item>
                    </_items>
                    <_size dataType="Int">1</_size>
                    <_version dataType="Int">1</_version>
                  </shapes>
                </item>
                <item dataType="Struct" type="Duality.Components.Renderers.RigidBodyRenderer" id="1511587237">
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
                  <gameobj dataType="ObjectRef">912894047</gameobj>
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
              <_version dataType="Int">43</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1915377592" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Object[]" id="3665681769">
                  <item dataType="ObjectRef">2421658734</item>
                  <item dataType="ObjectRef">3054665510</item>
                  <item dataType="ObjectRef">2535615162</item>
                </keys>
                <values dataType="Array" type="System.Object[]" id="622480064">
                  <item dataType="ObjectRef">3273208979</item>
                  <item dataType="ObjectRef">3975670571</item>
                  <item dataType="ObjectRef">1511587237</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">3273208979</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="2814623179">G+xu7gdIkESpG5AK20KH1A==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RigidBodyRenderer</name>
            <parent dataType="ObjectRef">2091764498</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">12</_size>
        <_version dataType="Int">34</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="64059286">
        <_items dataType="Array" type="Duality.Component[]" id="3137633622" length="0" />
        <_size dataType="Int">0</_size>
        <_version dataType="Int">0</_version>
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
    <item dataType="Struct" type="Duality.GameObject" id="956630495">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="245699357">
        <_items dataType="Array" type="Duality.Component[]" id="3841374438" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleController" id="2891787066">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">956630495</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1931241208" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3050672247">
            <item dataType="Type" id="1683129742" value="Duality.Samples.Physics.PhysicsSampleController" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="1984166208">
            <item dataType="ObjectRef">2891787066</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3536389333">Lk8/pTqrA0q/4zctYECx8g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleController</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2039052727">
        <changes />
        <obj dataType="ObjectRef">956630495</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleController.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1420527452">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3563312690">
        <_items dataType="Array" type="Duality.Component[]" id="1794435536" length="4">
          <item dataType="Struct" type="Duality.Samples.Physics.PhysicsSampleInfo" id="899662325">
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1420527452</gameobj>
          </item>
        </_items>
        <_size dataType="Int">1</_size>
        <_version dataType="Int">1</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1553972554" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Object[]" id="3640654120">
            <item dataType="Type" id="3609360300" value="Duality.Samples.Physics.PhysicsSampleInfo" />
          </keys>
          <values dataType="Array" type="System.Object[]" id="4123216030">
            <item dataType="ObjectRef">899662325</item>
          </values>
        </body>
      </compMap>
      <compTransform />
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="3062507028">RcyP7iSsqUunURZU1ta2sA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">PhysicsSampleInfo</name>
      <parent />
      <prefabLink dataType="Struct" type="Duality.Resources.PrefabLink" id="2050037378">
        <changes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Resources.PrefabLink+VarMod]]" id="902379168">
          <_items dataType="Array" type="Duality.Resources.PrefabLink+VarMod[]" id="1334258908" length="4">
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2998535624">
                <_items dataType="Array" type="System.Int32[]" id="1756595820"></_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">3609360300</componentType>
              <prop dataType="MemberInfo" id="1569488606" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleName" />
              <val dataType="String">Damping</val>
            </item>
            <item dataType="Struct" type="Duality.Resources.PrefabLink+VarMod">
              <childIndex dataType="Struct" type="System.Collections.Generic.List`1[[System.Int32]]" id="2569112628">
                <_items dataType="ObjectRef">1756595820</_items>
                <_size dataType="Int">0</_size>
                <_version dataType="Int">1</_version>
              </childIndex>
              <componentType dataType="ObjectRef">3609360300</componentType>
              <prop dataType="MemberInfo" id="2836872994" value="P:Duality.Samples.Physics.PhysicsSampleInfo:SampleDesc" />
              <val dataType="String">The /cFF8888FFDamping/cFFFFFFFF value of a body represents a constant force against its current velocity, slowing down its movement and rotation until it eventually stops. Higher damping values cause the body to stop faster.</val>
            </item>
          </_items>
          <_size dataType="Int">2</_size>
          <_version dataType="Int">448</_version>
        </changes>
        <obj dataType="ObjectRef">1420527452</obj>
        <prefab dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
          <contentPath dataType="String">Data\PhysicsSample\Content\PhysicsSampleInfo.Prefab.res</contentPath>
        </prefab>
      </prefabLink>
    </item>
    <item dataType="ObjectRef">553381659</item>
    <item dataType="ObjectRef">3227250136</item>
    <item dataType="ObjectRef">439923086</item>
    <item dataType="ObjectRef">1159983257</item>
    <item dataType="ObjectRef">120810273</item>
    <item dataType="ObjectRef">69165037</item>
    <item dataType="ObjectRef">673903748</item>
    <item dataType="ObjectRef">3585486453</item>
    <item dataType="ObjectRef">935545938</item>
    <item dataType="ObjectRef">723955831</item>
    <item dataType="ObjectRef">3794232846</item>
    <item dataType="ObjectRef">912894047</item>
  </serializeObj>
  <visibilityStrategy dataType="Struct" type="Duality.Components.DefaultRendererVisibilityStrategy" id="2035693768" />
</root>
<!-- XmlFormatterBase Document Separator -->
