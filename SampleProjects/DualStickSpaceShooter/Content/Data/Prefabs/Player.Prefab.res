<root dataType="Struct" type="Duality.Resources.Prefab" id="129723834">
  <objTree dataType="Struct" type="Duality.GameObject" id="1121329689">
    <active dataType="Bool">true</active>
    <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="4151978834">
      <_items dataType="Array" type="Duality.GameObject[]" id="361734480" length="4">
        <item dataType="Struct" type="Duality.GameObject" id="3475034345">
          <active dataType="Bool">true</active>
          <children />
          <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3154984749">
            <_items dataType="Array" type="Duality.Component[]" id="191170150">
              <item dataType="Struct" type="Duality.Components.Transform" id="1540381981">
                <active dataType="Bool">true</active>
                <angle dataType="Float">0</angle>
                <angleAbs dataType="Float">0</angleAbs>
                <angleVel dataType="Float">0</angleVel>
                <angleVelAbs dataType="Float">0</angleVelAbs>
                <deriveAngle dataType="Bool">true</deriveAngle>
                <gameobj dataType="ObjectRef">3475034345</gameobj>
                <ignoreParent dataType="Bool">false</ignoreParent>
                <parentTransform />
                <pos dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </pos>
                <posAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </posAbs>
                <scale dataType="Float">1</scale>
                <scaleAbs dataType="Float">1</scaleAbs>
                <vel dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </vel>
                <velAbs dataType="Struct" type="OpenTK.Vector3">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                  <Z dataType="Float">0</Z>
                </velAbs>
              </item>
              <item dataType="Struct" type="Duality.Components.Renderers.SpriteRenderer" id="822233617">
                <active dataType="Bool">true</active>
                <colorTint dataType="Struct" type="Duality.Drawing.ColorRgba">
                  <A dataType="Byte">255</A>
                  <B dataType="Byte">255</B>
                  <G dataType="Byte">160</G>
                  <R dataType="Byte">89</R>
                </colorTint>
                <customMat />
                <gameobj dataType="ObjectRef">3475034345</gameobj>
                <offset dataType="Int">0</offset>
                <pixelGrid dataType="Bool">false</pixelGrid>
                <rect dataType="Struct" type="Duality.Rect">
                  <H dataType="Float">30</H>
                  <W dataType="Float">24</W>
                  <X dataType="Float">-12</X>
                  <Y dataType="Float">-19</Y>
                </rect>
                <rectMode dataType="Enum" type="Duality.Components.Renderers.SpriteRenderer+UVMode" name="Stretch" value="0" />
                <sharedMat dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Material]]">
                  <contentPath dataType="String">Data\Materials\Player.Material.res</contentPath>
                </sharedMat>
                <visibilityGroup dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
              </item>
              <item dataType="Struct" type="Duality.Components.Physics.RigidBody" id="2242843573">
                <active dataType="Bool">true</active>
                <angularDamp dataType="Float">0.3</angularDamp>
                <angularVel dataType="Float">0</angularVel>
                <bodyType dataType="Enum" type="Duality.Components.Physics.BodyType" name="Dynamic" value="1" />
                <colCat dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat2" value="2" />
                <colWith dataType="Enum" type="Duality.Components.Physics.CollisionCategory" name="Cat1, Cat3, Cat4, Cat5, Cat6, Cat7, Cat8, Cat9, Cat10, Cat11, Cat12, Cat13, Cat14, Cat15, Cat16, Cat17, Cat18, Cat19, Cat20, Cat21, Cat22, Cat23, Cat24, Cat25, Cat26, Cat27, Cat28, Cat29, Cat30, Cat31" value="2147483645" />
                <continous dataType="Bool">true</continous>
                <explicitMass dataType="Float">20</explicitMass>
                <fixedAngle dataType="Bool">false</fixedAngle>
                <gameobj dataType="ObjectRef">3475034345</gameobj>
                <ignoreGravity dataType="Bool">false</ignoreGravity>
                <joints dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.JointInfo]]" id="254245605">
                  <_items dataType="Array" type="Duality.Components.Physics.JointInfo[]" id="2636970134" />
                  <_size dataType="Int">0</_size>
                  <_version dataType="Int">0</_version>
                </joints>
                <linearDamp dataType="Float">0.3</linearDamp>
                <linearVel dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </linearVel>
                <revolutions dataType="Float">0</revolutions>
                <shapes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Physics.ShapeInfo]]" id="2719037288">
                  <_items dataType="Array" type="Duality.Components.Physics.ShapeInfo[]" id="1648512911" length="4">
                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="1935975086">
                      <density dataType="Float">1</density>
                      <friction dataType="Float">0.3</friction>
                      <parent dataType="ObjectRef">2242843573</parent>
                      <restitution dataType="Float">0.3</restitution>
                      <sensor dataType="Bool">false</sensor>
                      <vertices dataType="Array" type="OpenTK.Vector2[]" id="1600081232">
                        <item dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">10.5</X>
                          <Y dataType="Float">9.826252</Y>
                        </item>
                        <item dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-18.17376</Y>
                        </item>
                        <item dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">8.326252</Y>
                        </item>
                      </vertices>
                    </item>
                    <item dataType="Struct" type="Duality.Components.Physics.PolyShapeInfo" id="3158729418">
                      <density dataType="Float">1</density>
                      <friction dataType="Float">0.3</friction>
                      <parent dataType="ObjectRef">2242843573</parent>
                      <restitution dataType="Float">0.3</restitution>
                      <sensor dataType="Bool">false</sensor>
                      <vertices dataType="Array" type="OpenTK.Vector2[]" id="4263675692">
                        <item dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">-10.5</X>
                          <Y dataType="Float">9.826252</Y>
                        </item>
                        <item dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-18.17376</Y>
                        </item>
                        <item dataType="Struct" type="OpenTK.Vector2">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">8.326252</Y>
                        </item>
                      </vertices>
                    </item>
                  </_items>
                  <_size dataType="Int">2</_size>
                  <_version dataType="Int">31</_version>
                </shapes>
              </item>
              <item dataType="Struct" type="DualStickSpaceShooter.Ship" id="402578415">
                <active dataType="Bool">true</active>
                <bulletType dataType="Struct" type="Duality.ContentRef`1[[DualStickSpaceShooter.BulletBlueprint]]">
                  <contentPath dataType="String">Data\Blueprints\RegularBullet.BulletBlueprint.res</contentPath>
                </bulletType>
                <damageEffect dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                  <contentPath dataType="String">Data\Prefabs\DamageSmoke.Prefab.res</contentPath>
                </damageEffect>
                <damageEffectInstance />
                <deathEffects dataType="Array" type="Duality.ContentRef`1[[Duality.Resources.Prefab]][]" id="2169343919">
                  <item dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Prefab]]">
                    <contentPath dataType="String">Data\Prefabs\ShipDebris.Prefab.res</contentPath>
                  </item>
                </deathEffects>
                <gameobj dataType="ObjectRef">3475034345</gameobj>
                <healRate dataType="Float">10</healRate>
                <hitpoints dataType="Float">100</hitpoints>
                <isDead dataType="Bool">false</isDead>
                <maxHitpoints dataType="Float">100</maxHitpoints>
                <maxSpeed dataType="Float">6.5</maxSpeed>
                <maxTurnSpeed dataType="Float">0.5</maxTurnSpeed>
                <owner dataType="Struct" type="DualStickSpaceShooter.Player" id="2034914690">
                  <active dataType="Bool">true</active>
                  <color dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">160</G>
                    <R dataType="Byte">89</R>
                  </color>
                  <controlObj dataType="ObjectRef">402578415</controlObj>
                  <gameobj dataType="ObjectRef">1121329689</gameobj>
                  <id dataType="Enum" type="DualStickSpaceShooter.PlayerId" name="Unknown" value="0" />
                  <respawnTime dataType="Float">0</respawnTime>
                </owner>
                <targetAngle dataType="Float">0</targetAngle>
                <targetAngleRatio dataType="Float">0</targetAngleRatio>
                <targetThrust dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">0</X>
                  <Y dataType="Float">0</Y>
                </targetThrust>
                <thrusterPower dataType="Float">5</thrusterPower>
                <turnPower dataType="Float">1</turnPower>
                <weaponDelay dataType="Float">200</weaponDelay>
                <weaponTimer dataType="Float">0</weaponTimer>
              </item>
            </_items>
            <_size dataType="Int">4</_size>
            <_version dataType="Int">4</_version>
          </compList>
          <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3351157368" surrogate="true">
            <header />
            <body>
              <keys dataType="Array" type="System.Type[]" id="3294293063">
                <item dataType="Type" id="1332492494" value="Duality.Components.Transform" />
                <item dataType="Type" id="3463065418" value="Duality.Components.Renderers.SpriteRenderer" />
                <item dataType="Type" id="984641150" value="Duality.Components.Physics.RigidBody" />
                <item dataType="Type" id="1922798426" value="DualStickSpaceShooter.Ship" />
              </keys>
              <values dataType="Array" type="Duality.Component[]" id="513700096">
                <item dataType="ObjectRef">1540381981</item>
                <item dataType="ObjectRef">822233617</item>
                <item dataType="ObjectRef">2242843573</item>
                <item dataType="ObjectRef">402578415</item>
              </values>
            </body>
          </compMap>
          <compTransform dataType="ObjectRef">1540381981</compTransform>
          <identifier dataType="Struct" type="System.Guid" surrogate="true">
            <header>
              <data dataType="Array" type="System.Byte[]" id="277197253">fZteZX4UbkuDvb5/I4Dl+g==</data>
            </header>
            <body />
          </identifier>
          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          <name dataType="String">PlayerShip</name>
          <parent dataType="ObjectRef">1121329689</parent>
          <prefabLink />
        </item>
      </_items>
      <_size dataType="Int">1</_size>
      <_version dataType="Int">1</_version>
    </children>
    <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="372541130">
      <_items dataType="Array" type="Duality.Component[]" id="1050561672" length="4">
        <item dataType="ObjectRef">2034914690</item>
      </_items>
      <_size dataType="Int">1</_size>
      <_version dataType="Int">1</_version>
    </compList>
    <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3333888994" surrogate="true">
      <header />
      <body>
        <keys dataType="Array" type="System.Type[]" id="2373568800">
          <item dataType="Type" id="2563561436" value="DualStickSpaceShooter.Player" />
        </keys>
        <values dataType="Array" type="Duality.Component[]" id="81153934">
          <item dataType="ObjectRef">2034914690</item>
        </values>
      </body>
    </compMap>
    <compTransform />
    <identifier dataType="Struct" type="System.Guid" surrogate="true">
      <header>
        <data dataType="Array" type="System.Byte[]" id="1059713084">aWzcLwrsd064qYEc+/zBBw==</data>
      </header>
      <body />
    </identifier>
    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
    <name dataType="String">Player</name>
    <parent />
    <prefabLink />
  </objTree>
  <sourcePath dataType="String">Player</sourcePath>
</root>
<!-- XmlFormatterBase Document Separator -->
