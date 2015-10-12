<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <assetInfo dataType="Struct" type="Duality.Editor.AssetManagement.AssetInfo" id="427169525">
    <importerId />
    <nameHint dataType="String">Obstacle</nameHint>
  </assetInfo>
  <objTree dataType="Struct" type="Duality.GameObject" id="2250821630">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1705466537">
      <_items dataType="Array" type="Duality.GameObject[]" id="428782606" length="4">
        <item dataType="Struct" type="Duality.GameObject" id="3817728430">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="837800546">
            <_items dataType="Array" type="Duality.Component[]" id="1448808336" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="1883076066">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">3817728430</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="Struct" type="Duality.Components.Transform" id="316169266">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">2250821630</gameobj>
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
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="1164927702">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">3817728430</gameobj>
                <offset dataType="Int">0</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">400</H>
                  <W dataType="Float">100</W>
                  <X dataType="Float">-50</X>
                  <Y dataType="Float">-500</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2585537658">
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
                <gameobj dataType="ObjectRef">3817728430</gameobj>
                <ignoreGravity dataType="Bool">true</ignoreGravity>
                <joints />
                <linearDamp dataType="Float">0</linearDamp>
                <linearVel dataType="Struct" type="Duality.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </linearVel>
                <revolutions dataType="Float">0</revolutions>
                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1220184882">
                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="39469008" length="4">
                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1892223676">
                      <density dataType="Float">1</density>
                      <friction dataType="Float">0</friction>
                      <parent dataType="ObjectRef">2585537658</parent>
                      <restitution dataType="Float">0</restitution>
                      <sensor dataType="Bool">false</sensor>
                      <vertices dataType="Array" type="Duality.Vector2[]" id="2727502404">
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">-50</X>
                          <Y dataType="Float">-100</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">50</X>
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
                  <_version dataType="Int">7</_version>
                </shapes>
              </item>
            </_items>
            <_size dataType="Int">3</_size>
            <_version dataType="Int">3</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3267346826" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Object[]" id="4167554296">
                <item dataType="Type" id="1150004076" value="Duality.Components.Transform" />
                <item dataType="Type" id="12848182" value="Duality.Components.Renderers.SpriteRenderer" />
                <item dataType="Type" id="3628985912" value="Duality.Components.Physics.RigidBody" />
              </keys>
              <values dataType="Array" type="System.Object[]" id="1817319390">
                <item dataType="ObjectRef">1883076066</item>
                <item dataType="ObjectRef">1164927702</item>
                <item dataType="ObjectRef">2585537658</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">1883076066</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="3499814820">Rahs++GSlUGAIYqIjTmlfg==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">Top</name>
          <parent dataType="ObjectRef">2250821630</parent>
          <prefabLink />
        </item>
        <item dataType="Struct" type="Duality.GameObject" id="3215806995">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1300136819">
            <_items dataType="Array" type="Duality.Component[]" id="670343462" length="4">
              <item dataType="Struct" type="Duality.Components.Transform" id="1281154631">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">3215806995</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform dataType="ObjectRef">316169266</parentTransform>
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
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="563006267">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">255</G>
                  <R dataType="Byte">255</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">3215806995</gameobj>
                <offset dataType="Int">0</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">400</H>
                  <W dataType="Float">100</W>
                  <X dataType="Float">-50</X>
                  <Y dataType="Float">100</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Default:Material:SolidWhite</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1983616223">
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
                <gameobj dataType="ObjectRef">3215806995</gameobj>
                <ignoreGravity dataType="Bool">true</ignoreGravity>
                <joints />
                <linearDamp dataType="Float">0</linearDamp>
                <linearVel dataType="Struct" type="Duality.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </linearVel>
                <revolutions dataType="Float">0</revolutions>
                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1443060639">
                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1020229998" length="4">
                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3192252496">
                      <density dataType="Float">1</density>
                      <friction dataType="Float">0</friction>
                      <parent dataType="ObjectRef">1983616223</parent>
                      <restitution dataType="Float">0</restitution>
                      <sensor dataType="Bool">false</sensor>
                      <vertices dataType="Array" type="Duality.Vector2[]" id="2541173180">
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">-50</X>
                          <Y dataType="Float">100</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">50</X>
                          <Y dataType="Float">100</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">50</X>
                          <Y dataType="Float">500</Y>
                        </item>
                        <item dataType="Struct" type="Duality.Vector2">
                          <X dataType="Float">-50</X>
                          <Y dataType="Float">500</Y>
                        </item>
                      </vertices>
                    </item>
                  </_items>
                  <_size dataType="Int">1</_size>
                  <_version dataType="Int">7</_version>
                </shapes>
              </item>
            </_items>
            <_size dataType="Int">3</_size>
            <_version dataType="Int">3</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="984139704" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Object[]" id="2862704153">
                <item dataType="ObjectRef">1150004076</item>
                <item dataType="ObjectRef">12848182</item>
                <item dataType="ObjectRef">3628985912</item>
              </keys>
              <values dataType="Array" type="System.Object[]" id="686957440">
                <item dataType="ObjectRef">1281154631</item>
                <item dataType="ObjectRef">563006267</item>
                <item dataType="ObjectRef">1983616223</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">1281154631</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="2601191515">fRceuLNuEUWsUGmjW2/Xfw==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">Bottom</name>
          <parent dataType="ObjectRef">2250821630</parent>
          <prefabLink />
        </item>
      </_items>
      <_size dataType="Int">2</_size>
      <_version dataType="Int">2</_version>
    </children>
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="4168733120">
      <_items dataType="Array" type="Duality.Component[]" id="1222635555" length="4">
        <item dataType="ObjectRef">316169266</item>
        <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="1018630858">
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
          <gameobj dataType="ObjectRef">2250821630</gameobj>
          <ignoreGravity dataType="Bool">false</ignoreGravity>
          <joints />
          <linearDamp dataType="Float">0</linearDamp>
          <linearVel dataType="Struct" type="Duality.Vector2">
            <X dataType="Float">0</X>
            <Y dataType="Float">0</Y>
          </linearVel>
          <revolutions dataType="Float">0</revolutions>
          <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="1848559972">
            <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="2650409924" length="4">
              <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3640380740">
                <density dataType="Float">1</density>
                <friction dataType="Float">0</friction>
                <parent dataType="ObjectRef">1018630858</parent>
                <restitution dataType="Float">0</restitution>
                <sensor dataType="Bool">true</sensor>
                <vertices dataType="Array" type="Duality.Vector2[]" id="2694337092">
                  <item dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">45</X>
                    <Y dataType="Float">-100</Y>
                  </item>
                  <item dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">45</X>
                    <Y dataType="Float">100</Y>
                  </item>
                  <item dataType="Struct" type="Duality.Vector2">
                    <X dataType="Float">60</X>
                    <Y dataType="Float">0</Y>
                  </item>
                </vertices>
              </item>
            </_items>
            <_size dataType="Int">1</_size>
            <_version dataType="Int">3</_version>
          </shapes>
        </item>
        <item dataType="Struct" type="FlapOrDie.Tags.Obstacle" id="362926354">
          <active dataType="Bool">true</active>
          <gameobj dataType="ObjectRef">2250821630</gameobj>
        </item>
      </_items>
      <_size dataType="Int">3</_size>
      <_version dataType="Int">5</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="671669387" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Object[]" id="3101390004">
          <item dataType="ObjectRef">1150004076</item>
          <item dataType="ObjectRef">3628985912</item>
          <item dataType="Type" id="3614203300" value="FlapOrDie.Tags.Obstacle" />
        </keys>
        <values dataType="Array" type="System.Object[]" id="1020735478">
          <item dataType="ObjectRef">316169266</item>
          <item dataType="ObjectRef">1018630858</item>
          <item dataType="ObjectRef">362926354</item>
        </values>
      </body>
    </compMap>
    <compTransform dataType="ObjectRef">316169266</compTransform>
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="464872208">iodM6WSZDUil1vamRX8ueQ==</data>
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
